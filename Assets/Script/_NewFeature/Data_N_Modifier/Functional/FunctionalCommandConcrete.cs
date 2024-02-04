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
    public struct SerialVisualAffectData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public Material _materialRef;
        [SerializeField] public VisualFXObject _visualFxRef;
    }

    [System.Serializable]
    public struct SerialAffectorData
    {
        [SerializeField] public E_AFFECT_TYPE _affectType;
        [SerializeField] public float _baseDurateTime;
        [SerializeField] public SerialVisualAffectData _visualAffectData;
        [SerializeField] public SerialTickDamageAffectData _tickDamageAffectData;
        [SerializeField] public SerialPhysicsAffectData _physicsAffectData;
        [SerializeField] public SerialCalculateDatas _calculateAffectData;
    }


    public class DefaultCommand<T> : IFunctionalCommand<T>
    {

        #region UI Access

        public string GetDescription() => "";
        public string GetName() => "";
        public Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public void Invoke(ref T referer) { return; }
    }

    public class AffectCommand
    {
        public static AffectConveyerCommand GetAffect(Entity owner, SerialAffectorData _serialAffectorData)
        {
            switch (_serialAffectorData._affectType)
            {
                case E_AFFECT_TYPE.Poisoned:
                    {
                        return new PoisionAffectConveyerCommand(owner, _serialAffectorData);
                    }
                case E_AFFECT_TYPE.Stern:
                    {
                        return new SternAffectConveyerCommand(owner, _serialAffectorData);
                    }
                case E_AFFECT_TYPE.Airborne:
                    {
                        return new AirborneAffectConveyerCommand(owner, _serialAffectorData);
                    }
                case E_AFFECT_TYPE.Cold:
                    {
                        return new ColdAffectConveyerCommand(owner, _serialAffectorData);
                    }
                case E_AFFECT_TYPE.BlackHole:
                    {
                        return new BlackHoleAffectConveyerCommand(owner, _serialAffectorData);
                    }
                default: { throw new System.Exception("현재 알맞는 어펙터가 없음"); }
            }
        }
    }

    public abstract class AffectConveyerCommand : IFunctionalCommand<Entitys.Entity>
    {

        #region UI Access

        public abstract string GetName();
        public abstract string GetDescription();
        public abstract Sprite GetSprite();

        #endregion
        protected abstract Affector CreateAffector(ref Entitys.Entity target);

        public void Invoke(ref Entitys.Entity referer)
        {
            CreateAffector(ref referer).ConveyToTarget();
        }
    }

    public class BurnAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public Entitys.Entity OwnerRef;

        public BurnAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Burn;

            BaseDurateTime = serialAffectorData._baseDurateTime;

            AffectSkin = serialAffectorData._visualAffectData._materialRef;
            AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;

            BaseTickDamage = serialAffectorData._tickDamageAffectData._baseTickDamage;
            IntervalTime = serialAffectorData._tickDamageAffectData._intervalTime;
            TickDamageRatio = serialAffectorData._tickDamageAffectData._tickDamageRatio;
        }
        public override string GetName() => "화상";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public override string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"플레이어가 화염 영역 위 위치할 때 활성화 되며, {BaseDurateTime}초 뒤 화상효과를 유발한 후 사라집니다\n");
            stringBuilder.Append($"화염 영역을 벗어나게 되는 경우, 화염 영역 위 존재한 시간과 비례해 효과가 유지 된 후 사라집니다.");
            return stringBuilder.ToString();
        }


        protected override Affector CreateAffector(ref Entity target)
        {
            return new BurnAffect(OwnerRef, target, BaseDurateTime)
                        .SetIntervalTime(this.IntervalTime)
                        .SetTickDamage(this.BaseTickDamage)
                        .SetTickDamageRatio(this.TickDamageRatio);
        }
    }

    public class PoisionAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public Entitys.Entity OwnerRef;

        public PoisionAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Poisoned;

            BaseDurateTime = serialAffectorData._baseDurateTime;

            AffectSkin = serialAffectorData._visualAffectData._materialRef;
            AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;

            BaseTickDamage = serialAffectorData._tickDamageAffectData._baseTickDamage;
            IntervalTime = serialAffectorData._tickDamageAffectData._intervalTime;
            TickDamageRatio = serialAffectorData._tickDamageAffectData._tickDamageRatio;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new PoisonedAffect(OwnerRef, target, BaseDurateTime)
                        .SetIntervalTime(this.IntervalTime)
                        .SetTickDamage(this.BaseTickDamage)
                        .SetTickDamageRatio(this.TickDamageRatio)
                        .SetMaterial(this.AffectSkin)
                        .SetVisualFXObject(this.AffectVisualFX);
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

    public class BleedAffectConveyerCommand : AffectConveyerCommand
    {

        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public Entitys.Entity OwnerRef;

        public BleedAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Bleed;

            BaseDurateTime = serialAffectorData._baseDurateTime;

            AffectSkin = serialAffectorData._visualAffectData._materialRef;
            AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;

            BaseTickDamage = serialAffectorData._tickDamageAffectData._baseTickDamage;
            IntervalTime = serialAffectorData._tickDamageAffectData._intervalTime;
            TickDamageRatio = serialAffectorData._tickDamageAffectData._tickDamageRatio;
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
            return new BleedAffect(OwnerRef, target, BaseDurateTime)
                        .SetIntervalTime(this.IntervalTime)
                        .SetTickDamage(this.BaseTickDamage)
                        .SetTickDamageRatio(this.TickDamageRatio);
        }
    }


    public class ColdAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public SerialCalculateDatas CalculateDatas { get; private set; }
        public Entitys.Entity OwnerRef;

        public ColdAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Cold;
            BaseDurateTime = serialAffectorData._baseDurateTime;
            CalculateDatas = serialAffectorData._calculateAffectData;
            AffectSkin = serialAffectorData._visualAffectData._materialRef;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new ColdAffect(OwnerRef, target, BaseDurateTime)
                        .SetModifyData(CalculateDatas.MoveSpeed)
                        .SetMaterial(AffectSkin);
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

    public class ConfusedAffectConveyerCommand : AffectConveyerCommand
    {
        public override string GetName() => "혼란";
        public override string GetDescription() => " 플레이어가 행동불능 상태가 되며, 주위를 방황합니다";


        public ConfusedAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {

        }

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        protected override Affector CreateAffector(ref Entity target)
        {
            throw new System.NotImplementedException();
        }
    }

    public class FearAffectConveyerCommand : AffectConveyerCommand
    {
        public override string GetName() => "공포";
        public override string GetDescription() => " 행동불능 상태가 되며, 특정 지점으로부터 도망칩니다";


        public FearAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {

        }

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        protected override Affector CreateAffector(ref Entity target)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class SternAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;

        public Entitys.Entity OwnerRef;

        public SternAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Stern;
            AffectSkin = serialAffectorData._visualAffectData._materialRef;
            AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;
            BaseDurateTime = serialAffectorData._baseDurateTime;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new SternAffect(OwnerRef, target, BaseDurateTime)
                        .SetMaterial(this.AffectSkin)
                        .SetVisualFXObject(this.AffectVisualFX);
        }

        #endregion

        #region UI Access
        public override string GetName() => "스턴";
        public override string GetDescription() => $"스턴에 걸린 상대는 {(BaseDurateTime).ToString()}초 간 행동불능 및 이동불가 상태가 됩니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class BoundedAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        
        public BoundedAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Stern;
            AffectSkin = serialAffectorData._visualAffectData._materialRef;
            AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;
            BaseDurateTime = serialAffectorData._baseDurateTime;
        }
        public Entitys.Entity OwnerRef;

        protected override Affector CreateAffector(ref Entity target)
        {
            return new BoundedAffect(OwnerRef, target, BaseDurateTime);
        }
        
        public override string GetDescription() => "이동불가 상태가 됩니다(조작 가능)";

        public override string GetName() => "속박";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

    }

    public class KnockbackAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public readonly float ForceAmount;
        public Entitys.Entity OwnerRef;

        public KnockbackAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Knockback;
            BaseDurateTime = serialAffectorData._baseDurateTime;
            IntervalTime = serialAffectorData._physicsAffectData._intervalTime;
            ForceAmount = serialAffectorData._physicsAffectData._physicsForce;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new KnockbackAffect(OwnerRef, target, BaseDurateTime)
                        .SetForceAmount(ForceAmount);
        }

        #endregion

        #region UI Access
        public override string GetName() => "넉백";
        public override string GetDescription() => $"넉백 에 걸린 상대는 {(IntervalTime).ToString()}초 간 강제 행동이 되며, 밀려납니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class BlackHoleAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public readonly float ForceAmount;
        public Entitys.Entity OwnerRef;

        public BlackHoleAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.BlackHole;
            BaseDurateTime = serialAffectorData._baseDurateTime;
            IntervalTime = serialAffectorData._physicsAffectData._intervalTime;
            ForceAmount = serialAffectorData._physicsAffectData._physicsForce;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new BlackHoleAffect(OwnerRef, target, BaseDurateTime)
                        .SetForceAmount(ForceAmount)
                        .SetIntervalTime(IntervalTime);
        }

        #endregion

        #region UI Access
        public override string GetName() => "이끌림";
        public override string GetDescription() => $"이끌림 에 걸린 상대는 {(IntervalTime).ToString()}초 간 강제행동을 하며. 끌어당겨집니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    public class AirborneAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public readonly float JumpForce;
        public Entitys.Entity OwnerRef;

        public AirborneAffectConveyerCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Airborne;
            BaseDurateTime = serialAffectorData._baseDurateTime;
            JumpForce = serialAffectorData._physicsAffectData._physicsForce;
        }

        #region Concrete Logic

        protected override Affector CreateAffector(ref Entitys.Entity target)
        {
            return new AirborneAffect(OwnerRef, target, BaseDurateTime)
                        .SetJumpForce(JumpForce);
        }

        #endregion

        #region UI Access
        public override string GetName() => "에어본";
        public override string GetDescription() => $"에어본 에 걸린 상대는 {(BaseDurateTime).ToString()}초 간 공중에 떠오릅니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}