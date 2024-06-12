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
        [SerializeField] PlayerInput playerInput;
        [SerializeField] SkillIndicator skillIndicator;

        static public bool IsMoveAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsAttackAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsReversedInput = false; //인풋을 받을수 있는지 없는지

        private void Awake()
        {
            IsMoveAllow = true;
            IsAttackAllow = true;
            TryGetComponent<PlayerInput>(out playerInput);
        }

        private void FixedUpdate()
        {
            if (StoryManager.Instance.IsTutorial)
            {
                DisallowInput();
                return;
            }
            playerRef.MoveTick();
        }

        private void Update()
        {
            if (GameManager.Instance.GlobalEvent.IsGamePaused) return;
            //playerRef.AimAssist();
            //playerRef.CheckAttack();
            if (StoryManager.Instance.IsTutorial || TextManager.Instance.IsStory) // 스토리대사가 진행중이면 입력 제한
            {
                DisallowInput();
                return;
            }
            else if (!StoryManager.Instance.IsTutorial && !TextManager.Instance.IsStory) // 스토리대사가 끝나면 입력 복구
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
                // 키가 눌려있을때에는 Indicate 함수를 통해 사거리를 띄우고,
                // 키가 떼어졌을때에는 기존의 Use 함수를 사용하여 스킬을 사용하도록 하자.
                if (Input.GetKeyDown(KeyCode.Q)) {playerRef.Indicate(KeyCode.Q);}
                if (Input.GetKeyDown(KeyCode.E)) {playerRef.Indicate(KeyCode.E);}
                if (Input.GetKeyDown(KeyCode.R)) {playerRef.Indicate(KeyCode.R);}

                if (Input.GetKeyUp(KeyCode.Q)) { playerRef.Use(KeyCode.Q); skillIndicator.currentSkillName = "";}
                if (Input.GetKeyUp(KeyCode.E)) { playerRef.Use(KeyCode.E); skillIndicator.currentSkillName = "";}
                if (Input.GetKeyUp(KeyCode.R)) { playerRef.Use(KeyCode.R); skillIndicator.currentSkillName = "";}
                
                if (Input.GetMouseButtonDown(0)) { playerRef.Attack(); }
            }
        }
        private void DisallowInput()
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