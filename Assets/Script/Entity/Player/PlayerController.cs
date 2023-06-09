using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 키보드의 인풋에 따른 동작을 담당한다.
/// Player.cs과 연관 관계다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    Player player;
    static public bool IsMoveAllow = true; //인풋을 받을수 있는지 없는지
    static public bool IsAttackAllow = true; //인풋을 받을수 있는지 없는지
    static public bool IsReversedInput = false; //인풋을 받을수 있는지 없는지
    private void Awake() {
        if (!TryGetComponent<Player>(out player)) { Debug.Log("컴포넌트 로드 실패 : Player"); }
    }

    private void Update() {
        if(IsMoveAllow){ 
            player.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(Input.GetKeyDown(KeyCode.Space)){player.Dash();}
            player.anim.SetFloat("Move", player.entityRigidbody.velocity.magnitude);
        }
        if(IsAttackAllow){
            //if(Input.GetKeyDown(KeyCode.Q)){player.Skill("Q");}
            //if(Input.GetKeyDown(KeyCode.E)){player.Skill("E");}
            //if(Input.GetKeyDown(KeyCode.R)){player.Skill("R");}
            if(Input.GetMouseButtonDown(0)){player.Attack();}
        }
    }
}