using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryManager : MonoBehaviour
{
    private static StoryManager _instance;
    public static StoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType(typeof(StoryManager)) as StoryManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public UnityEvent<string> OnIsTutorialEnter;
    public UnityEvent<string> OnIsTutorialExit;

    [SerializeField] private bool _isTutorial;
    public bool IsTutorial
    {
        get { return _isTutorial; }
        set
        {
            _isTutorial = value;
            if (_isTutorial == true)
            {
                OnIsTutorialEnter?.Invoke("StoryManager");
            }
            else
            {
                OnIsTutorialExit?.Invoke("StoryManager");
            }

        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        OnIsTutorialEnter ??= new UnityEvent<string>();
        OnIsTutorialExit ??= new UnityEvent<string>();
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        OnIsTutorialEnter.AddListener(Sophia.Entitys.PlayerController.DisallowInput);
        OnIsTutorialExit.AddListener(Sophia.Entitys.PlayerController.AllowInput);
    }

    private void OnDisable()
    {
        OnIsTutorialEnter.RemoveListener(Sophia.Entitys.PlayerController.DisallowInput);
        OnIsTutorialExit.RemoveListener(Sophia.Entitys.PlayerController.AllowInput);
    }

    void Start()
    {
        if (DontDestroyGameManager.Instance != null)
            IsTutorial = DontDestroyGameManager.Instance.SaveLoadManager.Data.IsTutorial; // IsTutorial
    }

    // void Update()
    // {
    //     if (!TextManager.Instance.IsStory && !IsBossClear && Sophia.UserInterface.InGameScreenUI.Instance._storyFadePanel.IsWaitOver)
    //     {
    //         Sophia.UserInterface.InGameScreenUI.Instance._fadeUI.FadeOut(0.05f, 2f);
    //         Sophia.UserInterface.InGameScreenUI.Instance._fadeUI.AddBindingAction(() => UnityEngine.SceneManagement.SceneManager.LoadScene("05_Demo_Clear"));
    //         IsBossClear = true;
    //     }
    // }
}
