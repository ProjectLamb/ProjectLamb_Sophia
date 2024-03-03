using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using DG.Tweening;

namespace Sophia.Instantiates
{
    using Cysharp.Threading.Tasks.Triggers;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.Entitys;
    using UnityEngine.VFX;

    /*변하는 녀석*/
    [System.Serializable]
    public struct SerialProjectileInstantiateData {
        [SerializeField] public ProjectileObject _projectileObjectRefer;
        [SerializeField] public E_INSTANTIATE_TYPE _InstantiateType;
        [SerializeField] public int _bucketIndex;
        [SerializeField] public bool _DeactivateForce;
        [SerializeField] public float _DurateTimeByRatio;
        [SerializeField] public float _SimulateSpeed;
        [SerializeField] public float _ScaleOverrideByRatio;
        [SerializeField] public float _ScaleMultiplyByRatio;
        [SerializeField] public float _ForwardingSpeedByRatio;
        [SerializeField] public int _ProjectilePower;
        [SerializeField] public SerialProjectileIntervalData _intervalData;
        [SerializeField] public E_AFFECT_TYPE _AffectType;
        [SerializeField] public SerialOnDamageExtrasModifierDatas _projectileDamageInfoByWaeponModifierDatas;
        [SerializeField] public SerialOnDamageExtrasModifierDatas _projectileDamageInfoBySkillModifierDatas;
    }

    [System.Serializable]
    public struct SerialProjectileIntervalData {
        [SerializeField] public float  _baseIntervalTime;
        [SerializeField] public float  _baseIntervalCheckTime;
        [SerializeField] public float  _baseIntervalMuls;
        [SerializeField] public bool   _isIntervalTrigger;
        [SerializeField] public bool   _isIntervalDamage;
        [SerializeField] public bool   _isIntervalExtrasConvey;
        [SerializeField] public bool   _isIntervalExtrasWeaponConvey;
        [SerializeField] public bool   _isIntervalExtrasSkillConvey;
        [HideInInspector] public bool   IsTriggerOnceOnStay;
        [HideInInspector] public float  triggerCountTime;
        [HideInInspector] public float  checkCountTime;

        private float intervalMuls;
        private float currentIntervalTime;

        public float GetCheckTime() => Time.fixedDeltaTime;
        
        public float GetCurrentIntervalTime() { 
            if(!_isIntervalTrigger) return 1000f;
            return currentIntervalTime; 
        }

        public void SetCurrentIntervalTime(float value)
        {
            if(!_isIntervalTrigger) {currentIntervalTime = 1000f; return;}
            if(value <= 0.05f)
            {
                currentIntervalTime = 0.05f; 
                return;
            }
            currentIntervalTime = value;
        }
        public float    GetIntervalMuls() => _baseIntervalMuls;
        public void     SetIntervalMuls(float muls) {
            intervalMuls = muls;
            SetCurrentIntervalTime(currentIntervalTime * intervalMuls);
        }

    }

    public enum E_INSTANTIATE_TYPE {
        None, Enemy, Weapon, Skill
    }

    public class GroundComposite {
        public Transform transformRef;
        public float detectingDistance = 0.1f;
        public GroundComposite(Transform transform, float Distance) {
            transformRef  = transform; 
            detectingDistance = Distance;
        }
        public void FixedTick() {
            RaycastHit hit;
            Vector3 distance = new Vector3(transformRef.position.x, transformRef.position.y + 1, transformRef.position.z);
            if (Physics.Raycast(distance, transformRef.TransformDirection(-Vector3.up), out hit, detectingDistance))
            {
                transformRef.position = new Vector3(transformRef.position.x, hit.point.y, transformRef.position.z);
            }
            else
            {
                transformRef.position = new Vector3(transformRef.position.x, 0, transformRef.position.z);
            }
            Debug.DrawRay(distance, transformRef.TransformDirection(-Vector3.up * detectingDistance), Color.red);
        }
    }

    public class ProjectileObject : Carrier, IPoolAccesable
    {

#region SerializeMember

