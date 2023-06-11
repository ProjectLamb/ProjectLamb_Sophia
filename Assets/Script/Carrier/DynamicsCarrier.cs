using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DynamicsCarrier : Carrier {
    public float   destroyTime = 0.5f;
    public List<EntityAffector> projectileAffector;
    Entity  ownerEntity;
    Entity  targetEntity;

    public override void Initialize(Entity _genOwner){
        ownerEntity     = _genOwner;
        if(projectileAffector == null) {throw new System.Exception("인스펙터에에 아무것도 지정이 안됨");}
        this.isInitialized = true;
    }

    // public void DestroySelf(){
    //    if(destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity).Initialize();
    //    Destroy(gameObject);ㅗㅜㅍ
    //}

    protected void OnTriggerEnter(Collider other){
        if(isInitialized == false) {throw new System.Exception("투사체가 초기화 되지 않음");}
        if(!other.TryGetComponent<Entity>(out targetEntity)){return;}
        
        if(ownerEntity.GetFinalData().EntityTag != targetEntity.GetFinalData().EntityTag){return;}
        if(projectileAffector == null) return;
        
        foreach (EntityAffector affector in projectileAffector) {
            affector.Init(ownerEntity, ownerEntity).Modifiy();
        }
    }
}