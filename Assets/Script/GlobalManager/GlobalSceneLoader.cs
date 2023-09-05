using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GlobalSceneLoader : MonoBehaviour {
    public static void LoadScene(string _scene){
        SceneManager.LoadScene(_scene);
    }
    public static void QuitGame(){
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public static async UniTask AsyncLoadScene(string _scene, Slider _progressBar) {
        //https://wlsdn629.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EC%BD%94%EB%A3%A8%ED%8B%B4-%EB%8C%80%EC%8B%A0-unitask
        await UniTask.NextFrame();
        AsyncOperation op = SceneManager.LoadSceneAsync(_scene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        
        while(!op.isDone) {
            await UniTask.NextFrame();
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                _progressBar.value = Mathf.Lerp(_progressBar.value, op.progress, timer);
                if (_progressBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                _progressBar.value = Mathf.Lerp(_progressBar.value, 1f, timer);
                if (_progressBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    break;
                }
            }
        }
    }
}