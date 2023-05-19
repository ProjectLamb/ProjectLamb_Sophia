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
    Player Player;
    static public bool IsMoveAllow = true; //인풋을 받을수 있는지 없는지
    static public bool IsAttackAllow = true; //인풋을 받을수 있는지 없는지
    static public bool IsReversedInput = false; //인풋을 받을수 있는지 없는지
    private void Awake() {
        if (!TryGetComponent<Player>(out Player)) { Debug.Log("컴포넌트 로드 실패 : Player"); }
    }

    private void Update() {
        if(IsMoveAllow){ 
            Player.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), IsReversedInput);
            if(Input.GetKeyDown(KeyCode.Space)){Player.Dash();}
        }
        if(IsAttackAllow){
            if(Input.GetKeyDown(KeyCode.Q)){Player.Skill("Q");}
            if(Input.GetKeyDown(KeyCode.E)){Player.Skill("E");}
            if(Input.GetKeyDown(KeyCode.R)){Player.Skill("R");}
            if(Input.GetMouseButtonDown(0)){Player.Attack();}
        }
    }
}