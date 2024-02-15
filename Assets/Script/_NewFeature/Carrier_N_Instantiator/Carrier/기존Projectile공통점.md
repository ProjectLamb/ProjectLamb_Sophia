```cs
namespace Sophia_Carriers{
    public abstract class Carrier : MonoBehaviour {
        public      VFXObject       DestroyEffect       = null;
        public      CARRIER_TYPE    CarrierType;
        public      BUCKET_POSITION BucketPosition;
        public      bool            IsInitialized       = false;
        public      bool            IsActivated         = false;
        public      bool            IsCloned         = false;
        protected   Collider        carrierCollider     = null;
        public      Entity          ownerEntity;
        public     CollisionDelegator collisionDelegator;
        
        public abstract Carrier Clone();
        
        protected virtual void Awake() {
            
        }

        public abstract void Init(Entity _ownerEntity);
        public abstract void InitByObject(Entity _ownerEntity, object[] _objects);
        public virtual void SetScale();
        public virtual void EnableSelf();
        public virtual void DisableSelf();
        public virtual void DestroySel();
        public virtual Entity GetOwner();
    }
}
```
```cs
namespace Sophia_Carriers {
    public class ProjectileObject : Carrier
    {
        public      CARRIER_TYPE                    CarrierType;
        public      BUCKET_POSITION                 BucketPosition;
        protected   Collider                        carrierCollider         = null;
        protected   Rigidbody                       carrierRigidBody        = null;
        public      Entity                          ownerEntity             = null;
        
        public      VFXObject                       DestroyEffect           = null;
        public      VFXObject                       HitEffect               = null;
        
        public      bool                            IsInitialized           = false;
        public      bool                            IsActivated             = false;
        public      bool                            IsCloned                = false;


        public      float                           CurrentDamage           = -1f;
        public      float                           CurrentSize             = -1f;
        public      float                           CurrentSpeed            = -1f;

        public      float                           DestroyTime             = 0.5f;
        public      float                           ColliderTime            = 0.5f;
        protected   bool                            isDestroyBySelf;
        
        public      List<EntityAffector>            projectileAffector      = null;
    
        public      ParticleSystem                  ProjectileParticle      = null;
        public      ParticleSystem.MainModule       ParticleMainModule      = null;
        public      ParticleSystem.EmissionModule   ParticleEmissionModule  = null;
        public      ParticleSystem.TriggerModule    ParticleTriggerModule   = null;
        public      ParticleSystem.CollisionModule  ParticleColliderModule  = null;

        protected override void Awake()
        {            
            base.Awake();
            projectileAffector = new List<EntityAffector>();
            
            TryGetComponent<ParticleSystem>(out ProjectileParticle);
            ParticleMainModule = ProjectileParticle.main;
            ParticleEmissionModule = ProjectileParticle.emission;
            ParticleTriggerModule = ProjectileParticle.trigger;
            projectileAffector = new List<EntityAffector>();
        }

        private void OnEnable() {throw new System.NotImplementedException();}

        protected virtual void OnTriggerEnter(Collider _other) {}
        private void OnParticleCollision(GameObject _other) {}

#region Clone By Prefeb Instantiate
        public override Carrier Clone(){
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Carrier res = Instantiate(this).DisableSelf();
            res = IsCloned = true;
            return res;
        }
        public virtual ProjectileObject CloneProjectile(){}
        public virtual ParticleProjectile CloneParticleProjectile() {throw new System.NotImplementedException();}
#endregion

#region getter

        public virtual Entity GetOwner();

#endregion

#region setter

        public abstract void Init(Entity _ownerEntity) {
            if(IsCloned == false)       
            if(_ownerEntity == null)    
            ownerEntity = _ownerEntity;
            particleModule.startLifetime    = DestroyTime;
            particleModule.duration         = DestroyTime;
            transform.tag = _ownerEntity.GetFinalData().EntityTag + "ProjectileObject";
            IsInitialized = true;
        }
        public abstract void InitByObject(Entity _ownerEntity, object[] _objects);
        public virtual void SetScale();

#endregion

#region bindings

        protected void HitTarget(Collider _other);
        public virtual void EnableSelf();
        public virtual void DisableSelf();
        public virtual void DestroySel();
        protected void ColliderDisenabled();
        protected void ConveyAffectorToTarget(Collider _other){}

#endregion

#region helper

        protected bool CheckIsOwnerCollider(Collider target) {}
        protected bool CheckIsSameEntity(Entity _targetEntity) {}
        protected void CheckException(){}

#endregion

    }
}
```

