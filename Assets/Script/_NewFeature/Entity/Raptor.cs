using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODPlus;
using DG.Tweening;
using MonsterLove.StateMachine;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.Composite.RenderModels;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Referer;
    using Sophia.Instantiates;

    public class Raptor : Enemy, IMovable
    {

#region Serialized Member

        // [SerializeField] protected SerialBaseEntityData       _baseEntityData;
        // [SerializeField] protected SerialFieldOfViewData      _fOVData;
        // [SerializeField] protected AffectorManager            _affectorManager;
        // [SerializeField] protected ProjectileBucketManager    _projectileBucketManager;
        // [SerializeField] protected ProjectileObject[]         _attckProjectiles;
        // [SerializeField] protected VisualFXObject             _spawnParticleRef;
        // [SerializeField] protected VisualFXObject             _dieParticleRef;
        // [SerializeField] public    Entity                     _objectiveEntity;
        // [SerializeField] protected E_MOB_AI_DIFFICULTY        _mobDifficulty;
        [SerializeField]    protected SerialFieldOfViewData         _fovData;
        [SerializeField]    protected FMODAudioSource               _audioSources;
        [SerializeField]    protected UnityEngine.AI.NavMeshAgent   _nav;

#endregion

#region Member 

        public LifeComposite            Life {get; private set;}
        public RecognizeEntityComposite Recognize {get; private set;}
        public Stat MoveSpeed;
        public Stat Power;
        public DataSystem.Atomics.DashAtomics DashAtomics;

        private enum States { 
            Idle, 
            FirstRecog, Wandering,
            Walk, Chase, Rush,
            Attack, Collider, None
        }
        private object NullRef = null;
        private StateMachine<States> fsm;

        protected override void CollectSettable(){
            Settables.Add(Life);
            Settables.Add(_projectileBucketManager);
            Settables.Add(_affectorManager);
        }

        protected override void SetDataToReferer(){
            StatReferer.SetRefStat(MoveSpeed);
            StatReferer.SetRefStat(Power);
            this.Settables.ForEach(E => {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }

        protected override void Awake() {
            base.Awake();
            Power       = new Stat(_baseEntityData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural);
            MoveSpeed   = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural);
            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            _affectorManager.Init(_baseEntityData.Tenacity);
            _objectiveEntity = GameManager.Instance.PlayerGameObject.GetComponent<Entitys.Entity>();
        }

        protected override void Start() {
            base.Start();
            Life.OnDamaged      += OnRaptorHit;
            Life.OnEnterDie     += OnRaptorEnterDie;
            Life.OnExitDie      += OnRaptorExitDie;
            Recognize = new RecognizeEntityComposite(gameObject, in _fovData);
            DashAtomics = new DataSystem.Atomics.DashAtomics(entityRigidbody, GetMoveBindingData, GetImpulsePower);
            NavMeshSet();

            fsm = new StateMachine<States>(this);
            fsm.ChangeState(States.Idle);
        }

        private void Update() {
            fsm.Driver.Update.Invoke();
        }

        private void FixedUpdate() {
            fsm.Driver.FixedUpdate.Invoke();
        }

        private void OnDisable() {
            Life.OnDamaged      -= OnRaptorHit;
            Life.OnEnterDie     -= OnRaptorEnterDie;
            Life.OnExitDie      -= OnRaptorExitDie;
        }

#endregion

#region Life Accessibe

        public override LifeComposite GetLifeComposite() => Life;
        
        public override bool GetDamaged(DamageInfo damage){
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            else {
                if(isDamaged = Life.Damaged(damage)) {}
            }
            if (Life.IsDie) { Die(); }
            return isDamaged;
        }

        public override bool Die(){
            Life.Died();
            return true;
        }

        public void OnRaptorHit(DamageInfo damageInfo) {
            GetModelManger().GetAnimator().SetTrigger("DoHit");
            GameManager.Instance.NewFeatureGlobalEvent.EnemyHit.PerformStartFunctionals(ref NullRef);
        }

        public void OnRaptorEnterDie() {
            GetModelManger().GetAnimator().SetTrigger("DoDie");
            GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformStartFunctionals(ref NullRef);
            SetMoveState(false);
            entityCollider.enabled = false;
            //IsLook
            //transform.parent.GetComponent<RaptorFlocks>().CurrentAmount--;
            //nav.enabled = false;
        }
        
        public void OnRaptorExitDie() {
            GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformExitFunctionals(ref NullRef);
        }

#endregion

#region Data Accessible

        public override EntityStatReferer GetStatReferer() => StatReferer;
        
        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;

        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);
        
        public override string GetStatsInfo(){
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

#endregion

#region Affector Accessible

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

#endregion

#region Stage

        // public Stage CurrentInstantiatedStage;

#endregion

#region AI
        public override RecognizeEntityComposite GetRecognizeComposite() => Recognize;
        
        // Parent public void DoHowl(){}
        // Parent public void DoBuff(){}
        
        // void DoWander()
        // {
        //     EQS();

        //     float range = recognize.CurrentViewRadius * 2;
        //     float minDistance = recognize.CurrentViewRadius;
        //     Vector3 randomVector = Random.insideUnitSphere * range;
        //     NavMeshHit hit;

        //     randomVector += transform.position;

        //     if (minDistance > Vector3.Distance(randomVector, transform.position))
        //         DoWander();

        //     if (NavMesh.SamplePosition(randomVector, out hit, range, NavMesh.AllAreas))
        //     {
        //         wanderPosition = hit.position;
        //         IsWandering = true;
        //         fsm.ChangeState(States.Wander);
        //     }
        //     else
        //     {
        //         DoWander();
        //     }
        // }



        public virtual void DoAttack() {
            ProjectileObject DashProjectile = ProjectilePool.GetObject(_attckProjectiles[0]).Init(this);
            _projectileBucketManager.InstantablePositioning(1, DashProjectile)
                                        .SetInstantiateType(E_INSTANTIATE_TYPE.Enemy)
                                        .SetProjectilePower(Power)
                                        .Activate();
            DashAtomics.Invoke();
        }

#endregion

#region Movable
        private bool isMovable = false;
        private Vector3 moveVec;
        public (Vector3, int) GetMoveBindingData() {
            return (moveVec, (int)MoveSpeed.GetValueForce());
        }
        public float GetImpulsePower() => 300f;

        public bool GetMoveState() => isMovable;

        public void SetMoveState(bool movableState)
        {
            isMovable = movableState;
        }

        public void MoveTick()
        {
            throw new System.NotImplementedException();
        }

        public UniTask Turning()
        {
            throw new System.NotImplementedException();
        }

        public virtual void NavMeshSet(){
            _nav.speed          = MoveSpeed.GetValueForce();
            _nav.acceleration   = _nav.speed * 1.5f;
            _nav.updateRotation = false;
            // _nav.stoppingDistance = rushRange;
            _nav.autoBraking = false;
        }

#endregion

// #region Finate State Machine
//         public void Idle_Enter() {
//             Recognize.CurrentViewRadius = _fovData.viewRadius;
            
//         }
//         public void Idle_Update() {
//             if(Recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose) {
//                 if(isMovable) {
                    
//                     if(_nav.remainingDistance <= _nav.stoppingDistance) {
//                     }
//                 }
//                 // fsm.ChangeState(States.Wandering);
//             }
//             else {
//                 if(Recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog) {
//                     return;
//                 }
//                 if(Recognize.GetCurrentRecogState() == E_RECOG_TYPE.ReRecog) {
//                     return;
//                 }
//             }
//         }

//         void Idle() {}
        
//         void Look() {}
//         void Look() {}
//         void Look() {}
        
//         public bool IsWalking;

//         void Chase() {}
//         void Chase() {}

//         void Chase_FixedUpdate() {
//             if(IsWalking) {
//                 GetModelManger().GetAnimator().SetBool("IsWalk", true);
//                 transform.DOLookAt(_objectiveEntity.transform.position, 0.5f);
//                 _nav.SetDestination(_objectiveEntity.transform.position);
//             }
//             else {
//                 GetModelManger().GetAnimator().SetBool("IsWalk", false);
//             }
//         }
        
//         void Rush() {}
//         void Rush() {}
//         void Rush() {}
        
// #endregion

#region Helper

#endregion
    }
}