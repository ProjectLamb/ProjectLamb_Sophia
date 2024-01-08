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
    public class PlayerController : MonoBehaviour
    {
        Player player;

        static public bool IsMoveAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsAttackAllow = true; //인풋을 받을수 있는지 없는지
        static public bool IsReversedInput = false; //인풋을 받을수 있는지 없는지

        private void Awake() {
            if (!TryGetComponent<Player>(out player)) { Debug.Log("컴포넌트 로드 실패 : Player"); }
            IsMoveAllow = true;
        }

        private void FixedUpdate() {
            player.MoveTick();
        }

        private void Update() {
            //player.AimAssist();
            //player.CheckAttack();

            if(IsMoveAllow){ 
                if(Input.GetKeyDown(KeyCode.Space)){player.Dash();}
            }
            
            // if(IsAttackAllow){
            //     if(Input.GetKeyDown(KeyCode.Q)){player.Skill(SKILL_KEY.Q);}
            //     if(Input.GetKeyDown(KeyCode.E)){player.Skill(SKILL_KEY.E);}
            //     if(Input.GetKeyDown(KeyCode.R)){player.Skill(SKILL_KEY.R);}
            //     if(Input.GetMouseButtonDown(0)){player.Attack();}
            // }
        }
    }
}