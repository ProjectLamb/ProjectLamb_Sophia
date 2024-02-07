using System;
using UnityEngine;
using DG.Tweening;
using System.Threading;

namespace Sophia.DataSystem
{
    namespace Modifiers.ConcreteAffectors
    {
        using Modifiers.Affector;
        using Sophia.Instantiates;
        using Sophia.Entitys;
        using System.Numerics;
        using Unity.VisualScripting;

        public class BurnAffect         : Affector
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

            public float TickDamage { get; private set; }
            public float TickDamageRatio { get; private set; }
            public float IntervalTime { get; private set; }

            public BurnAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Burn, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                this.IntervalTime = Time.deltaTime;
                this.TickDamageRatio = 1.0f;
                this.TickDamage = 1f;
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            /*
            public BurnAffect SetMaterial(Material material)
            {
                this.materialRef = material;
                Timer.OnStart += BurnMeshOn;
                Timer.OnFinished += BurnMeshOff;
                return this;
            }
            */
            /*
            public BurnAffect SetVisualFXObject(VisualFXObject vfx)
            {
                this.visualFxRef = vfx;
                Timer.OnStart += BurnVFXOn;
                Timer.OnFinished += BurnVFXOff;
                return this;
            }
            */
            public BurnAffect SetTickDamage(float damage)
            {
                this.TickDamage = damage;
                return this;
            }

            public BurnAffect SetTickDamageRatio(float Ratio)
            {
                this.TickDamageRatio = Ratio;
                return this;
            }

            public BurnAffect SetIntervalTime(float Interval)
            {
                IntervalTime = Interval;
                this.Timer.SetIntervalTime(IntervalTime);
                Timer.OnInterval += BurnDamage;
                return this;
            }

            public BurnAffect SetRewindCondition(Func<bool> func)
            {
                Timer.SetRewindCondition(func);
                return this;
            }

            #endregion

            #region Affects

            private void BurnDamage() => TargetRef.GetDamaged((int)CalcTickDamage());

            /*
            private async void BurnMeshOn()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token, materialRef);
                }
                catch (OperationCanceledException)
                {

                }
            }
            */

            /*
            private async void BurnMeshOff()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch (OperationCanceledException)
                {

                }
            }
            */
            /*
            private void BurnVFXOn()
            {
                if (visualFxRef.DEBUG) Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                VisualFXObject visualFX = VisualFXObjectPool.GetObject(visualFxRef);
                TargetRef.GetVisualFXBucket().InstantablePositioning(visualFX).Activate();
            }
            */

            /*
            private void BurnVFXOff()
            {
                if (visualFxRef.DEBUG) Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                TargetRef.GetVisualFXBucket().RemoveInstantableFromBucket(this.AffectType);
            }
            */
            #endregion

            #region Helper

            private float CalcTickDamage() => TickDamageRatio * TickDamage;

            #endregion
        }

        public class PoisonedAffect     : Affector
        {
            #region Members
            //          public readonly E_AFFECT_TYPE AffectType;
            //          public readonly Entity TargetRef;
            //          public readonly Entity ownerEntity;
            //          public float BaseDurateTime              {get; private set;}
            //          public TimerComposite Timer              {get; private set;}


            public Material materialRef { get; private set; }
            public VisualFXObject visualFxRef { get; private set; }

            public float TickDamage { get; private set; }
            public float TickDamageRatio { get; private set; }
            public float IntervalTime { get; private set; }

            public PoisonedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Poisoned, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                this.IntervalTime = Time.deltaTime;
                this.TickDamageRatio = 1.0f;
                this.TickDamage = 1f;
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            public PoisonedAffect SetMaterial(Material material)
            {
                this.materialRef = material;
                Timer.OnStart += PoisonedMeshOn;
                Timer.OnFinished += PoisonedMeshOff;
                return this;
            }

            public PoisonedAffect SetVisualFXObject(VisualFXObject vfx)
            {
                this.visualFxRef = vfx;
                Timer.OnStart += PoisonedVFXOn;
                Timer.OnFinished += PoisonedVFXOff;
                return this;
            }

            public PoisonedAffect SetTickDamage(float damage)
            {
                this.TickDamage = damage;
                return this;
            }

            public PoisonedAffect SetTickDamageRatio(float Ratio)
            {
                this.TickDamageRatio = Ratio;
                return this;
            }

            public PoisonedAffect SetIntervalTime(float Interval)
            {
                IntervalTime = Interval;
                this.Timer.SetIntervalTime(IntervalTime);
                Timer.OnInterval += PoisonedDamage;
                return this;
            }

