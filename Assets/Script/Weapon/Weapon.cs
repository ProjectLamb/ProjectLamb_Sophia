using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public PlayerData playerData;
    WeaponData weaponData;
    public GameObject weaponEffect;

    bool mIsReady = true;
    IEnumerator mCoWaitUse;
    
    private void Awake() {
        if(!TryGetComponent<WeaponData>(out weaponData)) {Debug.Log("컴포넌트 로드 실패 : WeaponData");}
    }

    public void Use(){
        if(!mIsReady) return;
        mIsReady = false;
        Instantiate(weaponEffect, transform.position, transform.rotation).GetComponent<CombatEffect>().SetDatas(this.playerData, this.weaponData);
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);
    }

    IEnumerator CoWaitUse(){
        yield return new WaitForSeconds(weaponData.numericData.WeaponDelay);
        mIsReady = true;
    }
}
