using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;

    Stack<GameObject> menuStack = new Stack<GameObject>();
    public void OpenMenu(GameObject _canvas){
        menuStack.Push(_canvas);
        var topMenu = menuStack.Peek();
        topMenu.SetActive(true);
    }

    public void CloseMenu(){
        var topMenu = menuStack.Peek();
        topMenu.SetActive(false);
        menuStack.Pop();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(menuStack.Count == 0){
                OpenMenu(pauseMenuObject);
                GameManager.Instance.GlobalEvent.IsGamePaused = true;
            }
            else if(menuStack.Count == 1){ 
                CloseMenu();
                GameManager.Instance.GlobalEvent.IsGamePaused = false;
            }
            else {
                CloseMenu();
            }
        }
    }
}
