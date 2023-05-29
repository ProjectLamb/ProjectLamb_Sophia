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
public class Projectile : MonoBehaviour {
    public bool    isMove;
    public bool    destroyBySelf;
    public float   moveSpeed;
    public float   destroyTime = 0.5f;
    public float   colliderTime = 0.5f;
    public List<EntityAffector> projectileEntityAffector;
    public GameObject hitEffect = null;
    public GameObject destroyEffect = null;
    
    Entity  ownerEntity;
    public int     damageAmount;
    
    ParticleSystem projectileParticle = null;
    Collider projectileCollider = null;
    Rigidbody projectileRigidBody = null;
    bool    isInitialized = false;
    Entity  targetEntity;

    private void Awake() {
        projectileEntityAffector ??= new List<EntityAffector>();
        TryGetComponent<Collider>(out projectileCollider);
        TryGetComponent<Rigidbody>(out projectileRigidBody);
        TryGetComponent<ParticleSystem>(out projectileParticle);
    }

    public void ColliderDisenabled(){
        projectileCollider.enabled = false;
    }

    private void Start() {
        if(isMove) {projectileRigidBody.velocity = Vector3.forward;}
        if(destroyBySelf == true)Destroy(gameObject, destroyTime);
        Invoke("ColliderDisenabled", colliderTime);
    }

    private void OnDestroy() {
        if(destroyEffect != null) Instantiate(destroyEffect);
    }

    public void DestroySelf(){
        if(destroyEffect != null) Instantiate(destroyEffect);
        Destroy(gameObject);
    }

    public void InitializeByDamage(int _damageAmount, Entity _genOwner){
        if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
        this.damageAmount = _damageAmount; 
        this.ownerEntity = _genOwner;
        this.isInitialized = true;
        transform.localScale *= _genOwner.transform.localScale.x;
    }

    public void Initialize(Entity _genOwner){
        if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
        this.ownerEntity = _genOwner;
        this.isInitialized = true;
        transform.localScale *= _genOwner.transform.localScale.x;
    }

    private void OnTriggerEnter(Collider other) {
        if(isInitialized == false) {throw new System.Exception("투사체가 초기화 되지 않음");}
        if(damageAmount == 0) {Debug.Log("데미지가 0임 의도한 거 맞지?");}
        if(!other.TryGetComponent<Entity>(out targetEntity)){return;}
        if(ownerEntity.GetEntityData().EntityTag == targetEntity.GetEntityData().EntityTag){return;}
        targetEntity.GetDamaged(damageAmount, hitEffect);
            //어? 분명 프로젝타일 자기자신이 가진 어펙터를 사용할 수 있어야 하는데 Entity가 필수 불가결하게 되는 상황이 생겼다..
                //어떻게 해야하는거지 수정해야겠다.
        // ✅ 아! 알았다 owner도 자신이고 target또한 자기자신이면. 가능하다.
        // affector.Init(target, target);
            //foreach (EntityAffector affector in projectileEntityAffector) {
            //    affector.Init()
            //    affector.Modifiy(targetEntity);
            //}
        ownerEntity.GetEntityData().ProjectileShootState?.Invoke(ownerEntity, targetEntity);
    }
}