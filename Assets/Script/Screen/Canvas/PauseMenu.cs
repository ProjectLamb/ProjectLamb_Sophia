using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;

    Queue<GameObject> menuQueue = new Queue<GameObject>();
    public void OpenMenu(){
        menuQueue.Enqueue(pauseMenuObject);
        var topMenu = menuQueue.Peek();
        topMenu.SetActive(true);
    }

    public void CloseMenu(){
        var topMenu = menuQueue.Peek();
        topMenu.SetActive(false);
        menuQueue.Dequeue();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(menuQueue.Count == 0){
                OpenMenu(); 
                GameManager.Instance.GlobalEvent.IsGamePaused = true;
            }
            else if(menuQueue.Count == 1){ 
                CloseMenu();
                GameManager.Instance.GlobalEvent.IsGamePaused = false;
            }
        }
    }
}
