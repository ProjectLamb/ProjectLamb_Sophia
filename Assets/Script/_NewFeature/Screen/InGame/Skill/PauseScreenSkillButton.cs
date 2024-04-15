using System;
using FMODPlus;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class PauseScreenSkillButton : MonoBehaviour {
        [SerializeField] public KeyCode            skillKey;
        [SerializeField] Image              backgroundImageObj;
        [SerializeField] TextMeshProUGUI    nameObj;
        [SerializeField] TextMeshProUGUI    descriptionObj;
        [SerializeField] Image              skillIconObj;
        [SerializeField] TextMeshProUGUI    skillKeyObj;
        [SerializeField] Sprite defaultSkillIcon;

        public void SetUserInterfaceData(IUserInterfaceAccessible userInterfaceData, KeyCode key)
        {
            StartCoroutine(GlobalAsync.PerformAndRenderUIUnScaled(() => {
                nameObj.text        = userInterfaceData.GetName();
                descriptionObj.text = userInterfaceData.GetDescription();
                skillIconObj.sprite = userInterfaceData.GetSprite() ? userInterfaceData.GetSprite() : defaultSkillIcon;
                skillKeyObj.text = key == KeyCode.None ? "" : skillKey.ToString();
            }));
        }

        void Awake()
        {
            backgroundImageObj  = transform.Find("ButtonContent/Content/StyleContent/Background").GetComponent<Image>();
            nameObj             = transform.Find("ButtonContent/Content/SkillInterfaceInfo/Texts/Name").GetComponent<TextMeshProUGUI>();
            descriptionObj      = transform.Find("ButtonContent/Content/SkillInterfaceInfo/Texts/Description").GetComponent<TextMeshProUGUI>();
            skillIconObj        = transform.Find("ButtonContent/Content/SkillInterfaceInfo/SkillIconMask/SkillIcon").GetComponent<Image>();
            skillKeyObj         = transform.Find("ButtonContent/Content/SkillInterfaceInfo/SkillIconMask/SkillKey").GetComponent<TextMeshProUGUI>();
        }
    }
}