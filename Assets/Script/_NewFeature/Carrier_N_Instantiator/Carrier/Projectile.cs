using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;
using Sophia_Carriers;
using Microsoft.SqlServer.Server;

namespace Feature_NewData
{
    
    /*변하는 녀석*/

    public class Projectile : MonoBehaviour, IInstantiable<Projectile, Entity>
    {

#region Serialize

        [SerializeField] private E_AFFECT_TYPE _affectType = E_AFFECT_TYPE.None;
        [SerializeField] private E_INSTANTIATE_STACKING_TYPE _stackingType = E_INSTANTIATE_STACKING_TYPE.Stack;
        [SerializeField] private E_INSTANTIATE_POSITION_TYPE _positioningType = E_INSTANTIATE_POSITION_TYPE.Outer;

        [SerializeField] public int    _baseProjectileDamage = 1;
        [SerializeField] private float _baseDurateTime = 5f; //파티클 기본 지속 시간
        [SerializeField] private float _baseSize = 1f;
        [SerializeField] private float _baseForwardingSpeed = 5f; //파티클 기본 지속 시간
        [SerializeField] private  Collider   _carrierCollider = null;
        [SerializeField] private  Rigidbody  _carrierRigidBody = null;
        [SerializeField] private  VisualFXObject  _destroyEffect= null;
        [SerializeField] private  VisualFXObject  _hitEffect= null;
        [SerializeField] public   ParticleSystem ProjectileParticle = null;

#endregion

#region Member

        public E_AFFECT_TYPE AffectType { get; private set; }
        public E_INSTANTIATE_STACKING_TYPE StackingType { get; private set; }
        public E_INSTANTIATE_POSITION_TYPE PositioningType { get; private set; }
        public Entity OwnerRef { get; private set; }

        public int CurrentProjectileDamage { get; private set; }

        private float mCurrentDurateTime;
        public float CurrentDurateTime 
        {
            get { return mCurrentDurateTime; }
            private set 
            {
                if(value <= 0.05f)
                {
                    mCurrentDurateTime = 0.05f; 
                    return;
                }
                mCurrentDurateTime = value;
            }
        }

        private float mCurrentSize;
        public float CurrentSize 
        {
            get {return mCurrentSize;}
            private set 
            {
                if(value <= 0) {
                    mCurrentSize = 0;
                    transform.localScale = Vector3.zero;
                    return;
                }
                mCurrentSize = value;
                transform.localScale = Vector3.one * mCurrentSize;
            }    
        }

        private float mCurrentForwardingSpeed;
        public float CurrentForwardingSpeed
        {
            get {return mCurrentForwardingSpeed;}
            private set 
            {
                mCurrentForwardingSpeed = value;
            }    
        }
        
        public bool IsInitialized { get; private set; }
        public bool IsActivated { get; private set; }

        public      ParticleSystem.MainModule       ProjectileMainModule;
        public      ParticleSystem.EmissionModule   ParticleEmissionModule;
        public      ParticleSystem.TriggerModule    ParticleTriggerModule;
        public      ParticleSystem.CollisionModule  ParticleColliderModule;

#endregion

#region ObjectPool

        private IObjectPool<Projectile> poolRefer { get; set; }
        public void SetPool(IObjectPool<Projectile> pool)
        {
            poolRefer = pool;
        }

#endregion

#region Getter

        public Entity GetOwner() => OwnerRef;
        public bool GetIsInitialized() => IsInitialized;

#endregion

#region Setter

