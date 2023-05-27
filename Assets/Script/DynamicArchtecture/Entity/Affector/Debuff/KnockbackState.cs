using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "KnockBack", menuName = "ScriptableObject/EntityAffector/Debuff/KnockBack", order = int.MaxValue)]
public class KnockbackState : EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
    //protected List<IEnumerator> AsyncAffectorCoroutine;
    //protected List<UnityAction> Affector;
    //protected Entity targetEntity //protected Entity ownerEntity;
    public float knockBackForce;

    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.Affector.Add(Knockback);
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AffectHandler(this.Affector);
    }

    public void Knockback(){
        Vector3 dir = this.ownerEntity.entityRigidbody.position - this.targetEntity.entityRigidbody.position; 
        Debug.Log(dir.normalized);
        this.targetEntity.entityRigidbody.AddForce(dir.normalized * knockBackForce, ForceMode.Impulse);
    }
}