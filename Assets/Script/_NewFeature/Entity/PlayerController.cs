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
        [SerializeField] Player playerRef;
        [SerializeField] ModelDebugger modelDebuggerRef;
        [SerializeField] PlayerInput playerInput;

        private static readonly Dictionary<string, bool> mIsMoveAllow = new Dictionary<string, bool>();
        private static readonly Dictionary<string, bool> mIsAttackAllow = new Dictionary<string, bool>();
        private static readonly Dictionary<string, bool> mIsReversedInput = new Dictionary<string, bool>();

        public static bool IsMoveAllow {
            get {return mIsMoveAllow.All(x => x.Value == true);}
        }
        public static bool IsAttackAllow {
            get {return mIsAttackAllow.All(x => x.Value == true);}
        }

        public static void SetMoveStateByHandlersString(string handler, bool moveState) {
            if(!mIsMoveAllow.ContainsKey(handler)) {
                mIsMoveAllow.TryAdd(handler, moveState); return;
            }
            mIsMoveAllow[handler] = moveState;
        }
        public static void SetAttackStateByHandlersString(string handler, bool attackState) {
            if(!mIsAttackAllow.ContainsKey(handler)) {
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
            if(IsMoveAllow && IsAttackAllow) playerRef.MoveTick();
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
                if (Input.GetKeyDown(KeyCode.Q)) { playerRef.Use(KeyCode.Q); }
                if (Input.GetKeyDown(KeyCode.E)) { playerRef.Use(KeyCode.E); }
                if (Input.GetKeyDown(KeyCode.R)) { playerRef.Use(KeyCode.R); }
                if (Input.GetMouseButtonDown(0)) { playerRef.Attack(); }
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