using System;

namespace Sophia.Composite
{
    namespace NewTimer
    {
        public interface ITimer<Receiver> {
            public TimerComposite GetTimer();
            public void Enter(Receiver receiver);
            public void Run(Receiver receiver);
            public void Exit(Receiver receiver);

        }

        public class TimerComposite
        {

#region Member

            public readonly float BaseTime;
            public float AccelerationAmount = 1f; // Ratio;

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
            public bool IsBlocked { get; internal set; }

            public TimerComposite(float baseTime)
            {
                this.BaseTime = baseTime;
                PassedTime = 0;
                IsBlocked = false;
            }
            
            public TimerComposite SetAcceleratrion(float amount)
            {
                if (amount <= 0) { amount = 0; }
                AccelerationAmount = amount;
                return this;
            }
#endregion

#region Interval

            public IntervalTimerComposite intervalTimer;
            public TimerComposite SetInterval(float interval)
            {
                intervalTimer = new IntervalTimerComposite(interval);
                return this;
            }
            public bool GetIsActivateInterval() => intervalTimer == null ? false : intervalTimer.GetIsActivateInterval(PassedTime);

#endregion

#region Rewind

            public RewaindTimerComposite rewaindTimer;
            public TimerComposite SetRewaind(Func<bool> condition)
            {
                rewaindTimer = new RewaindTimerComposite(condition);
                return this;
            }

            public bool GetIsRewainable() => rewaindTimer == null ? false : rewaindTimer.GetIsRewainable();

#endregion

            public void FrameTick(float passedTick) {PassedTime += passedTick;}
            public float GetProgressAmount() { return PassedTime / BaseTime; }
            public bool GetIsTimesUp() {return PassedTime >= BaseTime;}
            public void Puase() => IsBlocked = true;
            public void Continue() => IsBlocked = false;

            public void ResetTimer() {
                PassedTime = 0;
                IsBlocked = false;
                intervalTimer?.ResetNextInterval();
            }
        }

        public class IntervalTimerComposite
        {
            public float IntervalTime { get; private set; }
            internal float NextInterval;

            public IntervalTimerComposite(float intervel)
            {
                IntervalTime = intervel;
                NextInterval = IntervalTime;
            }
            public bool GetIsActivateInterval(float PassedTime)
            {
                if (PassedTime >= 0f && PassedTime >= NextInterval)
                {
                    NextInterval += IntervalTime;
                    return true;
                }
                return false;
            }
            public void ResetNextInterval() {
                NextInterval = IntervalTime;
            }
        }

        public class RewaindTimerComposite
        {
            public bool IsLoop { get; internal set; }
            public Func<bool> WhenRewindable;
            public RewaindTimerComposite(Func<bool> condition)
            {
                WhenRewindable = condition;
            }
            public bool GetIsRewainable() =>  WhenRewindable.Invoke();
            public void ClearRewindCondition() => WhenRewindable = null;
        }
    }
}