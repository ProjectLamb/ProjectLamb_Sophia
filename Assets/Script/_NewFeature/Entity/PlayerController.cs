using System;
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

        public TextManager textManager; // 텍스트 매니저

        static public bool IsMoveAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsAttackAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsReversedInput = false; //인풋을 받을수 있는지 없는지

        private void Awake()
        {
            IsMoveAllow = true;
        }

        private void FixedUpdate()
        {
            if (TextManager.Instance.IsStory)
            {
                ReverseInput();
                return;
            }
            playerRef.MoveTick();
        }

        private void Update()
        {

            if (GameManager.Instance.GlobalEvent.IsGamePaused) return;
            //playerRef.AimAssist();
            //playerRef.CheckAttack();
            if (TextManager.Instance.IsStory) // 스토리대사가 진행중이면 입력 제한
            {
                ReverseInput();
                return;
            }
            else if (!TextManager.Instance.IsStory) // 스토리대사가 끝나면 입력 복구
            {
                AllowInput();
            }
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
        private void ReverseInput()
        {
            IsMoveAllow = false;
            IsAttackAllow = false;
            IsReversedInput = false;
        }
        private void AllowInput()
        {
            IsMoveAllow = true;
            IsAttackAllow = true;
            IsReversedInput = true;
        }
    }
}