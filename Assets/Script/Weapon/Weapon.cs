using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    public ScriptableObjWeaponData ScriptableWD;
    public List<Projectile> AttackProjectiles;
    public ProjectileBucket projectileBucket;
    private Entity ownerEntity;
    protected bool isReady = true;
    protected bool isInitialized = false;

    public virtual void Initialisze(Player _owner) {
        ownerEntity = _owner;
        projectileBucket = _owner.projectileBucket;
        isInitialized = true;
    }

    public virtual void Use(int _amount){
        if(isInitialized == false) {throw new System.Exception("무기가 초기화 되지 않음 WeaponManager에서 weapon.Initialize(Entity _owner); 했는지 확인");}
        if(!isReady) return;
        Projectile  useProjectile   = AttackProjectiles[0];
        useProjectile.Initialize(ownerEntity);
        useProjectile.transform.localScale *= PlayerDataManager.GetWeaonData().Range;
        
        _amount = (int)(_amount * PlayerDataManager.GetWeaonData().DamageRatio);
        
        projectileBucket.ProjectileInstantiatorByDamage(ownerEntity, useProjectile, E_BucketPosition.Outer,_amount);
        PlayerDataManager.GetEntityData().AttackState.Invoke();
        WaitWeaponDelay();
    }
    public IEnumerator CoWaitUse(float waitSecondTime){
        isReady = false;
        yield return YieldInstructionCache.WaitForSeconds(waitSecondTime);
        isReady = true;
    }

    public void WaitWeaponDelay(){
        float waitSecondTime = PlayerDataManager.GetPlayerData().EntityDatas.AttackSpeed * PlayerDataManager.GetWeaonData().WeaponDelay;
        StartCoroutine(CoWaitUse(waitSecondTime));
    }
}
