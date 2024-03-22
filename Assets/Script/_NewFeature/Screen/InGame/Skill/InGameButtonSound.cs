using FMODPlus;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class InGameButtonSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {
#region Sound
        [SerializeField] FMODAudioSource    hoveSFX;
        [SerializeField] FMODAudioSource    clickSFX;
        [SerializeField] public bool enableHoverSound = true;
        [SerializeField] public bool enableClickSound = true;
        public bool checkForInteraction = true;
        private Button sourceButton;
        
        void OnEnable()
        {
            if (checkForInteraction == true) { sourceButton = gameObject.GetComponent<Button>(); }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;
            if (enableHoverSound == true)
            {
                hoveSFX.Play();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;
            if (enableClickSound == true)
            {
                clickSFX.Play();
            }
        }

#endregion
    }
}