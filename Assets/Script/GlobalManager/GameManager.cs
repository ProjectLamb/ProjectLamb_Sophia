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
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public GameObject Player;
    public GameObject UI;
    public GameObject CurrentRoom;

    private bool mIsGamePaused = false;

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
        Time.timeScale = mTimenum;
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //AudioManager.Instance.CleanEventInstance();
        }
    }

    public bool IsGamePaused {
        get {
            return mIsGamePaused; 
        }
        set {
            if(value == true){Time.timeScale = 0;}
            else{Time.timeScale = 1;}
            mIsGamePaused = value;
            Debug.Log("Time Changed");
        }
    }

    [Range(0,1)]
    public float mTimenum = 1f;
}