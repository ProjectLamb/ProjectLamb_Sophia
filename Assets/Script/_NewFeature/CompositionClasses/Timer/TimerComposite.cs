using UnityEngine;
using UnityEngine.Events;
using Feature_State;
using System;

namespace Sophia.Composite.Timer
{
    public class TimerComposite
    {
        #region Members
        public readonly float BaseTime;
        private float mPassedTime;
        public float PassedTime
        {
            get { return mPassedTime; }
            internal set
            {
                if (value <= 0) { mPassedTime = 0; return; }
                if (value >= BaseTime) { mPassedTime = BaseTime; return; }
                mPassedTime = value;
            }
        }

        public float IntervalTime {get; private set;}
        internal float NextInterval;

        public const float baseAccelerationAmount = 1f;
        internal float accelerationAmount = 1f; // Ratio;

        public bool IsBlocked { get; internal set; }
        public bool IsLoop {get; internal set;}
        public E_TIMER_STATE StateType;

        public TimerComposite(float baseTime)
        {
            this.StateType = E_TIMER_STATE.Initialized;
            this.currentState = TimerInitialize.Instance;
            this.PassedTime = BaseTime = baseTime;
            this.IntervalTime = Time.deltaTime;

            this.OnInitialized ??= () => { };
            this.OnStart ??= () => { };
            this.OnInterval ??= () => { };
            this.OnTicking ??= (float val) => { };
            this.OnFinished ??= () => { };
        }

        #endregion

        #region Events

        public event UnityAction OnInitialized = null;
        public event UnityAction OnStart = null;
        public event UnityAction OnInterval = null;
        public event UnityAction<float> OnTicking = null;
        public event UnityAction OnFinished = null;
        private event Func<bool> WhenRewindable = () => false;
        public TimerComposite SetRewindCondition(Func<bool> condition){
            WhenRewindable = condition;
            return this;
        }
        public void ClearRewindCondition() {
            WhenRewindable = null;
        }

        internal void InvoekInitializedAction() => OnInitialized?.Invoke();
        internal void InvokeStartAction() => OnStart?.Invoke();
        internal void InvokeIntervalAction() => OnInterval?.Invoke();
        internal void InvokeTickingAction(float input) => OnTicking?.Invoke(input);
        internal void InvokeFinishedAction() => OnFinished?.Invoke();

        public void ClearEvents()
        {
            OnInitialized = null;
            OnStart = null;
            OnInterval = null;
            OnTicking = null;
            OnFinished = null;
            WhenRewindable = null;

            OnInitialized = () => { };
            OnStart = () => { };
            OnInterval = () => { };
            OnTicking = (float val) => { };
            OnFinished = () => { };
        }

        #endregion

        #region Getter

        public float GetProgressAmount() { return PassedTime / BaseTime; }
        public bool  GetIsRewindable() {return WhenRewindable.Invoke();}

        #endregion

        #region Setter

        public TimerComposite SetAcceleratrion(float amount)
        {
            if (amount <= 0) { amount = 0; }
            accelerationAmount = amount;
            return this;
        }

        public TimerComposite SetIntervalTime(float interval)
        {
            this.IntervalTime = interval;
            return this;
        }

        public void AccelerateRemainByCurrentCoolTime(float dimRatio)
        {
            PassedTime += PassedTime * dimRatio;
        }

        public void AccelerateFixedCoolTime(ref float second)
        {
            PassedTime += second;
        }

        public void SetCooldownFixedTime(ref float value)
        {
            if (BaseTime < value) { PassedTime = BaseTime; }
            else if (value <= 0.01f) { PassedTime = 0.1f; }
            else { PassedTime = value; }
        }

        #endregion

        #region State

        private IState<TimerComposite> currentState = null;

        public void ChangeState(IState<TimerComposite> newState) {
            if(newState == null) return;
            if(currentState != null) {
                currentState.Exit(this);
            }
            currentState = newState;
            currentState.Enter(this);
        }

        public void Execute() => currentState?.Execute(this);

        public void SetInitialize() => ChangeState(TimerInitialize.Instance);
        public void SetStart() => ChangeState(TimerStart.Instance);
        public void Pause() => IsBlocked = true;
        public void Continue() => IsBlocked = false;

        #endregion
    }
}

/*
timer.Pause()
timer.Resume()
timer.GetTimeRemaining()
timer.GetRatioComplete()
timer.isDone
*/