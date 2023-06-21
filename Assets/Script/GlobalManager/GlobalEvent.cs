using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Ref, Out에 대한 이해가 필요하다
/// > 함수 매개변수를 Call by Reference 하기 위해서 사용한다. <br/>
/// > Call By Value매개변수는 함수값 VS 함수 호출자 위치값 별개의 값이 된다. <br/>
/// > 따라서 함수에서 변경된값을 외부 호출자 위치에도 적용 시키려면 Ref라는 문법을 써야한다  <br/>
/// </summary>
/// <param name="input"></param>
/// <typeparam name="T"></typeparam>
public delegate void UnityActionRef<T>(ref T input);

public class GlobalEvent : MonoBehaviour
{

    public UnityEvent OnHitEvents;
    public UnityEvent PausedEvent;

    public List<UnityAction> OnEnemyDieEvent;
    public List<UnityAction> OnEnemyHitEvent;

    private void Awake()
    {
        OnEnemyDieEvent = new List<UnityAction>();
        OnEnemyHitEvent = new List<UnityAction>();
    }

    ///////////////////////////////////////////////////////////////////
    [Range(0, 1)]
    public float CurrentTimeScale = 1f;
    float ContinouseTimeScale = 1f;
    bool mIsGamePaused = false;
    public bool IsGamePaused
    {
        get
        {
            return mIsGamePaused;
        }
        set
        {
            if (value == true) { CurrentTimeScale = 0; }
            else { CurrentTimeScale = ContinouseTimeScale; }
            mIsGamePaused = value;
            Debug.Log("Time Changed");
        }
    }

    ///////////////////////////////////////////////////////////////////

    IEnumerator mCoSlowedTime;
    public float mDurateTime;
    bool mIsSlowed = false;

    public void HandleTimeSlow()
    {
        if (mIsSlowed) return;
        Debug.Log("StartSlowed");
        mCoSlowedTime = SlowTimeCoroutine();
        StartCoroutine(mCoSlowedTime);
    }

    public void PlayerMoveStage(GameObject departStage, GameObject arrvieStage, Vector3 warpPos)
    {
        arrvieStage.GetComponent<StageGenerator>().SetOnStage();
        GameManager.Instance.currentStage = arrvieStage;
        GameManager.Instance.ChapterGenerator.GetComponent<ChapterGenerator>().stage[arrvieStage.GetComponent<StageGenerator>().StageNumber].Discovered = true;
        GameManager.Instance.playerGameObject.transform.position = warpPos;
        departStage.GetComponent<StageGenerator>().SetOffStage();

        //UI 반영하는 코드
        GameObject minimapUI = GameObject.Find("Minimap");
        minimapUI.transform.GetChild(0).GetComponent<Minimap>().ChangeCurrentPosition(departStage.GetComponent<StageGenerator>().StageNumber, arrvieStage.GetComponent<StageGenerator>().StageNumber);
        //
    }

    //DotTween 사용해서 증가 커브 설정하기
    IEnumerator SlowTimeCoroutine()
    {
        mIsSlowed = true;
        ContinouseTimeScale = 0.25f;
        float valueGap = 1f - ContinouseTimeScale;
        CurrentTimeScale = ContinouseTimeScale;
        float passedTime = 0f;
        while (mDurateTime > passedTime)
        {
            if (!IsGamePaused)
            {
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
    /////////////////////////////////////////////////////////////////
}