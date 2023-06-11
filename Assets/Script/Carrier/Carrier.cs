using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//Carrier Modifier은 언제나 자기자신에게 버프를 준다.
public enum E_CarrierType {
    Portal = 0, Roulette, Projectile, Dynamics, Item
}
public class Carrier : MonoBehaviour {
    public E_CarrierType carrierType;
    public    VFXObject destroyEffect = null;
    protected Collider  carrierCollider = null;
    protected Rigidbody carrierRigidBody = null;
    protected bool      isInitialized = false;

    protected virtual void Awake() {
        TryGetComponent<Collider>(out carrierCollider);
        TryGetComponent<Rigidbody>(out carrierRigidBody);
    }
    
    public virtual void Initialize(Entity _genOwner){
        if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
        transform.localScale *= _genOwner.transform.localScale.x;
        this.isInitialized = true;
    }

    public void DestroySelf(){
        if(destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity).Initialize();
        Destroy(gameObject);
    }
}