using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {
    public GlobalSceneLoader globalSceneLoader;
    public string sceneString;
    public Slider slider;
    private void Awake() {
        globalSceneLoader.AsyncLoadScene(sceneString, slider);
    }    
}