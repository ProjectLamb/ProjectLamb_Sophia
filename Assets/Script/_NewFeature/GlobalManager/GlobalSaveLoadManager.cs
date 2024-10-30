using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Sophia.DB;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using Sophia.DataSystem.Modifiers;
using Sophia.Instantiates;
using UnityEngine.InputSystem;
using AYellowpaper.SerializedCollections;


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
            if (Data.IsNewFile == true) { return true; }
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
            Data.IsNewFile = true;
            //SaveAsJson();
        }
    }

    private void OnApplicationQuit()
    {
        //SaveAsJson();   //Json 초기화되는 버그 때문에 주석처리
    }

    public void ResetData()
    {
        Data = new UserData();
        Data.IsNewFile = true;

        StartCoroutine(AsyncResetFile());
    }

    IEnumerator AsyncResetFile()
    {
        File.Delete(PATH);
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(() => { return File.Exists(PATH); });

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
        public bool IsNewFile; //새로 파일이 생성됐음을 알리는 Flag
        public bool IsTutorial;
        public int CurrentChapterNum;
        public CutSceneSaveData CutSceneSaveData;
        public ChapterClearSaveData ChapterClearSaveData;
        public PlayerData PlayerData;

        public UserData()
        {
            //New Game Setting
            PlayerData = new PlayerData();
            PlayerData.SkillDataDic.Add(KeyCode.Q, null);
            PlayerData.SkillDataDic.Add(KeyCode.E, null);
            PlayerData.SkillDataDic.Add(KeyCode.R, null);

            CutSceneSaveData = new CutSceneSaveData();
            ChapterClearSaveData = new ChapterClearSaveData();

            IsNewFile = false;
            IsTutorial = true;
            CurrentChapterNum = 1;

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

    [Serializable]
    public class PlayerData
    {
        //스텟, 기어, 부품, 스킬 정보
        public float Health = 100;
        public int Gear = 30;
        public bool IsDied = false;
        public List<SerialEquipmentData> EquipmentDataList = new List<SerialEquipmentData>();
        public Dictionary<KeyCode, Skill> SkillDataDic = new Dictionary<KeyCode, Skill>();
    }

}