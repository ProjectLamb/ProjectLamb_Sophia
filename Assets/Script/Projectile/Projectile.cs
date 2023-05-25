using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public enum E_ProjectileType {
    Attackor, Modifiers, Instantiator
}

/// <summary>
/// 다음이 만족해야한다. <br/>
///     > Entiy의 데이터와 로직에 접근이 가능해야한다. ✅ <br/>
///     > 반대로 Entity를 DownCasting 하지 않는 선에서 데이터 수정을 해야한다. <br/>
///     > 그리고 EntityData는 앞으로 변동이 가능하며, 그 자체가 Base가 아닌 FinalData로 구현한다. <br/>
///     > Entity, Projectie에 의해 Instantiate되야 한다. ⚠️ <br/>
///     > 룰렛(Projectile) 또한 Item을 내뿜는다. <br/>
///     > OnCollision, OnTrigger이 구현되어 있다. ✅ <br/>
///     > 즉시 삭제가 되든, 자가 삭제가 되든.. 가능하다. ✅ <br/>
///     > 나를 생성한 OwnerEntity가 있으며, 간섭 타겟인 TargetEntity또한 알고 있다. ✅ <br/>
///     > 플레이어의 공격 이펙트가 플레이어를 때려서는 안된다 <br/>
///     > Attack하는 타입인지, Modifiy하는 타입인지 알수 있다. ✅ <br/>
///     > 나를 생성한 OwnerEntity의 데이터를 참조해 전달 가능하기도 한데 Projectile자기 자신만이 가지고 있는 데이터또한 전달 가능하다 ⚠️ <br/>
///     > 독데미지를 가지고 있는 플레이어 상태와 상관없이 Projectile <br/>
///     > 밟으면 불데미지 입는 함정 <br/>
///     > Equipment 데이터를 가지고 있는 <br/>
///     > Gear + 100 올라가는 금화 <br/>
/// </summary>
public class Projectile : MonoBehaviour, IDestroyHandler {
    public E_ProjectileType projectileType;
    /*자기 자신 또한 기본 전달자가 있다*/
    public bool isMove;
    public float moveSpeed;
    public Rigidbody projectileRigid;
    public Collider projectileCollider;
    public Modulator projectileModulator;
    
    IEntityAddressable ownerEntity;
    int? damageAmount = null;

    public void Initialize(IEntityAddressable _owner){ ownerEntity = _owner; }
    public void Initialize(IEntityAddressable _owner, int transferAmount){
        ownerEntity = _owner;
        damageAmount = transferAmount;
    }

    private void Start() {
        if(isMove) {projectileRigid.velocity = Vector3.forward;}
    }

    private void OnTrigger(Collision _other){
        IEntityAddressable targetEntity = _other.gameObject.GetComponent<IEntityAddressable>();
        if(ownerEntity.GetEntityData().EntityTag == targetEntity.GetEntityData().EntityTag){return;}
        switch(projectileType) {
            case E_ProjectileType.Attackor:
                if(damageAmount == null){throw new System.Exception("전달된 데미지 값이 NULL 이다.");}
                targetEntity.GetDamaged((int)damageAmount);
                ownerEntity.GetEntityData().ProjectileShootState?.Invoke(_other.gameObject);
                break;
            case E_ProjectileType.Modifiers:
                break;
            case E_ProjectileType.Instantiator:
                break;
            default :
                break;
        }
    }

    public void DestroySelf(UnityAction _destroyCallback){
        _destroyCallback?.Invoke();
    }
    public void DestroySelf(UnityAction _destroyCallback, float _time){
        _destroyCallback?.Invoke();
        Destroy(gameObject, _time);
    }
}