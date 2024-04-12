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
}
