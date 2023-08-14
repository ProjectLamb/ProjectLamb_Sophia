using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    static public bool isAttack = false;
    static public bool isThrAttack = false;
    // Start is called before the first frame update
    void attackStart(){
        isAttack = true;
    }

    void attackEnd(){
        isAttack = false;
    }

    void thrAttackOn(){ // 세번째 공격이 실행됨
        isThrAttack = true;
    }

    void thrAttackOff(){ // 세번째 공격이 종료되고 idle 상태로 전환
        isThrAttack = false;
    }

    public bool nowAttack(){
        return isAttack;
        //return isThrAttack; ??
    }
}
