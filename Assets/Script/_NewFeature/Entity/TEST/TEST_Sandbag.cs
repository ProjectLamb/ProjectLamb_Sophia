using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.Entitys
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Sophia.Composite;
    using Sophia.Composite.RenderModels;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Referer;
    using Sophia.Instantiates;
    public class TEST_Sandbag : Entity, IMovable
    {

#region SerializeMember

//      [SerializeField] protected ModelManger  _modelManger;
//      [SerializeField] protected VisualFXBucket  _visualFXBucket;
        [SerializeField] private SerialBaseEntityData       _baseEntityData;
        [SerializeField] private AffectorManager            _affectorManager;
        [SerializeField] private ProjectileBucketManager    _projectileBucketManager;
        [SerializeField] private ProjectileObject[]         _attckProjectiles;
        [SerializeField] private VisualFXObject             _dieParticleRef;
        [SerializeField] public Entity                      _objectiveEntity;

#endregion

#region Members
//      [HideInInspector] public Collider entityCollider;
//      [HideInInspector] public Rigidbody entityRigidbody;
//      [HideInInspector] protected List<IDataSettable> Settables = new();

        public LifeComposite Life {get; private set;}

#endregion

        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(Power);
            this.Settables.ForEach(E => {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }
        protected override void CollectSettable()
        {
            this.Settables.Add(Life);
            this.Settables.Add(_affectorManager);
            this.Settables.Add(_projectileBucketManager);
        }

        protected override void Awake() {
            base.Awake();
            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            Life.OnDamaged  += OnSandbagDamaged;
            Life.OnEnterDie += OnSandBagDead;

            Power = new Stat(_baseEntityData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural);

            _affectorManager.Init(_baseEntityData.Tenacity);
        }

        protected override void Start() {
            base.Start();
            ObjectiveTransform = _objectiveEntity.GetGameObject().transform;
            Life.OnEnterDie += () => {
                object NullRef = null;
                GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformStartFunctionals(ref NullRef);
            };
        }

        private void Update() {
            if(GameManager.Instance?.GlobalEvent.IsGamePaused == true) {return;}
            AITickTime();
        }

        private void OnDestroy() {
            Life.OnDamaged  -= OnSandbagDamaged;
            Life.OnEnterDie -= OnSandBagDead;
        }


#region Life Accessible
        public override LifeComposite GetLifeComposite() => Life;
        public override bool GetDamaged(DamageInfo damage) {
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            else {
                if(isDamaged = Life.Damaged(damage)) {GameManager.Instance.GlobalEvent.OnEnemyHitEvent.ForEach(Event => Event.Invoke());}
            }
            if (Life.IsDie) { Die(); }
            return isDamaged;
        }
        public override bool Die() {
            Life.Died();
            return true;
        }

        public void OnSandbagDamaged(DamageInfo info) {
            // DoHit
            _modelManager.GetAnimator().SetTrigger("DoHit");
            // SomeUi
        }

        public void OnSandBagDead() {
            // DoDie
            entityCollider.enabled = false;
            _modelManager.GetAnimator().SetTrigger("DoDie");
            VisualFXObject visualFXFromPool = VisualFXObjectPool.GetObject(_dieParticleRef).Init();
            _visualFXBucket.InstantablePositioning(visualFXFromPool);
        }

#endregion

#region Stat Accessible 

        public override EntityStatReferer GetStatReferer() => StatReferer;
        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;
        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);
        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);
        [ContextMenu("Get Stats Info")]
        public override string GetStatsInfo() {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

#endregion

#region Affectable

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

#endregion

#region AI
    
        [HideInInspector] public Transform ObjectiveTransform;
        private Stat Power;
        public void AITickTime() {
            if(!Life.IsDie) transform.LookAt(ObjectiveTransform);
        }
        
        public void AI_AnimationMarker_NormalAttack(){
            ProjectileObject projectileFromPool = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.ATTACK]).Init(this);
            this._projectileBucketManager.InstantablePositioning((int)ANIME_STATE.ATTACK,projectileFromPool).Activate();
        }
        public void AI_AnimationMarker_JumpAttack(){
            ProjectileObject projectileFromPool = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.JUMP]).Init(this);
            this._projectileBucketManager.InstantablePositioning((int)ANIME_STATE.JUMP, projectileFromPool).Activate();
        }
        public void AI_AnimationMarker_DestroySelf() => Destroy(gameObject);
        [ContextMenu("평타", false, int.MaxValue)]
        void InstantiateProjectiles1(){
            //Find Instantiate On This Animator Events;
            _modelManager.GetAnimator().SetTrigger("DoAttack");
        }
    
        [ContextMenu("범위데미지",false, int.MaxValue)]
        void InstantiateProjectiles2(){
            //Find Instantiate On This Animator Events;
            _modelManager.GetAnimator().SetTrigger("DoJump");
        }
#endregion

#region Movable

        public bool GetMoveState() => false;
        public void SetMoveState(bool movableState){return;}
        public void MoveTick() {return;}

        public UniTask Turning(Vector3 forwardingVector)
        {
            throw new System.NotImplementedException();
        }

#endregion

    }
}