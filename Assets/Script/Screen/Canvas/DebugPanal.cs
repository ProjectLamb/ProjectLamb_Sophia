using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanal : MonoBehaviour
{
    public GameObject pauseMenuObject;

    Queue<GameObject> panalQueue = new Queue<GameObject>();
    public void OpenMenu(){
        panalQueue.Enqueue(pauseMenuObject);
        var topMenu = panalQueue.Peek();
        topMenu.SetActive(true);
    }

    public void CloseMenu(){
        var topMenu = panalQueue.Peek();
        topMenu.SetActive(false);
        panalQueue.Dequeue();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(panalQueue.Count == 0){
                OpenMenu(); 
            }
            else if(panalQueue.Count == 1){ 
                CloseMenu();
            }
        }
    }
}
