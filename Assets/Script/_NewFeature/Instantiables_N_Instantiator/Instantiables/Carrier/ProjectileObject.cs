using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;
using Sophia_Carriers;
using Microsoft.SqlServer.Server;

namespace Sophia.Instantiates
{
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.Entitys;   
    /*변하는 녀석*/

    public class ProjectileObject : Carrier, IPoolAccesable
    {

#region SerializeMember
        [SerializeField] private E_AFFECT_TYPE _affectType = E_AFFECT_TYPE.None;
        [SerializeField] private E_INSTANTIATE_STACKING_TYPE _stackingType = E_INSTANTIATE_STACKING_TYPE.Stack;
        [SerializeField] private E_INSTANTIATE_POSITION_TYPE _positioningType = E_INSTANTIATE_POSITION_TYPE.Outer;
        [SerializeField] private DamageInfo    _baseProjectileDamage;
        [SerializeField] private float _baseDurateTime = 5f; //파티클 기본 지속 시간
        [SerializeField] private float _baseSize = 1f;
        [SerializeField] private float _baseForwardingSpeed = 5f; //파티클 기본 지속 시간
        [SerializeField] private  Collider   _carrierCollider = null;
        [SerializeField] private  Rigidbody  _carrierRigidBody = null;
        [SerializeField] private  VisualFXObject  _destroyEffect= null;
        [SerializeField] private  VisualFXObject  _hitEffect= null;
        [SerializeField] private  ParticleSystem ProjectileParticle = null;
        [SerializeField] private  SerialAffectorData _serialAffectorData;

#endregion

#region Member

        public E_AFFECT_TYPE AffectType { get {return this._affectType;} private set{} }
        public E_INSTANTIATE_STACKING_TYPE StackingType { get {return this._stackingType;} private set{} }
        public E_INSTANTIATE_POSITION_TYPE PositioningType { get {return this._positioningType;} private set{} }
        public Entity OwnerRef { get; private set; }
        public Affector     NaturallyInherentAffector;

        private DamageInfo mCurrentProjectileDamage;
        public DamageInfo CurrentProjectileDamage 
        { 
            get {return mCurrentProjectileDamage;}
            private set {mCurrentProjectileDamage = value;}
        }

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
                //ProjectileMainModule.duration = mCurrentDurateTime;
            }
        }

        private Vector3 NomalizeScaleVector = Vector3.zero;
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
                if(NomalizeScaleVector == Vector3.zero) {NomalizeScaleVector = Vector3.Normalize(transform.localScale);}
                transform.localScale = NomalizeScaleVector * mCurrentSize;
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

        private IObjectPool<ProjectileObject> poolRefer { get; set; }

        public void SetByPool<T>(IObjectPool<T> pool) where T : MonoBehaviour
        {
            try
            {
                if(typeof(T).Equals(typeof(ProjectileObject))){
                    poolRefer = pool as IObjectPool<ProjectileObject>;
                    return;
                }
            }
            catch (System.Exception)
            {
            }
        }

        public void GetByPool()
        {
            gameObject.SetActive(false);
        }

        public void ReleaseByPool()
        {
            IsActivated = false;
            OnRelease?.Invoke();
            ResetSettings();
            gameObject.SetActive(false);
            return;
        }
        
        public event UnityAction OnActivated;
        public event UnityAction OnRelease;

        public void SetPoolEvents(UnityAction activated, UnityAction release)
        {
            OnActivated     = activated;
            OnRelease       = release;
        }


#endregion

#region Getter

        public Entity GetOwner() => OwnerRef;
        public bool GetIsInitialized() => IsInitialized;        
        public Collider GetOwnerCollider() => OwnerRef.entityCollider;

#endregion

