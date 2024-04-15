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
        [SerializeField] public PlayerSkillCoolUI[]     _playerSkillCoolUIs;
        [SerializeField] public HitCanvasShadeScript    _hitCanvasShadeScript;
        [SerializeField] public EquipmentDescriptionUI  _equipmentDescriptionUI;
        [SerializeField] public FadeUI                  _fadeUI;
        [SerializeField] public VideoController         _videoController;
        [SerializeField] public GameObject              _bossHealthBar;
        //Demo Version
        [SerializeField] public GameObject              demoClear;
    }
}