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
        GameManager.Instance.GlobalEvent.Play(gameObject.name);
    }
    IEnumerator CoRestart() {
        GameManager.Instance.GlobalEvent.ResetForce();
        yield return new WaitForSecondsRealtime(0.01f);
        if(LoadSceneString == "")
            SceneManager.LoadScene(1);
        else {
            SceneManager.LoadScene(LoadSceneString);
        }
    }

    IEnumerator CoExit() {
        GameManager.Instance.GlobalEvent.ResetForce();
        yield return new WaitForSecondsRealtime(0.01f);
        SceneManager.LoadScene(0);
        
        DontDestroyGameManager.Instance.SaveLoadManager.SaveAsJson();   //메인 돌아갈 때 저장
    }
    public void HandleRestart(){
        StartCoroutine(CoRestart());
    }
    public void HandleQuit(){
        StartCoroutine(CoExit());
    }
}
