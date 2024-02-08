// using UnityEngine;

// namespace Sophia.DataSystem.Functional
// {
//     using System.Text;
//     using Sophia.DataSystem.Referer;
//     
//     using Sophia.DataSystem.Modifiers.ConcreteAffectors;
    
//     using Sophia.Entitys;
//     using Sophia.Instantiates;
//     [System.Serializable]

//     /// <summary>
//     /// 오브젝트 풀로 객체를 가져오는 방식으로 할것!
//     /// </summary>
//     public class AffectCommandFactory
//     {
//         public static AffectCommand GetAffect(Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData _serialAffectorData) 
//         {
//             switch (_serialAffectorData._affectType)
//             {
//                 case E_AFFECT_TYPE.Poisoned:
//                     {
//                         return new PoisionAffectCommand(owner, _serialAffectorData);
//                     }
//                 case E_AFFECT_TYPE.Stern:
//                     {
//                         return new SternAffectCommand(owner, _serialAffectorData);
//                     }
//                 case E_AFFECT_TYPE.Airborne:
//                     {
//                         return new AirborneAffectCommand(owner, _serialAffectorData);
//                     }
//                 case E_AFFECT_TYPE.Cold:
//                     {
//                         return new ColdAffectCommand(owner, _serialAffectorData);
//                     }
//                 case E_AFFECT_TYPE.BlackHole:
//                     {
//                         return new BlackHoleAffectCommand(owner, _serialAffectorData);
//                     }
//                 default: { throw new System.Exception("현재 알맞는 어펙터가 없음"); }
//             }
//         }
//     }

//     public abstract class AffectCommand : IFunctionalCommand<Entitys.Entity>
//     {

//         #region UI Access

//         public abstract string GetName();
//         public abstract string GetDescription();
//         public abstract Sprite GetSprite();

//         #endregion
//         protected E_AFFECT_TYPE AffectType;
//         protected Sophia.DataSystem.Modifiers.SerialAffectorData AffectData; 
//         protected Entitys.Entity OwnerRef;

//         protected abstract Affector CreateAffector(ref Entitys.Entity target);

//         public AffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) 
//         {
//             OwnerRef = owner;
//             AffectData = serialAffectorData;
//         }

//         public void Invoke(ref Entitys.Entity referer)
//         {
//             CreateAffector(ref referer).ConveyToTarget();
//         }
//     }

//     public class BurnAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;
//         public readonly float IntervalTime;
//         public readonly float BaseTickDamage;
//         public readonly float TickDamageRatio;

//         public BurnAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Burn;

//             BaseDurateTime = serialAffectorData._baseDurateTime;

//             AffectSkin = serialAffectorData._skinAffectData._materialRef;
//             AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;

//             BaseTickDamage = serialAffectorData._tickDamageAffectData._baseTickDamage;
//             IntervalTime = serialAffectorData._tickDamageAffectData._intervalTime;
//             TickDamageRatio = serialAffectorData._tickDamageAffectData._tickDamageRatio;
//         }
//         public override string GetName() => "화상";

//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         public override string GetDescription()
//         {
//             StringBuilder stringBuilder = new StringBuilder();
//             stringBuilder.Append($"플레이어가 화염 영역 위 위치할 때 활성화 되며, {BaseDurateTime}초 뒤 화상효과를 유발한 후 사라집니다\n");
//             stringBuilder.Append($"화염 영역을 벗어나게 되는 경우, 화염 영역 위 존재한 시간과 비례해 효과가 유지 된 후 사라집니다.");
//             return stringBuilder.ToString();
//         }


//         protected override Affector CreateAffector(ref Entity target)
//         {
//             return new BurnAffect(OwnerRef, target, BaseDurateTime)
//                         .SetIntervalTime(this.IntervalTime)
//                         .SetTickDamage(this.BaseTickDamage)
//                         .SetTickDamageRatio(this.TickDamageRatio);
//         }
//     }

