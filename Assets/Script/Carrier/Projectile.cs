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
public class Projectile : Carrier {
    public bool    isMove;
    public float   moveSpeed;
    public bool    destroyBySelf;
    public float   destroyTime = 0.5f;
    public float   colliderTime = 0.5f;
    public List<EntityAffector> projectileAffector;
    public int     projecttileDamage;
    //public VFXObject destroyEffect = null;
    public VFXObject hitEffect = null;
    
    Entity  ownerEntity;
    Entity  targetEntity;
    
    //Collider carrierCollider = null;
    //Rigidbody carrierRigidBody = null;
    //bool    isInitialized = false;

    ParticleSystem projectileParticle = null;

    protected override void Awake() {
        base.Awake();
        if(projectileAffector == null) {throw new System.Exception("인스펙터에에 아무것도 지정이 안됨");}
        TryGetComponent<ParticleSystem>(out projectileParticle);
    }
    private void Start() {
        if(isMove) {carrierRigidBody.velocity = Vector3.forward;}
        if(destroyBySelf == true) Invoke("DestroySelf", destroyTime);
        Invoke("ColliderDisenabled", colliderTime);
    }

    public void InitializeByDamage(int _damageAmount, Entity _genOwner){
        if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
        this.projecttileDamage = _damageAmount; 
        this.ownerEntity = _genOwner;
        this.isInitialized = true;
        transform.localScale *= _genOwner.transform.localScale.x;
    }

    public void ColliderDisenabled(){
        this.carrierCollider.enabled = false;
    }

    /*
    public void DestroySelf(){
        if(destroyEffect != null) Instantiate(destroyEffect);
        Destroy(gameObject);
    }
    */

    protected void OnTriggerEnter(Collider other) {
        if(isInitialized == false) {throw new System.Exception("투사체가 초기화 되지 않음");}
        if(projecttileDamage == 0) {Debug.Log("데미지가 0임 의도한 거 맞지?");}
        if(!other.TryGetComponent<Entity>(out targetEntity)){return;}
        if(ownerEntity.GetFinalData().EntityTag == targetEntity.GetFinalData().EntityTag){return;}
        targetEntity.GetDamaged(projecttileDamage, hitEffect);
            //어? 분명 프로젝타일 자기자신이 가진 어펙터를 사용할 수 있어야 하는데 Entity가 필수 불가결하게 되는 상황이 생겼다..
                //어떻게 해야하는거지 수정해야겠다.
            // ✅ 아! 알았다 owner도 자신이고 target또한 자기자신이면. 가능하다.
            // affector.Init(target, target);
                //foreach (EntityAffector affector in projectileEntityAffector) {
                //    affector.Init()
                //    affector.Modifiy(targetEntity);
                //}
        ownerEntity.GetFinalData().ProjectileShootState?.Invoke(ownerEntity, targetEntity);
        
        if(projectileAffector == null) return;
        foreach (EntityAffector affector in projectileAffector) {
            affector.Init(ownerEntity, ownerEntity).Modifiy();
        }
    }
}