        [SerializeField] private E_AFFECT_TYPE _affectType = E_AFFECT_TYPE.None;
        [SerializeField] private E_INSTANTIATE_STACKING_TYPE _stackingType = E_INSTANTIATE_STACKING_TYPE.Stack;
        [SerializeField] private E_INSTANTIATE_POSITION_TYPE _positioningType = E_INSTANTIATE_POSITION_TYPE.Outer;
        [SerializeField] private DamageInfo         _baseProjectileDamage;
        [SerializeField] private float              _baseDurateTime = 5f; //파티클 기본 지속 시간
        [SerializeField] private float              _baseSize = 1f;
        [SerializeField] private float              _baseSimulateSpeed = 1f;
        [SerializeField] private float              _baseForwardingSpeed = 5f; //파티클 기본 지속 시간
        [SerializeField] private bool               _DeactivateForce; //파티클 기본 지속 시간
        [SerializeField] private SerialProjectileIntervalData _serialProjectileIntervalData;
        [SerializeField] private Collider           _carrierCollider = null;
        [SerializeField] private Rigidbody          _carrierRigidBody = null;

#region Projectile Visual

        [SerializeField] private VisualFXObject     _destroyEffect = null;
        [SerializeField] private VisualFXObject     _hitEffect = null;
        [SerializeField] private ParticleSystem     ProjectileParticle = null;
        [SerializeField] private VisualEffect       ProjectileVFXGraph = null;
        [SerializeField] private Material           ParticleMaterial = null;

#endregion

        [SerializeField] private SerialAffectorData _serialAffectorData;


#endregion

#region Member
        public int ProjectileID {get; private set;}

        public E_AFFECT_TYPE AffectType { get {return this._affectType;} private set{} }
        public E_INSTANTIATE_STACKING_TYPE StackingType { get {return this._stackingType;} private set{} }
        public E_INSTANTIATE_POSITION_TYPE PositioningType { get {return this._positioningType;} private set{} }
        public E_INSTANTIATE_TYPE instantiateType {get; private set;}
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
                ProjectileMainModule.duration       = mCurrentDurateTime;
                ProjectileMainModule.startLifetime  = mCurrentDurateTime;
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

        private float mCurrentSimulateSpeed;
        public float CurrentSimulateSpeed
        {
            get {return mCurrentSimulateSpeed;}
            private set 
            {
                if(value >= 0.01f) {
                    mCurrentSimulateSpeed = value;
                    ProjectileMainModule.simulationSpeed  = mCurrentSimulateSpeed;
                    if(ProjectileVFXGraph!=null)ProjectileVFXGraph.playRate = mCurrentSimulateSpeed;
                    return;
                }
                mCurrentSimulateSpeed = 1f;
                ProjectileMainModule.simulationSpeed  = 1f;
                if(ProjectileVFXGraph!=null)ProjectileVFXGraph.playRate = 1f;
            }    
        }

        private ProjectileVisualData mCurrentProjectileVisualData;
        public ProjectileVisualData CurrnetProjectileVisualData {
            get {return mCurrentProjectileVisualData;}
            private set 
            {
                mCurrentProjectileVisualData = value;
                ParticleRendererModule.material.SetColor("_Color", mCurrentProjectileVisualData.ShaderUnderbarColor);
                ParticleRendererModule.material.SetFloat("_ColorPower", mCurrentProjectileVisualData.ShaderUnderbarColorPower);
            }
        }

        
        public bool IsInitialized { get; private set; }
        public bool IsActivated { get; private set; }
        
        public bool IsMoveStoped { get; private set; }

        public      ParticleSystem.MainModule       ProjectileMainModule;
        public      ParticleSystem.EmissionModule   ParticleEmissionModule;
        public      ParticleSystem.TriggerModule    ParticleTriggerModule;
        public      ParticleSystem.CollisionModule  ParticleColliderModule;
        public      ParticleSystemRenderer          ParticleRendererModule;


#endregion

#region ObjectPool

        private IObjectPool<ProjectileObject> poolRefer { get; set; }
        public void SetIndex(int id) => ProjectileID = id;

        public void SetByPool(IObjectPool<ProjectileObject> pool)
        {
            poolRefer = pool;
        }

        public void GetByPool()
        {
            gameObject.SetActive(false);
        }

