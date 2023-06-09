using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BuffProjectile : Carrier {
    // public List<EntityAffector> carrierEntityAffector;
    // public VFXObject destroyEffect = null;
    // protected Entity  ownerEntity;
    // protected Entity  targetEntity;
    // protected Collider carrierCollider = null;
    // protected Rigidbody carrierRigidBody = null;
    // protected bool    isInitialized = false;

    protected override void Awake(){
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider _other){
        if(isInitialized == false) {throw new System.Exception("투사체가 초기화 되지 않음");}
        if(!_other.TryGetComponent<Entity>(out targetEntity)){return;}
        if(ownerEntity.GetFinalData().EntityTag != targetEntity.GetFinalData().EntityTag){return;}
        foreach(EntityAffector affector in carrierEntityAffector){
            affector.Init(ownerEntity, targetEntity).Modifiy();
        }
    }
}