            public PoisonedAffect SetRewindCondition(Func<bool> func)
            {
                Timer.SetRewindCondition(func);
                return this;
            }

            #endregion

            #region Affects

            private void PoisonedDamage() => TargetRef.GetDamaged((int)CalcTickDamage());
            private async void PoisonedMeshOn()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token, materialRef);
                }
                catch (OperationCanceledException)
                {

                }
            }
            private async void PoisonedMeshOff()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch (OperationCanceledException)
                {

                }
            }
            private void PoisonedVFXOn()
            {
                if (visualFxRef.DEBUG) Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                VisualFXObject visualFX = VisualFXObjectPool.GetObject(visualFxRef);
                TargetRef.GetVisualFXBucket().InstantablePositioning(visualFX).Activate();
            }
            private void PoisonedVFXOff()
            {
                if (visualFxRef.DEBUG) Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                TargetRef.GetVisualFXBucket().RemoveInstantableFromBucket(this.AffectType);
            }
            #endregion

            #region Helper

            private float CalcTickDamage() => TickDamageRatio * TickDamage;

            #endregion
        }

        public class BleedAffect        : Affector
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

            public float TickDamage { get; private set; }
            public float TickDamageRatio { get; private set; }
            public float IntervalTime { get; private set; }

            public BleedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Bleed, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                this.IntervalTime = Time.deltaTime;
                this.TickDamageRatio = 1.0f;
                this.TickDamage = 1f;
                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            /*
            public BleedAffect SetMaterial(Material material)
            {
                this.materialRef = material;
                Timer.OnStart += BleedMeshOn;
                Timer.OnFinished += BleedMeshOff;
                return this;
            }
            */
            /*
            public BleedAffect SetVisualFXObject(VisualFXObject vfx)
            {
                this.visualFxRef = vfx;
                Timer.OnStart += BleedVFXOn;
                Timer.OnFinished += BleedVFXOff;
                return this;
            }
            */
            public BleedAffect SetTickDamage(float damage)
            {
                this.TickDamage = damage;
                return this;
            }

            public BleedAffect SetTickDamageRatio(float Ratio)
            {
                this.TickDamageRatio = Ratio;
                return this;
            }

            public BleedAffect SetIntervalTime(float Interval)
            {
                IntervalTime = Interval;
                this.Timer.SetIntervalTime(IntervalTime);
                Timer.OnInterval += BleedDamage;
                return this;
            }

            public BleedAffect SetRewindCondition(Func<bool> func)
            {
                Timer.SetRewindCondition(func);
                return this;
            }

            #endregion

            #region Affects

            private void BleedDamage() => TargetRef.GetDamaged((int)CalcTickDamage());

            /*
            private async void BleedMeshOn()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token, materialRef);
                }
                catch (OperationCanceledException)
                {

                }
            }
            */

            /*
            private async void BleedMeshOff()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch (OperationCanceledException)
                {

                }
            }
            */
            /*
            private void BleedVFXOn()
            {
                if (visualFxRef.DEBUG) Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                VisualFXObject visualFX = VisualFXObjectPool.GetObject(visualFxRef);
                TargetRef.GetVisualFXBucket().InstantablePositioning(visualFX).Activate();
            }
            */

            /*
            private void BleedVFXOff()
            {
                if (visualFxRef.DEBUG) Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                TargetRef.GetVisualFXBucket().RemoveInstantableFromBucket(this.AffectType);
            }
            */
            #endregion

            #region Helper

            private float CalcTickDamage() => TickDamageRatio * TickDamage;

            #endregion
        }

        public class ColdAffect       : Affector
        {
            #region Members

            public Material materialRef { get; private set; }

            public StatModifier MoveSpeedModifier;

            public ColdAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Cold, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();

                Timer.OnFinished += Revert;

            }


            #endregion
            #region Setter
            public ColdAffect SetMaterial(Material material)
            {
                this.materialRef = material;
                Timer.OnStart += ColdMeshOn;
                Timer.OnFinished += ColdMeshOff;
                return this;
            }
            
            public ColdAffect SetModifyData(SerialStatModifireDatas moveSpeedModifiyData)
            {
                MoveSpeedModifier = new StatModifier(moveSpeedModifiyData.amount, moveSpeedModifiyData.calType, E_NUMERIC_STAT_TYPE.MoveSpeed);
                Timer.OnStart += ColdStart;
                Timer.OnFinished += ColdEnd;
                return this;
            }
            #endregion

            #region Affects

            private void ColdStart()
            {
                TargetRef.GetStatReferer().GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed).AddModifier(MoveSpeedModifier);
                Debug.Log(TargetRef.GetStatReferer().GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed).GetValueForce());
            }

            private void ColdEnd()
            {
                TargetRef.GetStatReferer().GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed).RemoveModifier(MoveSpeedModifier);
                Debug.Log(TargetRef.GetStatReferer().GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed).GetValueForce());
            }

            private async void ColdMeshOn()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token, materialRef);
                }
                catch (OperationCanceledException) { }
            }
            private async void ColdMeshOff()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch (OperationCanceledException) { }
            }
            #endregion
            #region Helper

            #endregion
        }

        public class ConfusedAffect     : Affector
        {
            public ConfusedAffect(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers, float durateTime) : base(affectType, ownerReceivers, targetReceivers, durateTime)
            {
            }
        }

        public class FearAffect         : Affector
        {
            public FearAffect(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers, float durateTime) : base(affectType, ownerReceivers, targetReceivers, durateTime)
            {
            }
        }

        public class SternAffect        : Affector
        {
            #region Members

            public Material materialRef { get; private set; }
            public VisualFXObject visualFxRef { get; private set; }

            public SternAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Stern, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;

                Timer.OnStart += FreezeMovementOn;
                Timer.OnFinished += FreezeMovementOff;
            }

            #endregion
            #region Setter
            public SternAffect SetMaterial(Material material)
            {
                this.materialRef = material;
                Timer.OnStart += SternMeshOn;
                Timer.OnFinished += SternMeshOff;
                return this;
            }
            public SternAffect SetVisualFXObject(VisualFXObject vfx)
            {
                this.visualFxRef = vfx;
                Timer.OnStart += SternVFXOn;
                Timer.OnFinished += SternVFXOff;
                return this;
            }
            #endregion

            #region Affects
            private void FreezeMovementOn()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    movableEntity.SetMoveState(false);
                }
            }
            private void FreezeMovementOff()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    movableEntity.SetMoveState(true);
                }
            }

            private async void SternMeshOn()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token, materialRef);
                }
                catch (OperationCanceledException) { }
            }
            private async void SternMeshOff()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch (OperationCanceledException) { }
            }
            private void SternVFXOn()
            {
                VisualFXObject visualFX = VisualFXObjectPool.GetObject(visualFxRef);
                TargetRef.GetVisualFXBucket().InstantablePositioning(visualFX).Activate();
            }
            private void SternVFXOff()
            {
                TargetRef.GetVisualFXBucket().RemoveInstantableFromBucket(E_AFFECT_TYPE.Stern);
            }
            #endregion
            #region Helper

            #endregion
        }

        public class BoundedAffect      : Affector
        {
            #region Members


            /*
            public Material materialRef { get; private set; }
            */

            /*
            public VisualFXObject visualFxRef { get; private set; }
            */

            public BoundedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Bounded, ownerReceivers, targetReceivers, durateTime)
            {
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;

                Timer.OnStart += BoundedMovementOn;
                Timer.OnFinished += BoundedMovementOff;
            }

            #endregion
            #region Setter

            /*
            public BoundedAffect SetMaterial(Material material)
            {
                this.materialRef = material;
                Timer.OnStart += BoundedMeshOn;
                Timer.OnFinished += BoundedMeshOff;
                return this;
            }
            */

            /*
            public BoundedAffect SetVisualFXObject(VisualFXObject vfx)
            {
                this.visualFxRef = vfx;
                Timer.OnStart += BoundedVFXOn;
                Timer.OnFinished += BoundedVFXOff;
                return this;
            }
            */

            #endregion

            #region Affects
            private void BoundedMovementOn()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    movableEntity.SetMoveState(false);
                }
            }
            private void BoundedMovementOff()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    movableEntity.SetMoveState(true);
                }
            }

            /*
            private async void BoundedMeshOn()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token, materialRef);
                }
                catch (OperationCanceledException) { }
            }
            private async void BoundedMeshOff()
            {
                try
                {
                    if (TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch (OperationCanceledException) { }
            }
            */

            /*
            private void BoundedVFXOn()
            {
                VisualFXObject visualFX = VisualFXObjectPool.GetObject(visualFxRef);
                TargetRef.GetVisualFXBucket().InstantablePositioning(visualFX).Activate();
            }
            private void BoundedVFXOff()
            {
                TargetRef.GetVisualFXBucket().RemoveInstantableFromBucket(E_AFFECT_TYPE.Stern);
            }
            */
            #endregion
            #region Helper

            #endregion
        }

        public class KnockbackAffect    : Affector
        {
            #region Members

            public float ForceAmount { get; private set; }
            public float IntervalTime { get; private set; }

            public KnockbackAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Knockback, ownerReceivers, targetReceivers, durateTime)
            {

                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            public KnockbackAffect SetForceAmount(float forceAmount)
            {
                ForceAmount = forceAmount;
                Timer.OnStart += KnockbackOn;
                Timer.OnFinished += KnockbackOff;
                return this;
            }
            /*
            public KnockbackAffect SetIntervalTime(float interval)
            {
                IntervalTime = interval;
                this.Timer.SetIntervalTime(IntervalTime);
                Timer.OnInterval += KnockbackRunning;
                return this;
            }
            */

            #endregion

            #region Affects

            public void KnockbackOn()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    TargetRef.entityRigidbody.velocity = UnityEngine.Vector3.zero;
                    movableEntity.SetMoveState(false);
                }
            }


            public void KnockbackRunning()
            {
                UnityEngine.Vector3 Dir = UnityEngine.Vector3.Normalize(TargetRef.entityRigidbody.position - OwnerRef.entityRigidbody.position);
                TargetRef.entityRigidbody.velocity = Dir * ForceAmount * IntervalTime;
            }

            public void KnockbackOff()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    TargetRef.entityRigidbody.velocity = UnityEngine.Vector3.zero;
                    movableEntity.SetMoveState(true);
                }
            }

            #endregion

            #region Helper
            #endregion
        }

        public class BlackHoleAffect    : Affector
        {
            #region Members

            public float ForceAmount { get; private set; }
            public float IntervalTime { get; private set; }

            public BlackHoleAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.BlackHole, ownerReceivers, targetReceivers, durateTime)
            {

                Timer.OnFinished += Revert;
            }

            #endregion

            #region Setter

            public BlackHoleAffect SetForceAmount(float forceAmount)
            {
                ForceAmount = forceAmount;
                Timer.OnStart += BlackHoleOn;
                Timer.OnFinished += BlackHoleOff;
                return this;
            }

            public BlackHoleAffect SetIntervalTime(float interval)
            {
                IntervalTime = interval;
                this.Timer.SetIntervalTime(IntervalTime);
                Timer.OnInterval += BlackHoleRunning;
                return this;
            }

            #endregion

            #region Affects

            public void BlackHoleOn()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    TargetRef.entityRigidbody.velocity = UnityEngine.Vector3.zero;
                    movableEntity.SetMoveState(false);
                }
            }


            public void BlackHoleRunning()
            {
                UnityEngine.Vector3 Dir = UnityEngine.Vector3.Normalize(OwnerRef.entityRigidbody.position - TargetRef.entityRigidbody.position);
                TargetRef.entityRigidbody.velocity = Dir * ForceAmount * IntervalTime;
            }

            public void BlackHoleOff()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;
                    TargetRef.entityRigidbody.velocity = UnityEngine.Vector3.zero;
                    movableEntity.SetMoveState(true);
                }
            }

            #endregion

            #region Helper
            #endregion
        }

        public class AirborneAffect     : Affector
        {
            #region Members
            public float JumpForce { get; private set; }
            public AirborneAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
            : base(E_AFFECT_TYPE.Airborne, ownerReceivers, targetReceivers, durateTime)
            {
                Timer.OnFinished += Revert;
            }

            #endregion
            #region Setter

            public AirborneAffect SetJumpForce(float jumpForce)
            {
                JumpForce = jumpForce;
                Timer.OnStart += AirborneStart;
                Timer.OnFinished += AirborneEnd;
                return this;
            }

            #endregion

            #region Affects
            private void AirborneStart()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;

                    GameObject targetModel = TargetRef.GetModelManger().GetModelObject();
                    targetModel.transform.DOLocalJump(UnityEngine.Vector3.zero, JumpForce, 1, BaseDurateTime);
                    movableEntity.SetMoveState(false);
                }
            }
            private void AirborneEnd()
            {
                if (TargetRef is IMovable)
                {
                    IMovable movableEntity = TargetRef as IMovable;

                    movableEntity.SetMoveState(true);
                }
            }

            #endregion
            #region Helper

            #endregion
        }


    }
}