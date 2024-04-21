using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sophia;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public UnityEvent OpenUnityEvent;
    public UnityEvent CloseUnityEvent;

    Stack<GameObject> menuStack = new Stack<GameObject>();
    public void OpenMenu(GameObject _canvas){
        if(menuStack.Count == 0) {
            GameManager.Instance.GlobalEvent.IsGamePaused = true;
        }
        OpenUnityEvent.Invoke();
        menuStack.Push(_canvas);
        var topMenu = menuStack.Peek();
        topMenu.SetActive(true);
    }

    public void CloseMenu(){
        if(menuStack.Peek().name == pauseMenuObject.name) {
            StartCoroutine(GlobalAsync.PerformAndRenderUIUnScaled(() => {
                CloseUnityEvent.Invoke();
                var topMenu = menuStack.Peek();
                topMenu.SetActive(false);
                menuStack.Pop();
                if(menuStack.Count == 0) GameManager.Instance.GlobalEvent.IsGamePaused = false;
            }));
            return;
        }
        var topMenu = menuStack.Peek();
        topMenu.SetActive(false);
        menuStack.Pop();
        if(menuStack.Count == 0) {GameManager.Instance.GlobalEvent.IsGamePaused = false;}
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!pauseMenuObject.activeSelf){
                OpenMenu(pauseMenuObject);
            }
            else if(pauseMenuObject.activeSelf && menuStack.Count > 0){
                CloseMenu();
            }
        }
    }
}
