using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockBackState : DebuffState{
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IEntityAddressable entityAddressable;
    public Rigidbody KnockBacktarget;
    public Rigidbody AttackOwner;

    public KnockBackState(GameObject _attackOwner, GameObject _target) {
        if(_target.TryGetComponent<Rigidbody>(out KnockBacktarget)){Debug.Log("컴포넌트찾음");}
        if(_attackOwner.TryGetComponent<Rigidbody>(out AttackOwner)){Debug.Log("컴포넌트찾음");}
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        this.Affector.Add(Knockback);
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AffectHandler(this.Affector);
    }
    public void Knockback(){
        Vector3 dir =  KnockBacktarget.position - AttackOwner.position;
        Debug.Log(dir.normalized);
        KnockBacktarget.AddForce(dir.normalized * 100, ForceMode.Impulse);
    }
}