using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    PlayerAction playerAction;
    float mInputHoriontalAxis = 0f;
    float mInputVerticalAxis = 0f;

    bool mInputQ = false;
    bool mInputE = false; 
    bool mInputR = false;
    bool mInputSpace = false;
    bool mInputMouseRight = false;
    bool mInputMouseLeft = false;

    [field: SerializeField]
    public bool IsInputAllow {get; set;} //인풋을 받을수 있는지 없는지

    private void Awake() {
        playerAction = GetComponent<PlayerAction>();
    }

    private void Update() {
        if(!IsInputAllow) return; //인풋을 받을지 말지.

        playerAction.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if(Input.GetKeyDown(KeyCode.Space)){playerAction.Dash();}
        if(Input.GetMouseButtonDown(0)){playerAction.Attack();}
    }
}