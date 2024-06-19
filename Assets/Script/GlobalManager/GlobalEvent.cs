using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sophia.UserInterface;
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
    public List<UnityAction> OnEnemyDieEvent;
    public List<UnityAction> OnEnemyHitEvent;
    public List<UnityAction<Stage>> OnStageClear;
    public List<UnityAction<Stage, Stage>> OnStageEnter;

    private void Awake()
    {
        OnEnemyDieEvent = new List<UnityAction>();
        OnEnemyHitEvent = new List<UnityAction>();
        OnStageClear = new List<UnityAction<Stage>>();
        OnStageEnter = new List<UnityAction<Stage, Stage>>();

        OnPlayEvent     ??= new UnityEvent<string>();
        OnPausedEvent   ??= new UnityEvent<string>();
        _IsGamePaused   ??= new SerializedDictionary<string, bool>
        {
            { gameObject.name, false }
        };
    }

    private void OnEnable() {

    }

    void Update()
    {
        Time.timeScale = GameTimeScale;
    }

    /////////////////////////////////////////////////////////////////////////////////

    #region TimeScaleEventHandler
    [Range(0, 1)]
    public float GameTimeScale = 1f;
    public UnityEvent<string> OnPlayEvent;
    public UnityEvent<string> OnPausedEvent;


    [SerializedDictionary("PauseCauseKey", "Flags")]
    private SerializedDictionary<string, bool> _IsGamePaused;
    public const float PLAY_SCALE = 1f;
    public const float PAUSE_SCALE = 0;

    public bool IsGamePaused {
        get {
            return !_IsGamePaused.All(x => x.Value == false); 
        }
    }

    public void SetTimeStateByHandlersString(string handler, bool timeState) {
        if(!_IsGamePaused.ContainsKey(handler)) {
            _IsGamePaused.TryAdd(handler, timeState); return;
        }
        _IsGamePaused[handler] = timeState;
    }

    public void Pause(string handler) {
        bool PrevTimeState = IsGamePaused;
        Debug.Log("TryPause");
        SetTimeStateByHandlersString(handler, true);
        if(PrevTimeState == false && IsGamePaused == true) {
            OnPausedEvent?.Invoke(gameObject.name);
            GameTimeScale = PAUSE_SCALE;
            Debug.Log("Time Changed");
        }
    }
    public void Play(string handler) {
        bool PrevTimeState = IsGamePaused;
        Debug.Log("TryPlay");
        SetTimeStateByHandlersString(handler, false);
        if(PrevTimeState == true && IsGamePaused == false) {
            OnPlayEvent?.Invoke(gameObject.name);
            GameTimeScale = PLAY_SCALE; 
            Debug.Log("Time Changed");
        }
    }

    public void ResetForce() {
        _IsGamePaused = new SerializedDictionary<string, bool>
        {
            { gameObject.name, false }
        };
        Time.timeScale = PLAY_SCALE;
        GameTimeScale = PLAY_SCALE;
        OnPlayEvent?.Invoke(gameObject.name);
    }

    public float TimeHoldingDuration;
    float mCurrentTimeScale = 1f;

    bool mIsSlowed = false;

    // public void HandleTimeSlow()
    // {
    //     if (mIsSlowed) return;
    //     Debug.Log("StartSlowed");

    //     StartCoroutine(SlowTimeCoroutine());
    // }

    // //DotTween 사용해서 증가 커브 설정하기
    // IEnumerator SlowTimeCoroutine()
    // {
    //     mIsSlowed = true;
    //     mCurrentTimeScale = 0.25f;
    //     float valueGap = 1f - mCurrentTimeScale;
    //     GameTimeScale = mCurrentTimeScale;
    //     float passedTime = 0f;
    //     while (TimeHoldingDuration > passedTime)
    //     {
    //         if (!IsGamePaused)
    //         {
    //             passedTime += (Time.deltaTime / TimeHoldingDuration);
    //             mCurrentTimeScale += valueGap * (Time.deltaTime / TimeHoldingDuration);
    //             GameTimeScale = mCurrentTimeScale;
    //         }
    //         yield return null;
    //     }
    //     mCurrentTimeScale = 1f;
    //     GameTimeScale = mCurrentTimeScale;
    //     mIsSlowed = false;
    // }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region MapEventHandler 
    public void PlayerMoveStage(GameObject departStage, GameObject arrvieStage, Vector3 warpPos)
    {
        if (arrvieStage.GetComponent<Stage>().Type == Stage.STAGE_TYPE.BOSS)
        {
            InGameScreenUI.Instance._videoController.StartVideo(VideoController.E_VIDEO_NAME.ElderOne);
        }

        OnStageEnter.ForEach(E => E.Invoke(departStage.GetComponent<Stage>(), arrvieStage.GetComponent<Stage>()));

        arrvieStage.GetComponent<Stage>().SetOnStage();
        GameManager.Instance.CurrentStage = arrvieStage;
        GameManager.Instance.ChapterGenerator.GetComponent<ChapterGenerator>().stage[arrvieStage.GetComponent<Stage>().StageNumber].Discovered = true;
        GameManager.Instance.PlayerGameObject.transform.position = warpPos;
        departStage.GetComponent<Stage>().SetOffStage();

        //UI 반영하는 코드
        GameObject minimapUI = GameObject.Find("Minimap");
        minimapUI.transform.GetChild(0).GetComponent<Minimap>().ChangeCurrentPosition(departStage.GetComponent<Stage>().StageNumber, arrvieStage.GetComponent<Stage>().StageNumber);
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////
}
