using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;
using Cysharp.Threading.Tasks;

public class Weapon_Melee_Mace : Weapon
{
    //public static bool                  IsReady = true;
    //public ScriptableObjWeaponData      ScriptableWD;
    //public List<Projectile>             AttackProjectiles;
    //internal WEAPON_STATE               originState = WEAPON_STATE.NORMAL;
    //internal WEAPON_STATE               curState = WEAPON_STATE.NORMAL;
    //internal Queue<Projectile>          OnHitProjectiles = new Queue<Projectile>();
    //protected CarrierBucket             projectileBucket;
    //protected bool                      isInitialized = false;
    //protected Entity                    ownerEntity;
    Player player;

    public int currentProjectileIndex = 0;
    public override void Initialisze(Player _owner)
    {
        ownerEntity = _owner;
        projectileBucket = _owner.carrierBucket;
        curState = WEAPON_STATE.NORMAL;
        currentProjectileIndex = 0;
        isInitialized = true;
    }
    
    public override void Use(int _amount)
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

    private void UseNormal(int _amount) {
        if (isInitialized == false) { throw new System.Exception("무기가 초기화 되지 않음 WeaponManager에서 weapon.Initialize(Entity _owner); 했는지 확인"); }
        if (!IsReady) return;
        PlayerDataManager.GetEntityData().AttackState.Invoke();
        Projectile useProjectile = AttackProjectiles[currentProjectileIndex++].CloneProjectile();
        useProjectile.Init(ownerEntity);
        useProjectile.ProjecttileDamage = _amount * PlayerDataManager.GetWeaonData().DamageRatio;
        useProjectile.SetScale(PlayerDataManager.GetWeaonData().Range);
        projectileBucket.CarrierTransformPositionning(ownerEntity, useProjectile);
        currentProjectileIndex %= 3;
        WaitWeaponDelay();
    }
    private void UseOnHit(int _amount){
        if (isInitialized == false) { throw new System.Exception("무기가 초기화 되지 않음 WeaponManager에서 weapon.Initialize(Entity _owner); 했는지 확인"); }
        PlayerDataManager.GetEntityData().AttackState.Invoke();
        Projectile useProjectile = OnHitProjectiles.Dequeue();
        useProjectile.Init(ownerEntity);
        useProjectile.ProjecttileDamage = _amount * useProjectile.ProjecttileDamage * PlayerDataManager.GetWeaonData().DamageRatio;
        useProjectile.SetScale(PlayerDataManager.GetWeaonData().Range);
        projectileBucket.CarrierTransformPositionning(ownerEntity, useProjectile);
        if(OnHitProjectiles.Count == 0) ChangeState(originState);
    }
}