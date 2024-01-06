using UnityEngine;
using UnityEngine.Events;

namespace Feature_Composite
{
    public class CoolTimeComposite
    {
        #region Members
        
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

        #endregion

        #region Getter

        public float GetProgressAmount() => timer.GetProgressAmount();
        public bool GetIsReadyToUse() => stackCounter.GetIsReadyToUse();
        public bool GetIsELoopable() => stackCounter.CurrentStacksCount < stackCounter.BaseStacksCount;

        #endregion

        #region Setter 
        public void SetBlock() => timer.Pause();
        public void SetRelease() => timer.Continue();


        public CoolTimeComposite SetMaxStackCounts(int counts)
        {
            stackCounter.SetMaxStackCounts(counts);
            stackCounter.CurrentStacksCount = counts;
            return this;
        }

        public CoolTimeComposite SetAcceleratrion(float amount)
        {
            if (amount <= 0) { amount = 0; }
            timer.SetAcceleratrion(amount);
            return this;
        }

        public void AccelerateRemainByCurrentCoolTime(float dimRatio)
        {
            timer.AccelerateRemainByCurrentCoolTime(dimRatio);
        }

        public void AccelerateFixedCoolTime(ref float second)
        {
            timer.AccelerateFixedCoolTime(ref second);
        }

        public void SetCooldownFixedTime(ref float value)
        {
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


        public void ClearEvents()
        {
            timer.ClearEvents();
            stackCounter.ClearEvents();
        }

        #endregion

        public void ActionStart(UnityAction action)
        {
            if (!GetIsReadyToUse()) { return; }
            action?.Invoke();
            timer.SetStart();
            timer.Execute();
            stackCounter.UseStack();
        }

        public void TickRunning()
        {
            timer.Execute();
        }

        public void RestoreStack()
        {
            if (stackCounter.CurrentStacksCount < stackCounter.BaseStacksCount)
            {
                stackCounter.CurrentStacksCount++;
            }
        }
    }
}

namespace Feature_NewData
{
    /*********************************************************************************
    * 쿨타임 매니저
    * 멤버
        baseCoolTime
        CurrentPassedTime : Getable


        BaseStacksCount : / 최대 사용 가능 사이즈 (Staking)
        CurrentStacksCount : Getable / 현재 사용 가능 개수 

        accelerationAmount : /가속도

        IsDebug

    * Getter
        현재 쿨타임이 돌아가고 있는지
        현재 사용할 수 있는지
        현재 진생 상황

    * Setter : 체이닝 방식 가농.
        가속도
        현재 진행상황 비례 가속
        고정 시간 가속
        현재 진행상황 대입

    * EventSetter : 체이닝 방식 가능
        OnStartCooldown     : 쿨타임 시작시
        OnTicking           : 틱당 실행할 함수
        OnUseAction         : 사용할떄 작동하는 액션
        OnFinished          : 종료될때 함수
        OnInitialized       : 원 상태로 초기화 되었을떄
    *********************************************************************************/
    public class CoolTimeComposite
    {

        #region Members 

        private readonly float baseCoolTime;
        public float CurrentPassedTime { get; private set; }
        public float IntervalTime { get; private set; }

        public int BaseStacksCount { get; private set; }
        private int mCurrentStacksCount;

        public int CurrentStacksCount
        {
            get { return mCurrentStacksCount; }
            private set
            {
                if (value > BaseStacksCount)
                {
                    mCurrentStacksCount = BaseStacksCount; return;
                }
                if (value < 0) { mCurrentStacksCount = 0; return; }
                mCurrentStacksCount = value;
            }
        }

        private const float baseAccelerationAmount = 1f;
        private float accelerationAmount = 1f; // Ratio;

        private readonly bool IsDebug = false;

        public bool IsBlocked { get; private set; }

        public CoolTimeComposite(float baseCoolTime, int stackAmount, bool debugState)
        {
            this.CurrentPassedTime = this.baseCoolTime = baseCoolTime;
            this.CurrentStacksCount = this.BaseStacksCount = stackAmount;
            this.IsDebug = debugState;
            this.IntervalTime = -1f;

            OnInitialized ??= (this.IsDebug) ? () => { Debug.Log("Initialized"); } : () => { };
            OnFinished ??= (this.IsDebug) ? () => { Debug.Log("Started"); } : () => { };
            OnUseAction ??= (this.IsDebug) ? () => { Debug.Log($"Use ... {CurrentStacksCount}"); } : () => { };
            OnInterval ??= (this.IsDebug) ? () => { Debug.Log($"Interval"); } : () => { };
            OnTicking ??= (this.IsDebug) ? (float val) => { Debug.Log($"Ticking ... {val}"); } : (float val) => { };
            OnStartCooldown ??= (this.IsDebug) ? () => { Debug.Log("Finished"); } : () => { };
        }

        public CoolTimeComposite(float baseCoolTime, int stackAmount) : this(baseCoolTime, stackAmount, false) { }
        public CoolTimeComposite(float baseCoolTime) : this(baseCoolTime, 1, false) { }

        #endregion

        #region Gettter 

        public bool GetIsInitialized()
        {
            if (CurrentPassedTime >= baseCoolTime - 0.01f && BaseStacksCount == CurrentStacksCount) { return true; }
            return false;
        }

        public bool GetIsReadyToUse() { return (IsBlocked == true || CurrentStacksCount <= 0) ? false : true; }

