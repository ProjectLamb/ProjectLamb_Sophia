using System;
using UnityEngine;
using DG.Tweening;
using System.Threading;

namespace Sophia.DataSystem
{
    namespace Modifiers.ConcreteAffectors
    {
        using Modifiers.Affector;
        using Sophia.Entitys;
        using Sophia.DataSystem.Functional;
#region Debuff
        public class BurnAffect : Affector
        {
            #region Members
            //          public readonly E_AFFECT_TYPE AffectType;
            //          public readonly Entity TargetRef;
            //          public readonly Entity ownerEntity;
            //          public float BaseDurateTime              {get; private set;}
            //          public TimerComposite Timer              {get; private set;}


            /*
            public Material materialRef { get; private set; }
            */
            /*
            public VisualFXObject visualFxRef { get; private set; }
            */
            private TickDamageCommand FunctionalTickDamageCommand;

            public BurnAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Burn, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            public BurnAffect SetTickDamage(SerialTickDamageAffectData serialTickDamageAffectData)
            {
                if (FunctionalTickDamageCommand == null)
                {

                    FunctionalTickDamageCommand = new TickDamageCommand(serialTickDamageAffectData);
                    void TickDamage() => FunctionalTickDamageCommand.Invoke(ref TargetRef);

                    Timer.SetIntervalTime(CalcDurateTime(serialTickDamageAffectData._intervalTime));
                    Timer.OnInterval += TickDamage;
                }
                return this;
            }
            public BurnAffect SetRewindCondition(Func<bool> func)
            {
                Timer.SetRewindCondition(func);
                return this;
            }
            #endregion
        }

        public class PoisonedAffect : Affector
        {
            #region Members
            //          public readonly E_AFFECT_TYPE AffectType;
            //          public readonly Entity TargetRef;
            //          public readonly Entity ownerEntity;
            //          public float BaseDurateTime              {get; private set;}
            //          public TimerComposite Timer              {get; private set;}

            private TickDamageCommand FunctionalTickDamageCommand;
            private SkinCommand FunctionalSkinCommand;
            private VisualFXCommand FunctionalVisualFXCommand;

            public PoisonedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Poisoned, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            public PoisonedAffect SetMaterial(SerialSkinAffectData serialSkinAffectData)
            {
                if (FunctionalSkinCommand == null)
                {

                    FunctionalSkinCommand = new SkinCommand(serialSkinAffectData, this.cts);
                    void MeshOn() => FunctionalSkinCommand.Invoke(ref TargetRef);
                    void MeshOff() => FunctionalSkinCommand.Revert(ref TargetRef);

                    Timer.OnStart += MeshOn;
                    Timer.OnFinished += MeshOff;
                }
                return this;
            }

            public PoisonedAffect SetVisualFXObject(SerialVisualAffectData serialVisualAffectData)
            {
                if (FunctionalVisualFXCommand == null)
                {

                    FunctionalVisualFXCommand = new VisualFXCommand(E_AFFECT_TYPE.Poisoned, serialVisualAffectData);
                    void VFXOn() => FunctionalVisualFXCommand.Invoke(ref TargetRef);
                    void VFXOff() => FunctionalVisualFXCommand.Revert(ref TargetRef);

                    Timer.OnStart += VFXOn;
                    Timer.OnFinished += VFXOff;
                }
                return this;
            }

            public PoisonedAffect SetTickDamage(SerialTickDamageAffectData serialTickDamageAffectData)
            {
                if (FunctionalTickDamageCommand == null)
                {

                    FunctionalTickDamageCommand = new TickDamageCommand(serialTickDamageAffectData);
                    void TickDamage() => FunctionalTickDamageCommand.Invoke(ref TargetRef);

                    Timer.SetIntervalTime(CalcDurateTime(serialTickDamageAffectData._intervalTime));
                    Timer.OnInterval += TickDamage;
                }
                return this;
            }

            public PoisonedAffect SetRewindCondition(Func<bool> func)
            {
                Timer.SetRewindCondition(func);
                return this;
            }

            #endregion
        }

        public class BleedAffect : Affector
        {
            #region Members
            //          public readonly E_AFFECT_TYPE AffectType;
            //          public readonly Entity TargetRef;
            //          public readonly Entity ownerEntity;
            //          public float BaseDurateTime              {get; private set;}
            //          public TimerComposite Timer              {get; private set;}


            /*
            public Material materialRef { get; private set; }
            */
            /*
            public VisualFXObject visualFxRef { get; private set; }
            */
            public TickDamageCommand FunctionalTickDamageCommand;
            public BleedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Bleed, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter


