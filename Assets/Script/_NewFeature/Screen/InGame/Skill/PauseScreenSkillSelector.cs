using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class PauseScreenSkillSelector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        private static PauseScreenSkillSelector _instance;

        public static PauseScreenSkillSelector Instance {
            get {
                if(_instance == null) {
                    _instance = FindFirstObjectByType(typeof(PauseScreenSkillSelector), findObjectsInactive: FindObjectsInactive.Include) as PauseScreenSkillSelector;
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
        [SerializeField] PauseScreenSkillButton[] collectedSkillButton = new PauseScreenSkillButton[3];
        [SerializeField] PauseUISkillButton pauseUISkillButton;
        [SerializeField] QuitSkillButton quitSkillButton;

        private Canvas _canvas;
        private VerticalLayoutGroup _verticalLayoutGroup;
        private GameObject _dragButtonContent;
        private RectTransform _dragButtonContentRectTransform;
        private int _dragStartIndex;
        private KeyCode _dragStartSkillKey;
        private bool _isDragStart;

        private void Awake()
        {
            _canvas = pauseMenu.GetComponent<Canvas>();
            _verticalLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
        }

        public void OnEnable()
        {
            Debug.Log("PauseScreenSkillSelector) OnEnable");
            StartCoroutine(GlobalAsync.PerformAndRenderUIUnScaled(() => {
                collectedSkillButton[0].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.Q), KeyCode.Q);
                collectedSkillButton[1].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.E), KeyCode.E);
                collectedSkillButton[2].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.R), KeyCode.R);
                quitSkillButton.func                = ClosePauseSkillSelector;
            }));
        }

        public void OpenPauseSkillSelector()
        {
            Debug.Log("PauseScreenSkillSelector) OpenPauseSkillSelector");
            pauseMenu.OpenMenu(gameObject);
        }

        public void ClosePauseSkillSelector()
        {
            pauseMenu.CloseMenu();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("PauseScreenSkillSelector) OnBeginDrag");
            _verticalLayoutGroup.enabled = false;
            for (int i = 0; i < skillButtonTransform.Length; i++) {
                if (RectTransformUtility.RectangleContainsScreenPoint(skillButtonTransform[i], eventData.position)) {
                    _dragStartSkillKey = collectedSkillButton[i].skillKey;
                    if (!player._hasSkill[_dragStartSkillKey])
                        return;
                    _dragStartIndex = i;
                    _dragButtonContent = collectedSkillButton[i].transform.Find("ButtonContent").gameObject;
                    _dragButtonContentRectTransform = _dragButtonContent.GetComponent<RectTransform>();
                    collectedSkillButton[i].transform.SetAsLastSibling();
                    _isDragStart = true;
                    Debug.Log("PauseScreenSkillSelector) _startSkillKey " + _dragStartSkillKey);
                    break;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("PauseScreenSkillSelector) OnDrag");
            if (!_isDragStart)
                return;

            for (int i = 0; i < skillButtonTransform.Length; i++) {
                if (i == _dragStartIndex) continue;
                if (RectTransformUtility.RectangleContainsScreenPoint(skillButtonTransform[i], eventData.position)) {
                    collectedSkillButton[i].transform.Find("ButtonContent/Glow").gameObject.SetActive(false);
                    collectedSkillButton[i].transform.Find("ButtonContent/GlowOnSelected").gameObject.SetActive(true);
                    collectedSkillButton[i].transform.Find("OnSelected").gameObject.SetActive(true);
                } else {
                    collectedSkillButton[i].transform.Find("ButtonContent/Glow").gameObject.SetActive(true);
                    collectedSkillButton[i].transform.Find("ButtonContent/GlowOnSelected").gameObject.SetActive(false);
                    collectedSkillButton[i].transform.Find("OnSelected").gameObject.SetActive(false);
                }
            }
            _dragButtonContentRectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("PauseScreenSkillSelector) OnEndDrag");
            collectedSkillButton[_dragStartIndex].transform.SetSiblingIndex(_dragStartIndex);
            if (_dragButtonContent != null)
                _dragButtonContent.transform.localPosition = Vector3.zero;
            _dragButtonContent = null;
            _dragButtonContentRectTransform = null;
            _dragStartSkillKey = KeyCode.None;
            _isDragStart = false;
            _verticalLayoutGroup.enabled = true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("PauseScreenSkillSelector) OnDrop");
            if (_isDragStart) {
                for (int i = 0; i < skillButtonTransform.Length; i++) {
                    collectedSkillButton[i].transform.Find("ButtonContent/Glow").gameObject.SetActive(true);
                    collectedSkillButton[i].transform.Find("ButtonContent/GlowOnSelected").gameObject.SetActive(false);
                    collectedSkillButton[i].transform.Find("OnSelected").gameObject.SetActive(false);
                    if (RectTransformUtility.RectangleContainsScreenPoint(skillButtonTransform[i], eventData.position)) {
                        var targetSkillKey = collectedSkillButton[i].skillKey;
                        skillManager.SwapSkill(_dragStartSkillKey, targetSkillKey);
                        collectedSkillButton[0].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.Q), KeyCode.Q);
                        collectedSkillButton[1].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.E), KeyCode.E);
                        collectedSkillButton[2].SetUserInterfaceData(skillManager.GetSkillInfoByKey(KeyCode.R), KeyCode.R);
                        pauseUISkillButton.UIUpdate();
                        Debug.Log("PauseScreenSkillButton) skill moved " + _dragStartSkillKey + " to " + targetSkillKey);
                        break;
                    }
                }
            }
            _isDragStart = false;
        }
    }
}
