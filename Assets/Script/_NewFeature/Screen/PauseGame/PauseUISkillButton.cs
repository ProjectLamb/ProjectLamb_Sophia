using System.Collections;
using System.Collections.Generic;
using Sophia.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sophia
{
    public class PauseUISkillButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI[] SkillName = new TextMeshProUGUI[3];
        [SerializeField] Image[] SkillIcon = new Image[3];
        Composite.SkillManager skillManagerRef;
        private void Awake() {
            skillManagerRef ??= GameManager.Instance.PlayerGameObject.GetComponent<Entitys.Player>().GetSkillManager();    
        }

        private void OnEnable() {
            UIUpdate();
        }

        public void UIUpdate() {
            IUserInterfaceAccessible QUI = skillManagerRef.GetSkillInfoByKey(KeyCode.Q);
            IUserInterfaceAccessible EUI = skillManagerRef.GetSkillInfoByKey(KeyCode.E);
            IUserInterfaceAccessible RUI = skillManagerRef.GetSkillInfoByKey(KeyCode.R);
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => {
                SkillName[0].text       = QUI.GetName(); if(QUI.GetSprite() !=null) { SkillIcon[0].type = Image.Type.Simple; SkillIcon[0].sprite = QUI.GetSprite();}
                SkillName[1].text       = EUI.GetName(); if(EUI.GetSprite() !=null) { SkillIcon[1].type = Image.Type.Simple; SkillIcon[1].sprite = EUI.GetSprite();}
                SkillName[2].text       = RUI.GetName(); if(RUI.GetSprite() !=null) { SkillIcon[2].type = Image.Type.Simple; SkillIcon[2].sprite = RUI.GetSprite();}
            }));      
        }
    }

}