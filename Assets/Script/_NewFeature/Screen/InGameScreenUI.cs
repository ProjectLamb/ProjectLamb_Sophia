using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sophia.Composite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sophia.UserInterface
{

    public class InGameScreenUI : MonoBehaviour {
        private static InGameScreenUI _instance;
        public static InGameScreenUI Instance {
            get {
                if(_instance == null) {
                    _instance = FindFirstObjectByType(typeof(InGameScreenUI)) as InGameScreenUI;
                    if(_instance == null)
                        throw new System.NullReferenceException("InGameScreenUI 게임 오브젝트를 찾을 수 없음");
                }
                return _instance;
            }
            set {}
        }
        
        [SerializeField] public Entitys.Player          _player;
        [SerializeField] public PlayerHealthBarUI       _playerHealthBarUI;
        [SerializeField] public Slider                  _playerBarrierBarUI;
        [SerializeField] public PlayerStaminaBarUI      _playerStaminaBarUI;
        [SerializeField] public PlayerWealthBar         _playerWealthBarUI;
        [SerializeField] public GameObject              _playerSkillCoolUI;
        [SerializeField] public Minimap                 _chapterMinimap;
        [SerializeField] public PlayerSkillCoolUI[]     _playerSkillCoolUIElement;
        [SerializeField] public HitCanvasShadeScript    _hitCanvasShadeScript;
        [SerializeField] public EquipmentDescriptionUI  _equipmentDescriptionUI;
        [SerializeField] public FadeUI                  _fadeUI;
        [SerializeField] public VideoController         _videoController;
        [SerializeField] public GameObject              _bossHealthBar;
        //Demo Version
        [SerializeField] public GameObject              demoClear;

        public void UIVisibleOn() 
        {
            _playerHealthBarUI.gameObject.SetActive(true);
            _playerBarrierBarUI.gameObject.SetActive(true);
            _playerStaminaBarUI.gameObject.SetActive(true);
            _playerWealthBarUI.gameObject.SetActive(true);
            _chapterMinimap.gameObject.SetActive(true);
            _playerSkillCoolUI.SetActive(true);
        }

        public void UIVisibleOff() 
        {
            _playerHealthBarUI.gameObject.SetActive(false);
            _playerBarrierBarUI.gameObject.SetActive(false);
            _playerStaminaBarUI.gameObject.SetActive(false);
            _playerWealthBarUI.gameObject.SetActive(false);
            _chapterMinimap.gameObject.SetActive(false);
            _playerSkillCoolUI.SetActive(false);
        }

    }
}