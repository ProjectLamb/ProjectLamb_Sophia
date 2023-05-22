using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>

public class Weapon_TripleAttack : Weapon
{
    int mAttackCount = 0;
    int attackCount {
        get {return mAttackCount;}
        set {
            if(value > 2){mAttackCount = 0; return;}
            mAttackCount = value;
        }
    }

    public override void Use(AddingData AddingData){
        SubUse();
    }
    
    public void SubUse(){
        if(!this.mIsReady) return;
        mIsReady = false;
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);
        Vector3 EffectRotate = transform.eulerAngles;
        EffectRotate += weaponData.AttackProjectiles[attackCount].transform.eulerAngles;
        //Instantiate(weaponEffect[attackCount], transform.position, Quaternion.Euler(EffectRotate)).GetComponent<CombatEffect>().SetDatas(this.playerData, this.weaponData);
        attackCount = attackCount + 1;
    }
    public override IEnumerator CoWaitUse(){
        PlayerController.IsMoveAllow = false;
        yield return YieldInstructionCache.WaitForSeconds(weaponData.WeaponDelay);
        mIsReady = true;
        PlayerController.IsMoveAllow = true;
    }
}
