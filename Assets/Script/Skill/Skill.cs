using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [HideInInspector]
    public PlayerData playerData;
    
    [HideInInspector]
    WeaponData weaponData;

    public GameObject skillEffect;

    bool mIsReady = true;
    IEnumerator mCoWaitUse;

    public void Use(){
        if(!mIsReady) return;
        mIsReady = false;
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);

        Instantiate(skillEffect, transform.position, transform.rotation).GetComponent<CombatEffect>().SetDatas(this.playerData, this.weaponData);
    }
    IEnumerator CoWaitUse(){
        yield return new WaitForSeconds(1f);
        mIsReady = true;
    }
}