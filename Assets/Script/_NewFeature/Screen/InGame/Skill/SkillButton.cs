using System;
using FMODPlus;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class SkillButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {
        [SerializeField] KeyCode            skillKey;
        [SerializeField] Image              backgroundImageObj;
        [SerializeField] TextMeshProUGUI    nameObj;
        [SerializeField] TextMeshProUGUI    descriptionObj;
        [SerializeField] Image              skillIconObj;
        [SerializeField] TextMeshProUGUI    skillKeyObj;
        [SerializeField] Sprite defaultSkillIcon;
        public UnityAction<bool, KeyCode> func;
        public UnityAction<KeyCode> HoverFunc;


        public void OnPointerClick(PointerEventData eventData)
        {
            func?.Invoke(true, skillKey);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoverFunc?.Invoke(skillKey);
        }

        public void SetUserInterfaceData(IUserInterfaceAccessible userInterfaceData, KeyCode key)
        {
            StartCoroutine(GlobalAsync.PerformAndRenderUIUnScaled(()=>{
                nameObj.text        = userInterfaceData.GetName();
                descriptionObj.text = userInterfaceData.GetDescription();
                skillIconObj.sprite = userInterfaceData.GetSprite() ? userInterfaceData.GetSprite() : defaultSkillIcon;
                skillKeyObj.text = key == KeyCode.None ? "" : skillKey.ToString();
            }));
        }

        void Awake(){
            backgroundImageObj  = transform.Find("Content/StyleContent/Background").GetComponent<Image>();
            nameObj             = transform.Find("Content/SkillInterfaceInfo/Texts/Name").GetComponent<TextMeshProUGUI>();
            descriptionObj      = transform.Find("Content/SkillInterfaceInfo/Texts/Description").GetComponent<TextMeshProUGUI>();
            skillIconObj        = transform.Find("Content/SkillInterfaceInfo/SkillIconMask/SkillIcon").GetComponent<Image>();
            skillKeyObj         = transform.Find("Content/SkillInterfaceInfo/SkillIconMask/SkillKey").GetComponent<TextMeshProUGUI>();
        }
    }
}