using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>

//추후 스크립터블 오브젝트를 삭제하는것을 생각해보자..
// 이유는 웨펀 추가 클래스 만들고 
    // -> 웨펀 프리펩 만들고 
    // -> 스크립터블만들고 
    // -> 스크립터블 웨폰에 넣고 이 과정이 너무 복잡하다.
    // 어차핀 웨폰의 다형성은 프리펩과 상속 클래스로 실현되므로 너무 투머치
    // 굳이 쓸데가 없다는게 결론.
// 없다면 무슨일이 생길까?
// 프리펩으로 다양화를 시키겠지.
// 스크립터블을 없에는 방향으로 생각해보자.

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
