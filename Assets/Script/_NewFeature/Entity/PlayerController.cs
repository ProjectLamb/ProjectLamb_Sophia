using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// 키보드의 인풋에 따른 동작을 담당한다.
/// Player.cs과 연관 관계다.
/// </summary>

namespace Sophia.Entitys
{
    using Sophia.Instantiates;
    using Sophia.UserInterface;

    public class PlayerController : MonoBehaviour
    {
        #region Serialized Members
        [SerializeField] Player playerRef;
        [SerializeField] ModelDebugger modelDebuggerRef;
        [SerializeField] PlayerInput playerInput;
        [SerializeField] SkillIndicator skillIndicator;

        #endregion

        public Ray ray;

        #region Dictionary

        private static readonly Dictionary<string, bool> mIsMoveAllow = new Dictionary<string, bool>();
        private static readonly Dictionary<string, bool> mIsAttackAllow = new Dictionary<string, bool>();
        private static readonly Dictionary<string, bool> mIsReversedInput = new Dictionary<string, bool>();

        #endregion

        public static bool IsMoveAllow
        {
            get { return mIsMoveAllow.All(x => x.Value == true); }
        }
        public static bool IsAttackAllow
        {
            get { return mIsAttackAllow.All(x => x.Value == true); }
        }

        public static void SetMoveStateByHandlersString(string handler, bool moveState)
        {
            if (!mIsMoveAllow.ContainsKey(handler))
            {
                mIsMoveAllow.TryAdd(handler, moveState); return;
            }
            mIsMoveAllow[handler] = moveState;
        }
        public static void SetAttackStateByHandlersString(string handler, bool attackState)
        {
            if (!mIsAttackAllow.ContainsKey(handler))
            {
                mIsAttackAllow.TryAdd(handler, attackState); return;
            }
            mIsAttackAllow[handler] = attackState;
        }

        static public bool IsReversedInput = false; //인풋을 받을수 있는지 없는지

        private void Awake()
        {
            SetMoveStateByHandlersString(this.name, true);
            SetAttackStateByHandlersString(this.name, true);
            TryGetComponent<PlayerInput>(out playerInput);
        }

        private void Start()
        {
            GameManager.Instance.GlobalEvent.OnPlayEvent.AddListener(AllowInput);
            GameManager.Instance.GlobalEvent.OnPausedEvent.AddListener(DisallowInput);
        }

        private void FixedUpdate()
        {
            if (IsMoveAllow && IsAttackAllow) playerRef.MoveTick();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void Update()
        {
            // if (GameManager.Instance.GlobalEvent.IsGamePaused) return;
            //playerRef.AimAssist();
            //playerRef.CheckAttack();

            if (Input.GetKeyDown(KeyCode.Tab)) modelDebuggerRef.ToggleMenu();

            if (IsMoveAllow)
            {
                if (Input.GetKeyDown(KeyCode.Space)) { playerRef.Dash(); }
            }

            if (IsAttackAllow)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerRef.Attack();
                    playerRef.ClickEffect();
                }
                // 키가 눌려있을때에는 Indicate 함수를 통해 사거리를 띄우고,
                // 키가 떼어졌을때에는 기존의 Use 함수를 사용하여 스킬을 사용하도록 하자.
                if (Input.GetKeyDown(KeyCode.Q)) { playerRef.Indicate(KeyCode.Q); }
                if (Input.GetKeyDown(KeyCode.E)) { playerRef.Indicate(KeyCode.E); }
                if (Input.GetKeyDown(KeyCode.R)) { playerRef.Indicate(KeyCode.R); }

                if (Input.GetKeyUp(KeyCode.Q)) { playerRef.Use(KeyCode.Q); skillIndicator.IsIndicate = false; }
                if (Input.GetKeyUp(KeyCode.E)) { playerRef.Use(KeyCode.E); skillIndicator.IsIndicate = false; }
                if (Input.GetKeyUp(KeyCode.R)) { playerRef.Use(KeyCode.R); skillIndicator.IsIndicate = false; }
            }
        }
        public static void DisallowInput(string handler)
        {
            SetMoveStateByHandlersString(handler, false);
            SetAttackStateByHandlersString(handler, false);
            IsReversedInput = false;
        }
        public static void AllowInput(string handler)
        {
            SetMoveStateByHandlersString(handler, true);
            SetAttackStateByHandlersString(handler, true);
            IsReversedInput = true;
        }
    }
}