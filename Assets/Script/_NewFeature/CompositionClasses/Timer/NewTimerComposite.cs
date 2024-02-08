using System;

namespace Sophia.Composite
{
    namespace NewTimer
    {
        public class TimerComposite
        {
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
            public IntervalTimerComposite intervalTimer;
            public RewaindTimerComposite rewaindTimer;

            public TimerComposite(float baseTime)
            {
                this.BaseTime = baseTime;
            }

            public TimerComposite SetInterval(float interval)
            {
                intervalTimer = new IntervalTimerComposite(interval);
                return this;
            }

            public TimerComposite SetRewaind(Func<bool> condition)
            {
                rewaindTimer = new RewaindTimerComposite(condition);
                return this;
            }
            public TimerComposite SetAcceleratrion(float amount)
            {
                if (amount <= 0) { amount = 0; }
                AccelerationAmount = amount;
                return this;
            }

            public void FrameTick(float passedTick) => PassedTime += passedTick;
            public float GetProgressAmount() { return PassedTime / BaseTime; }
            public bool GetIsTimesUp() => PassedTime >= BaseTime;
            public bool GetIsActivateInterval() => intervalTimer.GetIsActivateInterval(PassedTime);
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
        }

        public class RewaindTimerComposite
        {
            public bool IsLoop { get; internal set; }
            public Func<bool> WhenRewindable;
            public RewaindTimerComposite(Func<bool> condition)
            {
                WhenRewindable = condition;
            }
            public bool GetIsRewainable() => WhenRewindable.Invoke();
            public void ClearRewindCondition() => WhenRewindable = null;
        }
    }
}