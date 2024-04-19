using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class TextManager : MonoBehaviour
{

    [Header("StoryUI")]
    public GameObject talkPanel;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;
    public Sprite decussSprite;
    public Sprite OffusiaSprite;
    public Image storyBarImage;
    public Animator storyImageAnimator;
    public string[] dialogStrings;
    TalkData[] talkDatas;
    private int currentPage = 0; // 대화문 개수 변수
    private bool IsStory = false; // 스토리 대화 진행중인지 여부

    [Header("PlayerUI")]
    [SerializeField] public GameObject _playerHealthBar;
    [SerializeField] public GameObject _playerBarrierBar;
    [SerializeField] public GameObject _playerStaminaBar;
    [SerializeField] public GameObject _playerWealthBar;
    [SerializeField] public GameObject _playerSkillCool;
    [SerializeField] public GameObject _dissolvePanel;


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
        IsStory = true;
        TextBarOn();
        talkDatas = this.GetComponent<Dialogue>().GetObjectDialogue();
        TypingManager._instance.Typing(talkDatas[0].contexts, talkText);
        nameText.text = talkDatas[0].name;
        currentPage++;
        storyImageAnimator.SetTrigger("DoChange");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {

            TypingManager._instance.GetInputDown();
            if (TypingManager._instance.isTypingEnd)
            {
                if(currentPage == talkDatas.Length && TypingManager._instance.isDialogEnd){
                    SceneManager.LoadScene("_01_Chapter_Tutorial");
                    IsStory = false;
                }

                if(nameText.text != talkDatas[currentPage].name) // 스토리 진행 중 화자 변경 시 이미지 변경
                { 
                    ChangeSprite();
                    storyImageAnimator.SetTrigger("DoChange");
                }

                nameText.text = talkDatas[currentPage].name;
                TypingManager._instance.Typing(talkDatas[currentPage].contexts, talkText);
                currentPage++;
            }
        }
        if(GameManager.Instance.GlobalEvent.IsGamePaused){
            talkPanel.SetActive(false);
        }

        else if(!GameManager.Instance.GlobalEvent.IsGamePaused && IsStory)
            talkPanel.SetActive(true);
        
        if(!StoryManager.Instance.IsTutorial) // 튜토리얼이 끝났다면
        {
            TextBarOff();
            _dissolvePanel.SetActive(true);

        }
        else if(StoryManager.Instance.IsTutorial && !GameManager.Instance.GlobalEvent.IsGamePaused) // 튜토리얼이 끝나지 않은 상태에서 게임 일시정지
        {
            talkPanel.SetActive(true);
            _dissolvePanel.SetActive(false);
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
    }
    private void TextBarOn()
    {
        talkPanel.SetActive(true);
        _playerHealthBar.SetActive(false);
        _playerBarrierBar.SetActive(false);
        _playerStaminaBar.SetActive(false);
        _playerWealthBar.SetActive(false);
        _playerSkillCool.SetActive(false);
    }


    private void ChangeSprite()
    {
        if(this.storyBarImage.sprite == decussSprite){
            this.storyBarImage.sprite = OffusiaSprite;
        }

        else if(this.storyBarImage.sprite == OffusiaSprite){                 
            this.storyBarImage.sprite = decussSprite;
        }
    }

    
}