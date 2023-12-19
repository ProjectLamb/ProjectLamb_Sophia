using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
        IsMoveAllow = true;
    }

    private void Update() {
        player.AimAssist();
        player.CheckAttack();
        
        if(IsMoveAllow){ 
            player.Move();
            if(Input.GetKeyDown(KeyCode.Space)){
                player.Dash();
            }
        }
        if(IsAttackAllow){
            if(Input.GetKeyDown(KeyCode.Q)){player.Skill(SKILL_KEY.Q);}
            if(Input.GetKeyDown(KeyCode.E)){player.Skill(SKILL_KEY.E);}
            if(Input.GetKeyDown(KeyCode.R)){player.Skill(SKILL_KEY.R);}
            if(Input.GetMouseButtonDown(0)){player.Attack();}
        }
    }
    Vector3 inputDirection;
    private void OnMovement(InputValue value) {
        Vector2 inputMovement = value.Get<Vector2>();
        inputDirection = new Vector3(inputMovement.x, 0, inputMovement.y);
        Debug.Log(inputDirection);
    }

    private void OnAttack(InputAction value) {
        Debug.Log("Input Attack");
    }
}