        /* 생성자를 통해서 하자 */
        public Projectile Init(Entity owner)
        {
            if (GetIsInitialized() == true) { throw new System.Exception("이미 초기화가 됨."); }
            if(owner == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");

            OwnerRef = owner;

            ClearEvents();
            transform.tag = "Projectile";
            IsInitialized = true;
            return this;
        }

        public Projectile InitByObject(Entity owner, object[] objects)
        {
            throw new System.NotImplementedException();
        }
        
        public Projectile SetDurateTimeByRatio(float muls)
        {
            CurrentDurateTime = _baseDurateTime * muls;
            return this;
        }
        
        public Projectile SetScaleByRatio(float sizeRatio)
        {
            CurrentSize = _baseSize * sizeRatio;
            return this;
        }

        public Projectile SetForwardingSpeedByRatio(float speedRatio) 
        {
            CurrentForwardingSpeed = _baseForwardingSpeed * speedRatio;
            return this;
        }

        /*
        이거 같은 경우는 플레이어가 자체 프로젝타일 사용 안할 수 도 있기 떄문임
        */
        public Projectile SetProjectileDamage(int damage) {
            CurrentProjectileDamage = damage;
            return this;
        }

        public Projectile SetAffectType(E_AFFECT_TYPE affectType)
        {
            AffectType = affectType;
            return this;
        }


        private void ResetSettings()
        {
            if (IsActivated != false) { throw new System.Exception("아직 사용중임 리셋 불가능"); }

            CurrentSize = _baseSize;
            CurrentDurateTime = _baseDurateTime;
            CurrentProjectileDamage = _baseProjectileDamage;
            CurrentForwardingSpeed = _baseForwardingSpeed;
            AffectType = E_AFFECT_TYPE.None;

            ClearEvents();

            OwnerRef = null;
            IsInitialized = false;

            this.transform.transform.parent = null;
            this.transform.localScale = Vector3.one;
            this.transform.rotation = Quaternion.identity;
            this.transform.position = Vector3.zero;
        }

#endregion

#region Event
        
        private UnityAction OnActivated;
        public Projectile AddOnActivatedEvent(UnityAction action)
        {
            OnActivated += action;
            return this;
        }
        private UnityAction OnDeActivated;
        public Projectile AddOnDeActivatedEvent(UnityAction action)
        {
            OnDeActivated += action;
            return this;
        }
        private UnityAction OnRelease;
        public Projectile AddOnReleaseEvent(UnityAction action)
        {
            OnRelease += action;
            return this;
        }

        public event UnityAction OnProjectileCreated = null;
        public Projectile SetOnProjectileCreatedEvent(UnityAction action)
        {
            OnProjectileCreated = action;
            return this;
        }

        public event UnityAction OnProjectileTriggerd = null;
        public Projectile SetOnProjectileTriggerdEvent(UnityAction action)
        {
            OnProjectileTriggerd = action;
            return this;
        }

        public event UnityAction OnProjectileReleased = null;
        public Projectile SetOnProjectileReleasedEvent(UnityAction action)
        {
            OnProjectileReleased = action;
            return this;
        }

        public event UnityAction OnProjectileForwarding = null;
        public Projectile SetOnProjectileForwardingEvent(UnityAction action)
        {
            OnProjectileForwarding = action;
            return this;
        }


        public void ClearEvents()
        {
            OnActivated = null;
            OnDeActivated = null;
            OnRelease = null;

            OnProjectileCreated = null;
            OnProjectileTriggerd = null;
            OnProjectileReleased = null;
            OnProjectileForwarding = null;

            OnActivated             ??= () => {};
            OnDeActivated           ??= () => {};
            OnRelease               ??= () => {};
            
            OnProjectileCreated     ??= () => {};
            OnProjectileTriggerd    ??= () => {};
            OnProjectileReleased    ??= () => {};
            OnProjectileForwarding  ??= () => {};
        }

#endregion


#region Bindings

        public void Get()
        {
            this.ProjectileParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            OnActivated?.Invoke();
            IsActivated = true;
            return;
        }

        public void DeActivate()
        {
            IsActivated = false;
            OnDeActivated?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        public void Release()
        {
            IsActivated = false;
            OnRelease?.Invoke();
            ResetSettings();
            gameObject.SetActive(false);
            return;
        }

#endregion
        private void Awake()
        {
            if (TryGetComponent<ParticleSystem>(out ProjectileParticle))
            {
                ProjectileMainModule    = ProjectileParticle.main;
                ParticleEmissionModule  = ProjectileParticle.emission;
                ParticleTriggerModule   = ProjectileParticle.trigger;
                ParticleColliderModule  = ProjectileParticle.collision;
            }

            AffectType = _affectType;
            StackingType = _stackingType;
            PositioningType = _positioningType;

            CurrentSize = _baseSize;
            CurrentDurateTime = _baseDurateTime;
            CurrentProjectileDamage = _baseProjectileDamage;
            CurrentForwardingSpeed = _baseForwardingSpeed;   
        }

        private void OnParticleSystemStopped()
        {
            if (!IsInitialized) { return; }
            poolRefer.Release(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(CheckIsOwnerCollider(other)) {return;}
            if (other.TryGetComponent<IDamagable>(out IDamagable damagables))
            {
                damagables.GetDamaged(CurrentProjectileDamage, _hitEffect);
                OnProjectileTriggerd.Invoke();
            }
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * CurrentForwardingSpeed * Time.deltaTime);
            OnProjectileForwarding.Invoke();
        }


#region Helper

        public bool CheckIsSameOwner(Entity owner)
        {
            throw new System.NotImplementedException();
        }
        public bool CheckIsOwnerCollider(Collider target)
        {
            return OwnerRef.entityCollider.Equals(target);
        }

#endregion

    }
}