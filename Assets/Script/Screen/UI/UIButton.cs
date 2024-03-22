using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;


public interface IPointerInteractable {
    public void OnPointerClick();
    public void OnPointerEnter();
    public void OnPointerExit();
    public void OnPointerDown();
    public void OnPointerUp();
}

[System.Serializable]
public class UIImageData : IPointerInteractable
{
    public bool IsActivate = false;
    public Image ImageCompoenet;
    // public ImageGroup    _image;
    public Material _imageMaterial;
    public bool IsOutLine;
    public Gradient _outLineGradient;

    public void OnPointerClick() {
        if(!IsActivate){return;}
    }
    public void OnPointerEnter() {
        if(!IsActivate){return;}
        OutLineView(0, 1, 0.25f, true);
    }
    public void OnPointerExit() {
        if(!IsActivate){return;}
        OutLineView(1,0, 0.25f, false);
    }
    public void OnPointerDown() {
        if(!IsActivate){return;}

    }
    public void OnPointerUp() {
        if(!IsActivate){return;}

    }
    private void OutLineView(float _from,float _to,float _duration, bool _outLine)
    {
        if(!IsActivate){return;}
        if (this.IsOutLine)
        {
            float outlineAmount = _outLine ? 5 : 1;
            DOVirtual.Float(_from, _to, _duration, 
                (_E) => {
                    ImageCompoenet.material .SetColor("_Color", _outLineGradient.Evaluate(_E) * (outlineAmount * _E));
                }
            ).SetEase(Ease.InOutBack);
        }
        else
        {
            DOVirtual.Float(_from, _to, _duration,
                (_E) => {
                    ImageCompoenet.material .SetColor("_Color", _outLineGradient.Evaluate(_E) * 1);
                }
            ).SetEase(Ease.InOutBack);
        }
    }
}

[System.Serializable]
public class UITextData : IPointerInteractable
{
    public bool IsActivate = false;
    public TextMeshProUGUI TMPComponent;
    // public TextGroup     _textGroup;
    public Color _defaultColor;
    public Color _hoverColor;
    public Gradient _hovergradient;
    public Color _clickColor;
    public void OnPointerClick() {
        if(!IsActivate){return;}
        DOVirtual.Color(_clickColor, _defaultColor, 0.1f, (E) =>
        {
            TMPComponent.color = E;
        }).SetEase(Ease.Flash);
    }
    public void OnPointerEnter() {
        if(!IsActivate){return;}
        DOVirtual.Float(0, 1, 0.25f, (E) =>
        {
            this.TMPComponent.color = this._hovergradient.Evaluate(E);
        });
    }
    public void OnPointerExit() {
        if(!IsActivate){return;}
        DOVirtual.Float(1, 0, 0.25f, (E) =>
        {
            this.TMPComponent.color = this._hovergradient.Evaluate(E);
        });
    }
    public void OnPointerDown() {
        if(!IsActivate){return;}
        TMPComponent.color = _clickColor;
    }
    public void OnPointerUp() {
        if(!IsActivate){return;}
        TMPComponent.color = _hoverColor;
    }
}

public class UIButton : MonoBehaviour
{
    private EventTrigger ButtonTrigger;
    public UnityEvent OnClickEvent;
    public UnityEvent OnEnterEvent;
    public UnityEvent OnExitEvent;
    public UnityEvent OnDownEvent;
    public UnityEvent OnUpEvent;
    public UIImageData _uiImageData;
    public UITextData _uiTextData;
    
    List<IPointerInteractable> PointerInteractables = new List<IPointerInteractable>();

    private void Awake()
    {
        if (transform.TryGetComponent(out EventTrigger ButtonTrigger))
        {
            //Debug.Log("Find Compoenent EventTrigger");
        }
        if (transform.Find("Header") != null && transform.Find("Header").TryGetComponent(out TextMeshProUGUI TMP))
        {
            //Debug.Log("Find Compoenent TestMeshProUGUI");
            _uiTextData.IsActivate = true;
            _uiTextData.TMPComponent = TMP;
            PointerInteractables.Add(_uiTextData);
        }
        if (transform.TryGetComponent(out Image image))
        {
            //Debug.Log("Find Compoenent Image");
            _uiTextData.IsActivate = true;
            _uiImageData.ImageCompoenet = image;
            _uiImageData._imageMaterial = image.material;
            _uiImageData.ImageCompoenet.material = Instantiate(_uiImageData._imageMaterial);
            PointerInteractables.Add(_uiImageData);
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
    public void OnPointerClick()
    {
        Debug.Log("Pointer Click");
        PointerInteractables.ForEach((E) => E.OnPointerClick());
        OnClickEvent.Invoke();
    }
    
    public void OnPointerEnter()
    {
        Debug.Log("Pointer Enter");
        PointerInteractables.ForEach((E) => E.OnPointerEnter());
        OnEnterEvent.Invoke();
    }

    public void OnPointerExit()
    {
        Debug.Log("Pointer Exit");
        PointerInteractables.ForEach((E) => E.OnPointerExit());
        OnExitEvent.Invoke();
    }

    public void OnPointerDown()
    {
        Debug.Log("Pointer Down");
        PointerInteractables.ForEach((E) => E.OnPointerDown());
        OnDownEvent.Invoke();
    }

    public void OnPointerUp()
    {
        Debug.Log("Pointer Up");
        PointerInteractables.ForEach((E) => E.OnPointerUp());
        OnUpEvent.Invoke();
    }

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