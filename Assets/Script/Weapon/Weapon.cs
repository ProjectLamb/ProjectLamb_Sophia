using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>

public class Weapon : MonoBehaviour
{
    public ScriptableObjWeaponData scriptableObjWeapon;
    public WeaponData weaponData;

    protected bool mIsReady = true;
    protected IEnumerator mCoWaitUse;
    
    private void Awake() {
        //if(!TryGetComponent<WeaponData>(out weaponData)) {Debug.Log("컴포넌트 로드 실패 : WeaponData");}
        weaponData = new WeaponData(scriptableObjWeapon);
    }

    public virtual void Use(){
        if(!mIsReady) return;
        mIsReady = false;
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);
        Vector3 EffectRotate = transform.eulerAngles;
        EffectRotate += weaponData.Projectile[0].transform.eulerAngles;
        weaponData.Projectile[0].InstanciateProjectile(gameObject, transform, Quaternion.Euler(EffectRotate));
    }

    public virtual IEnumerator CoWaitUse(){
        yield return YieldInstructionCache.WaitForSeconds(weaponData.WeaponDelay);
        mIsReady = true;
    }

}
