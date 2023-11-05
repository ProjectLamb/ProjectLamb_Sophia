using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    public bool isAttack = false;
    public bool isThrAttack = false;
    public bool canExitAttack = false;
    public bool attackProTime = false;
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
    
    void NowMove() // 이동 중일때
    {
        canExitAttack = false;
        isAttack = false;
        attackProTime = false;
    }

    void AttackPro()
    {
        attackProTime = true;
    }
}
