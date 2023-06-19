using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class KnockbackState : EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
    // affectorStruct.affectorType;
    // affectorStruct.AsyncAffectorCoroutine;
    // affectorStruct.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    public float knockBackForce;

    public KnockbackState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = STATE_TYPE.KNOCKBACK;
        this.affectorStruct.Affector.Add(Knockback);
    }

    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        KnockbackState Instance = new KnockbackState(EAInstance);
        Instance.knockBackForce = this.knockBackForce;
        Instance.isInitialized  = true;
        return Instance;
    }

    public void Knockback(){
        Vector3 dir =  this.targetEntity.entityRigidbody.position- this.ownerEntity.entityRigidbody.position; 
        Debug.Log(dir.normalized);
        this.targetEntity.entityRigidbody.AddForce(dir.normalized * knockBackForce, ForceMode.Impulse);
    }
}