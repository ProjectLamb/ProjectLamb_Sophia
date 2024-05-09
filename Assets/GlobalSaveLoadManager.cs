using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Sophia.DB;
using UnityEngine;
using UnityEngine.Events;


public class GlobalSaveLoadManager : MonoBehaviour
{
    private string      PATH;
    public  UserData    Data;
    public bool GetIsDataExist() {
        return File.Exists(PATH);
    }

    private void Awake() {
        PATH = Path.Combine(Application.dataPath, "Resources" ,"DB", "UserData.json");
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
    }

    public void LoadFromJson() 
    {
        if (File.Exists(PATH))
        {
            string json = File.ReadAllText(PATH);
            Debug.Log(json);
            Data = JsonConvert.DeserializeObject<UserData>(json);
        }
        else  { 
            Data = new UserData(); 
        }
    }

    public void SaveAsJson() 
    {
        string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
        File.WriteAllText(PATH, json);
    }
}

namespace Sophia.DB 
{

    [Serializable]
    public class UserData {
        public bool IsTutorial;
        public CutSceneSaveData CutSceneSaveData;

        public UserData() {
            CutSceneSaveData = new CutSceneSaveData();
            IsTutorial = true;
            CutSceneSaveData.IsSkipStory = false;
        }
    }

    [Serializable]
    public class CutSceneSaveData {
        public bool IsSkipStory;
    }

}