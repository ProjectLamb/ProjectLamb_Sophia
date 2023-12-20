using System.Net.Http.Headers;
using AYellowpaper.SerializedCollections.Editor;
using UniRx;
using UnityEditor.Searcher;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace Feature_NewData
{    /*********************************************************************************
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
        OnStart : 쿨타임 시작시
        Ticking : 틱당 실행할 함수
        OnFinished : 종료될때 함수
    *********************************************************************************/
    public class CoolTimeManager {

#region Members 
        
        private readonly float baseCoolTime;
        public float CurrentPassedTime {get; private set;}

        public int BaseStacksCount {get; private set;}
        private int mCurrentStacksCount;

        public int CurrentStacksCount {
            get { return mCurrentStacksCount; }
            private set {
                if(value > BaseStacksCount) {
                    mCurrentStacksCount = BaseStacksCount; return;
                }
                if(value < 0) {mCurrentStacksCount = 0; return;}
                mCurrentStacksCount = value;
            }
        }
        
        private const float baseAccelerationAmount = 1f;
        private float accelerationAmount = 1f; // Ratio;
        private readonly bool IsDebug = false;

        public bool IsBlocked {get; private set;}

        public CoolTimeManager(float baseCoolTime, int stackAmount, bool debugState) {
            this.CurrentPassedTime = this.baseCoolTime = baseCoolTime;
            this.CurrentStacksCount = this.BaseStacksCount = stackAmount;
            this.IsDebug = debugState;

            OnInitialized       ??= (this.IsDebug) ? () => {Debug.Log("Initialized");}                   :  () => {};
            OnFinished          ??= (this.IsDebug) ? () => {Debug.Log("Started");}                       :  () => {};
            OnUseAction         ??= (this.IsDebug) ? () => {Debug.Log($"Use ... {CurrentStacksCount}");} :  () => {};
            OnTicking           ??= (this.IsDebug) ? (float val) => {Debug.Log($"Ticking ... {val}");}   :  (float val) => {};
            OnStartCooldown     ??= (this.IsDebug) ? () => {Debug.Log("Finished");}                      :  () => {};
        }

        public CoolTimeManager(float baseCoolTime, int stackAmount) : this (baseCoolTime, stackAmount, false){}
        public CoolTimeManager(float baseCoolTime) : this (baseCoolTime, 1, false){}

#endregion

#region Gettter 
        
        public bool GetIsInitialized() {
            if(CurrentPassedTime >= baseCoolTime - 0.001f && BaseStacksCount == CurrentStacksCount) {return true;}
            return false;
        }

        public bool GetIsReadyToUse() { return (IsBlocked == true || CurrentStacksCount <= 0) ? false : true; }

        public float GetProgressAmount() { return CurrentPassedTime / baseCoolTime; }

        
#endregion
             
#region Setter

        public CoolTimeManager SetMaxStackCounts(int counts) {
            BaseStacksCount = counts;
            CurrentStacksCount = counts;
            return this;
        }

        public CoolTimeManager SetAcceleratrion(float amount) { 
            if(amount <= 0) {amount = 0;}
            accelerationAmount = amount; 
            return this;
        }
        
        public void AccelerateRemainByCurrentCoolTime(float dimRatio) {
            CurrentPassedTime += CurrentPassedTime * dimRatio;
            if(CurrentPassedTime >= baseCoolTime) {OnFinished.Invoke();}
        }

        public void AccelerateFixedCoolTime (ref float second) {
            CurrentPassedTime += second;
        }
        public void SetCooldownFixedTime(ref float value){
            if( baseCoolTime < value ) {CurrentPassedTime = baseCoolTime;}
            else if(value <= 0.001f) {CurrentPassedTime = 0.1f;}
            else {CurrentPassedTime = value;}
        }

        public void SetBlock() {IsBlocked = true;}

        public void SetRelease() {IsBlocked = false;}

#endregion

#region Event Adder

        private event UnityAction OnStartCooldown = null;
        public CoolTimeManager AddOnStartCooldownEvent(UnityAction action) {
            OnStartCooldown += action;
            return this;
        }

        private event UnityAction OnUseAction = null;
        public CoolTimeManager AddOnUseEvent(UnityAction action) {
            OnUseAction += action;
            return this;
        }
        
        private event UnityAction<float> OnTicking = null;
        public CoolTimeManager AddOnTickingEvent(UnityAction<float> action) {
            OnTicking += action;
            return this;
        }
        private event UnityAction OnFinished = null;
        public CoolTimeManager AddOnFinishedEvent(UnityAction action) {
            OnFinished += action;
            return this;
        }

        private event UnityAction OnInitialized = null;
        public CoolTimeManager AddOnInitialized(UnityAction action) {
            OnInitialized += action;
            return this;
        }

        public void ClearEvents() {
            OnStartCooldown = null; 
            OnTicking = null; 
            OnFinished = null; 
                 
            OnInitialized       ??= (this.IsDebug) ? () => {Debug.Log("Initialized");}                   :  () => {};
            OnFinished          ??= (this.IsDebug) ? () => {Debug.Log("Started");}                       :  () => {};
            OnUseAction         ??= (this.IsDebug) ? () => {Debug.Log($"Use ... {CurrentStacksCount}");} :  () => {};
            OnTicking           ??= (this.IsDebug) ? (float val) => {Debug.Log($"Ticking ... {val}");}   :  (float val) => {};
            OnStartCooldown     ??= (this.IsDebug) ? () => {Debug.Log("Finished");}                      :  () => {};
        }

#endregion


        public void ActionStart(UnityAction action) {
            if(!GetIsReadyToUse()) return;
            action?.Invoke();
            StartCooldown();
        }

        public void DebugStart() {
            if(!IsDebug) throw new System.Exception("디버그 상태가 아님");

            if(!GetIsReadyToUse()) return;
            Debug.Log("Some Action Started");
            StartCooldown();
        }

        private void StartCooldown() {
            // 꽉 채워져 있는 상황인가?
            if(GetIsReadyToUse()) {
                if(GetIsInitialized()) {
                    CurrentPassedTime = 0.0f;
                    OnStartCooldown.Invoke();
                }
                CurrentStacksCount--;
                OnUseAction.Invoke();
            }
        }

        public void Tick() {
            if(CurrentStacksCount == BaseStacksCount) {return;}
            if(CurrentPassedTime < baseCoolTime -0.001f) {
                CurrentPassedTime += Time.deltaTime * accelerationAmount;
                OnTicking.Invoke(GetProgressAmount());
            }
            else {
                if(CurrentStacksCount++ < BaseStacksCount){
                    if(CurrentStacksCount == BaseStacksCount) {
                        CurrentPassedTime = baseCoolTime;
                        OnFinished.Invoke();
                        OnInitialized.Invoke();
                        return;
                    }
                    CurrentPassedTime = 0.0f;
                    OnFinished.Invoke();
                }
                else {return;}
            }
        }
    }
}