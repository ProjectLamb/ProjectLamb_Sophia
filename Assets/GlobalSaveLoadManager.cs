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
    private string      PATH;
    public  UserData    Data;
    public bool GetIsDataExist() {
        if(File.Exists(PATH)) {
            return true;
        }
        else {
            if(Data.IsOnceUsed == true) {return true;}
            else {return false;}
        }
    }

    private void Awake() {
        Debug.Log(Application.dataPath);
        Debug.Log(Application.persistentDataPath);

# if UNITY_EDITOR
        PATH = Path.Combine(Application.dataPath, "UserData.json");
        Application.quitting += SaveAsJson;
# elif UNITY_STANDALONE_WIN
        PATH = Path.Combine(Application.dataPath, "UserData.json");
        Application.quitting += SaveAsJson;
# elif UNITY_STANDALONE_OSX
        PATH = Path.Combine(Application.persistentDataPath, "UserData.json");
# endif

    }

    private void Start() {
        LoadFromJson();
    }

    private void OnApplicationQuit() {
        SaveAsJson();
    }

    public void ResetData() {
        StartCoroutine(AsyncFileDelete());
    }

    IEnumerator AsyncFileDelete() {
        File.Delete(PATH);
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(() => {return File.Exists(PATH);});   
        Data = new UserData();
        Data.IsOnceUsed = true;
    }

    public async void LoadFromJson() 
    {
        if (File.Exists(PATH))
        {
            string json = await File.ReadAllTextAsync(PATH);
            Debug.Log(json);
            Data = JsonConvert.DeserializeObject<UserData>(json);
        }
        else  { 
            Data = new UserData(); 
        }
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
    public class UserData {
        public bool IsOnceUsed;
        public bool IsTutorial;
        public CutSceneSaveData CutSceneSaveData;

        public UserData() {
            CutSceneSaveData = new CutSceneSaveData();
            IsOnceUsed = false;
            IsTutorial = true;
            CutSceneSaveData.IsSkipStory = false;
        }
    }

    [Serializable]
    public class CutSceneSaveData {
        public bool IsSkipStory;
    }

}