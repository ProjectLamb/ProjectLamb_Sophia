using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Sophia.DB;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;


public class GlobalSaveLoadManager : MonoBehaviour
{
    private string PATH;
    public UserData Data;
    public bool GetIsDataExist()
    {
        if (File.Exists(PATH))
        {
            return true;
        }
        else
        {
            if (Data.IsOnceUsed == true) { return true; }
            else { return false; }
        }
    }

    private void Awake()
    {
        Debug.Log(Application.dataPath);
        Debug.Log(Application.persistentDataPath);

# if UNITY_EDITOR   //만약 유니티 에디터라면
        PATH = Path.Combine(Application.dataPath, "UserData.json");
        Application.quitting += SaveAsJson;
# elif UNITY_STANDALONE_WIN //운영체제가 Windows
        PATH = Path.Combine(Application.dataPath, "UserData.json");
        Application.quitting += SaveAsJson;
# elif UNITY_STANDALONE_OSX //운영체제가 Mac
        PATH = Path.Combine(Application.persistentDataPath, "UserData.json");
# endif

    }

    private void Start()
    {
        if (File.Exists(PATH))
            LoadFromJson();
        else
        {
            Data = new UserData();
            Data.IsOnceUsed = true;
            //SaveAsJson();
        }
    }

    private void OnApplicationQuit()
    {
        //SaveAsJson();   //Json 초기화되는 버그 때문에 주석처리
    }

    public void ResetData()
    {
        StartCoroutine(AsyncResetFile());
    }

    IEnumerator AsyncResetFile()
    {
        File.Delete(PATH);
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(() => { return File.Exists(PATH); });

        Data = new UserData();
        Data.IsOnceUsed = true;
        SaveAsJson();
    }

    public async void LoadFromJson()
    {
        if (File.Exists(PATH))
        {
            string json = await File.ReadAllTextAsync(PATH);
            Debug.Log(json);
            Data = JsonConvert.DeserializeObject<UserData>(json);
        }
        // else
        // {
        //     Data = new UserData();
        // }
    }

    public async void SaveAsJson()
    {
        string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
        await File.WriteAllTextAsync(PATH, json);
    }
}

namespace Sophia.DB
{

    [Serializable]
    public class UserData
    {
        public bool IsOnceUsed; //파일이 존재하지 않을 때, 데이터는 존재한다고 명시할 flag
        public bool IsTutorial;
        public CutSceneSaveData CutSceneSaveData;
        public ChapterClearSaveData ChapterClearSaveData;

        public UserData()
        {
            //New Game Setting
            CutSceneSaveData = new CutSceneSaveData();
            ChapterClearSaveData = new ChapterClearSaveData();

            IsOnceUsed = false;
            IsTutorial = true;

            CutSceneSaveData.IsSkipStory = false;

            ChapterClearSaveData.IsChapter1Clear = false;
        }
    }

    [Serializable]
    public class CutSceneSaveData
    {
        public bool IsSkipStory;    //Chapter 1 Tutorial
    }

    [Serializable]
    public class ChapterClearSaveData
    {
        public bool IsChapter1Clear;
    }

}