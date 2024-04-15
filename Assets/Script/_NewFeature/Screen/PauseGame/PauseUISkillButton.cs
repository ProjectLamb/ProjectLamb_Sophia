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
        [SerializeField] Sprite defaultSkillImage;

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
                SkillName[0].text       = QUI.GetName();
                if(QUI.GetSprite() != null) {
                    SkillIcon[0].sprite = QUI.GetSprite();
                    SkillIcon[0].type = Image.Type.Simple;
                } else {
                    SkillIcon[0].sprite = defaultSkillImage;
                    SkillIcon[0].type = Image.Type.Tiled;
                }

                SkillName[1].text       = EUI.GetName();
                if (EUI.GetSprite() != null) {
                    SkillIcon[1].sprite = EUI.GetSprite();
                    SkillIcon[1].type = Image.Type.Simple;
                } else {
                    SkillIcon[1].sprite = defaultSkillImage;
                    SkillIcon[1].type = Image.Type.Tiled;
                }

                SkillName[2].text       = RUI.GetName();
                if (RUI.GetSprite() != null) {
                    SkillIcon[2].sprite = RUI.GetSprite();
                    SkillIcon[2].type = Image.Type.Simple;
                } else {
                    SkillIcon[2].sprite = defaultSkillImage;
                    SkillIcon[2].type = Image.Type.Tiled;
                }
            }));      
        }
    }

}