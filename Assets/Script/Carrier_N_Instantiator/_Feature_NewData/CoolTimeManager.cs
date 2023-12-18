using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace Feature_NewData
{
    public class CoolTimeManager {
        private readonly float baseCoolTime;
        public float CurrentPassedTime {get; private set;}

        private int BaseStacksCount;
        public int CurrentStacksCount {get; private set;}
        
        private float AccelerationAmount; // Ratio;

        public CoolTimeManager(float baseCoolTime) {
            this.CurrentPassedTime = this.baseCoolTime = baseCoolTime;
            this.CurrentStacksCount = this.BaseStacksCount = 1;
        }

        public CoolTimeManager(float baseCoolTime, int stackAmount) {
            this.CurrentPassedTime = this.baseCoolTime = baseCoolTime;
            this.CurrentStacksCount = this.BaseStacksCount = stackAmount;
        }

        #region Gettter 
        public bool GetIsCoolingDown() {
            return CurrentStacksCount < BaseStacksCount ? true : false;
        }
        public bool GetIsReadyToUse() {
            return CurrentStacksCount <= 0 ? false : true;
        }
        #endregion
        
        #region Setter

        public void SetAcceleratrion(float amount) { AccelerationAmount = amount; }
        
        public void AccelerateRemainByCurrentCoolTime(float dimRatio) {
            CurrentPassedTime += CurrentPassedTime * dimRatio;
        }

        public void AccelerateFixedCoolTime (float second) {
            CurrentPassedTime += second;
        }

        #endregion

        UnityAction actionRef;

        public void ActionStart(UnityAction action) {
            if(!GetIsReadyToUse()) return;
            if(actionRef == null) {
                action.Invoke();
                actionRef = action; 
                StartCooldown();
            }
        }

        public void StartCooldown() {

            if(CurrentPassedTime >= baseCoolTime - 0.001f && CurrentStacksCount == BaseStacksCount) {
                CurrentStacksCount--;
                CurrentPassedTime = 0.0f;
            }
            else if (0 <= CurrentStacksCount) {
                CurrentStacksCount--;
            }
        }

        public void Tick() {
            if(CurrentPassedTime < baseCoolTime) {
                CurrentPassedTime += Time.deltaTime * AccelerationAmount;
                return;
            }
            if(CurrentStacksCount < BaseStacksCount) {
                if(++CurrentStacksCount == BaseStacksCount) {FinishedCooldown(); return;}
                CurrentPassedTime = 0.0f;
            }
        }

        public void FinishedCooldown() {
            CurrentPassedTime = baseCoolTime;
            actionRef = null;
        }
    }
}