        public void ReleaseByPool()
        {
            IsActivated = false;
            OnRelease?.Invoke();
            gameObject.SetActive(false);
            ResetSettings();
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
            CurrentSimulateSpeed = _baseSimulateSpeed;
            
            ProjectileVisualData data = new ProjectileVisualData {
                ShaderUnderbarColor = ParticleMaterial.GetColor("_Color"),
                ShaderUnderbarColorPower = ParticleMaterial.GetFloat("_ColorPower"),
                DestroyEffect = _destroyEffect,
                HitEffect = _hitEffect
            };

            CurrnetProjectileVisualData = data;


            ClearEvents();
            transform.tag = owner.transform.tag + "Projectile";
            IsInitialized = true;
            return this;
        }

        public ProjectileObject SetInstantiateType(E_INSTANTIATE_TYPE type) {
            instantiateType = type;
            return this;
        }

        public ProjectileObject SetDurateTimeByRatio(float muls)
        {
            CurrentDurateTime = _baseDurateTime * muls;
            return this;
        }

        public ProjectileObject SetSimulateSpeedByRatio(float muls) {
            CurrentSimulateSpeed = _baseSimulateSpeed * muls;
            return this;
        }

        public ProjectileObject SetIntervalData(in SerialProjectileIntervalData ProjectileIntervalData) {
            _serialProjectileIntervalData = ProjectileIntervalData;
            if(_serialProjectileIntervalData._isIntervalTrigger == true) {
                _serialProjectileIntervalData.IsTriggerOnceOnStay = true;
                _serialProjectileIntervalData.SetCurrentIntervalTime(_serialProjectileIntervalData._baseIntervalTime);
                _serialProjectileIntervalData.SetIntervalMuls(_serialProjectileIntervalData._baseIntervalMuls);
            }
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
            // Vector3 moveVec = (transform.forward * - Mathf.Log(1 / this._carrierRigidBody.drag)).normalized;
            // _carrierRigidBody.AddForce(moveVec * CurrentForwardingSpeed, ForceMode.VelocityChange);
            _carrierRigidBody.velocity = transform.forward * CurrentForwardingSpeed;
            return this;
        }

        /*
        이거 같은 경우는 플레이어가 자체 프로젝타일 사용 안할 수 도 있기 떄문임
        */
        public ProjectileObject SetProjectilePower(int power) 
        {
            mCurrentProjectileDamage.damageAmount = power;
            return this;
        }

        public ProjectileObject SetProjectileDamageInfoByWaepon(Extras<DamageInfo> weaponUse) 
        {
            weaponUse.PerformStartFunctionals(ref mCurrentProjectileDamage);
            return this;
        }
        
        public ProjectileObject SetProjectileDamageInfoBySkill(Extras<DamageInfo> skillUse) 
        {
            skillUse.PerformStartFunctionals(ref mCurrentProjectileDamage);
            return this;
        }

        public ProjectileObject SetAffectType(E_AFFECT_TYPE affectType) 
        {
            AffectType = affectType;
            return this;
        }

        public ProjectileObject SetProjectileVisual(ProjectileVisualData pvd) {
            CurrnetProjectileVisualData = pvd;
            return this;
        }


        private void ResetSettings()
        {
            if (IsActivated != false) { throw new System.Exception("아직 사용중임 리셋 불가능"); }

            CurrentSize = _baseSize;
            CurrentDurateTime = _baseDurateTime;
            CurrentProjectileDamage = _baseProjectileDamage;
            CurrentForwardingSpeed = _baseForwardingSpeed;
            CurrentSimulateSpeed = _baseSimulateSpeed;
            
            ProjectileVisualData data = new ProjectileVisualData {
                ShaderUnderbarColor = ParticleMaterial.GetColor("_Color"),
                ShaderUnderbarColorPower = ParticleMaterial.GetFloat("_ColorPower"),
                DestroyEffect = _destroyEffect,
                HitEffect = _hitEffect
            };

            CurrnetProjectileVisualData = data;

            
            AffectType = E_AFFECT_TYPE.None;
            instantiateType = E_INSTANTIATE_TYPE.None;

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
            if(_DeactivateForce == true) { Invoke("DeActivate", CurrentDurateTime); }
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
            ProjectileMainModule                = ProjectileParticle.main;
            ParticleEmissionModule              = ProjectileParticle.emission;
            ParticleTriggerModule               = ProjectileParticle.trigger;
            ParticleColliderModule              = ProjectileParticle.collision;
            
            if(ParticleRendererModule != null && ParticleMaterial != null) {
                ParticleRendererModule.material     = ParticleMaterial;
            }
            
            AffectType = _affectType;
            StackingType = _stackingType;
            PositioningType = _positioningType;
            ProjectileParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ProjectileMainModule.stopAction = ParticleSystemStopAction.Callback;

            NomalizeScaleVector = Vector3.Normalize(this.transform.localScale);
        }
        
