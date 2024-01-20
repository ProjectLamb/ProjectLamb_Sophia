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

        public readonly float IntervalTime;
        internal float NextInterval;

        public const float baseAccelerationAmount = 1f;
        internal float accelerationAmount = 1f; // Ratio;

        public bool IsBlocked { get; internal set; }
        public bool IsLoop {get; internal set;}
        public E_TIMER_STATE StateType;

        public TimerComposite(float baseTime, float intervalTime)
        {
            StateType = E_TIMER_STATE.Initialized;
            currentState = TimerInitialize.Instance;
            PassedTime = BaseTime = baseTime;
            IntervalTime = intervalTime;

            OnInitialized ??= () => { };
            OnStart ??= () => { };
            OnInterval ??= () => { };
            OnTicking ??= (float val) => { };
            OnFinished ??= () => { };
        }

        #endregion

        #region Events

        public UnityAction OnInitialized = null;
        public UnityAction OnStart = null;
        public UnityAction OnInterval = null;
        public UnityAction<float> OnTicking = null;
        public UnityAction OnFinished = null;
        public Func<bool>  WhenRewindable = null;

        public void ClearEvents()
        {
            OnInitialized = null;
            OnStart = null;
            OnInterval = null;
            OnTicking = null;
            OnFinished = null;

            OnInitialized = () => { };
            OnStart = () => { };
            OnInterval = () => { };
            OnTicking = (float val) => { };
            OnFinished = () => { };
        }

        #endregion

        #region Getter

        public float GetProgressAmount() { return PassedTime / BaseTime; }

        #endregion

        #region Setter

        public TimerComposite SetAcceleratrion(float amount)
        {
            if (amount <= 0) { amount = 0; }
            accelerationAmount = amount;
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