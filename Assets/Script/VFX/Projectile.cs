using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Projectile : MonoBehaviour {
    public GameObject OnHitSubEmmiter;

    public Dictionary<Affector_PlayerState ,IModifier> Modifiers;
    
    [HideInInspector]
    public GameObject spawnOwner;
    Collider HitCollider;
    
    private void Awake() {
        TryGetComponent<Collider>(out HitCollider);
    }
    
    private void OnTriggerEnter(Collider other) {
        Debug.Log($"{other.gameObject.name}, Collider Trigger");
        Debug.Log($"{spawnOwner.tag}, {other.gameObject.tag}");
        if(spawnOwner.tag == other.gameObject.tag) {return;}

        //전달 타겟 찾기
        IDamagable damagebleEntity = other.gameObject.GetComponent<IDamagable>();
        IAffectable affectableEntity = other.gameObject.GetComponent<IAffectable>();

        //전달 하기
        damagebleEntity.GetDamaged(_amount: 5, OnHitSubEmmiter);
        affectableEntity.AsyncAffectHandler(new PoisonState(other.gameObject).AsyncAffectorCoroutine);
    }

    //여기를 통해서 이 스크립트를 실행한 놈을 호출 할 수 있다.
    public GameObject InstanciateProjectile(GameObject _owner, Transform parent){
        spawnOwner = _owner;
        GameObject ProjectileObj = Instantiate(gameObject, parent);
        ProjectileObj.transform.position += Vector3.forward * transform.position.z;
        return ProjectileObj;
    }

    public GameObject InstanciateProjectile(GameObject _owner, Transform parent, Quaternion _rotate){
        spawnOwner = _owner;
        GameObject ProjectileObj = Instantiate(gameObject, parent.position, _rotate);
        ProjectileObj.transform.localScale *= 10;
        return ProjectileObj;
    }
}