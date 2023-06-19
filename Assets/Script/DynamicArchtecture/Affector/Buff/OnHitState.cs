using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Sophia_Carriers;

[System.Serializable]
public class OnHitState :  EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
    // affectorStruct.affectorType;
    // affectorStruct.AsyncAffectorCoroutine;
    // affectorStruct.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    [HideInInspector] public float Ratio;
    public UnityAction activateTrigger;
    public OnHitState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = STATE_TYPE.ON_HIT;
        //this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        OnHitState Instance = new OnHitState(EAInstance);
        //this.AsyncAffectorCoroutine.Add();
        Instance.isInitialized  = true;
        //Instance.OnEnemyHit     = false;
        Instance.Ratio          = this.Ratio;
        return Instance;
    }

    public Projectile ActivateOnHit(Projectile _projectile){
        Projectile onHitProjectile = _projectile.CloneProjectile();
        onHitProjectile.ProjecttileDamage = Ratio;
        return onHitProjectile;
    }
    public Projectile ActivateOnHitByAffectors(Projectile _projectile, List<EntityAffector> _affectors){
        Projectile onHitProjectile = _projectile.CloneProjectile();
        onHitProjectile.ProjecttileDamage = Ratio;
        
        if(_affectors != null) {foreach (EntityAffector affector in _affectors) { onHitProjectile.projectileAffector.Add(affector); }}
        return onHitProjectile;
    }

    // IEnumerator VisualActivate(){
    //     Debug.Log("활성화");
    //     Debug.Log("비활성화");
    // }
}