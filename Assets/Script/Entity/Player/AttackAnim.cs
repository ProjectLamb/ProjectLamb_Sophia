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
    
    static public bool isAttack = false;
    static public bool attackTrigger = false;
    static public bool canExitAttack = false;
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
    
    void AttackStart()
    {
        isAttack = true;
    }

    void AttackEnd()
    {
        isAttack = false;
    }

    void ExitAttack()
    {
        canExitAttack = true;
    }
    
    void NowMove() // 이동 중일때
    {
        canExitAttack = false;
        isAttack = false;
        attackTrigger = true;
        attackProTime = false;
    }

    void ResetAttack()
    {
        attackTrigger = false;
    }

    void AttackPro()
    {
        attackProTime = true;
    }
}
