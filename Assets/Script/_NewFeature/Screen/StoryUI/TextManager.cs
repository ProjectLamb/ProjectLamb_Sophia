using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Sophia.UserInterface;

public class TextManager : MonoBehaviour
{

    [Header("StoryUI")]
    public GameObject talkPanel;
    public SpeakerImage speakerImage;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;
    public Animator storyImageAnimator;
    string[] dialogStrings;
    TalkData[] talkDatas;
    public string storyEventName;
    private int currentPage = 0; // 대화문 개수 변수
    public bool IsStory = true;
    public bool IsSkipStory;
    public bool IsGameStart;
    bool IsOnce = false;

    [Header("PlayerUI")]

    [SerializeField] public GameObject _playerHealthBar;
    [SerializeField] public GameObject _playerBarrierBar;
    [SerializeField] public GameObject _playerStaminaBar;
    [SerializeField] public GameObject _playerWealthBar;
    [SerializeField] public GameObject _playerSkillCool;
    [SerializeField] public GameObject _minimap;

    private static TextManager _instance;
    public static TextManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType(typeof(TextManager)) as TextManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Start()
    {
        //IsSkipStory = DontDestroyGameManager.Instance.SaveLoadManager.Data.CutSceneSaveData.IsSkipStory; // IsTutorial
        //if (!(IsSkipStory && !StoryManager.Instance.IsTutorial))
        if (StoryManager.Instance.IsTutorial)
        {
            InGameScreenUI.Instance._fadeUI.FadeIn(0.02f, 2f);
            InGameScreenUI.Instance._storyFadePanel.fadeStoryBarOn();
            IsStory = true;
            TextBarOn();
            storyEventName = "Prologue";
            SetDialogue();
        }
    }
    public void SetDialogue()
    {
        talkDatas = this.GetComponent<Dialogue>().GetObjectDialogue();
        TypingManager._instance.Typing(talkDatas[0].contexts, talkText);
        nameText.text = talkDatas[0].name;
        speakerImage.ChangeSprite(talkDatas[0].name, talkDatas[0].emotionState);
        currentPage++;
        storyImageAnimator.SetTrigger("DoChange");
    }

    // State Pattern으로 변경하기.
    // 코루틴으로 바꾸는것이 좋아 보인다.

    private void Update()
    {
        //if(IsSkipStory && !StoryManager.Instance.IsTutorial) {return;}
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) ) && IsStory)
        {
            TypingManager._instance.GetInputDown();
            if (TypingManager._instance.isTypingEnd)
            {
                if (currentPage == talkDatas.Length && TypingManager._instance.isDialogEnd)
                {
                    currentPage = talkDatas.Length;
                    if (!IsOnce)
                    {
                        //TextBarOff();
                        //1챕 튜토리얼 한정
                        InGameScreenUI.Instance._fadeUI.FadeOut(0.02f, 1.5f);
                        InGameScreenUI.Instance._fadeUI.AddBindingAction(() => { InGameScreenUI.Instance._videoController.StartVideo(VideoController.E_VIDEO_NAME.Opening); });
                        IsStory = false;
                        IsOnce = true;
                        //DontDestroyGameManager.Instance.SaveLoadManager.Data.CutSceneSaveData.IsSkipStory = true;
                    }
                    IsStory = false;
                }

                if (nameText.text != talkDatas[currentPage].name) // 스토리 진행 중 화자 변경 시 이미지 변경
                {
                    storyImageAnimator.SetTrigger("DoChange");
                }
                speakerImage.ChangeSprite(talkDatas[currentPage].name, talkDatas[currentPage].emotionState);
                nameText.text = talkDatas[currentPage].name;
                TypingManager._instance.Typing(talkDatas[currentPage].contexts, talkText);
                currentPage++;
            }
        }

        // 
        if (GameManager.Instance.GlobalEvent.IsGamePaused)
        {
            talkPanel.SetActive(false);
        }

        else if (!IsStory) // 튜토리얼이 끝났다면
        {
            TextBarOff();
            nameText.text = "";
            currentPage = 0;
        }
        else if (IsStory && !GameManager.Instance.GlobalEvent.IsGamePaused)
        {
            TextBarOn();
            //_dissolvePanel.SetActive(false);
        }
    }

    private void TextBarOff()
    {
        talkPanel.SetActive(false);
        _playerHealthBar.SetActive(true);
        _playerBarrierBar.SetActive(true);
        _playerStaminaBar.SetActive(true);
        _playerWealthBar.SetActive(true);
        _playerSkillCool.SetActive(true);
        _minimap.SetActive(true);
    }

    private void TextBarOn()
    {
        talkPanel.SetActive(true);
        _playerHealthBar.SetActive(false);
        _playerBarrierBar.SetActive(false);
        _playerStaminaBar.SetActive(false);
        _playerWealthBar.SetActive(false);
        _playerSkillCool.SetActive(false);
        _minimap.SetActive(false);
    }
}