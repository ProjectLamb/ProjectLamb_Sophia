using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UIButton : MonoBehaviour {
    public UnityEvent OnClickEvent;
    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }
    #region View
    
    [SerializeField] TextMeshProUGUI text;
    private void OnPointerEnterView(){
        DOVirtual.Color(Color.white, Color.cyan, 0.25f, (E) => {
            text.color = E;
        });
    }
    private void OnPointerExitView(){
        DOVirtual.Color(Color.cyan, Color.white, 0.25f, (E) => {
            text.color = E;
        });
    }
    private void OnPointerClickView(){
        DOVirtual.Color(Color.red, Color.cyan, 0.1f, (E) => {
            text.color = E;
        }).SetEase(Ease.Flash);
    }
    
    #endregion

    #region Controller
    public void OnPointerEnter()
    {
        OnPointerEnterView();
        Debug.Log("Pointer Enter");
    }

    public void OnPointerExit()
    {
        OnPointerExitView();
        Debug.Log("Pointer Exit");
    }

    public void OnPointerClick()
    {
        Debug.Log("Pointer Click");
        OnPointerClickView();
        OnClickEvent.Invoke();
    }

    public void OnSelected()
    {
        Debug.Log("Pointer Selected");
    }
    #endregion
}