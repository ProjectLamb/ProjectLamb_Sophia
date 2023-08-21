using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    static public bool isAttack = false;
    static public bool isThrAttack = false;
    static public bool canExitAttack = false;
    // Start is called before the first frame update
    void AttackStart()
    {
        isAttack = true;
    }

    void AttackEnd()
    {
        isAttack = false;
    }

    void ThrAttackOn()
    { // 세번째 공격이 실행됨
        isThrAttack = true;
    }

    void ThrAttackOff()
    { // 세번째 공격이 종료되고 idle 상태로 전환
        isThrAttack = false;
    }

    void ExitAttack()
    {
        canExitAttack = true;
    }
    
    void NowMove()
    {
        canExitAttack = false;
        isAttack = false;
    }

    public bool NowAttack(){
        return isAttack;
        return isThrAttack;
        return canExitAttack;
    }
}