//     public class PoisionAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;
//         public readonly float IntervalTime;
//         public readonly float BaseTickDamage;
//         public readonly float TickDamageRatio;

//         public PoisionAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Poisoned;

//             BaseDurateTime = serialAffectorData._baseDurateTime;

//             AffectSkin = serialAffectorData._skinAffectData._materialRef;
//             AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;

//             BaseTickDamage = serialAffectorData._tickDamageAffectData._baseTickDamage;
//             IntervalTime = serialAffectorData._tickDamageAffectData._intervalTime;
//             TickDamageRatio = serialAffectorData._tickDamageAffectData._tickDamageRatio;
//         }

//         #region Concrete Logic

//         protected override Affector CreateAffector(ref Entitys.Entity target)
//         {
//             return new PoisonedAffect(OwnerRef, target, BaseDurateTime)
//                         .SetIntervalTime(this.IntervalTime)
//                         .SetTickDamage(this.BaseTickDamage)
//                         .SetTickDamageRatio(this.TickDamageRatio)
//                         .SetMaterial(this.AffectSkin)
//                         .SetVisualFXObject(this.AffectVisualFX);
//         }

//         #endregion

//         #region UI Access
//         public override string GetName() => "중독";
//         public override string GetDescription()
//         {
//             StringBuilder stringBuilder = new StringBuilder();
//             stringBuilder.Append($"본 효과는 일정시간(0.5초)마다 플레이어에게 피해를 입히며, 그 2배의 시간(1초) 뒤 사라집니다.").Append("\n");
//             stringBuilder.Append($"본 효과의 피해는 플레이어 전체 체력에 비례해 적용됩니다.(체력 퍼뎀)").Append("\n");
//             stringBuilder.Append($"본 효과는 중첩되며, 효과가 사라지기 전 새롭게 상태이상에 걸리게 된 경우 중첩이 증가하며 효과의 갱신 시간은 초기화됩니다.").Append("\n");
//             stringBuilder.Append($"1초가 채 가기 전 새롭게 상태이상에 걸린 경우, 효과는 누적되며 누적된 독 효과는 다시 1초 뒤 사라진다, 데미지는 0.5초마다 꾸준히 들어온다.").Append("\n");
//             stringBuilder.Append($"최대 5번까지 효과 중첩이 가능하며, 스택마다 일정시간(0.5초)마다 받는 피해는 산술적으로 증가합니다.").Append("\n");
//             return stringBuilder.ToString();
//         }
//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }


//         #endregion
//     }

//     public class BleedAffectCommand : AffectCommand
//     {

//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;
//         public readonly float IntervalTime;
//         public readonly float BaseTickDamage;
//         public readonly float TickDamageRatio;

//         public BleedAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Bleed;

//             BaseDurateTime = serialAffectorData._baseDurateTime;

//             AffectSkin = serialAffectorData._skinAffectData._materialRef;
//             AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;

//             BaseTickDamage = serialAffectorData._tickDamageAffectData._baseTickDamage;
//             IntervalTime = serialAffectorData._tickDamageAffectData._intervalTime;
//             TickDamageRatio = serialAffectorData._tickDamageAffectData._tickDamageRatio;
//         }
//         public override string GetName() => "출혈";

//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         public override string GetDescription()
//         {
//             StringBuilder stringBuilder = new StringBuilder();
//             stringBuilder.Append("본 효과는 일정시간(0.5초)마다 플레이어에게 피해를 입히며, 그 2배의 시간(1초) 뒤 사라집니다.").Append("\n");
//             stringBuilder.Append("본 효과의 피해는 플레이어의 방어력을 무시하고 적용됩니다.").Append("\n");
//             stringBuilder.Append("본 효과는 중첩되지 않으나, 효과가 사라지기 전 새롭게 상태이상에 걸리게 된 경우 효과의 갱신 시간은 초기화됩니다.").Append("\n");
//             stringBuilder.Append("(1초가 채 가기 전 새롭게 상태이상에 걸린 경우, 출혈 효과는 다시 1초 뒤 사라진다, 데미지는 0.5초마다 꾸준히 들어온다.").Append("\n");
//             return stringBuilder.ToString();
//         }


