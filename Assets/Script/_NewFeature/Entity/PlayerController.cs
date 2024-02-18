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

        static public bool IsMoveAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsAttackAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsReversedInput = false; //인풋을 받을수 있는지 없는지

        private void Awake() {
            IsMoveAllow = true;
        }

        private void FixedUpdate() {
            playerRef.MoveTick();
        }

        private void Update() {
            //playerRef.AimAssist();
            //playerRef.CheckAttack();
            if(Input.GetKeyDown(KeyCode.Tab)) modelDebuggerRef.ToggleMenu();

            if(IsMoveAllow){ 
                if(Input.GetKeyDown(KeyCode.Space)){playerRef.Dash();}
            }
            
            if(IsAttackAllow){
                // if(Input.GetKeyDown(KeyCode.Q)){playerRef.Skill(E_SKILL_KEY.QKey);}
                // if(Input.GetKeyDown(KeyCode.E)){playerRef.Skill(E_SKILL_KEY.EKey);}
                // if(Input.GetKeyDown(KeyCode.R)){playerRef.Skill(E_SKILL_KEY.RKey);}
                if(Input.GetMouseButtonDown(0)){playerRef.Attack();}
            }
        }
    }
}