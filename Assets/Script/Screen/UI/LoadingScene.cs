using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AYellowpaper.SerializedCollections;

public class LoadingScene : MonoBehaviour
{
    //public string sceneString;
    public Slider slider;
    [SerializeField] private SerializedDictionary<int, string> ChapterSceneDic;
    GlobalSaveLoadManager globalSaveLoadManager;

    private async void Start()
    {
        globalSaveLoadManager = DontDestroyGameManager.Instance.SaveLoadManager;

        int chapterNum = globalSaveLoadManager.Data.CurrentChapterNum;

        if (chapterNum > 0 && ChapterSceneDic[chapterNum] != null)
            await GlobalSceneLoader.AsyncLoadScene(ChapterSceneDic[chapterNum], slider);
        else
        {
            //Chapter Number Exception
            Debug.Log(gameObject.name + ": " + "Invalid ChapterNumber");
        }
    }
    // private async void OnEnable()
    // {

    // }
}