using UnityEngine;
using UnityEngine.Events;
using Feature_State;
using System;

namespace Feature_Composite
{
    public enum E_TIMER_STATE {
        Initialized, Start, Timer, End, Terminate
    }
    public class TimerInitialize : IState<TimerComposite>
    {
        private static TimerInitialize _instance = new TimerInitialize();
        public static TimerInitialize Instance = _instance;

        public void Enter(TimerComposite owner) 
        {
            owner.StateType = E_TIMER_STATE.Initialized;
            owner.PassedTime = owner.BaseTime;
            owner.NextInterval = 0f;
            owner.OnInitialized?.Invoke();
        }

        public void Execute(TimerComposite owner)
        {
        }

        public void Exit(TimerComposite owner) { 
            owner.PassedTime = 0;
        }
    }

    public class TimerStart : IState<TimerComposite>
    {
        private static TimerStart _instance = new TimerStart();
        public static TimerStart Instance = _instance;

        public void Enter(TimerComposite owner)
        {
            owner.StateType = E_TIMER_STATE.Start;
            owner.OnStart?.Invoke();
        }

        public void Execute(TimerComposite owner)
        {
            owner.ChangeState(TimerRunning.Instance);
            return;
        }

        public void Exit(TimerComposite owner) { return; }
    }

    public class TimerRunning : IState<TimerComposite>
    {
        private static TimerRunning _instance = new TimerRunning();
        public static TimerRunning Instance = _instance;

        public void Enter(TimerComposite owner) {
            owner.StateType = E_TIMER_STATE.Timer;
        }

        public void Execute(TimerComposite owner)
        {
            if (owner.PassedTime >= owner.BaseTime) { owner.ChangeState(TimerEnd.Instance); return;} // ChangeState(TimerEnd)
            if (owner.IntervalTime > 0.01f && owner.PassedTime >= owner.NextInterval)
            {
                owner.NextInterval += owner.IntervalTime;
                owner.OnInterval?.Invoke();
            }
            owner.OnTicking.Invoke(owner.GetProgressAmount());
            owner.PassedTime += owner.IsBlocked ? 0f : Time.deltaTime * owner.accelerationAmount;
        }

        public void Exit(TimerComposite owner) { return; }
    }

    public class TimerEnd : IState<TimerComposite>
    {
        private static TimerEnd _instance = new TimerEnd();
        public static TimerEnd Instance = _instance;

        public void Enter(TimerComposite owner) {
            owner.PassedTime = owner.BaseTime;
            owner.StateType = E_TIMER_STATE.End;
            owner.OnFinished?.Invoke();
            if(owner.WhenLoopable()) {
                owner.PassedTime = 0;
                owner.NextInterval = 0f;
                owner.ChangeState(TimerStart.Instance);
            }
            else {
                owner.ChangeState(TimerInitialize.Instance);
            }
        }

        public void Execute(TimerComposite owner)
        {
        }

        public void Exit(TimerComposite owner) { return; }
    }

    public class TimerExit : IState<TimerComposite>
    {
        private static TimerExit _instance = new TimerExit();
        public static TimerExit Instance = _instance;
        public void Enter(TimerComposite owner)
        {
            owner.StateType = E_TIMER_STATE.Terminate;
        }

        public void Execute(TimerComposite owner) {}

        public void Exit(TimerComposite owner) {}
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
        public Func<bool>  WhenLoopable = null;

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
    }

    public class StackCounterComposite
    {

        #region Members
        public int BaseStacksCount { get; private set; }
        
        private int mCurrentStacksCount;

        public int CurrentStacksCount
        {
            get { return mCurrentStacksCount; }
            internal set
            {
                if (value >= BaseStacksCount)
                {
                    mCurrentStacksCount = BaseStacksCount; return;
                }
                if (value < 0) { mCurrentStacksCount = 0; return; }
                mCurrentStacksCount = value;
            }
        }

        public StackCounterComposite(int stackAmount)
        {
            this.CurrentStacksCount = this.BaseStacksCount = stackAmount;
            OnUseAction ??= ()=> {};
        }