        public float GetProgressAmount() { return CurrentPassedTime / baseCoolTime; }


        #endregion

        #region Setter

        public CoolTimeComposite SetMaxStackCounts(int counts)
        {
            BaseStacksCount = counts;
            CurrentStacksCount = counts;
            return this;
        }

        public CoolTimeComposite SetAcceleratrion(float amount)
        {
            if (amount <= 0) { amount = 0; }
            accelerationAmount = amount;
            return this;
        }

        public CoolTimeComposite SetIntervalTime(float amount)
        {
            if (amount <= 0) { amount = -1f; }
            IntervalTime = amount;
            return this;
        }

        public void AccelerateRemainByCurrentCoolTime(float dimRatio)
        {
            CurrentPassedTime += CurrentPassedTime * dimRatio;
            if (CurrentPassedTime >= baseCoolTime) { OnFinished.Invoke(); }
        }

        public void AccelerateFixedCoolTime(ref float second)
        {
            CurrentPassedTime += second;
        }

        public void SetCooldownFixedTime(ref float value)
        {
            if (baseCoolTime < value) { CurrentPassedTime = baseCoolTime; }
            else if (value <= 0.01f) { CurrentPassedTime = 0.1f; }
            else { CurrentPassedTime = value; }
        }

        public void SetBlock() { IsBlocked = true; }

        public void SetRelease() { IsBlocked = false; }

        public void SetCoolDownInitialize()
        {
            CurrentStacksCount = BaseStacksCount;
            CurrentPassedTime = baseCoolTime;
            OnInitialized.Invoke();
        }

        #endregion

        #region Event

        private event UnityAction OnStartCooldown = null;
        public CoolTimeComposite AddOnStartCooldownEvent(UnityAction action)
        {
            OnStartCooldown += action;
            return this;
        }

        private event UnityAction OnUseAction = null;
        public CoolTimeComposite AddOnUseEvent(UnityAction action)
        {
            OnUseAction += action;
            return this;
        }

        private event UnityAction<float> OnTicking = null;
        public CoolTimeComposite AddOnTickingEvent(UnityAction<float> action)
        {
            OnTicking += action;
            return this;
        }
        private event UnityAction OnInterval = null;
        public CoolTimeComposite AddOnIntervalEvent(UnityAction action)
        {
            OnInterval += action;
            return this;
        }

        private event UnityAction OnFinished = null;
        public CoolTimeComposite AddOnFinishedEvent(UnityAction action)
        {
            OnFinished += action;
            return this;
        }

        private event UnityAction OnInitialized = null;
        public CoolTimeComposite AddOnInitialized(UnityAction action)
        {
            OnInitialized += action;
            return this;
        }

        public void ClearEvents()
        {
            OnInitialized = null;
            OnFinished = null;
            OnUseAction = null;
            OnInterval = null;
            OnTicking = null;
            OnStartCooldown = null;

            OnInitialized ??= (this.IsDebug) ? () => { Debug.Log("Initialized"); } : () => { };
            OnFinished ??= (this.IsDebug) ? () => { Debug.Log("Started"); } : () => { };
            OnUseAction ??= (this.IsDebug) ? () => { Debug.Log($"Use ... {CurrentStacksCount}"); } : () => { };
            OnInterval ??= (this.IsDebug) ? () => { Debug.Log("Interval"); } : () => { };
            OnTicking ??= (this.IsDebug) ? (float val) => { Debug.Log($"Ticking ... {val}"); } : (float val) => { };
            OnStartCooldown ??= (this.IsDebug) ? () => { Debug.Log("Finished"); } : () => { };
        }

        #endregion

        public void ActionStart(UnityAction action)
        {
            if (!GetIsReadyToUse()) return;
            action?.Invoke();
            StartCooldown();
        }

        public void DebugStart()
        {
            if (!IsDebug) throw new System.Exception("디버그 상태가 아님");

            if (!GetIsReadyToUse()) return;
            Debug.Log("Some Action Started");
            StartCooldown();
        }

        private void StartCooldown()
        {
            // 꽉 채워져 있는 상황인가?
            if (GetIsReadyToUse())
            {
                if (GetIsInitialized())
                {
                    CurrentPassedTime = 0.0f;
                    OnStartCooldown.Invoke();
                }
                CurrentStacksCount--;
                OnUseAction.Invoke();
            }
        }

        private bool IntervalChecker(float passedTime)
        {
            if (IntervalTime <= 0f) { return false; }
            return passedTime % IntervalTime <= 0.01f;
        }

        public void TickRunning()
        {
            if (CurrentStacksCount == BaseStacksCount) { return; }
            if (CurrentPassedTime < baseCoolTime - 0.01f)
            {
                CurrentPassedTime += Time.deltaTime * accelerationAmount;
                OnTicking.Invoke(GetProgressAmount());
                if (IntervalChecker(CurrentPassedTime)) OnInterval.Invoke();
            }
            else
            {
                if (CurrentStacksCount++ < BaseStacksCount)
                {
                    if (CurrentStacksCount == BaseStacksCount)
                    {
                        CurrentPassedTime = baseCoolTime;
                        OnFinished.Invoke();
                        OnInitialized.Invoke();
                        return;
                    }
                    CurrentPassedTime = 0.0f;
                    OnFinished.Invoke();
                }
                else { return; }
            }
        }
    }
}