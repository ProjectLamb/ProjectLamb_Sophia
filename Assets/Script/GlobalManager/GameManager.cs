using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    

    public GlobalEvent  GlobalEvent;
    public GlobalAudio  GlobalAudio;
    public GlobalCarrierManger GlobalCarrierManager;
    public GameObject   PlayerGameObject;
    public GameObject   ChapterGenerator;
    public GameObject   Shop;
    public GameObject   CurrentStage;

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (_instance == null) {
            _instance = this;
        }
        else if (_instance != this) {
            Destroy(gameObject);
        }
        InitializeComponents();
        //DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    
    public void InitializeComponents(){
        if(GlobalEvent == null) GlobalEvent = GetComponentInChildren<GlobalEvent>();
        if(GlobalAudio == null) GlobalAudio = GetComponentInChildren<GlobalAudio>();
    }
}