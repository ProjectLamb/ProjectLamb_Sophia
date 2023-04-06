using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEvent : MonoBehaviour{
    public Action OnHitEvents;
    public Action PausedEvent;

    void Awake() {
        OnHitEvents = new Action(() => Debug.Log("액션 시작"));
        OnHitEvents += HandleTimeSlow;
    }

    ///////////////////////////////////////////////////////////////////
    [Range(0,1)]
    public float CurrentTimeScale = 1f;
    float ContinouseTimeScale = 1f;
    bool mIsGamePaused = false;
    public bool IsGamePaused {
        get {
            return mIsGamePaused; 
        }
        set {
            if(value == true){CurrentTimeScale = 0;}
            else{CurrentTimeScale = ContinouseTimeScale;}
            mIsGamePaused = value;
            Debug.Log("Time Changed");
        }
    }

    ///////////////////////////////////////////////////////////////////
    
    IEnumerator mCoSlowedTime;
    public float mDurateTime;
    bool mIsSlowed = false;

    public void HandleTimeSlow(){
        if(mIsSlowed) return;
        Debug.Log("StartSlowed");
        mCoSlowedTime = SlowTimeCoroutine();
        StartCoroutine(mCoSlowedTime);
    }

    //DotTween 사용해서 증가 커브 설정하기
    IEnumerator SlowTimeCoroutine(){
        mIsSlowed = true;
        ContinouseTimeScale = 0.25f; 
        float valueGap = 1f - ContinouseTimeScale;
        CurrentTimeScale = ContinouseTimeScale;
        float passedTime = 0f;
        while(mDurateTime > passedTime){
            if(!IsGamePaused) {
                Debug.Log(CurrentTimeScale);
                passedTime += (Time.deltaTime / mDurateTime);
                ContinouseTimeScale += valueGap * (Time.deltaTime / mDurateTime);
                CurrentTimeScale = ContinouseTimeScale;
            }
            yield return null;
        }
        ContinouseTimeScale = 1f;
        CurrentTimeScale = ContinouseTimeScale;
        mIsSlowed = false;
    }
}