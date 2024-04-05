using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Composite.Stacks
{
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
            OnUseAction ??= () => { };
        }

        #endregion

        #region Event

        public event UnityAction OnUseAction = null;
        public event UnityAction OnRecoverAction = null;
        public event UnityAction OnResetAction = null;

        public void ClearEvents()
        {
            OnUseAction         = null;
            OnRecoverAction     = null;
            OnResetAction       = null;

            OnUseAction         = () => {};
            OnRecoverAction     = () => {};
            OnResetAction       = () => {};
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

        public bool GetIsReadyToUse() => (CurrentStacksCount == 0) ? false : true;
        public bool GetIsUsedOnce() => (CurrentStacksCount < BaseStacksCount) ? true : false;
        
        public void UseStack()
        {
            if (GetIsReadyToUse()){
                CurrentStacksCount--;
                OnUseAction?.Invoke();
            }
        }
        public void RecoverStack()
        {
            if (GetIsUsedOnce())
            {
                CurrentStacksCount++;
                OnRecoverAction?.Invoke();
            }
        }
        public void ResetStacks() {
            CurrentStacksCount = BaseStacksCount;
            OnResetAction?.Invoke();
        }
    }
}