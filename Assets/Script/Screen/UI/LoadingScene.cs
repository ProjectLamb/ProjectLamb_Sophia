using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {
    public string sceneString;
    public Slider slider;
    private async void OnEnable() {
        await GlobalSceneLoader.AsyncLoadScene(sceneString, slider);
    }    
}