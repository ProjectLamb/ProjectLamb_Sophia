using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using Sophia.Instantiates;
using DG.Tweening;
using System;

namespace Sophia.UserInterface
{
    public class InGameSkillSelector : MonoBehaviour {
        private static InGameSkillSelector _instance;
        
        public static InGameSkillSelector Instance {
            get {
                if(_instance == null) {
                    _instance = FindFirstObjectByType(typeof(InGameSkillSelector), findObjectsInactive: FindObjectsInactive.Include) as InGameSkillSelector;
                    if(_instance == null) 
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set{}
        }

        [SerializeField] Entitys.Player player;
        [SerializeField] Composite.SkillManager skillManager;
        [SerializeField] RectTransform[] skillButtonTransform = new RectTransform[3];
        [SerializeField] SkillButton[]   collectedSkillButton = new SkillButton[3];
        [SerializeField] QuitSkillButton quitSkillButton;
        [SerializeField] SkillButton     currentSkillButton;
        [SerializeField] UnityAction<bool, KeyCode> actionFromItem;

        private void OnEnable() {
            StartCoroutine(AsyncRender.PerformAndRenderUIUnScaled(() => {
                collectedSkillButton[0].SetUserInterfaceData(skillManager.SkillGetSkillInfoByKey(KeyCode.Q), KeyCode.Q);
                collectedSkillButton[1].SetUserInterfaceData(skillManager.SkillGetSkillInfoByKey(KeyCode.E), KeyCode.E);
                collectedSkillButton[2].SetUserInterfaceData(skillManager.SkillGetSkillInfoByKey(KeyCode.R), KeyCode.R);
                collectedSkillButton[0].func        = SendSelectData;
                collectedSkillButton[1].func        = SendSelectData;
                collectedSkillButton[2].func        = SendSelectData;
                collectedSkillButton[0].HoverFunc   = MoveCurrentHovering;
                collectedSkillButton[1].HoverFunc   = MoveCurrentHovering;
                collectedSkillButton[2].HoverFunc   = MoveCurrentHovering;
                quitSkillButton.func                = SendSelectData;
            }));
        }

        public void OpenSkillSelector(Skill skill, UnityAction<bool, KeyCode> action) {
            GameManager.Instance.GlobalEvent.IsGamePaused = true;
            actionFromItem = action;
            gameObject.SetActive(true);
            StartCoroutine(AsyncRender.PerformAndRenderUIUnScaled(() => {
                currentSkillButton.SetUserInterfaceData(skill, KeyCode.None);
                currentSkillButton.transform.localPosition = Vector3.zero;
            }));
        }

        public void SendSelectData(bool isSelected, KeyCode assignedKey) {
            bool IsSelected = isSelected;
            KeyCode AssignedKey = assignedKey;
            Debug.Log($"{IsSelected} {AssignedKey}");
            actionFromItem.Invoke(IsSelected, AssignedKey);
            StartCoroutine(AsyncRender.PerformUnScaled(0.5f, CloseSkillSelector));
        }

        public void CloseSkillSelector() {
            GameManager.Instance.GlobalEvent.IsGamePaused = false;
            actionFromItem = null;
            gameObject.SetActive(false);
        }

        public void MoveCurrentHovering(KeyCode key){
            RectTransform destRect;
            switch(key) {
                case KeyCode.Q : {
                    destRect = skillButtonTransform[0];
                    currentSkillButton.transform.DOLocalMove(destRect.localPosition, 0.25f).SetEase(Ease.InCubic).SetUpdate(true);
                    break;
                }
                case KeyCode.E : {
                    destRect = skillButtonTransform[1];
                    currentSkillButton.transform.DOLocalMove(destRect.localPosition, 0.25f).SetEase(Ease.InCubic).SetUpdate(true);
                    break;
                }
                case KeyCode.R : {
                    destRect = skillButtonTransform[2];
                    currentSkillButton.transform.DOLocalMove(destRect.localPosition, 0.25f).SetEase(Ease.InCubic).SetUpdate(true);
                    break;
                }
            }
        }
    }
}