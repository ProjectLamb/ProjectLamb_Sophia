using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Instantiate().Initialize(int amount, Entity owner)를 꼭 설정해야함 <br/>
/// 초기화가 잘 되었는지 예외처리도 하겠다. 
/// 프로젝타일은 다~~ 오브젝트 풀 사용하게끔 해보자.
/// </summary>
public class Projectile : Carrier
{

    //  public E_CarrierType carrierType;
    //  public    VFXObject destroyEffect = null;
    //  protected Collider  carrierCollider = null;
    //  protected Rigidbody carrierRigidBody = null;
    //  protected bool      isInitialized = false;
    [SerializeField] public int projecttileDamage;
    [SerializeField] protected bool isMove;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected bool destroyBySelf;
    [SerializeField] protected float destroyTime = 0.5f;
    [SerializeField] protected float colliderTime = 0.5f;

    public List<EntityAffector> projectileAffector;
    [SerializeField] public VFXObject hitEffect = null;

    protected Entity ownerEntity;
    protected ParticleSystem projectileParticle = null;

    protected override void Awake()
    {
        base.Awake();
        //  TryGetComponent<Collider>(out carrierCollider);
        //  TryGetComponent<Rigidbody>(out carrierRigidBody);
        TryGetComponent<ParticleSystem>(out projectileParticle);
    }
    private void Start()
    {
        if (isMove) { carrierRigidBody.velocity = Vector3.forward; }
        if (destroyBySelf == true) Invoke("DestroySelf", destroyTime);
        Invoke("ColliderDisenabled", colliderTime);
    }

    //  public virtual void Initialize(Entity _genOwner){
    //      if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
    //      transform.localScale *= _genOwner.transform.localScale.x;
    //      this.isInitialized = true;
    //  }
    public void InitializeByDamage(Entity _genOwner, int _damageAmount)
    {
        if (_genOwner == null) { throw new System.Exception("투사체 생성 엔티티가 NULL임"); }
        this.projecttileDamage = _damageAmount;
        this.ownerEntity = _genOwner;
        this.transform.localScale *= _genOwner.transform.localScale.x;
        this.isInitialized = true;
    }

    public void ColliderDisenabled() { this.carrierCollider.enabled = false; }

    //  public void DestroySelf(){
    //      if(destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity).Initialize();
    //      Destroy(gameObject);
    //  }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (isInitialized == false) { throw new System.Exception("투사체가 초기화 되지 않음"); }
        if (projecttileDamage == 0) { Debug.Log("데미지가 0임 의도한 거 맞지?"); }
        if (other.TryGetComponent<Entity>(out Entity targetEntity))
        {
            switch (this.carrierType)
            {
                case E_CarrierType.Nutral:
                    NutralTrigger(ownerEntity, targetEntity);
                    break;
                case E_CarrierType.Attack:
                    AttackTrigger(ownerEntity, targetEntity);
                    break;
                default:
                    break;
            }
        }
    }
    protected bool IsSameEntity(Entity _ownerEntity, Entity _targetEntity)
    {
        return _ownerEntity.GetFinalData().EntityTag == _targetEntity.GetFinalData().EntityTag;
    }
    
    protected void NutralTrigger(Entity _ownerEntity, Entity _targetEntity)
    {
        if (IsSameEntity(_ownerEntity, _targetEntity) == true)
        {
            if(projectileAffector != null) projectileAffector.ForEach(affector => affector.Init(_ownerEntity, ownerEntity).Modifiy());
        }
    }
    protected void AttackTrigger(Entity _ownerEntity, Entity _targetEntity)
    {
        if (IsSameEntity(_ownerEntity, _targetEntity) == false)
        {
            _targetEntity.GetDamaged(projecttileDamage, hitEffect);
            _ownerEntity.GetFinalData().ProjectileShootState?.Invoke(_ownerEntity, _targetEntity);
            if(projectileAffector != null) projectileAffector.ForEach(affector => affector.Init(_ownerEntity, ownerEntity).Modifiy());
        }
    }
}