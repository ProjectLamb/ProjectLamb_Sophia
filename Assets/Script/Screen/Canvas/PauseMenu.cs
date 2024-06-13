using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sophia;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public UnityEvent OnOpenMenuEvent;
    public UnityEvent OnCloseMenuEvent;
    public static UnityEvent OnOpenMenuStaticEvent = new UnityEvent();
    public static UnityEvent OnCloseMenuStaticEvent = new UnityEvent();
    
    Stack<GameObject> menuStack = new Stack<GameObject>();
    public void OpenMenu(GameObject _canvas){
        if(menuStack.Count == 0) {
            GameManager.Instance.GlobalEvent.Pause(gameObject.name);
            OnOpenMenuEvent?.Invoke();
            OnOpenMenuStaticEvent?.Invoke();
        }
        menuStack.Push(_canvas);
        var topMenu = menuStack.Peek();
        topMenu.SetActive(true);
    }

    public void CloseMenu(){
        if(menuStack.Peek().name == pauseMenuObject.name) {
            StartCoroutine(GlobalAsync.PerformAndRenderUIUnScaled(() => {
                OnCloseMenuEvent?.Invoke();
                OnCloseMenuStaticEvent?.Invoke();
                var topMenu = menuStack.Peek();
                topMenu.SetActive(false);
                menuStack.Pop();
                if(menuStack.Count == 0) GameManager.Instance.GlobalEvent.Play(gameObject.name);
            }));
            return;
        }
        var topMenu = menuStack.Peek();
        topMenu.SetActive(false);
        menuStack.Pop();
        if(menuStack.Count == 0) {GameManager.Instance.GlobalEvent.Play(gameObject.name);}
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
