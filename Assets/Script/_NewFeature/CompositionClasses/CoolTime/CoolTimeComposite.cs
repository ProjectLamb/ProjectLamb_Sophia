using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Sophia.Composite
{
    using Stacks;

    public class CoolTimeComposite
    {
#region Members
        
        public TimerComposite timer { get; private set; }        
        public StackCounterComposite stackCounter { get; private set; }
        public CoolTimeComposite(float baseTime, float intervalTime, int stackAmount)
        {
            timer = new TimerComposite(baseTime)
                        .SetIntervalTime(intervalTime)
                        .SetRewindCondition(GetIsRewind);
            stackCounter = new StackCounterComposite(stackAmount);
            
            timer.OnFinished += stackCounter.RecoverStack;
        }
        public CoolTimeComposite(float baseTime, int stackAmount) : this(baseTime, -1, stackAmount) { }

#endregion

#region Getter

        public float GetProgressAmount() => timer.GetProgressAmount();
        public bool GetIsReadyToUse() => stackCounter.GetIsReadyToUse();
        public bool GetIsRewind() => stackCounter.GetIsUsedOnce();

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

#endregion

#region Event
        public CoolTimeComposite AddBindingAction(UnityAction action) {
            stackCounter.OnUseAction += action;
            return this;
        }
        
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
        
        public CoolTimeComposite AddOnRecoverEvent(UnityAction action)
        {
            stackCounter.OnRecoverAction += action;
            return this;
        }

        public void ClearEvents()
        {
            timer.ClearEvents();
            stackCounter.ClearEvents();
        }

#endregion

        public void ActionStart()
        {
            if (!GetIsReadyToUse()) { return; }
            stackCounter.UseStack();
            timer.SetStart();
            timer.Execute();
        }

        public void TickRunning()
        {
            timer.Execute();
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

    }
}