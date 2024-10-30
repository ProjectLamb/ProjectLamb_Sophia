using FMODPlus;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class InGameButtonFeature : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
#region Sound
        [SerializeField] FMODAudioSource    hoveSFX;
        [SerializeField] FMODAudioSource    clickSFX;
        [SerializeField] Image glowImage;
        [SerializeField] public bool enableHoverSound = true;
        [SerializeField] public bool enableClickSound = true;
        [SerializeField] public bool enableHoverGlow = true;
        public bool checkForInteraction = true;
        private Button sourceButton;
        private Color originGlowColor;
        
        void OnEnable()
        {
            if (checkForInteraction == true) { sourceButton = gameObject.GetComponent<Button>(); }
            if (glowImage != null) { originGlowColor = glowImage.color; }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;
            if (enableHoverSound == true)
            {
                hoveSFX.Play();
            }
            if(enableHoverGlow == true)
            {
                glowImage.color = new Color(55, 185, 145);
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

        public void OnPointerExit(PointerEventData eventData)
        {
            if(enableHoverGlow == true)
            {
                glowImage.color = originGlowColor;
            }
        }

        #endregion
    }
}