        #endregion

        #region Event

        public event UnityAction OnUseAction = null;

        public void ClearEvents() {
            OnUseAction = null;
            OnUseAction = ()=> {};
        }

        #endregion

        #region Setter

        public StackCounterComposite SetMaxStackCounts(int counts)
        {
            BaseStacksCount = counts;
            CurrentStacksCount = counts;
            return this;
        }

        #endregion

        public bool GetIsReadyToUse() { return (CurrentStacksCount <= 0) ? false : true; }
        public void ResetStacks() => CurrentStacksCount = BaseStacksCount;

        public void UseStack()
        {
            CurrentStacksCount--;
            OnUseAction.Invoke();
        }
    }

    public class CoolTimeComposite
    {
        public TimerComposite timer { get; private set; }
        public StackCounterComposite stackCounter { get; private set; }

        public CoolTimeComposite(float baseTime, float intervalTime, int stackAmount)
        {
            timer = new TimerComposite(baseTime, intervalTime);
            stackCounter = new StackCounterComposite(stackAmount);
            timer.OnFinished += RestoreStack;
            timer.WhenLoopable += GetIsELoopable;
        }

        public CoolTimeComposite(float baseTime, int stackAmount) : this(baseTime, -1, stackAmount) { }

#region Getter
        
        public float GetProgressAmount() => timer.GetProgressAmount();
        public bool GetIsReadyToUse() => stackCounter.GetIsReadyToUse();
        public bool GetIsELoopable() => stackCounter.CurrentStacksCount < stackCounter.BaseStacksCount;

#endregion

#region Setter 
        public void SetBlock() => timer.Pause();
        public void SetRelease() => timer.Continue();
        

        public CoolTimeComposite SetMaxStackCounts(int counts) {
            stackCounter.SetMaxStackCounts(counts);
            stackCounter.CurrentStacksCount = counts;
            return this;
        }

        public CoolTimeComposite SetAcceleratrion(float amount) { 
            if(amount <= 0) {amount = 0;}
            timer.SetAcceleratrion(amount);
            return this;
        }
        
        public void AccelerateRemainByCurrentCoolTime(float dimRatio) {
            timer.AccelerateRemainByCurrentCoolTime(dimRatio);
        }

        public void AccelerateFixedCoolTime (ref float second) {
            timer.AccelerateFixedCoolTime(ref second);
        }
        
        public void SetCooldownFixedTime(ref float value){
            timer.SetCooldownFixedTime(ref value);
        }

#endregion

#region Event
        public CoolTimeComposite AddOnInitialized(UnityAction action)
        {
            timer.OnInitialized += action;
            return this;
        }

        public CoolTimeComposite AddOnStartCooldownEvent(UnityAction action)
        {
            timer.OnStart += action;
            return this;
        }

        public CoolTimeComposite AddOnTickingEvent(UnityAction<float> action)
        {
            timer.OnTicking += action;
            return this;
        }
        
        public CoolTimeComposite AddOnIntervalEvent(UnityAction action)
        {
            timer.OnInterval += action;
            return this;
        }

        public CoolTimeComposite AddOnFinishedEvent(UnityAction action)
        {
            timer.OnFinished += action;
            return this;
        }

        public CoolTimeComposite AddOnUseEvent(UnityAction action)
        {
            stackCounter.OnUseAction += action;
            return this;
        }


        public void ClearEvents() {
            timer.ClearEvents();              
            stackCounter.ClearEvents();
        }

#endregion

        public void ActionStart(UnityAction action) {
            if(!GetIsReadyToUse()) {return;}
            action?.Invoke();
            timer.SetStart();
            timer.Execute();
            stackCounter.UseStack();
        }

        public void TickRunning() {
            timer.Execute();
        }

        public void RestoreStack() {
            if(stackCounter.CurrentStacksCount < stackCounter.BaseStacksCount) {
                stackCounter.CurrentStacksCount++;
            }
        }
    }
}