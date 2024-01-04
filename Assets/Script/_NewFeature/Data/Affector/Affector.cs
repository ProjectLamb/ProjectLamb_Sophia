using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Feature_NewData
{
    public enum E_AFFECT_TYPE {
        None = 0,

        // 화상, 독, 출혈, 수축, 냉기, 혼란, 공포, 스턴, 속박, 처형
        // 블랙홀
        Debuff = 100,
        Burn, Poisoned, Bleed, Contracted, Freeze, Confused, Fear, Stern, Bounded, Execution,
        BlackHole,

        // 이동속도증가, 고유시간가속, 공격력증가, 보호막상태, CC저항, 은신, 무적, 방어/페링, 투사체생성, 회피,
        Buff = 200,
        MoveSpeedUp, Accelerated, PowerUp, Barrier, Resist, Invisible, Invincible, Defence, ProjectileGenerate, Dodgeing, 
    }
    namespace Affector {
        public abstract class Affector : IUpdatable{
            public readonly E_AFFECT_TYPE AffectType;
            public float BaseDurateTime;
            public Entity targetEntity;
            public Entity ownerEntity;
    /*비주얼 데이터*/
            
            public Material material;
            public VisualFXObject visualFx;
            public CoolTimeComposite timer;

            public Affector(E_AFFECT_TYPE affectType) {
                this.AffectType = affectType;
            }

    #region Setter

            public Affector SetOwner(Entity ownerReceivers){
                this.ownerEntity = ownerReceivers;
                return this;
            }

            public Affector SetTarget(Entity targetReceivers){
                this.targetEntity = targetReceivers;
                return this;
            }

            public Affector SetDurateTime(float time) {
                this.BaseDurateTime = time;
                timer = new CoolTimeComposite(BaseDurateTime);
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

            public abstract UniTask Modifiy();

            public abstract UniTask Revert();

    #region Updatable
            
            public void LateTick() {
                throw new NotImplementedException();
            }

            public void FrameTick() => this.timer.Tick();

            public void PhysicsTick() {
                throw new NotImplementedException();
            }

    #endregion

    #region Helper
            public float CalcDurateTime() {
                return BaseDurateTime * (1-this.targetEntity.StatReferer.GetStat(E_NUMERIC_STAT_TYPE.Tenacity));
            }

    #endregion
        }
    }

    namespace ConcreteAffectors {
        using Affector;
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
            private CancellationTokenSource cancel = new CancellationTokenSource();
            public float IntervalTime;
            public float TickDamageRatio;
            private float TickDamage;
            
            public PoisonedAffect() : base(E_AFFECT_TYPE.Poisoned){
                TickDamage = CalcTickDamage();
            }

            public PoisonedAffect SetTickDamageRatio(float Ratio){
                this.TickDamageRatio = Ratio;
                return this;
            }

            public PoisonedAffect SetIntervalTime(float Interval) {
                IntervalTime = Interval;
                timer.SetIntervalTime(IntervalTime);
                return this;
            }

            /*
            Updator로 해결해도 되지 않나?
            */
            public override async UniTask Modifiy()
            {
                await UniTask.Yield(PlayerLoopTiming.LastUpdate);
                timer
                    .AddOnStartCooldownEvent(PoisonedVisualOn)
                    .AddOnIntervalEvent(PoisonedDamage)
                    .AddOnFinishedEvent(PoisonedVisualOff)
                    .AddOnFinishedEvent(OnAffectEndHandler);
                GlobalTimeUpdator.CheckAndAdd(this);
            }

            public override async UniTask Revert()
            {
                await UniTask.Yield(PlayerLoopTiming.LastUpdate);
                timer.SetCoolDownInitialize();
                timer.ClearEvents();
                GlobalTimeUpdator.CheckAndRemove(this);
            }

            public void PoisonedDamage() => targetEntity.GetDamaged((int)TickDamage);
            
            public void PoisonedVisualOn() {
            }
            public void PoisonedVisualOff() {
            }

            public void OnAffectEndHandler() {
                timer.SetCoolDownInitialize();
                timer.ClearEvents();
                GlobalTimeUpdator.CheckAndRemove(this);
            }

    #region Helper
            public float CalcTickDamage() {
                return TickDamageRatio * this.ownerEntity.StatReferer.GetStat(E_NUMERIC_STAT_TYPE.Power);
            }
    #endregion

        }
    }

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
}