//         protected override Affector CreateAffector(ref Entity target)
//         {
//             return new BleedAffect(OwnerRef, target, BaseDurateTime)
//                         .SetIntervalTime(this.IntervalTime)
//                         .SetTickDamage(this.BaseTickDamage)
//                         .SetTickDamageRatio(this.TickDamageRatio);
//         }
//     }

//     public class ColdAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;
//         public readonly float IntervalTime;
//         public readonly float BaseTickDamage;
//         public readonly float TickDamageRatio;
//         public SerialCalculateDatas CalculateDatas { get; private set; }

//         public ColdAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Cold;
//             BaseDurateTime = serialAffectorData._baseDurateTime;
//             CalculateDatas = serialAffectorData._calculateAffectData;
//             AffectSkin = serialAffectorData._skinAffectData._materialRef;
//         }

//         #region Concrete Logic

//         protected override Affector CreateAffector(ref Entitys.Entity target)
//         {
//             return new ColdAffect(OwnerRef, target, BaseDurateTime)
//                         .SetModifyData(CalculateDatas.MoveSpeed)
//                         .SetMaterial(AffectSkin);
//         }

//         #endregion

//         #region UI Access

//         public override string GetName() => "냉기";
//         public override string GetDescription()
//         {
//             StringBuilder stringBuilder = new StringBuilder();
//             stringBuilder.Append(" 플레이어가 한기 영역 위 위치할 때 활성화 되며, 일정시간동안 플레이어의 이동 속도가 늦춰집니다.").Append("\n");
//             stringBuilder.Append(" 일정 시간(1초) 뒤 빙결 효과를 유발한 후 사라집니다.").Append("\n");
//             stringBuilder.Append(" 한기 영역을 벗어나게 되는 경우, 한기 영역 위 존재한 시간과 비례해 효과가 유지 된 후 사라집니다.").Append("\n");
//             return stringBuilder.ToString();
//         }
//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         #endregion
//     }

//     public class ConfusedAffectCommand : AffectCommand
//     {
//         public override string GetName() => "혼란";
//         public override string GetDescription() => " 플레이어가 행동불능 상태가 되며, 주위를 방황합니다";


//         public ConfusedAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {

//         }

//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         protected override Affector CreateAffector(ref Entity target)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class FearAffectCommand : AffectCommand
//     {
//         public override string GetName() => "공포";
//         public override string GetDescription() => " 행동불능 상태가 되며, 특정 지점으로부터 도망칩니다";


//         public FearAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {

//         }

//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         protected override Affector CreateAffector(ref Entity target)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class SternAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;


//         public SternAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Stern;
//             AffectSkin = serialAffectorData._skinAffectData._materialRef;
//             AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;
//             BaseDurateTime = serialAffectorData._baseDurateTime;
//         }

//         #region Concrete Logic

//         protected override Affector CreateAffector(ref Entitys.Entity target)
//         {
//             return new SternAffect(OwnerRef, target, BaseDurateTime)
//                         .SetMaterial(this.AffectSkin)
//                         .SetVisualFXObject(this.AffectVisualFX);
//         }

//         #endregion

//         #region UI Access
//         public override string GetName() => "스턴";
//         public override string GetDescription() => $"스턴에 걸린 상대는 {(BaseDurateTime).ToString()}초 간 행동불능 및 이동불가 상태가 됩니다.";
//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         #endregion
//     }

//     public class BoundedAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;

//         public BoundedAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Stern;
//             AffectSkin = serialAffectorData._skinAffectData._materialRef;
//             AffectVisualFX = serialAffectorData._visualAffectData._visualFxRef;
//             BaseDurateTime = serialAffectorData._baseDurateTime;
//         }

