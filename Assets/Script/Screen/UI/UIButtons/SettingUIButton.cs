using UnityEngine;
using UnityEngine.Events;

public class SettingUIButton : PointerInteractableUIButton
{
    private void OnEnable() {
        OnClickEvent.AddListener(OnClickHandler);
    }
    private void OnDisable() {
        OnClickEvent.RemoveListener(OnClickHandler);   
    }

    private void OnDestroy() {
        OnDisable();
    }

    public void OnClickHandler() {
        DontDestroyGameManager.Instance.AudioSetterScreenObject.SetActive(true);
    }
}