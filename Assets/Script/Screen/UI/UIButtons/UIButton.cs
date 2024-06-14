using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IPointerInteractable {
    public void OnPointerClick();
    public void OnPointerEnter();
    public void OnPointerExit();
    public void OnPointerDown();
    public void OnPointerUp();
}

public abstract class PointerInteractableUIButton : MonoBehaviour, IPointerInteractable {
    public UnityEvent OnClickEvent;
    public UnityEvent OnEnterEvent;
    public UnityEvent OnExitEvent;
    public UnityEvent OnDownEvent;
    public UnityEvent OnUpEvent;
    public virtual void OnPointerClick() {OnClickEvent?.Invoke();}
    public virtual void OnPointerEnter() {OnEnterEvent?.Invoke();}
    public virtual void OnPointerExit() {OnExitEvent?.Invoke();}
    public virtual void OnPointerDown() {OnDownEvent?.Invoke();}
    public virtual void OnPointerUp() {OnUpEvent?.Invoke();}
}

public class UIButton : MonoBehaviour, IPointerInteractable
{
    private EventTrigger ButtonTrigger;

    [SerializeField] PointerInteractableUIButton _interactableUIButton;

    // List<IPointerInteractable> PointerInteractables = new List<IPointerInteractable>();

    private void Awake()
    {
        if(!transform.TryGetComponent(out PointerInteractableUIButton interactableUIButton)) {
            Debug.LogError("Can't Find PointerInteractableUIButton");
            return;
        }
        else {
            _interactableUIButton = interactableUIButton;
        }

        if (!transform.TryGetComponent(out EventTrigger ButtonTrigger))
        {
            Debug.LogError("Can't Find Compoenent EventTrigger");
            return;
        }

        EventTrigger.Entry PointerClick = new EventTrigger.Entry();
        PointerClick.eventID = EventTriggerType.PointerClick;
        PointerClick.callback.AddListener((data) => { OnPointerClick(); });
        ButtonTrigger.triggers.Add(PointerClick);

        EventTrigger.Entry PointerEnter = new EventTrigger.Entry();
        PointerEnter.eventID = EventTriggerType.PointerEnter;
        PointerEnter.callback.AddListener((data) => { OnPointerEnter(); });
        ButtonTrigger.triggers.Add(PointerEnter);

        EventTrigger.Entry PointerExit = new EventTrigger.Entry();
        PointerExit.eventID = EventTriggerType.PointerExit;
        PointerExit.callback.AddListener((data) => { OnPointerExit(); });
        ButtonTrigger.triggers.Add(PointerExit);

        // EventTrigger.Entry Select = new EventTrigger.Entry();
        // Pointer.eventID = EventTriggerType;
        // Pointer.callback.Add(() => {});
        // ButtonTrigger.triggers.Add();
        // EventTrigger.Entry DeSelect = new EventTrigger.Entry();
        // Pointer.eventID = EventTriggerType;
        // Pointer.callback.Add(() => {});
        // ButtonTrigger.triggers.Add();

        EventTrigger.Entry PointerDown = new EventTrigger.Entry();
        PointerDown.eventID = EventTriggerType.PointerDown;
        PointerDown.callback.AddListener((data) => { OnPointerDown(); });
        ButtonTrigger.triggers.Add(PointerDown);

        EventTrigger.Entry PointerUp = new EventTrigger.Entry();
        PointerUp.eventID = EventTriggerType.PointerUp;
        PointerUp.callback.AddListener((data) => { OnPointerUp(); });
        ButtonTrigger.triggers.Add(PointerUp);

    }

    #region Controller
    public void OnPointerClick()    =>  _interactableUIButton.OnPointerClick();
    public void OnPointerEnter()    =>  _interactableUIButton.OnPointerEnter();
    public void OnPointerExit()     =>  _interactableUIButton.OnPointerExit();
    public void OnPointerDown()     =>  _interactableUIButton.OnPointerDown();
    public void OnPointerUp()       =>  _interactableUIButton.OnPointerUp();

    public void OnSelected()
    {
        Debug.Log("Pointer Selected");
    }
    public void DeSelected()
    {
        Debug.Log("Pointer DeSelected");
    }
    #endregion
}