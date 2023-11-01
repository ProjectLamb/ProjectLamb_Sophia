using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;
using Cysharp.Threading.Tasks;

public class Weapon : MonoBehaviour
{
    public static bool                  IsReady = true;
    public ScriptableObjWeaponData      ScriptableWD;
    public List<Projectile>             AttackProjectiles;
    internal WEAPON_STATE               originState = WEAPON_STATE.NORMAL;
    internal WEAPON_STATE               curState = WEAPON_STATE.NORMAL;
    internal Queue<Projectile>          OnHitProjectiles = new Queue<Projectile>();
    protected CarrierBucket             projectileBucket;
    protected bool                      isInitialized = false;
    protected Entity                    ownerEntity;

    public virtual void ChangeState(WEAPON_STATE _STATE){ curState = _STATE;}

    public virtual void Initialisze(Player _owner)
    {
        ownerEntity = _owner;
        projectileBucket = _owner.carrierBucket;
        curState = WEAPON_STATE.NORMAL;
        isInitialized = true;
    }

    public virtual void Use(int _amount)
    {
        switch (curState)
        {
            case WEAPON_STATE.NORMAL :
                UseNormal(_amount);
                break;
            case WEAPON_STATE.ON_HIT :
                UseOnHit(_amount);
                break;
        }
    }
    
    private void UseNormal(int _amount){
        if (isInitialized == false) { throw new System.Exception("무기가 초기화 되지 않음 WeaponManager에서 weapon.Initialize(Entity _owner); 했는지 확인"); }
        if (!IsReady) return;
        PlayerDataManager.GetEntityData().AttackState.Invoke();
        Projectile useProjectile = AttackProjectiles[0].CloneProjectile();
        useProjectile.Init(ownerEntity);
        useProjectile.ProjecttileDamage = _amount * PlayerDataManager.GetWeaonData().DamageRatio;
        //useProjectile.SetScale(PlayerDataManager.GetWeaonData().Range);
        projectileBucket.CarrierTransformPositionning(ownerEntity, useProjectile);
        //Carrier useProjectile = projectileBucket.CarrierInstantiatorByObjects(ownerEntity, AttackProjectiles[0], new object[1] {_amount});
        WaitWeaponDelay();
    }

    private void UseOnHit(int _amount){
        if (isInitialized == false) { throw new System.Exception("무기가 초기화 되지 않음 WeaponManager에서 weapon.Initialize(Entity _owner); 했는지 확인"); }
        PlayerDataManager.GetEntityData().AttackState.Invoke();
        Projectile useProjectile = OnHitProjectiles.Dequeue();
        useProjectile.Init(ownerEntity);
        useProjectile.ProjecttileDamage = _amount * useProjectile.ProjecttileDamage * PlayerDataManager.GetWeaonData().DamageRatio;
        //useProjectile.SetScale(PlayerDataManager.GetWeaonData().Range);
        projectileBucket.CarrierTransformPositionning(ownerEntity, useProjectile);
        if(OnHitProjectiles.Count == 0) ChangeState(originState);
    }

    public async UniTaskVoid AsyncWaitUse(float _waitSecondTime)
    {
        IsReady = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
        IsReady = true;
    }

    public void WaitWeaponDelay()
    {
        float waitSecondTime = PlayerDataManager.GetPlayerData().EntityDatas.AttackSpeed * PlayerDataManager.GetWeaonData().WeaponDelay;
        AsyncWaitUse(waitSecondTime).Forget();
    }
}
