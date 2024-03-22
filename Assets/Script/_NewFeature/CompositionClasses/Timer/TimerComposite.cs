using UnityEngine;
using UnityEngine.Events;
using System;

/*
AffectorManager 는 자체적으로

PassTime의 역할을 한다., UPdate를 돌면서 어느게 시간이 다 닳았는지 앟아야 하고, 그 어펙터를 Revert를 시켜야한다.

Affect는 지나고, 남은 시간을 알 수 있다, 그리고 자체적으로 연결된 지난 시간에 대한 이벤트를 실행한다.
Affect는 세가지 상태가 존재 한다
    1. Ready                    : 오직 Start로만 전이 가능하다.
    2. Start                    : 오직 시작하고, Running으로 바로 전환한다.
    3. Running                  : 달리는 중이고, 
                                    Interval 이벤트를 실행하고 
                                    Terminate로 상태 전이가 가능하다.
    4. Terminate가 존재한다.
                                : Terminate가 됨을 알린다.


AffectorManager는 어펙터가 Terminate라는것을 감지하여 뺴낼 준비를 한다.

*/

namespace Sophia.Composite
{
    using Sophia.State;
    
    public enum E_TIMER_STATE
    {
        Initialized, Start, Timer, End, Terminate
    }
    
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

        public float IntervalTime { get; private set; }
        internal float NextInterval;

        public const float baseAccelerationAmount = 1f;
        internal float accelerationAmount = 1f; // Ratio;

        public bool IsBlocked { get; internal set; }
        public bool IsLoop { get; internal set; }
        public E_TIMER_STATE StateType { get; internal set; }

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
        public TimerComposite SetRewindCondition(Func<bool> condition)
        {
            WhenRewindable = condition;
            return this;
        }
        public void ClearRewindCondition()
        {
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

        public E_TIMER_STATE GetCurrentState() => this.StateType;
        public float GetProgressAmount() { return PassedTime / BaseTime; }
        public bool GetIsRewindable() { return WhenRewindable.Invoke(); }

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

        public void ChangeState(IState<TimerComposite> newState)
        {
            if (newState == null) return;
            if (currentState != null)
            {
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

    public class TimerInitialize : IState<TimerComposite>
    {
        private static TimerInitialize _instance = new TimerInitialize();
        public static TimerInitialize Instance => _instance;

        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Initialized;
            timer.PassedTime = timer.BaseTime;
            timer.NextInterval = 0f;
            timer.InvoekInitializedAction();
        }

        public void Execute(TimerComposite timer) { return; }

        public void Exit(TimerComposite timer)
        {
            timer.PassedTime = 0;
            return;
        }
    }

    public class TimerStart : IState<TimerComposite>
    {
        private static TimerStart _instance = new TimerStart();
        public static TimerStart Instance => _instance;

        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Start;
            timer.InvokeStartAction();
            timer.ChangeState(TimerRunning.Instance);
        }

        public void Execute(TimerComposite timer) { return; }

        public void Exit(TimerComposite timer) { return; }
    }

    public class TimerRunning : IState<TimerComposite>
    {
        private static TimerRunning _instance = new TimerRunning();
        public static TimerRunning Instance => _instance;

        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Timer;
        }

        public void Execute(TimerComposite timer)
        {
            if (timer.PassedTime >= timer.BaseTime) { timer.ChangeState(TimerEnd.Instance); return; } // ChangeState(TimerEnd)
            if (timer.IntervalTime > 0.01f && timer.PassedTime >= timer.NextInterval)
            {
                timer.NextInterval += timer.IntervalTime * timer.accelerationAmount;
                timer.InvokeIntervalAction();
            }
            timer.InvokeTickingAction(timer.GetProgressAmount());
            timer.PassedTime += timer.IsBlocked ? 0f : Time.deltaTime * timer.accelerationAmount;
        }

        public void Exit(TimerComposite timer) { return; }
    }

    public class TimerEnd : IState<TimerComposite>
    {
        private static TimerEnd _instance = new TimerEnd();
        public static TimerEnd Instance => _instance;

        public void Enter(TimerComposite timer)
        {
            timer.PassedTime = timer.BaseTime;
            timer.StateType = E_TIMER_STATE.End;
            timer.InvokeFinishedAction();

            if (timer.GetIsRewindable())
            {
                timer.PassedTime = 0;
                timer.NextInterval = 0f;
                timer.ChangeState(TimerStart.Instance);
            }
            else
            {
                timer.ChangeState(TimerInitialize.Instance);
            }
        }

        public void Execute(TimerComposite timer) { return; }

        public void Exit(TimerComposite timer) { return; }
    }

    public class TimerExit : IState<TimerComposite>
    {
        private static TimerExit _instance = new TimerExit();
        public static TimerExit Instance => _instance;
        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Terminate;
        }

        public void Execute(TimerComposite timer) { return; }

        public void Exit(TimerComposite timer) { return; }
    }
}

/*
timer.Pause()
timer.Resume()
timer.GetTimeRemaining()
timer.GetRatioComplete()
timer.isDone
*/