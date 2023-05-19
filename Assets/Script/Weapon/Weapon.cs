using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public PlayerData playerData;
    public WeaponData weaponData;
    public List<Projectile> weaponEffect;

    protected bool mIsReady = true;
    protected IEnumerator mCoWaitUse;
    
    private void Awake() {
        if(!TryGetComponent<WeaponData>(out weaponData)) {Debug.Log("컴포넌트 로드 실패 : WeaponData");}
    }

    public virtual void Use(){
        if(!mIsReady) return;
        mIsReady = false;
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);
        Vector3 EffectRotate = transform.eulerAngles;
        EffectRotate += weaponEffect[0].transform.eulerAngles;
        weaponEffect[0].InstanciateProjectile(gameObject, transform, Quaternion.Euler(EffectRotate));
    }

    public virtual IEnumerator CoWaitUse(){
        yield return YieldInstructionCache.WaitForSeconds(weaponData.numericData.WeaponDelay);
        mIsReady = true;
    }

}
