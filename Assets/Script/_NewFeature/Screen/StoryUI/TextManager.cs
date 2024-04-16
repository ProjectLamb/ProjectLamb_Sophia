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
    public GameObject talkPanel;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;
    public string[] dialogStrings;
    TalkData[] talkDatas;
    private int currentPage = 0; // 대화문 개수 변수
    public bool IsStory = false;

    [SerializeField] public GameObject _playerHealthBar;
    [SerializeField] public GameObject _playerBarrierBar;
    [SerializeField] public GameObject _playerStaminaBar;
    [SerializeField] public GameObject _playerWealthBar;
    [SerializeField] public GameObject _playerSkillCool;

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
        TypingManager.instance.Typing(talkDatas[0].contexts, talkText);
        nameText.text = talkDatas[0].name;
        currentPage++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {

            TypingManager.instance.GetInputDown();
            if (TypingManager.instance.isTypingEnd)
            {
                if(currentPage == talkDatas.Length && TypingManager.instance.isDialogEnd){
                    SceneManager.LoadScene("_01_Chapter_Tutorial");
                    //TextBarOff();
                    IsStory = false;
                }
                nameText.text = talkDatas[currentPage].name;
                TypingManager.instance.Typing(talkDatas[currentPage].contexts, talkText);
                currentPage++;
            }
        }
        if(GameManager.Instance.GlobalEvent.IsGamePaused)
            talkPanel.SetActive(false);
        else if(!GameManager.Instance.GlobalEvent.IsGamePaused && IsStory)
            talkPanel.SetActive(true);

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
}