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
    public class InGameSkillSelector : MonoBehaviour
    {
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
        [SerializeField] PauseMenu pauseMenu;

        [SerializeField] Entitys.Player player;
        [SerializeField] Composite.SkillManager skillManager;
        [SerializeField] RectTransform[] skillButtonTransform = new RectTransform[3];
        [SerializeField] SkillButton[]   collectedSkillButton = new SkillButton[3];
        [SerializeField] QuitSkillButton quitSkillButton;
        [SerializeField] SkillButton     currentSkillButton;
        [SerializeField] GameObject     currentSkillButtonGameObject;
        [SerializeField] UnityAction<bool, KeyCode> actionFromItem;

        private Skill _currentSkill;
        private bool _isAlreadyInvoked;

        private void OnEnable() {
            StartCoroutine(GlobalAsync.PerformAndRenderUIUnScaled(() => {
                collectedSkillButton[0].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.Q), KeyCode.Q);
                collectedSkillButton[1].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.E), KeyCode.E);
                collectedSkillButton[2].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.R), KeyCode.R);
                collectedSkillButton[0].func        = SendSelectData;
                collectedSkillButton[1].func        = SendSelectData;
                collectedSkillButton[2].func        = SendSelectData;
                collectedSkillButton[0].HoverFunc   = MoveCurrentHovering;
                collectedSkillButton[1].HoverFunc   = MoveCurrentHovering;
                collectedSkillButton[2].HoverFunc   = MoveCurrentHovering;
                quitSkillButton.func                = CloseSkillSelector;
            }));
        }

        public void OpenSkillSelector(Skill skill, UnityAction<bool, KeyCode> action)
        {
            _isAlreadyInvoked = false;
            actionFromItem = action;
            pauseMenu.OpenMenu(gameObject);

            StartCoroutine(GlobalAsync.PerformAndRenderUIUnScaled(() => {
                currentSkillButtonGameObject.SetActive(true);
                _currentSkill = skill;
                currentSkillButton.SetUserInterfaceData(_currentSkill, KeyCode.None);
                currentSkillButton.transform.localPosition = Vector3.zero;
            }));
        }

        public void SendSelectData(bool isSelected, KeyCode assignedKey)
        {
            bool IsSelected = isSelected;
            KeyCode AssignedKey = assignedKey;

            // 할당하려는 KeyCode에 이미 스킬이 있을 경우, currentSkillButton과 스왑
            // 없을 경우 currentSkillButton 비활성화
            Skill tempSkill = skillManager.GetSkillByKey(AssignedKey);
            if (_currentSkill != null) {
                player.CollectSkill(_currentSkill, AssignedKey);
            } else {
                player.DropSkill(AssignedKey);
            }
            if (tempSkill != null) {
                _currentSkill = tempSkill;
                currentSkillButton.SetUserInterfaceData(_currentSkill, KeyCode.None);
            } else {
                currentSkillButtonGameObject.SetActive(false);
            }

            if (!_isAlreadyInvoked) {
                actionFromItem.Invoke(IsSelected, AssignedKey);
                Debug.Log($"{IsSelected} {AssignedKey}");
                _isAlreadyInvoked = true;
            }

            collectedSkillButton[0].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.Q), KeyCode.Q);
            collectedSkillButton[1].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.E), KeyCode.E);
            collectedSkillButton[2].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.R), KeyCode.R);

            InGameScreenUI.Instance._playerSkillCoolUIElement[0].DrawForce();
            InGameScreenUI.Instance._playerSkillCoolUIElement[1].DrawForce();
            InGameScreenUI.Instance._playerSkillCoolUIElement[2].DrawForce();

            if (!currentSkillButtonGameObject.activeSelf) {  // currentSkillButton 오브젝트가 비활성화 되어있는 경우 => 창 종료
                StartCoroutine(GlobalAsync.PerformUnScaled(0.5f, CloseSkillSelector));
            }
        }

        public void CloseSkillSelector() {
            actionFromItem = null;
            pauseMenu.CloseMenu();
        }

        public void MoveCurrentHovering(KeyCode key) {  // 스킬 선택 창에서 획득한 스킬을 Q, E, R 스킬창 옆으로 호버링시키는 함수
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