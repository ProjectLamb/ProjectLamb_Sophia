using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UIButton : MonoBehaviour {
    public UnityEvent OnClickEvent;
    public Image image;
    public TextMeshProUGUI text;

    private void Awake() {
        if(!TryGetComponent(out TextMeshProUGUI text)){Debug.Log("Can't Find Compoenent");}
        if(!TryGetComponent(out Image image)){Debug.Log("Can't Find Compoenent");}
    }
    #region View
    
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

    public void OnPointerEnterViewMaterial(){
        DOVirtual.Color(Color.black, Color.green, 0.1f, (E) => {
            image.material.SetColor("_Color", E);
        }).SetEase(Ease.InOutBack);
    }

    public void OnPointerExitViewMaterial(){
        DOVirtual.Color(Color.green,Color.black, 0.1f, (E) => {
            image.material.SetColor("_Color", E);
        }).SetEase(Ease.InOutBack);
    }
    
    #endregion

    #region Controller
    public void OnPointerEnter()
    {
        Debug.Log("Pointer Enter");
        OnPointerEnterView();
    }

    public void OnPointerExit()
    {
        Debug.Log("Pointer Exit");
        OnPointerExitView();
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