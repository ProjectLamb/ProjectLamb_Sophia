using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Sophia.DataSystem
{
    using Sophia.Entitys;
    using Sophia.Instantiates;

    /*
    이것도 객체 풀을 할 수는 없나?
    게임 오브젝트 뿐만 아니라 말이자..
    */

    namespace Affector {
        public abstract class Affector {
            public readonly E_AFFECT_TYPE AffectType;
            public float BaseDurateTime;
            public Entity targetEntity;
            public Entity ownerEntity;
   
    /*비주얼 데이터*/
            
            public Material material;
            public VisualFXObject visualFx;

            public Affector(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers) {
                this.AffectType = affectType;
                this.ownerEntity = ownerReceivers;
                this.targetEntity = targetReceivers;
            }

    #region Setter

            public Affector SetDurateTime(float time) {
                this.BaseDurateTime = time;
                return this;
            }

            public Affector SetMaterial(Material material) {
                this.material = material;
                return this;
            }

            public Affector SetVisualFXObject(VisualFXObject vfx) {
                this.visualFx =vfx;
                return this;
            }

    #endregion

            public abstract void Modifiy();
            public abstract void TickRunning();
            public abstract void Revert();

    #region Helper
            public float CalcDurateTime() {
                return BaseDurateTime * (1-this.targetEntity.GetStat(E_NUMERIC_STAT_TYPE.Tenacity));
            }

    #endregion
        }
    }

    namespace ConcreteAffectors {
        using Affector;
        using Sophia.Composite.Timer;

        /*
public class BurnAffect : Affector {
private CancellationTokenSource cancel = new CancellationTokenSource();
public float TickDamageRatio;
public float TickDamage;

public BurnAffect() : base(E_AFFECT_TYPE.Burn){
TickDamage = CalcTickDamage();
}

public BurnAffect SetTickDamageRatio(float Ratio){
this.TickDamageRatio = Ratio;
return this;
}


// Updator로 해결해도 되지 않나?

public override async UniTask Modifiy()
{
await UniTask.Yield(PlayerLoopTiming.LastUpdate);
BurnVisualOn();
while(!cancel.Token.IsCancellationRequested){
BurnDamage();
await UniTask.Delay(500, cancellationToken : cancel.Token);
}
}

public override async UniTask Revert()
{
cancel.Cancel();
await UniTask.Yield(PlayerLoopTiming.LastUpdate);
BurnVisualOff();
}

public void BurnDamage() => targetEntity.GetDamaged((int)TickDamage);

public void BurnVisualOn() {
// this.targetEntity.Model..
//this.targetEntity.visualModulator.InteractByVFX( vfx);
}
public void BurnVisualOff() {

}
#region Helper
public float CalcTickDamage() {
return TickDamageRatio * this.ownerEntity.StatReferer.GetStat(E_NUMERIC_STAT_TYPE.Power);
}
#endregion
}
*/
        public class PoisonedAffect : Affector {
        
            public float IntervalTime;
            public float TickDamageRatio;
            private float TickDamage;
            public TimerComposite Timer;
            
            public PoisonedAffect(Entity ownerReceivers , Entity targetReceivers)  : base(E_AFFECT_TYPE.Poisoned, ownerReceivers, targetReceivers){
                TickDamage = CalcTickDamage();
            }

            public PoisonedAffect SetTickDamageRatio(float Ratio){
                this.TickDamageRatio = Ratio;
                return this;
            }
            public PoisonedAffect SetTickDamage(float damage){
                this.TickDamage = damage;
                return this;
            }

            public PoisonedAffect SetIntervalTime(float Interval) {
                IntervalTime = Interval;
                return this;
            }
            
            /*
            Updator로 해결해도 되지 않나?
            */
            public override void Modifiy()
            {
                Timer = new TimerComposite(CalcDurateTime(), IntervalTime);
//                Timer.OnStart += PoisonedVisualOn;
//                Timer.OnFinished += PoisonedVisualOff;
                Timer.OnInterval += PoisonedDamage;
                Timer.WhenLoopable += () => false;

                Timer.SetStart();
            }

            public override void TickRunning() {
                Timer.Execute();
            }

            public override void Revert()
            {
                Timer.Pause();
                Timer.ChangeState(TimerEnd.Instance);
                Timer = null; // 가비지에게 삭제 맡김
            }

            public void PoisonedDamage() => targetEntity.GetDamaged((int)TickDamage);
            public void PoisonedVisualOn() {throw new System.NotImplementedException();}
            public void PoisonedVisualOff() {throw new System.NotImplementedException();}

    #region Helper
            public float CalcTickDamage() {
                return TickDamageRatio * this.ownerEntity.GetStat(E_NUMERIC_STAT_TYPE.Power);
            }
            #endregion

        }
    }
    /*
    namespace AffectInvoker {
        using Affector;

        public class CompositeAffectInvoker {
            private readonly Dictionary<E_AFFECT_TYPE, Affector> AffectSets = new Dictionary<E_AFFECT_TYPE, Affector>();

            public CompositeAffectInvoker(){
                foreach(E_AFFECT_TYPE E in Enum.GetValues(typeof(E_AFFECT_TYPE))) {
                    this.AffectSets.Add(E, null);
                }
            }

            public async void InvokeAffector(Affector command) {
                if(AffectSets.TryGetValue(command.AffectType, out Affector affector)) {
                    await affector.Revert();
                    AffectSets[command.AffectType] = null;
                }
                AffectSets[command.AffectType] = command;
                await AffectSets[command.AffectType].Modifiy();
            }
        }
    }
    */
}