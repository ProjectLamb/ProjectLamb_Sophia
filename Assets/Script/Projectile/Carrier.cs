using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//Carrier Modifier은 언제나 자기자신에게 버프를 준다.
public class Carrier : MonoBehaviour {
    public bool isMove;
    //파괴는 무조건 플레이어와 닿았을때, 닿기 전까지는 안됨
    public List<EntityAffector> projectileEntityAffector;
    public GameObject destroyEffect = null;

    Entity  ownerEntity;
    Collider projectileCollider = null;
    Rigidbody projectileRigidBody = null;
    bool    isInitialized = false;
    Entity  targetEntity;

    private void Awake() {
        projectileEntityAffector ??= new List<EntityAffector>();
        TryGetComponent<Collider>(out projectileCollider);
        TryGetComponent<Rigidbody>(out projectileRigidBody);
    }
    private void OnDestroy() {
        if(destroyEffect != null) Instantiate(destroyEffect);
    }

    public void DestroySelf(){
        if(destroyEffect != null) Instantiate(destroyEffect);
        Destroy(gameObject);
    }

    public void Initialize(Entity _genOwner){
        if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
        this.ownerEntity = _genOwner;
        this.isInitialized = true;
        transform.localScale *= _genOwner.transform.localScale.x;
    }

    private void OnTriggerEnter(Collider other){
        if(isInitialized == false) {throw new System.Exception("투사체가 초기화 되지 않음");}
        if(!other.TryGetComponent<Entity>(out targetEntity)){return;}
        if(ownerEntity.GetEntityData().EntityTag != targetEntity.GetEntityData().EntityTag){return;}
        foreach(EntityAffector affector in projectileEntityAffector){
            affector.Init(targetEntity,targetEntity);
            affector.Modifiy();
        }
    }
}