        private void OnParticleSystemStopped() {
            DeActivate();
        }

        protected override void OnTriggerLogic(Collider entity)
        {
            if(CheckIsOwnerCollider(entity)) {return;}
            if (entity.TryGetComponent<Entity>(out Entity targetEntity))
            {
                if(targetEntity.GetDamaged(CurrentProjectileDamage)) {
                    OwnerRef.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect)?.PerformStartFunctionals(ref targetEntity);
                    GetExtrasWithProjectileInstantiatedType(ref targetEntity);
                    VisualFXObject visualFX = VisualFXObjectPool.GetObject(CurrnetProjectileVisualData.HitEffect).Init();
                    targetEntity.GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();
                    
                    OnProjectileTriggerd.Invoke();
                }
            }
        }

        private void OnTriggerStay(Collider other) {
            if(CheckIsOwnerCollider(other)) {return;}
            if(!_serialProjectileIntervalData.IsTriggerOnceOnStay) return;
            if (other.TryGetComponent<Entity>(out Entity targetEntity))
            {
                if(_serialProjectileIntervalData._isIntervalDamage && targetEntity.GetDamaged(CurrentProjectileDamage)) {
                    if(_serialProjectileIntervalData._isIntervalExtrasConvey) OwnerRef.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect)?.PerformStartFunctionals(ref targetEntity);
                    VisualFXObject visualFX = VisualFXObjectPool.GetObject(CurrnetProjectileVisualData.HitEffect).Init();
                    targetEntity.GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();
                    OnProjectileTriggerd.Invoke();
                }
            }     
        }
        private void Update() {
            if(!_serialProjectileIntervalData._isIntervalTrigger) return;
            if(_serialProjectileIntervalData.IsTriggerOnceOnStay == false){
                if(_serialProjectileIntervalData.triggerCountTime < _serialProjectileIntervalData.GetCurrentIntervalTime()) {
                    _serialProjectileIntervalData.triggerCountTime += Time.deltaTime;
                }
                else {
                    _serialProjectileIntervalData.IsTriggerOnceOnStay = true;
                    _serialProjectileIntervalData.triggerCountTime = 0;
                }
            }
            else {                
                if(_serialProjectileIntervalData.checkCountTime < _serialProjectileIntervalData.GetCheckTime()) {
                    _serialProjectileIntervalData.IsTriggerOnceOnStay = true;
                    _serialProjectileIntervalData.checkCountTime += Time.deltaTime;
                }
                else {
                    _serialProjectileIntervalData.IsTriggerOnceOnStay = false;
                    _serialProjectileIntervalData.checkCountTime = 0;
                }
            }
        }

        private void FixedUpdate() {
            if(!IsMoveStoped) {
                OnProjectileForwarding?.Invoke();
            }
        }

#region Helper

        public bool CheckIsSameOwner(Entity owner)
        {
            throw new System.NotImplementedException();
        }
        
        public bool CheckIsOwnerCollider(Collider target)
        {
            if(OwnerRef == null) {return false;}
            return OwnerRef.entityCollider.Equals(target);
        }

        public void GetExtrasWithProjectileInstantiatedType(ref Entity targetEntity) {
            Extras<Entity> targetAffectedExtras;
            switch(instantiateType) {
                case E_INSTANTIATE_TYPE.Weapon  : {
                    targetAffectedExtras = OwnerRef.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect);
                    break;
                }
                case E_INSTANTIATE_TYPE.Skill   : {
                    targetAffectedExtras = OwnerRef.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.SkillConveyAffect);
                    break;
                }
                default : {
                    return;
                }
            }
            targetAffectedExtras?.PerformStartFunctionals(ref targetEntity);
        }

        #endregion

    }
}