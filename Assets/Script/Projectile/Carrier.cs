using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//Carrier Modifier은 언제나 자기자신에게 버프를 준다.
public class Carrier : MonoBehaviour {
    public List<EntityAffector> carrierEntityAffector;
    public VFXObject destroyEffect = null;

    protected Entity  ownerEntity;
    protected Entity  targetEntity;
    protected Collider carrierCollider = null;
    protected Rigidbody carrierRigidBody = null;
    protected bool    isInitialized = false;

    protected virtual void Awake() {
        if(carrierEntityAffector == null) {throw new System.Exception("인스펙터에에 아무것도 지정이 안됨");}
        TryGetComponent<Collider>(out carrierCollider);
        TryGetComponent<Rigidbody>(out carrierRigidBody);
    }
    public virtual void Initialize(Entity _genOwner){
        if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
        this.ownerEntity = _genOwner;
        transform.localScale *= _genOwner.transform.localScale.x;
        this.isInitialized = true;
    }

    protected virtual void OnTriggerEnter(Collider other){
        if(isInitialized == false) {throw new System.Exception("투사체가 초기화 되지 않음");}
        if(!other.TryGetComponent<Entity>(out targetEntity)){return;}
        if(ownerEntity.GetFinalData().EntityTag != targetEntity.GetFinalData().EntityTag){return;}
        foreach(EntityAffector affector in carrierEntityAffector){
            affector.Init(ownerEntity,targetEntity).Modifiy();
        }
    }
    public void DestroySelf(){
        if(destroyEffect != null) Instantiate(destroyEffect);
        Destroy(gameObject);
    }
    protected virtual void OnDestroy() {
        if(destroyEffect != null) Instantiate(destroyEffect).Initialize();
    }
}