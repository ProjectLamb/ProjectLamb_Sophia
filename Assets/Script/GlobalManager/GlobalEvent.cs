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

public class GlobalEvent : MonoBehaviour
{
    public List<UnityAction>                OnEnemyDieEvent;
    public List<UnityAction>                OnEnemyHitEvent;
    public List<UnityAction<Stage>>         OnStageClear;
    public List<UnityAction<Stage, Stage>>  OnStageEnter;

    private void Awake()
    {
        OnEnemyDieEvent = new List<UnityAction>();
        OnEnemyHitEvent = new List<UnityAction>();
        OnStageClear    = new List<UnityAction<Stage>>();
        OnStageEnter    = new List<UnityAction<Stage, Stage>>();
    }
    
    void Update()
    {
        Time.timeScale = GameTimeScale;
    }

    /////////////////////////////////////////////////////////////////////////////////

#region TimeScaleEventHandler
    [Range(0, 1)]
    public UnityEvent       PausedEvent;
    public float            GameTimeScale = 1f;
    public float            TimeHoldingDuration;
    float                   mCurrentTimeScale = 1f;
    bool                    mIsGamePaused = false;
    bool                    mIsSlowed = false;

    public bool IsGamePaused
    {
        get
        {
            return mIsGamePaused;
        }
        set
        {
            if (value == true) { GameTimeScale = 0; }
            else { GameTimeScale = mCurrentTimeScale; }
            mIsGamePaused = value;
            Debug.Log("Time Changed");
        }
    }

    public void HandleTimeSlow()
    {
        if (mIsSlowed) return;
        Debug.Log("StartSlowed");

        StartCoroutine(SlowTimeCoroutine());
    }
    
    //DotTween 사용해서 증가 커브 설정하기
    IEnumerator SlowTimeCoroutine()
    {
        mIsSlowed = true;
        mCurrentTimeScale = 0.25f;
        float valueGap = 1f - mCurrentTimeScale;
        GameTimeScale = mCurrentTimeScale;
        float passedTime = 0f;
        while (TimeHoldingDuration > passedTime)
        {
            if (!IsGamePaused)
            {
                passedTime += (Time.deltaTime / TimeHoldingDuration);
                mCurrentTimeScale += valueGap * (Time.deltaTime / TimeHoldingDuration);
                GameTimeScale = mCurrentTimeScale;
            }
            yield return null;
        }
        mCurrentTimeScale = 1f;
        GameTimeScale = mCurrentTimeScale;
        mIsSlowed = false;
    }
#endregion

    /////////////////////////////////////////////////////////////////////////////////

#region MapEventHandler 
    public void PlayerMoveStage(GameObject departStage, GameObject arrvieStage, Vector3 warpPos)
    {
        OnStageEnter.ForEach(E => E.Invoke(departStage.GetComponent<Stage>(), arrvieStage.GetComponent<Stage>()));

        arrvieStage.GetComponent<Stage>().SetOnStage();
        GameManager.Instance.CurrentStage = arrvieStage;
        GameManager.Instance.ChapterGenerator.GetComponent<ChapterGenerator>().stage[arrvieStage.GetComponent<Stage>().StageNumber].Discovered = true;
        GameManager.Instance.PlayerGameObject.transform.position = warpPos;
        departStage.GetComponent<Stage>().SetOffStage();

        //UI 반영하는 코드
        GameObject minimapUI = GameObject.Find("Minimap");
        minimapUI.transform.GetChild(0).GetComponent<Minimap>().ChangeCurrentPosition(departStage.GetComponent<Stage>().StageNumber, arrvieStage.GetComponent<Stage>().StageNumber);
        
        //
    }
#endregion

    /////////////////////////////////////////////////////////////////////////////////
}
