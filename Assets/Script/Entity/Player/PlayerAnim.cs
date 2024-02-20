using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class PlayerAnim : MonoBehaviour
{
    //public UnityEvent[] animCallback;

    public Player player;
    public Weapon_Melee_Mace weapon;
    
    static public bool IsExitAttack = false; // 공격 모션 도중 탈출 여부
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
        player.ChangeState(PLAYERSTATES.Attack);
    }

    void AttackEnd() // 공격 종료 시점
    {
        player.ChangeState(PLAYERSTATES.Idle);
    }

    void ExitAttack() // 공격 애니메이션 중 이동 입력 시 탈출
    {
        IsExitAttack = true;
    }
    
    void IdleStart() // idle 상태 돌입 시 변수 초기화
    {
        IsExitAttack = false;
        player.ChangeState(PLAYERSTATES.Idle);
    }

    void AttackPro()
    {
        attackProTime = true;
    }
}
