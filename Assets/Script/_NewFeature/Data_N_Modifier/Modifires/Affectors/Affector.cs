using UnityEngine;

namespace Sophia.DataSystem
{
    using System;
    using System.Threading;
    using Sophia.Composite.Timer;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    using UnityEngine.Events;

    namespace Modifiers.Affector
    {

        /// <summary>
        /// Modifier이자, 일시적인 상태 변화를 야기하는 것.
        /// </summary>
        public abstract class Affector
        {
            #region Members
            public readonly CancellationTokenSource cts;
            public readonly E_AFFECT_TYPE AffectType;
            public readonly Entity TargetRef;
            public readonly Entity OwnerRef;
            public float BaseDurateTime { get; private set; }
            public TimerComposite Timer { get; private set; }
            public Affector(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers, float durateTime)
            {
                cts = new CancellationTokenSource();
                this.AffectType = affectType;

                this.OwnerRef = ownerReceivers;
                this.TargetRef = targetReceivers;
                this.BaseDurateTime = durateTime;

                this.Timer = new TimerComposite(BaseDurateTime);

                OnModifiy = (ref float a) => { };
                OnTickRunning = () => { };
                OnRevert = () => { };
            }
            #endregion

            #region Setter
            public Affector SetAccelarationByTenacity(float tenecity)
            {
                Timer.SetAcceleratrion(tenecity);
                return this;
            }
            #endregion

            #region Event 

            public event UnityActionRef<float> OnModifiy;
            public event UnityAction OnTickRunning;
            public event UnityAction OnRevert;
            public event UnityAction OnCancle;
            public void ClearEvent()
            {
                OnModifiy = null;
                OnTickRunning = null;
                OnRevert = null;
                OnCancle = null;

                OnModifiy = (ref float a) => {};
                OnTickRunning = () => {};
                OnRevert = () => {};
                OnCancle = () => {};
            }
            #endregion

            public void ConveyToTarget() => TargetRef.ModifiedByAffector(this);
            public void Modifiy(float tenacity) { 
                Timer.SetStart();
                Timer.Execute();
                OnModifiy?.Invoke(ref tenacity);
            }
            public void TickRunning() { 
                Timer.Execute();
                OnTickRunning?.Invoke(); 
            }
            public void Revert() { OnRevert?.Invoke(); }
            public void CancleModify()
            {
                Timer.Pause();
                Timer.ChangeState(TimerExit.Instance);
                Timer = null;
                OnCancle?.Invoke();
                ClearEvent();
            }
        }
    }
}