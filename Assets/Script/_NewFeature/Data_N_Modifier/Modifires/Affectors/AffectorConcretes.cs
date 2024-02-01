using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Sophia.DataSystem
{
    namespace Modifiers.ConcreteAffectors {
        using Modifiers.Affector;
        using Sophia.Composite.Timer;
        using Sophia.Instantiates;
        using Sophia.Entitys;
        using System.Threading;
        using Unity.VisualScripting;
        using System.Numerics;

        public class PoisonedAffect : Affector {
#region Members
//          public readonly E_AFFECT_TYPE AffectType;
//          public readonly Entity TargetRef;
//          public readonly Entity ownerEntity;
//          public float BaseDurateTime              {get; private set;}
//          public TimerComposite Timer              {get; private set;}
            private CancellationTokenSource cts;

            public Material materialRef {get; private set;}
            public VisualFXObject visualFxRef {get; private set;}
    
            public float TickDamage                  {get; private set;}
            public float TickDamageRatio             {get; private set;}
            public float IntervalTime                {get; private set;}
   
            public PoisonedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime) 
            : base(E_AFFECT_TYPE.Poisoned, ownerReceivers, targetReceivers, durateTime) {
                cts = new CancellationTokenSource();
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                this.IntervalTime = Time.deltaTime;                
                this.TickDamageRatio = 1.0f;
                this.TickDamage = 1f;
                Timer.OnFinished += Revert;
            }

#endregion

#region Setter

            public PoisonedAffect SetMaterial(Material material) {
                this.materialRef = material;
                Timer.OnStart    += PoisonedMeshOn;
                Timer.OnFinished += PoisonedMeshOff;
                return this;
            }

            public PoisonedAffect SetVisualFXObject(VisualFXObject vfx) {
                this.visualFxRef = vfx;
                Timer.OnStart    += PoisonedVFXOn;
                Timer.OnFinished += PoisonedVFXOff;
                return this;
            }
            
            public PoisonedAffect SetTickDamage(float damage){
                this.TickDamage = damage;
                return this;
            }

            public PoisonedAffect SetTickDamageRatio(float Ratio){
                this.TickDamageRatio = Ratio;
                return this;
            }

            public PoisonedAffect SetIntervalTime(float Interval) {
                IntervalTime = Interval;
                this.Timer.SetIntervalTime(IntervalTime);
                Timer.OnInterval += PoisonedDamage;
                return this;
            }

            public PoisonedAffect SetRewindCondition(Func<bool> func) {
                Timer.SetRewindCondition(func);
                return this;
            }
            
#endregion
    
#region Modifiy
                    
            public override void Modifiy(float tenacity)
            {
                Timer.SetStart();
                Timer.Execute();
                base.Modifiy(tenacity);
            }

            public override void TickRunning() {
                Timer.Execute();
                base.TickRunning();
            }
            
            public override void Revert()
            {
                base.Revert();
            }

            public override void CancleModify()
            {
                Timer.Pause();
                Timer.ChangeState(TimerExit.Instance);
                base.CancleModify();
            }

#endregion

#region Affects

            private void PoisonedDamage() =>  TargetRef.GetDamaged((int)CalcTickDamage());
            private async void PoisonedMeshOn()    {
                try
                {
                    if(TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token,materialRef);
                }
                catch (OperationCanceledException) {

                }
            }
            private async void PoisonedMeshOff()   {
                try
                {
                    if(TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch (OperationCanceledException) {

                }
            }
            private void PoisonedVFXOn()  {
                if(visualFxRef.DEBUG)Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                VisualFXObject visualFX = VisualFXObjectPool.GetObject(visualFxRef, OwnerRef);
                TargetRef.GetVisualFXBucket().InstantablePositioning(visualFX).Activate();
            }
            private void PoisonedVFXOff() {
                if(visualFxRef.DEBUG)Debug.Log($"{visualFxRef.gameObject.name} 지금 타입은 {visualFxRef.AffectType.ToString()}");
                TargetRef.GetVisualFXBucket().RemoveInstantableFromBucket(this.AffectType);
            }
#endregion

#region Helper

            private float CalcTickDamage() => TickDamageRatio * TickDamage;
            private float CalcDurateTime(float tenacity) => BaseDurateTime * (1-tenacity);

#endregion
        }
        public class SternAffect : Affector {
#region Members
            private CancellationTokenSource cts;
            public Material materialRef {get; private set;}
            public VisualFXObject visualFxRef {get; private set;}

            public SternAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime) 
            : base(E_AFFECT_TYPE.Stern, ownerReceivers, targetReceivers, durateTime) {
                cts = new CancellationTokenSource();
                targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                Timer.OnFinished += Revert;

                Timer.OnStart += FreezeMovementOn;
                Timer.OnFinished += FreezeMovementOff;
            }
            
#endregion
#region Setter
            public SternAffect SetMaterial(Material material) {
                this.materialRef = material;
                Timer.OnStart       += SternMeshOn;
                Timer.OnFinished    += SternMeshOff;
                return this;
            }
            public SternAffect SetVisualFXObject(VisualFXObject vfx) {
                this.visualFxRef = vfx;
                Timer.OnStart       += SternVFXOn;
                Timer.OnFinished    += SternVFXOff;
                return this;
            }
#endregion
#region Modifiy

            public override void Modifiy(float tenacity)
            {
                Timer.SetStart();
                Timer.Execute();
                base.Modifiy(tenacity);
            }

            public override void TickRunning() {
                Timer.Execute();
                base.TickRunning();
            }
            
            public override void Revert()
            {
                base.Revert();
            }

            public override void CancleModify()
            {
                Timer.Pause();
                Timer.ChangeState(TimerExit.Instance);
                base.CancleModify();
            }

#endregion
#region Affects
            private void FreezeMovementOn() {
                if(TargetRef is IMovable) {
                    IMovable movableEntity = TargetRef as IMovable;
                    movableEntity.SetMoveState(false);
                }
            }
            private void FreezeMovementOff() {
                if(TargetRef is IMovable) {
                    IMovable movableEntity = TargetRef as IMovable;
                    movableEntity.SetMoveState(true);
                }
            }

            private async void SternMeshOn()    {
                try {
                    if(TargetRef == null) return;
                    await TargetRef.GetModelManger().ChangeSkin(cts.Token, materialRef);
                }   
                catch(OperationCanceledException){}             
            }
            private async void SternMeshOff()   {
                try {
                    if(TargetRef == null) return;
                    await TargetRef.GetModelManger().RevertSkin(cts.Token);
                }
                catch(OperationCanceledException){}
            }
            private void SternVFXOn()  {
                VisualFXObject visualFX = VisualFXObjectPool.GetObject(visualFxRef, OwnerRef);
                TargetRef.GetVisualFXBucket().InstantablePositioning(visualFX).Activate();
            }
            private void SternVFXOff() {
                TargetRef.GetVisualFXBucket().RemoveInstantableFromBucket(E_AFFECT_TYPE.Stern);
            }
#endregion
#region Helper

#endregion
        }
        public class AirborneAffect : Affector {
#region Members
            public AirborneAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime) 
            : base(E_AFFECT_TYPE.Airborne, ownerReceivers, targetReceivers, durateTime) {
                Timer.OnFinished += Revert;
                Timer.OnStart += AirboaneStart;
                Timer.OnFinished += AirboaneEnd;
            }
            
#endregion
#region Setter

#endregion
#region Modifiy

            public override void Modifiy(float tenacity)
            {
                Timer.SetStart();
                Timer.Execute();
                base.Modifiy(tenacity);
            }

            public override void TickRunning() {
                Timer.Execute();
                base.TickRunning();
            }
            
            public override void Revert()
            {
                base.Revert();
            }

            public override void CancleModify()
            {
                Timer.Pause();
                Timer.ChangeState(TimerExit.Instance);
                base.CancleModify();
            }

#endregion
#region Affects
            private void AirboaneStart() {
                if(TargetRef is IMovable) {
                    IMovable movableEntity = TargetRef as IMovable;
                    
                    GameObject targetModel = TargetRef.GetModelManger().GetModelObject();
                    targetModel.transform.DOLocalJump(UnityEngine.Vector3.zero, 1, 1, BaseDurateTime);
                    movableEntity.SetMoveState(false);
                }
            }
            private void AirboaneEnd() {
                if(TargetRef is IMovable) {
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