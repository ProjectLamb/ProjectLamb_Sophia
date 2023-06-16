using System;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

/// <summary>
/// Instantiate().Initialize(int amount, Entity owner)를 꼭 설정해야함 <br/>
/// 초기화가 잘 되었는지 예외처리도 하겠다. 
/// 프로젝타일은 다~~ 오브젝트 풀 사용하게끔 해보자.
/// </summary>

namespace Sophia_Carriers {
    public class Projectile : Carrier
    {
//       public      VFXObject       DestroyEffect       = null;
//       public      CARRIER_TYPE    CarrierType;
//       public      bool            IsInitialized       = false;
//       public      bool            IsActivated         = false;
//       public      bool            IsCloned         = false;
//       protected   Collider        carrierCollider     = null;
//       protected   Rigidbody       carrierRigidBody    = null;

        [SerializeField] public VFXObject       HitEffect = null;
        [SerializeField] public ParticleSystem  ProjectileParticle = null;

        [SerializeField] public float           ProjecttileDamage;
        [SerializeField] public float           DestroyTime = 0.5f;
        [SerializeField] public float           ColliderTime = 0.5f;
        [SerializeField] private bool           isDestroyBySelf;
        public List<EntityAffector>             projectileAffector;

        public override Carrier Clone()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Carrier res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }
        
        public virtual Projectile CloneProjectile()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Projectile res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent<ParticleSystem>(out ProjectileParticle);
            projectileAffector = new List<EntityAffector>();
        }

        public override void Init(Entity _ownerEntity)
        {
            if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            if(_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
            ownerEntity = _ownerEntity;
            transform.tag = _ownerEntity.GetFinalData().EntityTag + "Projectile";
            IsInitialized = true;
        }
        /// <summary>
        /// 특정 변수를 넣어서 초기화를 하는것이다. 
        /// </summary>
        /// <param name="_ownerEntity"></param>
        /// <param name="_objects"></param>
        public override void InitByObject(Entity _ownerEntity, object[] _objects)
        {
            if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            if(_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
            ownerEntity = _ownerEntity;
            Debug.Log(_ownerEntity.GetFinalData().EntityTag + "Projectile");
            transform.tag = _ownerEntity.GetFinalData().EntityTag + "Projectile";
            ProjecttileDamage = (int)_objects[0];
            IsInitialized = true;
        }
        /// <summary>
        /// Start 대신 enable이 되면 실행시작하게끔 한다.
        /// </summary>
        private void OnEnable() {
            if(!IsInitialized) return;
            if (isDestroyBySelf == true) Invoke("DestroySelf", DestroyTime);
            Invoke("ColliderDisenabled", ColliderTime);
        }

        public override void EnableSelf() {gameObject.SetActive(true); this.IsActivated = true; }
        public override void DisableSelf() {gameObject.SetActive(false); this.IsActivated = false;}
        public override void DestroySelf()  {
            if(DestroyEffect != null){
                Instantiate(DestroyEffect, transform.position, Quaternion.identity).Initialize();
            }
            Destroy(gameObject);
        }
        // public virtual void SetScale(float _sizeRatio){transform.localScale *= _sizeRatio;}
        protected virtual void OnTriggerEnter(Collider _other) {
            CheckException();
            if(!CheckIsOwnerCollider(_other)) {
                HitTarget(_other);
                ConveyAffectorToTarget(_other);
            }
        }

        protected void ColliderDisenabled() { this.carrierCollider.enabled = false; }

        protected bool CheckIsOwnerCollider(Collider target) { 
            return ownerEntity.entityCollider.Equals(target); 
        }

        protected bool CheckIsSameEntity(Entity _targetEntity) { 
            return ownerEntity.GetFinalData().EntityTag == _targetEntity.GetFinalData().EntityTag; 
        }
        protected void CheckException(){
            if (IsInitialized == false) { throw new System.Exception("투사체가 초기화 되지 않음"); }
            if (ProjecttileDamage == 0) { Debug.Log("데미지가 0임 의도한 거 맞지?"); }
        }    
        protected void HitTarget(Collider _other) {
            IDamagable damagableTarget;
            if(transform.CompareTag(_other.transform.tag)) {return;}
            if(_other.TryGetComponent<IDamagable>(out damagableTarget)){
                damagableTarget.GetDamaged((int)ProjecttileDamage, HitEffect); 
            }
        }

        protected void ConveyAffectorToTarget(Collider _other){
            Entity  targetEntity; 
            if(_other.TryGetComponent<Entity>(out targetEntity)){
                if(CheckIsSameEntity(targetEntity)) {return;}
                ownerEntity.GetFinalData().ProjectileShootState?.Invoke(ownerEntity, targetEntity);
                projectileAffector.ForEach(Affector => Affector.Init(ownerEntity, targetEntity).Modifiy());
            }
        }
    }
}