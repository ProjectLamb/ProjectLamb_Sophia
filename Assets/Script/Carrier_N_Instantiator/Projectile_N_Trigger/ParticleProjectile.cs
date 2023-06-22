using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiate().Initialize(int amount, Entity owner)를 꼭 설정해야함 <br/>
/// 초기화가 잘 되었는지 예외처리도 하겠다. 
/// 프로젝타일은 다~~ 오브젝트 풀 사용하게끔 해보자.
/// </summary>

namespace Sophia_Carriers {
    public class ParticleProjectile : Projectile
    {
//      public      VFXObject                   DestroyEffect       = null;
//      public      CARRIER_TYPE                CarrierType;
//      public      BUCKET_POSITION             BucketPosition;
//      public      bool                        IsInitialized       = false;
//      public      bool                        IsActivated         = false;
//      public      bool                        IsCloned         = false;
//      protected   Collider                    carrierCollider     = null;
//      protected   Rigidbody                   carrierRigidBody    = null;
//      [SerializeField] public VFXObject       HitEffect = null;
//      [SerializeField] public ParticleSystem  ProjectileParticle = null;
        [SerializeField] 
        public ParticleSystem.MainModule        ParticleMainModule;
        public ParticleSystem.EmissionModule    ParticleEmissionModule;
        public ParticleSystem.TriggerModule     ParticleTriggerModule;
        public ParticleSystem.CollisionModule   ParticleColliderModule;
//      
//      [SerializeField] public float           ProjecttileDamage;
//      [SerializeField] public float           DestroyTime = 0.5f;
//      [SerializeField] public float           ColliderTime = 0.5f;
//      [SerializeField] private bool           isDestroyBySelf;
//      public List<EntityAffector>             projectileAffector;
        
//      public override Carrier Clone()
//      {
//          if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
//          Carrier res = Instantiate(this);
//          res.DisableSelf();
//          res.IsCloned = true;
//          return res;
//      }
      
//      public virtual Projectile CloneProjectile()
//      {
//          if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
//          Projectile res = Instantiate(this);
//          res.DisableSelf();
//          res.IsCloned = true;
//          return res;
//      }

        public virtual ParticleProjectile CloneParticleProjectile()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            ParticleProjectile res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent<ParticleSystem>(out ProjectileParticle);
            ParticleMainModule = ProjectileParticle.main;
            ParticleEmissionModule = ProjectileParticle.emission;
            ParticleTriggerModule = ProjectileParticle.trigger;
            projectileAffector = new List<EntityAffector>();
        }

//      public override void Init(Entity _ownerEntity)
//      {
//          if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
//          if(_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
//          ownerEntity = _ownerEntity;
//          transform.tag = _ownerEntity.GetFinalData().EntityTag + "Projectile";
//          IsInitialized = true;
//      }

//      /// <summary>
//      /// 특정 변수를 넣어서 초기화를 하는것이다. 
//      /// </summary>
//      /// <param name="_ownerEntity"></param>
//      /// <param name="_objects"></param>
//      public override void InitByObject(Entity _ownerEntity, object[] _objects)
//      {
//          if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
//          if(_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
//          ownerEntity = _ownerEntity;
//          Debug.Log(_ownerEntity.GetFinalData().EntityTag + "Projectile");
//          transform.tag = _ownerEntity.GetFinalData().EntityTag + "Projectile";
//          ProjecttileDamage = (int)_objects[0];
//          IsInitialized = true;
//      }

        private void OnEnable() {
            if(!IsInitialized) return;
            if (this.isDestroyBySelf == true) Invoke("DestroySelf", DestroyTime);
        }

        private void OnParticleCollision(GameObject _other) {
            Debug.Log("you hit");
            Collider other = _other.GetComponent<Collider>();
            ConveyAffectorToTarget(other);
        }
    }
}