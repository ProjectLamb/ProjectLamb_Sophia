using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleButtons : MonoBehaviour
{
    public string LoadSceneString;
    public PauseMenu pauseMenu;
    private void Awake() {
    }
    public void HandleReturn(){
        pauseMenu.CloseMenu();
        GameManager.Instance.GlobalEvent.Play(gameObject.name);;
    }
    IEnumerator enumerator() {
        GameManager.Instance.GlobalEvent.Play(gameObject.name);;
        yield return new WaitForSecondsRealtime(0.01f);
        if(LoadSceneString == "")
            SceneManager.LoadScene(1);
        else {
            SceneManager.LoadScene(LoadSceneString);
        }
    }
    public void HandleRestart(){
        StartCoroutine(enumerator());
    }
    public void HandleQuit(){
        SceneManager.LoadScene(0);
        GameManager.Instance.GlobalEvent.Play(gameObject.name);;
        // #if UNITY_EDITOR
        //     // Application.Quit() does not work in the editor so
        //     // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        //     UnityEditor.EditorApplication.isPlaying = false;
        // #else
        //     Application.Quit();
        // #endif
    }
}
