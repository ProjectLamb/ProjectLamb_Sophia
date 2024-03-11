using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleButtons : MonoBehaviour
{
    public PauseMenu pauseMenu;
    private void Awake() {
    }
    public void HandleReturn(){
        pauseMenu.CloseMenu();
        GameManager.Instance.GlobalEvent.IsGamePaused = false;
    }
    IEnumerator enumerator() {
        GameManager.Instance.GlobalEvent.IsGamePaused = false;
        yield return new WaitForSecondsRealtime(0.01f);
        SceneManager.LoadScene("_TA_001_Loading_");
    }
    public void HandleRestart(){
        StartCoroutine(enumerator());
    }
    public void HandleQuit(){
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
