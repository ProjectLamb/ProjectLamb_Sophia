using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public enum E_ProjectileType {
    Attack, Nutral
}

public class Projectile : MonoBehaviour {
    public E_ProjectileType projectileType;
    public GameObject OnHitSubEmmiter;
    public Dictionary<Affector_PlayerState ,IModifier> Modifiers;
    
    [HideInInspector]
    public EntityData spawnOwner;

    public int AttackDamage;
    Collider HitCollider;
    
    private void Awake() {
        TryGetComponent<Collider>(out HitCollider);
    }
    
    private void OnTriggerEnter(Collider other) {
        if(this.spawnOwner.EntityTag == other.gameObject.tag) {return;}
        //전달 타겟 찾기
        IEntityAddressable entityAddressable = other.gameObject.GetComponent<IEntityAddressable>();
        if(entityAddressable == null) return;

        //전달 하기
        switch(this.projectileType) {
            case E_ProjectileType.Attack :
                entityAddressable.GetDamaged(-1);
                spawnOwner.ProjectileShootState.Invoke(other.gameObject);
                break;
            case E_ProjectileType.Nutral :
                Debug.Log("중립 투사체, 이건 버프용으로도 사용 가능");
                break;
        }
    }

    public void SetProjectileData(EntityData _owner, MasterData _AddingData, E_ProjectileType _type){
        this.spawnOwner = _owner;
        //this.addingData = _AddingData;
        this.projectileType = _type;
    }
    //여기를 통해서 이 스크립트를 실행한 놈을 호출 할 수 있다.
    public GameObject InstanciateProjectile(EntityData _owner, MasterData _AddingData,E_ProjectileType _type, Transform parent){
        GameObject ProjectileObj = Instantiate(gameObject, parent);
        ProjectileObj.GetComponent<Projectile>().SetProjectileData(_owner, _AddingData,_type);
        ProjectileObj.transform.position += Vector3.forward * transform.position.z;
        return ProjectileObj;
    }

    public GameObject InstanciateProjectile(EntityData _owner, MasterData _AddingData, E_ProjectileType _type, Transform parent, Quaternion _rotate){
        GameObject ProjectileObj = Instantiate(gameObject, parent.position, _rotate);
        ProjectileObj.GetComponent<Projectile>().SetProjectileData(_owner, _AddingData, _type);
        ProjectileObj.transform.localScale *= 10;
        return ProjectileObj;
    }
}