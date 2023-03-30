using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TEST_GloablEvent : MonoBehaviour {
    private static readonly TEST_GloablEvent _instance = new TEST_GloablEvent();

    public static TEST_GloablEvent Instance {
        get {
            if(_instance == null) return new TEST_GloablEvent();
            return _instance;
        }
    }

    public TEST_Player player;
    public Dictionary<string, Action<int>> Evnets;
    private void Awake(){
        Evnets = new Dictionary<string, Action<int>>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}