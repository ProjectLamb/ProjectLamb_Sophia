using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiate().Initialize(int amount, Entity owner)를 꼭 설정해야함 <br/>
/// 초기화가 잘 되었는지 예외처리도 하겠다. 
/// 프로젝타일은 다~~ 오브젝트 풀 사용하게끔 해보자.
/// </summary>

namespace Sophia_Carriers {
    public class Trigger : Carrier 
    {
//      public      VFXObject           DestroyEffect       = null;
//      public      CARRIER_TYPE        CarrierType;
//      public      BUCKET_POSITION     BucketPosition;
//      public      bool                IsInitialized       = false;
//      public      bool                IsActivated         = false;
//      public      bool                IsCloned         = false;
//      protected   Collider            carrierCollider     = null;
//      protected   Rigidbody           carrierRigidBody    = null;
        [SerializeField] public float           DestroyTime = 0.5f;
        [SerializeField] public float           ColliderTime = 0.5f;
        [SerializeField] protected bool         isDestroyBySelf;
        public List<EntityAffector>             triggerAffectors;
        
        public override Carrier Clone()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Carrier res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }

        public Trigger CloneTrigger()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Trigger res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }


        protected override void Awake()
        {
        //  TryGetComponent<Collider>(out carrierCollider);
        //  TryGetComponent<Rigidbody>(out carrierRigidBody);
            base.Awake();
            this.triggerAffectors ??= new List<EntityAffector>();
        }
        
        public override void Init(Entity _ownerEntity)
        {
            if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            if(_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
            this.ownerEntity = _ownerEntity;
            this.IsInitialized = true;
        }
        
        public override void InitByObject(Entity _ownerEntity, object[] _objects)
        {
            Init(_ownerEntity);
        }
        
        protected virtual void OnEnable() {
            if (isDestroyBySelf == true) Invoke("DestroySelf", this.DestroyTime);
            Invoke("ColliderDisenabled", this.ColliderTime);
        }

        // public override void EnableSelf() {gameObject.SetActive(true); this.IsActivated = true; }
        // public override void DisableSelf() {gameObject.SetActive(false); this.IsActivated = false;}
        // public override void DestroySelf()  {
        //     if(DestroyEffect != null){
        //         Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        //     }
        //     Destroy(gameObject);
        // }
        
        protected virtual void OnTriggerEnter(Collider _other) {
            CheckException();
            if(CheckIsOwnerCollider(_other)) {
                ConveyAffectorToTarget(_other);
            }
        }
        protected void ColliderDisenabled() { this.carrierCollider.enabled = false; }
        protected bool CheckIsOwnerCollider(Collider target) { 
            return this.ownerEntity.entityCollider.Equals(target); 
        }
        protected bool CheckIsSameEntity(Entity _targetEntity) { 
            return ownerEntity.GetFinalData().EntityTag == _targetEntity.GetFinalData().EntityTag; 
        }
        protected void CheckException(){
            if (IsInitialized == false) { throw new System.Exception("투사체가 초기화 되지 않음"); }
        }    
        protected void ConveyAffectorToTarget(Collider _other){
            Entity  targetEntity; 
            if(_other.TryGetComponent<Entity>(out targetEntity)){
                if(CheckIsSameEntity(targetEntity)) {return;}
                triggerAffectors.ForEach(Affector => Affector.Init(ownerEntity, targetEntity).Modifiy());
            }
        }
    }
}