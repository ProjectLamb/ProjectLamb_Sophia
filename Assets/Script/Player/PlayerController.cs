using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 키보드의 인풋에 따른 동작을 담당한다.
/// PlayerAction.cs과 연관 관계다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlayerAction playerAction;

    [field: SerializeField]
    public bool IsInputAllow = true; //인풋을 받을수 있는지 없는지
    private void Awake() {
        if (!TryGetComponent<PlayerAction>(out playerAction)) { Debug.Log("컴포넌트 로드 실패 : PlayerAction"); }
    }

    private void Update() {
        if(!IsInputAllow) return; //인풋을 받을지 말지.

        playerAction.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if(Input.GetKeyDown(KeyCode.Space)){playerAction.Dash();}
        if(Input.GetKeyDown(KeyCode.Q)){playerAction.Skill("Q");}
        if(Input.GetKeyDown(KeyCode.E)){playerAction.Skill("E");}
        if(Input.GetKeyDown(KeyCode.R)){playerAction.Skill("R");}
        if(Input.GetMouseButtonDown(0)){playerAction.Attack();}
    }
}