using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODPlus;
using DG.Tweening;
using MonsterLove.StateMachine;
using UnityEngine.AI;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.Composite.RenderModels;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Referer;
    using Sophia.Instantiates;

    public enum E_RAPTOR_AUDIO_INDEX
    {
        Hit = 0, Roar, Death, FootStep, Found, Howling, Idle
    }
    public abstract class Raptor : Enemy, IMovable
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
        [SerializeField] protected UnityEngine.AI.NavMeshAgent _nav;
        [Header("Raptor Settings")]
        [SerializeField] protected float TurnSpeed = 1;
        [SerializeField] protected int wanderingCoolTime = 3;
        [SerializeField] protected float AttackRange = 10;
        [SerializeField] protected SerialProjectileInstantiateData serialProjectileInstantiateData;

        #endregion

        #region Member 

        public LifeComposite Life { get; private set; }
        public RecognizeEntityComposite Recognize { get; private set; }
        public Stat MoveSpeed;
        public Stat Power;
        protected List<string> animBoolParamList;
        protected List<string> animTriggerParamList;
        protected Vector3 wanderPosition;
        protected bool IsWandering = false;
        protected float originViewRadius;
        #endregion
        protected enum States
        {
            Init,
            Idle,
            Wander,
            Chase,
            Attack,
            Death,
            //RaptorBig
            Escape,
            Howl,
            //RaptorSmall
            Tap,
            Rush,
        }

        protected StateMachine<States> fsm;
        protected override void Awake()
        {
            base.Awake();

            animBoolParamList = new List<string>();
            animTriggerParamList = new List<string>();

            Power = new Stat(_baseEntityData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural, () => { Debug.Log("공격력 수치 변경"); });
            MoveSpeed = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural, () => { Debug.Log("이동속도 수치 변경"); });

            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            Recognize = new RecognizeEntityComposite(gameObject, in _fOVData);

            _affectorManager.Init(_baseEntityData.Tenacity);
            _objectiveEntity = GameManager.Instance.PlayerGameObject.GetComponent<Entitys.Entity>();

            TryGetComponent<NavMeshAgent>(out _nav);
            TryGetComponent<Outline>(out outline);

            fsm = new StateMachine<States>(this);
            fsm.ChangeState(States.Init);
        }

        protected override void Start()
        {
            base.Start();

            Life.OnDamaged += OnRaptorHit;
            Life.OnEnterDie += OnRaptorEnterDie;
            Life.OnExitDie += OnRaptorExitDie;
        }

        protected virtual void Update()
        {
            fsm.Driver.Update.Invoke();

            if (isMovable)
            {
                _nav.enabled = true;
            }
            else
            {
                _nav.enabled = false;
                transform.DOKill();
            }

            if (IsOutline)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }

        protected virtual void FixedUpdate()
        {
            fsm.Driver.FixedUpdate.Invoke();
        }

        private void OnDisable()
        {
            Life.OnDamaged -= OnRaptorHit;
            Life.OnEnterDie -= OnRaptorEnterDie;
            Life.OnExitDie -= OnRaptorExitDie;
        }

        protected void InitAnimParamList()
        {
            for (int i = 0; i < GetModelManager().GetAnimator().parameterCount; i++)
            {
                AnimatorControllerParameter acp = this.GetModelManager().GetAnimator().GetParameter(i);
                switch (this.GetModelManager().GetAnimator().GetParameter(i).type)
                {
                    case AnimatorControllerParameterType.Bool:
                        animBoolParamList.Add(acp.name);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        animTriggerParamList.Add(acp.name);
                        break;
                    default:
                        continue;
                }
            }
        }

        protected void ResetAnimParam()
        {
            foreach (string b in animBoolParamList)
                this.GetModelManager().GetAnimator().SetBool(b, false);
            foreach (string t in animTriggerParamList)
                this.GetModelManager().GetAnimator().ResetTrigger(t);
        }

        #region Attack
        protected void DoAttack()
        {
            GetModelManager().GetAnimator().SetTrigger("DoAttack");
            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Roar].Play();
        }
        public void UseProjectile_NormalAttack()
        {
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.ATTACK]).Init(this);

            _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.ATTACK, useProjectile)
                                    .SetProjectilePower((int)GetStat(E_NUMERIC_STAT_TYPE.Power).GetValueForce())
                                    .Activate();
        }

        #endregion

        #region Life Accessible

        public override LifeComposite GetLifeComposite() => Life;

        public override bool GetDamaged(DamageInfo damage)
        {
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            else
            {
                if (isDamaged = Life.Damaged(damage))
                {
                    GameManager.Instance.NewFeatureGlobalEvent.OnEnemyHitEvent.Invoke();
                }
            }
            if (Life.IsDie) { fsm.ChangeState(States.Death); }
            return isDamaged;
        }

        public override bool Die()
        {
            Life.Died();
            return true;
        }

        public void OnRaptorHit(DamageInfo damageInfo)
        {
            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Hit].Play();
            GetModelManager().GetAnimator().SetTrigger("DoHit");
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Damaged].PlayFunctionalActOneShotWithDuration(0.3f);
            GameManager.Instance.NewFeatureGlobalEvent.EnemyHit.PerformStartFunctionals(ref GlobalHelper.NullRef);
        }

        public void OnRaptorEnterDie()
        {
            GetModelManager().GetAnimator().SetTrigger("DoDie");

            for (int i = 0; i < 4; i++)
            {
                if (_projectileBucketManager.GetProjectileBucket(i) != null)
                    _projectileBucketManager.GetProjectileBucket(i).gameObject.SetActive(false);
            }

            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Death].Play();
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Dead].PlayFunctionalActOneShotWithDuration(0.5f);

            Sophia.Instantiates.VisualFXObject visualFX = VisualFXObjectPool.GetObject(_dieParticleRef).Init();
            GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();

            CurrentInstantiatedStage.mobGenerator.RemoveMob(this.gameObject);
            GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformStartFunctionals(ref GlobalHelper.NullRef);
            SetMoveState(false);
            entityCollider.enabled = false;
        }

        public void OnRaptorExitDie()
        {
            GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformExitFunctionals(ref GlobalHelper.NullRef);
            Destroy(gameObject, 0.5f);
        }

        public void PlayRaptorFootStep()
        {
            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.FootStep].Play();
        }

        #endregion

        #region Data Accessible

        public override EntityStatReferer GetStatReferer() => StatReferer;

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;

        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);

        protected override void CollectSettable()
        {
            Settables.Add(Life);
            Settables.Add(_projectileBucketManager);
            Settables.Add(_affectorManager);
        }

        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(MoveSpeed);
            StatReferer.SetRefStat(Power);
            this.Settables.ForEach(E =>
            {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }
        public override string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

        #endregion

        #region Affector Accessible
        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

        #endregion

        #region AI
        public override RecognizeEntityComposite GetRecognizeComposite() => Recognize;

        protected void DoWander()
        {
            EQS();

            float range = Recognize.CurrentViewRadius * 2;
            float minDistance = Recognize.CurrentViewRadius;
            Vector3 randomVector = Random.insideUnitSphere * range;
            NavMeshHit hit;

            randomVector += transform.position;

            if (minDistance > Vector3.Distance(randomVector, transform.position))
                DoWander();

            if (NavMesh.SamplePosition(randomVector, out hit, range, NavMesh.AllAreas))
            {
                wanderPosition = hit.position;
                IsWandering = true;
                fsm.ChangeState(States.Wander);
            }
            else
            {
                DoWander();
            }
        }

        protected void EQS()
        {
            //Environmental Query System
        }

        #endregion

        #region Movable
        protected bool isMovable = false;
        private Vector3 moveVec;
        public (Vector3, int) GetMoveBindingData()
        {
            return (moveVec, (int)MoveSpeed.GetValueForce());
        }
        public float GetImpulsePower() => 300f;

        public bool GetMoveState() => isMovable;

        public void SetMoveState(bool movableState)
        {
            _nav.enabled = true;

            isMovable = movableState;
            _nav.isStopped = !movableState;
            if (!movableState)
            {
                _nav.enabled = false;
                transform.DOKill();
            }
        }

        public void MoveTick()
        {
            throw new System.NotImplementedException();
        }

        public UniTask Turning(Vector3 forwardingVector)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetNavMeshData()
        {
            _nav.speed = MoveSpeed.GetValueForce();
            _nav.acceleration = _nav.speed * 1.5f;
            _nav.updateRotation = false;
            // _nav.stoppingDistance = rushRange;
            //_nav.autoBraking = false;
        }
        #endregion
        [SerializeField] protected List<FMODAudioSource> _audioSources;
    }
}