            public BleedAffect SetTickDamage(SerialTickDamageAffectData serialTickDamageAffectData)
            {
                if (FunctionalTickDamageCommand == null)
                {

                    FunctionalTickDamageCommand = new TickDamageCommand(serialTickDamageAffectData);
                    void TickDamage() => FunctionalTickDamageCommand.Invoke(ref TargetRef);

                    Timer.SetIntervalTime(CalcDurateTime(serialTickDamageAffectData._intervalTime));
                    Timer.OnInterval += TickDamage;
                }
                return this;
            }

            public BleedAffect SetRewindCondition(Func<bool> func)
            {
                Timer.SetRewindCondition(func);
                return this;
            }

            #endregion
        }

        public class ColdAffect : Affector
        {
            #region Members

            private SkinCommand FunctionalSkinCommand;
            private TemporaryModifiyCommand FunctionalTemporaryModifiyCommand;

            public ColdAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Cold, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();

                Timer.OnFinished += Revert;

            }

            #endregion
            #region Setter
            public ColdAffect SetMaterial(SerialSkinAffectData serialSkinAffectData)
            {
                if (FunctionalSkinCommand == null)
                {

                    FunctionalSkinCommand = new SkinCommand(serialSkinAffectData, this.cts);
                    void MeshOn() => FunctionalSkinCommand.Invoke(ref TargetRef);
                    void MeshOff() => FunctionalSkinCommand.Revert(ref TargetRef);

                    Timer.OnStart += MeshOn;
                    Timer.OnFinished += MeshOff;
                }
                return this;
            }

            public ColdAffect SetModifyData(SerialStatModifireDatas ModifiyData)
            {
                if (FunctionalTemporaryModifiyCommand == null)
                {

                    FunctionalTemporaryModifiyCommand = new TemporaryModifiyCommand(ModifiyData, E_NUMERIC_STAT_TYPE.MoveSpeed);
                    void Apply() => FunctionalTemporaryModifiyCommand.Invoke(ref TargetRef);
                    void Unapply() => FunctionalTemporaryModifiyCommand.Revert(ref TargetRef);

                    Timer.OnStart += Apply;
                    Timer.OnFinished += Unapply;
                }
                return this;
            }
            #endregion
        }

        public class ConfusedAffect : Affector
        {
            public ConfusedAffect(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers, float durateTime) : base(affectType, ownerReceivers, targetReceivers, durateTime)
            {

            }
        }

        public class FearAffect : Affector
        {
            public FearAffect(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers, float durateTime) : base(affectType, ownerReceivers, targetReceivers, durateTime)
            {

            }
        }

        public class SternAffect : Affector
        {
            #region Members

            private SkinCommand FunctionalSkinCommand;
            private PauseMoveCommand FunctionalPauseMoveCommand;
            private VisualFXCommand FunctionalVisualFXCommand;

            public SternAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Stern, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;

                FunctionalPauseMoveCommand = new PauseMoveCommand();

                Timer.OnStart += () => FunctionalPauseMoveCommand.Invoke(ref TargetRef);
                Timer.OnFinished += () => FunctionalPauseMoveCommand.Revert(ref TargetRef);
            }

            #endregion
            #region Setter
            public SternAffect SetMaterial(SerialSkinAffectData serialSkinAffectData)
            {
                if (FunctionalSkinCommand == null)
                {

                    FunctionalSkinCommand = new SkinCommand(serialSkinAffectData, this.cts);
                    void MeshOn() => FunctionalSkinCommand.Invoke(ref TargetRef);
                    void MeshOff() => FunctionalSkinCommand.Revert(ref TargetRef);

                    Timer.OnStart += MeshOn;
                    Timer.OnFinished += MeshOff;
                }
                return this;
            }

