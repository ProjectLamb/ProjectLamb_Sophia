using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Sophia.UserInterface
{
    public class MouseHandlePannelUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnityEvent OnPointerClickEvent;
        private void Start() {
            OnPointerClickEvent ??= new UnityEvent();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickEvent.Invoke();
        }
    }
}