#region Setter

        /* 생성자를 통해서 하자 */
        public ProjectileObject Init(Entity owner)
        {
            if (GetIsInitialized() == true) { throw new System.Exception("이미 초기화가 됨."); }
            if(owner == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");

            OwnerRef = owner;
            CurrentSize = _baseSize;
            CurrentDurateTime = _baseDurateTime;
            CurrentProjectileDamage = _baseProjectileDamage;
            CurrentForwardingSpeed = _baseForwardingSpeed;

            ClearEvents();
            transform.tag = owner.transform.tag + "Projectile";
            IsInitialized = true;
            return this;
        }

        public ProjectileObject SetDurateTimeByRatio(float muls)
        {
            CurrentDurateTime = _baseDurateTime * muls;
            return this;
        }
        
        public ProjectileObject SetScaleOverrideByRatio(float sizeRatio)
        {
            CurrentSize = _baseSize * sizeRatio;
            return this;
        }
        
        public ProjectileObject SetScaleMultiplyByRatio(float sizeMulRatio)
        {
            CurrentSize *= sizeMulRatio;
            return this;
        }

        public ProjectileObject SetForwardingSpeedByRatio(float speedRatio) 
        {
            CurrentForwardingSpeed = _baseForwardingSpeed * speedRatio;
            return this;
        }

        /*
        이거 같은 경우는 플레이어가 자체 프로젝타일 사용 안할 수 도 있기 떄문임
        */
        public ProjectileObject SetProjectilePower(int power) {
            mCurrentProjectileDamage.damageAmount = power;
            return this;
        }

        public ProjectileObject SetProjectileDamageInfoByWaepon(Extras<DamageInfo> weaponUse) {
            weaponUse.PerformStartFunctionals(ref mCurrentProjectileDamage);
            return this;
        }
        public ProjectileObject SetProjectileDamageInfoBySkill(Extras<DamageInfo> skillUse) {
            skillUse.PerformStartFunctionals(ref mCurrentProjectileDamage);
            return this;
        }

        public ProjectileObject SetAffectType(E_AFFECT_TYPE affectType)
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

            this.transform.parent = null;
            this.transform.localScale = Vector3.one;
            this.transform.rotation = Quaternion.identity;
            this.transform.position = Vector3.zero;
        }

#endregion

#region Event

        public event UnityAction OnProjectileCreated = null;
        public ProjectileObject SetOnProjectileCreatedEvent(UnityAction action)
        {
            OnProjectileCreated = action;
            return this;
        }

        public event UnityAction OnProjectileTriggerd = null;
        public ProjectileObject SetOnProjectileTriggerdEvent(UnityAction action)
        {
            OnProjectileTriggerd = action;
            return this;
        }

        public event UnityAction OnProjectileReleased = null;
        public ProjectileObject SetOnProjectileReleasedEvent(UnityAction action)
        {
            OnProjectileReleased = action;
            return this;
        }

        public event UnityAction OnProjectileForwarding = null;
        public ProjectileObject SetOnProjectileForwardingEvent(UnityAction action)
        {
            OnProjectileForwarding = action;
            return this;
        }


        public void ClearEvents()
        {
            OnActivated = null;
            OnRelease = null;

            OnProjectileCreated = null;
            OnProjectileTriggerd = null;
            OnProjectileReleased = null;
            OnProjectileForwarding = null;

            OnActivated             ??= () => {};
            OnRelease               ??= () => {};
            
            OnProjectileCreated     ??= () => {};
            OnProjectileTriggerd    ??= () => {};
            OnProjectileReleased    ??= () => {};
            OnProjectileForwarding  ??= () => {};
        }

#endregion

#region Bindings

        public void Activate()
        {
            gameObject.SetActive(true);
            OnActivated?.Invoke();
            IsActivated = true;
            return;
        }

        public void DeActivate()
        {
            if (!IsInitialized) { return; }
            poolRefer.Release(this);
        }

#endregion

        private void Awake()
        {
            ProjectileMainModule    = ProjectileParticle.main;
            ParticleEmissionModule  = ProjectileParticle.emission;
            ParticleTriggerModule   = ProjectileParticle.trigger;
            ParticleColliderModule  = ProjectileParticle.collision;
            
            AffectType = _affectType;
            StackingType = _stackingType;
            PositioningType = _positioningType;
            ProjectileParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ProjectileMainModule.stopAction = ParticleSystemStopAction.Callback;

            NomalizeScaleVector = Vector3.Normalize(this.transform.localScale);
        }
        
        private void OnParticleSystemStopped() => DeActivate();

        protected override void OnTriggerLogic(Collider entity) {
            if(CheckIsOwnerCollider(entity)) {return;}
            if (entity.TryGetComponent<Entity>(out Entity targetEntity))
            {

                if(targetEntity.GetDamaged(CurrentProjectileDamage)) {
                    // Extras<Entity> targetAffectedExtras = OwnerRef.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
                    // targetAffectedExtras.PerformStartFunctionals(ref targetEntity);

                    VisualFXObject visualFX = VisualFXObjectPool.GetObject(_hitEffect).Init();
                    targetEntity.GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();

                    OnProjectileTriggerd.Invoke();
                }
            }
        }

        private void Update()
        {
            if(CurrentForwardingSpeed != 0) {
                transform.Translate(Vector3.forward * CurrentForwardingSpeed * Time.deltaTime);
            }
            OnProjectileForwarding?.Invoke();
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