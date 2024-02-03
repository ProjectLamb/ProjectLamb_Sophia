using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    using Sophia.DataSystem.Modifiers.Affector;
    using Sophia.DataSystem.Modifiers.ConcreteAffectors;
    using Sophia.DataSystem.Numerics;
    using Sophia.Entitys;
    using Sophia.Instantiates;

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

    public abstract class AffectConveyerCommand : IFunctionalCommand<Entitys.Entity>
    {
        
#region UI Access

        public abstract string GetName();
        public abstract string GetDescription();
        public abstract Sprite GetSprite();

#endregion

        public abstract void SetOwner(Entitys.Entity entity);

        protected abstract Affector AffectorFactory(ref Entitys.Entity target);

        public void Invoke(ref Entitys.Entity referer)
        {
            AffectorFactory(ref referer).ConveyToTarget();
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

        public PoisionAffectConveyerCommand(Entitys.Entity owner, Material affectSkin, VisualFXObject affectVisualFX, float baseDurateTime, float intervalTime, float baseTickDamage, float tickDamageRatio)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Poisoned;
            AffectSkin = affectSkin;
            AffectVisualFX = affectVisualFX;
            BaseDurateTime = baseDurateTime;
            IntervalTime = intervalTime;
            BaseTickDamage = baseTickDamage;
            TickDamageRatio = tickDamageRatio;
        }

        #region Concrete Logic

        protected override Affector AffectorFactory(ref Entitys.Entity target)
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
        public override string GetDescription() => $"{(IntervalTime).ToString()}초당 {(BaseTickDamage * TickDamageRatio).ToString()}의 피해를 입습니다.";

        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public override void SetOwner(Entity entity) => OwnerRef = entity;

        #endregion
    }

    public class SternAffectConveyerCommand : AffectConveyerCommand
    {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly Material AffectSkin;
        public readonly VisualFXObject AffectVisualFX;
        public readonly float BaseDurateTime;
        public readonly float IntervalTime;
        public readonly float BaseTickDamage;
        public readonly float TickDamageRatio;
        public Entitys.Entity OwnerRef;

        public SternAffectConveyerCommand(Entitys.Entity owner, Material affectSkin, VisualFXObject affectVisualFX, float baseDurateTime, float intervalTime, float baseTickDamage, float tickDamageRatio)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Stern;
            AffectSkin = affectSkin;
            AffectVisualFX = affectVisualFX;
            BaseDurateTime = baseDurateTime;
        }

        #region Concrete Logic

        protected override Affector AffectorFactory(ref Entitys.Entity target)
        {
            return new SternAffect(OwnerRef, target, BaseDurateTime)
                        .SetMaterial(this.AffectSkin)
                        .SetVisualFXObject(this.AffectVisualFX);
        }

        #endregion

        #region UI Access
        public override string GetName() => "기절";
        public override string GetDescription() => $"기절에 걸린 상대는 {(IntervalTime).ToString()}초 간 어떠한 행동을 하지 못합니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public override void SetOwner(Entity entity) => OwnerRef = entity;
        #endregion
    }

    public class FrozenAffectConveyerCommand : AffectConveyerCommand
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

        public FrozenAffectConveyerCommand(Entitys.Entity owner, Material affectSkin, VisualFXObject affectVisualFX, float baseDurateTime, float intervalTime, float baseTickDamage, float tickDamageRatio, SerialCalculateDatas calculateDatas)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Frozen;
            BaseDurateTime = baseDurateTime;
            CalculateDatas = calculateDatas;
            AffectSkin = affectSkin;
        }

        #region Concrete Logic

        protected override Affector AffectorFactory(ref Entitys.Entity target)
        {
            return new FrozenAffect(OwnerRef, target, BaseDurateTime)
                        .SetModifyData(CalculateDatas.MoveSpeed)
                        .SetMaterial(AffectSkin);
        }

        #endregion

        #region UI Access
        public override string GetName() => "빙결";
        public override string GetDescription() => $"빙결에 에 걸린 상대는 {BaseDurateTime}초 동안 {CalculateDatas.MoveSpeed.amount}만큼 느려집니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public override void SetOwner(Entity entity) => OwnerRef = entity;
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

        public AirborneAffectConveyerCommand(Entitys.Entity owner, Material affectSkin, VisualFXObject affectVisualFX, float baseDurateTime, float intervalTime, float baseTickDamage, float tickDamageRatio, float jumpForce)
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.Airborne;
            BaseDurateTime = baseDurateTime;
            JumpForce = jumpForce;
        }

        #region Concrete Logic

        protected override Affector AffectorFactory(ref Entitys.Entity target)
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

        public override void SetOwner(Entity entity) => OwnerRef = entity;
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

        public BlackHoleAffectConveyerCommand(
            Entitys.Entity owner,
            Material affectSkin, VisualFXObject affectVisualFX,
            float baseDurateTime, float intervalTime,
            float baseTickDamage, float tickDamageRatio,
            float forceAmount
        )
        {
            OwnerRef = owner;
            AffectType = E_AFFECT_TYPE.BlackHole;
            BaseDurateTime = baseDurateTime;
            IntervalTime = intervalTime;
            ForceAmount = forceAmount;
        }

        #region Concrete Logic

        protected override Affector AffectorFactory(ref Entitys.Entity target)
        {
            return new BlackHoleAffect(OwnerRef, target, BaseDurateTime)
                        .SetForceAmount(ForceAmount)
                        .SetIntervalTime(IntervalTime);
        }

        #endregion

        #region UI Access
        public override string GetName() => "에어본";
        public override string GetDescription() => $"에어본 에 걸린 상대는 {(IntervalTime).ToString()}초 간 정지하며. 공중에 떠오릅니다.";
        public override Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public override void SetOwner(Entity entity) => OwnerRef = entity;
        #endregion
    }

}