```cs
namespace Sophia_Carriers {
    /*********************************************************************************
    * Carrier 
    * ProjectileObject 방법도 있고, MeshRender방법도 있다.
    *********************************************************************************/
    public class ProjectileObject : Carrier
    {
        public      CARRIER_TYPE                    CarrierType;
        public      BUCKET_POSITION                 BucketPosition;
        public      BUCKET_STACKING_TYPE            BucketStaking;

        protected   Collider                        carrierCollider         = null;
        protected   Rigidbody                       carrierRigidBody        = null;
        public      Entity                          ownerEntity             = null;
        
        public      VFXObject                       DestroyEffect           = null;
        public      VFXObject                       HitEffect               = null;
        
        public      bool                            IsInitialized           = false;
        public      bool                            IsActivated             = false;
        public      bool                            IsCloned                = false;


        public      float                           CurrentDamage           = -1f;
        public      float                           CurrentSize             = -1f;
        public      float                           CurrentSpeed            = -1f;

        public      float                           DurateTime              = 0.5f;
        public      float                           ColliderTime            = 0.5f;
        protected   bool                            isDestroyBySelf;
        
        public      List<EntityAffector>            projectileAffector      = null;
    
        public      ParticleSystem                  ProjectileParticle      = null;
        public      ParticleSystem.MainModule       ParticleMainModule      = null;
        public      ParticleSystem.EmissionModule   ParticleEmissionModule  = null;
        public      ParticleSystem.TriggerModule    ParticleTriggerModule   = null;
        public      ParticleSystem.CollisionModule  ParticleColliderModule  = null;

        /* VFX 만의 특이한 것.
        근데 기본적으로 갖고 있는 Affector가 있을 수 도 있잖아?
        public STATE_TYPE                           AffectorType;
        */

        protected override void Awake()
        {            
            base.Awake();
            projectileAffector = new List<EntityAffector>();
            
            TryGetComponent<ParticleSystem>(out ProjectileParticle);
            ParticleMainModule = ProjectileParticle.main;
            ParticleEmissionModule = ProjectileParticle.emission;
            ParticleTriggerModule = ProjectileParticle.trigger;
            projectileAffector = new List<EntityAffector>();
        }

        private void OnEnable() {throw new System.NotImplementedException();}
        private void OnDisable() {throw new System.NotImplementedException();}
        private void OnDestroy() {throw new System.NotImplementedException();}

        protected virtual void OnTriggerEnter(Collider _other) {}
        private void OnParticleCollision(GameObject _other) {}

#region Clone By Prefeb Instantiate

        public override Carrier Clone(){}
        public virtual ProjectileObject CloneProjectile(){}
        public virtual ParticleProjectile CloneParticleProjectile() {throw new System.NotImplementedException();}

#endregion

#region getter

        public virtual Entity GetOwner();

#endregion

#region setter

        public abstract void Init(Entity _ownerEntity);
        public abstract void InitByObject(Entity _ownerEntity, object[] _objects);
        public virtual  void SetScale(float _sizeRatio);

#endregion

#region bindings

        public UnityEvent                           OnDestroyEvent;
        public UnityAction<STATE_TYPE>              OnDestroyActionByState;

        protected void HitTarget(Collider _other);
        public virtual void EnableSelf();
        public virtual void DisableSelf();
        public virtual void DestroySelf();
        public void    DestroyVFXForce();
        protected void ColliderDisenabled();
        protected void ConveyAffectorToTarget(Collider _other){}

#endregion

#region helper

        protected bool CheckIsOwnerCollider(Collider target) {}
        protected bool CheckIsSameEntity(Entity _targetEntity) {}
        protected void CheckException(){}

#endregion

    }
}

    public interface IInstantiable<T, U> {

        public T    Clone();

        public U    GetOwner();

        public void Init(U owner);
        public void InitByObject(U owner, object[] objects);
        public void SetScale(float sizeRatio);
        public void SetDurateTime(float time);

        public void Activate();
        public void DeActivate();
        public void Release();

        public bool IsCloned();
        public bool CheckIsSameOwner(U owner);
    
    }

```