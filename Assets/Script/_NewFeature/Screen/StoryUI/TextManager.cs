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

    [SerializeField] public GameObject          _playerHealthBar;
    [SerializeField] public GameObject          _playerBarrierBar;
    [SerializeField] public GameObject          _playerStaminaBar;
    [SerializeField] public GameObject          _playerWealthBar;
    [SerializeField] public GameObject          _playerSkillCool;

    private static TextManager _instance;
    public static TextManager Instance
    {
        get {
            if(_instance == null)
            {
                _instance = FindFirstObjectByType(typeof(TextManager)) as TextManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

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

    private void Start()
    {
        _playerHealthBar.SetActive(false);
        _playerBarrierBar.SetActive(false);
        _playerStaminaBar.SetActive(false);
        _playerWealthBar.SetActive(false);
        _playerSkillCool.SetActive(false);

        talkPanel.SetActive(true);
        TypingManager.instance.Typing(dialogStrings, talkText);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            TypingManager.instance.GetInputDown();
        }
        TalkData[] talkDatas = this.GetComponent<Dialogue>().GetObjectDialogue();
                // 대사가 null이 아니면 대사 출력
        if(talkDatas != null) DebugDialogue(talkDatas);
    }

    void DebugDialogue(TalkData[] talkDatas)
    {
        for (int i = 0; i < talkDatas.Length; i++)
        {
            // 캐릭터 이름 출력
            Debug.Log(talkDatas[i].name);
            // 대사들 출력
            foreach (string context in talkDatas[i].contexts) 
            	Debug.Log(context);
        }
    }
}
