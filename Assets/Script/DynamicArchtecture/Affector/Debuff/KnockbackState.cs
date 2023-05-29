using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "KnockBack", menuName = "ScriptableObject/EntityAffector/Debuff/KnockBack", order = int.MaxValue)]
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

    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.affectorStruct.affectorType = E_StateType.KnockBack;
        this.affectorStruct.Affector.Add(Knockback);
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AffectHandler(affectorStruct);
    }

    public void Knockback(){
        Vector3 dir = this.ownerEntity.entityRigidbody.position - this.targetEntity.entityRigidbody.position; 
        Debug.Log(dir.normalized);
        this.targetEntity.entityRigidbody.AddForce(dir.normalized * knockBackForce, ForceMode.Impulse);
    }
}