//         protected override Affector CreateAffector(ref Entity target)
//         {
//             return new BoundedAffect(OwnerRef, target, BaseDurateTime);
//         }

//         public override string GetDescription() => "이동불가 상태가 됩니다(조작 가능)";

//         public override string GetName() => "속박";

//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//     }

//     public class KnockbackAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;
//         public readonly float IntervalTime;
//         public readonly float BaseTickDamage;
//         public readonly float TickDamageRatio;
//         public readonly float ForceAmount;

//         public KnockbackAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Knockback;
//             BaseDurateTime = serialAffectorData._baseDurateTime;
//             IntervalTime = serialAffectorData._physicsAffectData._intervalTime;
//             ForceAmount = serialAffectorData._physicsAffectData._physicsForce;
//         }

//         #region Concrete Logic

//         protected override Affector CreateAffector(ref Entitys.Entity target)
//         {
//             return new KnockbackAffect(OwnerRef, target, BaseDurateTime)
//                         .SetForceAmount(ForceAmount);
//         }

//         #endregion

//         #region UI Access
//         public override string GetName() => "넉백";
//         public override string GetDescription() => $"넉백 에 걸린 상대는 {(IntervalTime).ToString()}초 간 강제 행동이 되며, 밀려납니다.";
//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         #endregion
//     }

//     public class BlackHoleAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;
//         public readonly float IntervalTime;
//         public readonly float BaseTickDamage;
//         public readonly float TickDamageRatio;
//         public readonly float ForceAmount;

//         public BlackHoleAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.BlackHole;
//             BaseDurateTime = serialAffectorData._baseDurateTime;
//             IntervalTime = serialAffectorData._physicsAffectData._intervalTime;
//             ForceAmount = serialAffectorData._physicsAffectData._physicsForce;
//         }

//         #region Concrete Logic

//         protected override Affector CreateAffector(ref Entitys.Entity target)
//         {
//             return new BlackHoleAffect(OwnerRef, target, BaseDurateTime)
//                         .SetForceAmount(ForceAmount)
//                         .SetIntervalTime(IntervalTime);
//         }

//         #endregion

//         #region UI Access
//         public override string GetName() => "이끌림";
//         public override string GetDescription() => $"이끌림 에 걸린 상대는 {(IntervalTime).ToString()}초 간 강제행동을 하며. 끌어당겨집니다.";
//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         #endregion
//     }

//     public class AirborneAffectCommand : AffectCommand
//     {
//         public readonly Material AffectSkin;
//         public readonly VisualFXObject AffectVisualFX;
//         public readonly float BaseDurateTime;
//         public readonly float IntervalTime;
//         public readonly float BaseTickDamage;
//         public readonly float TickDamageRatio;
//         public readonly float JumpForce;

//         public AirborneAffectCommand(Entitys.Entity owner, Sophia.DataSystem.Modifiers.SerialAffectorData serialAffectorData) : base(owner, serialAffectorData)
//         {
//             OwnerRef = owner;
//             AffectType = E_AFFECT_TYPE.Airborne;
//             BaseDurateTime = serialAffectorData._baseDurateTime;
//             JumpForce = serialAffectorData._physicsAffectData._physicsForce;
//         }

//         #region Concrete Logic

//         protected override Affector CreateAffector(ref Entitys.Entity target)
//         {
//             return new AirborneAffect(OwnerRef, target, BaseDurateTime)
//                         .SetJumpForce(JumpForce);
//         }

//         #endregion

//         #region UI Access
//         public override string GetName() => "에어본";
//         public override string GetDescription() => $"에어본 에 걸린 상대는 {(BaseDurateTime).ToString()}초 간 공중에 떠오릅니다.";
//         public override Sprite GetSprite()
//         {
//             throw new System.NotImplementedException();
//         }

//         #endregion
//     }
// }