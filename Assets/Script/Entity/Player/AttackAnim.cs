using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class AttackAnim : MonoBehaviour
{
    //public UnityEvent[] animCallback;

    public Player player;
    public Weapon_Melee_Mace weapon;
    
    static public bool isAttack = false; // 공격 가능 여부
    static public bool resetAtkTrigger = false; // doattack 트리거 reset 여부(true일때 reset)
    static public bool canExitAttack = false; // 공격 중 탈출 가능 여부
    static public bool attackProTime = false;

    private void OnEnable() {
        weapon = (Weapon_Melee_Mace) player.weaponManager.weapon;
    }
    void AttackProjectile1(){
        weapon.currentProjectileIndex = 0;
        player.JustAttack();
    }
    void AttackProjectile2(){
        weapon.currentProjectileIndex = 1;
        player.JustAttack();
    }
    void AttackProjectile3(){
        weapon.currentProjectileIndex = 2;
        player.JustAttack();
    }

    // Start is called before the first frame update
    
    void AttackStart() // 공격 시작 시점
    {
        isAttack = true;
    }

    void AttackEnd() // 공격 종료 시점
    {
        isAttack = false;
    }

    void ExitAttack() // 공격 애니메이션 중 이동 입력 시 탈출
    {
        canExitAttack = true;
    }
    
    void ResetIdle() // idle 상태 돌입 시 변수 초기화
    {
        canExitAttack = false;
        isAttack = false;
        attackProTime = false;
        resetAtkTrigger = false;
    }

    void AttackPro()
    {
        attackProTime = true;
    }

    void thrAttackEnd()
    {
        isAttack = false;
        resetAtkTrigger = true;
    }

    void thrAttackExit()
    {
        canExitAttack = true;
        resetAtkTrigger = true;
    }
}