            public SternAffect SetVisualFXObject(SerialVisualAffectData serialVisualAffectData)
            {
                if (FunctionalVisualFXCommand == null)
                {

                    FunctionalVisualFXCommand = new VisualFXCommand(E_AFFECT_TYPE.Stern, serialVisualAffectData);
                    void VFXOn() => FunctionalVisualFXCommand.Invoke(ref TargetRef);
                    void VFXOff() => FunctionalVisualFXCommand.Revert(ref TargetRef);

                    Timer.OnStart += VFXOn;
                    Timer.OnFinished += VFXOff;
                }
                return this;
            }
            #endregion
        }

        public class BoundedAffect : Affector
        {
            #region Members


            /*
            public Material materialRef { get; private set; }
            */

            /*
            public VisualFXObject visualFxRef { get; private set; }
            */

            private PauseMoveCommand FunctionalPauseMoveCommand;

            public BoundedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Bounded, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;

                FunctionalPauseMoveCommand = new PauseMoveCommand();

                Timer.OnStart += () => FunctionalPauseMoveCommand.Invoke(ref TargetRef);
                Timer.OnFinished += () => FunctionalPauseMoveCommand.Revert(ref TargetRef);
            }

            #endregion
            #region Setter

            #endregion
        }

        public class KnockbackAffect : Affector
        {
            #region Members
            public ImpulseForceCommand FunctionalImpulseForce;

            public KnockbackAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Knockback, ownerReceivers, targetReceivers, durateTime)
            {
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            public KnockbackAffect SetForceAmount(SerialPhysicsAffectData serialPhysicsAffectData)
            {
                if (FunctionalImpulseForce == null)
                {

                    FunctionalImpulseForce = new ImpulseForceCommand(serialPhysicsAffectData, OwnerRef);
                    void KnockBackOn() => FunctionalImpulseForce.Invoke(ref TargetRef);

                    Timer.OnStart += KnockBackOn;
                }
                return this;
            }

            #endregion
        }

        public class BlackHoleAffect : Affector
        {
            #region Members

            public GradualForceCommand FunctionalGradualForceCommand;

            public BlackHoleAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.BlackHole, ownerReceivers, targetReceivers, durateTime)
            {
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            public BlackHoleAffect SetForceAmount(SerialPhysicsAffectData serialPhysicsAffectData)
            {
                if (FunctionalGradualForceCommand == null)
                {

                    FunctionalGradualForceCommand = new GradualForceCommand(serialPhysicsAffectData, OwnerRef);
                    void BlackHoleOn() => FunctionalGradualForceCommand.Invoke(ref TargetRef);
                    void BlackHoleOff() => TargetRef.entityRigidbody.velocity = Vector3.zero;
                    this.Timer.SetIntervalTime(serialPhysicsAffectData._intervalTime);
                    Timer.OnInterval += BlackHoleOn;
                    Timer.OnFinished += BlackHoleOff;
                }
                return this;
            }

            #endregion
        }

        public class AirborneAffect : Affector
        {
            #region Members
            private DotweenForceCommand FunctionalDotweenForceCommand;
            private PauseMoveCommand FunctionalPauseMoveCommand;

            public AirborneAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Airborne, ownerReceivers, targetReceivers, durateTime)
            {
                Timer.OnFinished += Revert;
            }

            #endregion
            #region Setter

            public AirborneAffect SetJumpForce(SerialPhysicsAffectData serialPhysicsAffectData)
            {
                if (FunctionalDotweenForceCommand == null && FunctionalPauseMoveCommand == null)
                {
                    FunctionalDotweenForceCommand = new DotweenForceCommand(serialPhysicsAffectData, BaseDurateTime);
                    FunctionalPauseMoveCommand = new PauseMoveCommand();

                    void AirborneStart()
                    {
                        FunctionalPauseMoveCommand.Invoke(ref TargetRef);
                        FunctionalDotweenForceCommand.Invoke(ref TargetRef);
                    }
                    void AirborneEnd() => FunctionalPauseMoveCommand.Revert(ref TargetRef);

                    Timer.OnStart += AirborneStart;
                    Timer.OnFinished += AirborneEnd;
                }
                return this;
            }

            #endregion
        }
        #endregion

        #region Buff
        public class MoveSpeedUpAffect : Affector
        {
            public MoveSpeedUpAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime) 
            : base(E_AFFECT_TYPE.MoveSpeedUp, ownerReceivers, targetReceivers, durateTime)
            {
                Timer.OnFinished += Revert;
            }

            private TemporaryModifiyCommand FunctionalTemporaryModifiyCommand;
            private VisualFXCommand FunctionalVisualFXCommand;
            
            public MoveSpeedUpAffect SetVisualFXObject(SerialVisualAffectData serialVisualAffectData)
            {
                if (FunctionalVisualFXCommand == null)
                {

                    FunctionalVisualFXCommand = new VisualFXCommand(E_AFFECT_TYPE.Poisoned, serialVisualAffectData);
                    void VFXOn() => FunctionalVisualFXCommand.Invoke(ref TargetRef);
                    void VFXOff() => FunctionalVisualFXCommand.Revert(ref TargetRef);

                    Timer.OnStart += VFXOn;
                    Timer.OnFinished += VFXOff;
                }
                return this;
            }

            public MoveSpeedUpAffect SetModifyData(SerialStatModifireDatas ModifiyData)
            {
                if (FunctionalTemporaryModifiyCommand == null)
                {

                    FunctionalTemporaryModifiyCommand = new TemporaryModifiyCommand(ModifiyData, E_NUMERIC_STAT_TYPE.MoveSpeed);
                    void Apply() => FunctionalTemporaryModifiyCommand.Invoke(ref TargetRef);
                    void Unapply() => FunctionalTemporaryModifiyCommand.Revert(ref TargetRef);

                    Timer.OnStart += Apply;
                    Timer.OnFinished += Unapply;
                }
                return this;
            }
        }
        #endregion
    }


}