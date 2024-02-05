using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    using System.Text;
    using Sophia.DataSystem.Modifiers.Affector;
    using Sophia.DataSystem.Modifiers.ConcreteAffectors;
    using Sophia.DataSystem.Numerics;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    [System.Serializable]
    public struct SerialTickDamageAffectData
    {
        [SerializeField] public float _baseTickDamage;
        [SerializeField] public float _tickDamageRatio;
        [SerializeField] public float _intervalTime;
    }

    [System.Serializable]
    public struct SerialPhysicsAffectData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public float _physicsForce;
    }

    [System.Serializable]
    public struct SerialSkinAffectData {
        [SerializeField] public float _intervalTime;
        [SerializeField] public Material _materialRef;
    }
    [System.Serializable]
    public struct SerialVisualAffectData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public VisualFXObject _visualFxRef;
    }

    [System.Serializable]
    public struct SerialAffectorData
    {
        [SerializeField] public E_AFFECT_TYPE _affectType;
        [SerializeField] public float _baseDurateTime;
        [SerializeField] public SerialSkinAffectData _skinAffectData;
        [SerializeField] public SerialVisualAffectData _visualAffectData;
        [SerializeField] public SerialTickDamageAffectData _tickDamageAffectData;
        [SerializeField] public SerialPhysicsAffectData _physicsAffectData;
        [SerializeField] public SerialCalculateDatas _calculateAffectData;
    }

    /// <summary>
    /// 오브젝트 풀로 객체를 가져오는 방식으로 할것!
    /// </summary>
    public class AffectCommandFactory
    {
        public static AffectCommand GetAffect(Entity owner, SerialAffectorData _serialAffectorData)
        {
            switch (_serialAffectorData._affectType)
            { 
                case E_AFFECT_TYPE.Poisoned:
                {
                    return new PoisionAffectCommand(owner, _serialAffectorData);
                }
                case E_AFFECT_TYPE.Stern:
                {
                    return new SternAffectCommand(owner, _serialAffectorData);
                }
                case E_AFFECT_TYPE.Airborne:
                {
                    return new AirborneAffectCommand(owner, _serialAffectorData);
                }
                case E_AFFECT_TYPE.Cold:
                {
                    return new ColdAffectCommand(owner, _serialAffectorData);
                }
                case E_AFFECT_TYPE.BlackHole:
                {
                    return new BlackHoleAffectCommand(owner, _serialAffectorData);
                }
                default: { throw new System.Exception("현재 알맞는 어펙터가 없음"); }
            }
        }
    }

    public abstract class AffectCommand : IFunctionalCommand<Entitys.Entity>, IUserInterfaceAccessible
    {

        #region UI Access

        public abstract string GetName();
        public abstract string GetDescription();
        public abstract Sprite GetSprite();

        #endregion
        protected E_AFFECT_TYPE AffectType;
        protected SerialAffectorData AffectData;
        protected Entitys.Entity OwnerRef;

        protected abstract Affector CreateAffector(ref Entitys.Entity target);

        public AffectCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData) {
            OwnerRef = owner;
            AffectData = serialAffectorData;
        }

        public void Invoke(ref Entitys.Entity referer)
        {
            CreateAffector(ref referer).ConveyToTarget();
        }
    }

    public class BurnAffectCommand      : AffectCommand
    {
        public BurnAffectCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Burn;
        }

        public override string GetName() => "화상";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public override string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"플레이어가 화염 영역 위 위치할 때 활성화 되며, {AffectData._baseDurateTime}초 뒤 화상효과를 유발한 후 사라집니다\n");
            stringBuilder.Append($"화염 영역을 벗어나게 되는 경우, 화염 영역 위 존재한 시간과 비례해 효과가 유지 된 후 사라집니다.");
            return stringBuilder.ToString();
        }


        protected override Affector CreateAffector(ref Entity target)
        {
            return new BurnAffect(OwnerRef, target, AffectData._baseDurateTime)
                        .SetTickDamage(AffectData._tickDamageAffectData);
        }
    }

    public class PoisionAffectCommand   : AffectCommand
    {
        public PoisionAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Poisoned;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new PoisonedAffect(OwnerRef, target, AffectData._baseDurateTime)
                        .SetTickDamage(this.AffectData._tickDamageAffectData)
                        .SetMaterial(this.AffectData._skinAffectData)
                        .SetVisualFXObject(this.AffectData._visualAffectData);
        }

        #endregion

        #region UI Access
        public override string GetName() => "중독";
        public override string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"본 효과는 일정시간(0.5초)마다 플레이어에게 피해를 입히며, 그 2배의 시간(1초) 뒤 사라집니다.").Append("\n");
            stringBuilder.Append($"본 효과의 피해는 플레이어 전체 체력에 비례해 적용됩니다.(체력 퍼뎀)").Append("\n");
            stringBuilder.Append($"본 효과는 중첩되며, 효과가 사라지기 전 새롭게 상태이상에 걸리게 된 경우 중첩이 증가하며 효과의 갱신 시간은 초기화됩니다.").Append("\n");
            stringBuilder.Append($"1초가 채 가기 전 새롭게 상태이상에 걸린 경우, 효과는 누적되며 누적된 독 효과는 다시 1초 뒤 사라진다, 데미지는 0.5초마다 꾸준히 들어온다.").Append("\n");
            stringBuilder.Append($"최대 5번까지 효과 중첩이 가능하며, 스택마다 일정시간(0.5초)마다 받는 피해는 산술적으로 증가합니다.").Append("\n");
            return stringBuilder.ToString();
        }
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }


        #endregion
    }

    public class BleedAffectCommand     : AffectCommand
    {
        public BleedAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Bleed;
        }

        public override string GetName() => "출혈";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public override string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("본 효과는 일정시간(0.5초)마다 플레이어에게 피해를 입히며, 그 2배의 시간(1초) 뒤 사라집니다.").Append("\n");
            stringBuilder.Append("본 효과의 피해는 플레이어의 방어력을 무시하고 적용됩니다.").Append("\n");
            stringBuilder.Append("본 효과는 중첩되지 않으나, 효과가 사라지기 전 새롭게 상태이상에 걸리게 된 경우 효과의 갱신 시간은 초기화됩니다.").Append("\n");
            stringBuilder.Append("(1초가 채 가기 전 새롭게 상태이상에 걸린 경우, 출혈 효과는 다시 1초 뒤 사라진다, 데미지는 0.5초마다 꾸준히 들어온다.").Append("\n");
            return stringBuilder.ToString();
        }


        protected override Affector CreateAffector(ref Entity target)
        {
            return new BleedAffect(OwnerRef, target, AffectData._baseDurateTime)
                        .SetTickDamage(AffectData._tickDamageAffectData);
        }
    }

    public class ColdAffectCommand      : AffectCommand
    {
        public ColdAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Cold;
        }


        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new ColdAffect(OwnerRef, target, AffectData._baseDurateTime)
                        .SetModifyData(AffectData._calculateAffectData.MoveSpeed)
                        .SetMaterial(AffectData._skinAffectData);
        }

        #endregion

        #region UI Access

        public override string GetName() => "냉기";
        public override string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 플레이어가 한기 영역 위 위치할 때 활성화 되며, 일정시간동안 플레이어의 이동 속도가 늦춰집니다.").Append("\n");
            stringBuilder.Append(" 일정 시간(1초) 뒤 빙결 효과를 유발한 후 사라집니다.").Append("\n");
            stringBuilder.Append(" 한기 영역을 벗어나게 되는 경우, 한기 영역 위 존재한 시간과 비례해 효과가 유지 된 후 사라집니다.").Append("\n");
            return stringBuilder.ToString();
        }
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class ConfusedAffectCommand  : AffectCommand
    {
        public ConfusedAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Confused;
        }

        public override string GetName() => "혼란";
        public override string GetDescription() => " 플레이어가 행동불능 상태가 되며, 주위를 방황합니다";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        protected override Affector CreateAffector(ref Entity target)
        {
            throw new System.NotImplementedException();
        }
    }

    public class FearAffectCommand      : AffectCommand
    {
        public FearAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Fear;
        }

        public override string GetName() => "공포";
        public override string GetDescription() => " 행동불능 상태가 되며, 특정 지점으로부터 도망칩니다";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        protected override Affector CreateAffector(ref Entity target)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class SternAffectCommand     : AffectCommand
    {
        public SternAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Stern;
        }


        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new SternAffect(OwnerRef, target, AffectData._baseDurateTime)
                        .SetMaterial(AffectData._skinAffectData)
                        .SetVisualFXObject(AffectData._visualAffectData);
        }

        #endregion

        #region UI Access
        public override string GetName() => "스턴";
        public override string GetDescription() => $"스턴에 걸린 상대는 {(AffectData._baseDurateTime).ToString()}초 간 행동불능 및 이동불가 상태가 됩니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class BoundedAffectCommand   : AffectCommand
    {
        public BoundedAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Bounded;
        }

        protected override Affector CreateAffector(ref Entity target)
        {
            return new BoundedAffect(OwnerRef, target, AffectData._baseDurateTime);
        }
        
        public override string GetDescription() => "이동불가 상태가 됩니다(조작 가능)";

        public override string GetName() => "속박";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

    }

    public class KnockbackAffectCommand : AffectCommand
    {
        public KnockbackAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Knockback;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new KnockbackAffect(OwnerRef, target, AffectData._baseDurateTime)
                        .SetForceAmount(AffectData._physicsAffectData);
        }

        #endregion

        #region UI Access
        public override string GetName() => "넉백";
        public override string GetDescription() => $"넉백 에 걸린 상대는 {(AffectData._baseDurateTime).ToString()}초 간 강제 행동이 되며, 밀려납니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class BlackHoleAffectCommand : AffectCommand
    {
        public BlackHoleAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.BlackHole;
        }


        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new BlackHoleAffect(OwnerRef, target, AffectData._baseDurateTime)
                            .SetForceAmount(AffectData._physicsAffectData);
        }

        #endregion

        #region UI Access
        public override string GetName() => "이끌림";
        public override string GetDescription() => $"이끌림 에 걸린 상대는 {(AffectData._baseDurateTime).ToString()}초 간 강제행동을 하며. 끌어당겨집니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class AirborneAffectCommand  : AffectCommand
    {
        public AirborneAffectCommand(Entity owner, SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
        {
            AffectType = E_AFFECT_TYPE.Airborne;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new AirborneAffect(OwnerRef, target, AffectData._baseDurateTime)
                        .SetJumpForce(AffectData._physicsAffectData);
        }

        #endregion

        #region UI Access
        public override string GetName() => "에어본";
        public override string GetDescription() => $"에어본 에 걸린 상대는 {(AffectData._baseDurateTime).ToString()}초 간 공중에 떠오릅니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}