using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    public GlobalEvent globalEvent;
    public GlobalAudio globalAudio;

    //public GameObject currentRoom;
    public GameObject playerGameObject;
    public PlayerData playerData;
    public GameObject ChapterGenerator;
    public GameObject currentStage;
    public GlobalEvent globalEvent;

    void Awake()
    {
        Application.targetFrameRate = 65;

        if (_instance == null) {
            _instance = this;
        }
        else if (_instance != this) {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = globalEvent.CurrentTimeScale;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //AudioManager.Instance.CleanEventInstance();
        }
    }
}