using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Sophia.UserInterface
{
    public class MouseHandlePannelUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private UnityEvent OnPointerClickEvent;
        [SerializeField] private UnityEvent OnPointerEnterEvent;

        private void Start() {
            OnPointerClickEvent ??= new UnityEvent();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickEvent.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent.Invoke();
        }
    }
}