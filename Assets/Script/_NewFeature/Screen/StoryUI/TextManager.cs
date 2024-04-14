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
    public string[] dialogStrings;
    TalkData[] talkDatas;
    private int currentPage = 0;

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
    /*
        public void TextAction(){
            _playerHealthBar.SetActive(false);
            _playerBarrierBar.SetActive(false);
            _playerStaminaBar.SetActive(false);
            _playerWealthBar.SetActive(false);
            _playerSkillCool.SetActive(false);

            talkPanel.SetActive(true);
            talkText.text = "";
            TypingManager.instance.Typing(dialogStrings, talkText);
        }
    */
    private void Start()
    {
        TextBarOn();
        talkDatas = this.GetComponent<Dialogue>().GetObjectDialogue();
        TypingManager.instance.Typing(talkDatas[0].contexts, talkText);
        currentPage++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            TypingManager.instance.GetInputDown();
            if (TypingManager.instance.isTypingEnd)
            {
                if(currentPage == talkDatas.Length && TypingManager.instance.isDialogEnd)
                    TextBarOff();
                TypingManager.instance.Typing(talkDatas[currentPage].contexts, talkText);
                currentPage++;
            }
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
}