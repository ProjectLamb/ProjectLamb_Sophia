using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HUDIndicator;
using Sophia.Composite;
using UnityEngine.AI;
using MonsterLove.StateMachine;
using DG.Tweening;
using Sophia.DataSystem;
using Sophia.DataSystem.Referer;
using Sophia.DataSystem.Modifiers;
using Cysharp.Threading.Tasks;
using FMODPlus;

namespace Sophia.Entitys
{
    public class Aeternus : Enemy, IMovable
    {
        #region Public

        public IndicatorOffScreen indicatorOffScreen;
        public int AttackRange;
        public int TurnSpeed;

        #endregion

        #region Private
        private LifeComposite Life { get; set; }
        private Vector3 wanderPosition;
        List<string> animBoolParamList;
        List<string> animTriggerParamList;
        private bool IsWandering;
        private float originViewRadius;
        private NavMeshAgent nav;

        #endregion

        #region Serialize Member
        [SerializeField] protected RecognizeEntityComposite recognize;
        [SerializeField] private bool IsMovable = false;
        [SerializeField] private int wanderingCoolTime = 3;

        #endregion

        public enum States
        {
            Init,
            Idle,
            Move,
            Wander,
            Attack,
            Death,
        }

        StateMachine<States> fsm;

        protected override void Awake()
        {
            base.Awake();

            power = new Stat(_baseEntityData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural, () => { Debug.Log("Aeternus) 공격력 수치 변경"); });
            moveSpeed = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural, () => { Debug.Log("Aeternus) 이동속도 수치 변경"); });

            recognize = new RecognizeEntityComposite(this.gameObject, this._fOVData);
            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);

            _affectorManager.Init(_baseEntityData.Tenacity);
            _objectiveEntity = GameManager.Instance.PlayerGameObject.GetComponent<Entity>();

            animBoolParamList = new List<string>();
            animTriggerParamList = new List<string>();

            TryGetComponent<NavMeshAgent>(out nav);

            fsm = new StateMachine<States>(this);
            fsm.ChangeState(States.Init);
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            Life.OnDamaged += OnAeternusHit;
            Life.OnEnterDie += OnAeternusEnterDie;
            Life.OnExitDie += OnAeternusExitDie;
        }

        // Update is called once per frame
        void Update()
        {
            fsm.Driver.Update.Invoke();

            if (IsMovable)
            {
                nav.enabled = true;
            }
            else
            {
                nav.enabled = false;
                transform.DOKill();
            }
        }

        private void FixedUpdate()
        {
            fsm.Driver.FixedUpdate.Invoke();
        }

        public void OnAeternusHit(DamageInfo damageInfo)
        {
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Damaged].PlayFunctionalActOneShotWithDuration(0.3f);
            GameManager.Instance.NewFeatureGlobalEvent.EnemyHit.PerformStartFunctionals(ref GlobalHelper.NullRef);
        }

        public void OnAeternusEnterDie()
        {
            //VFX
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Dead].PlayFunctionalActOneShotWithDuration(0.5f);
            Sophia.Instantiates.VisualFXObject visualFX = VisualFXObjectPool.GetObject(_dieParticleRef).Init();
            GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();

            if (CurrentInstantiatedStage != null)
                CurrentInstantiatedStage.mobGenerator.RemoveMob(gameObject);

            GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformStartFunctionals(ref GlobalHelper.NullRef);
            SetMoveState(false);
            entityCollider.enabled = false;
        }

        public void OnAeternusExitDie()
        {
            GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformExitFunctionals(ref GlobalHelper.NullRef);
            Destroy(gameObject, 0.5f);
        }

        void SetNavMeshData()
        {
            nav.speed = moveSpeed.GetValueForce();
            nav.acceleration = nav.speed * 1.5f;
            nav.updateRotation = false;
            nav.stoppingDistance = AttackRange;
            SetMoveState(true);
        }

        public override bool Die()
        {
            Life.Died();
            return true;
        }

        void InitAnimParamList()
        {
            for (int i = 0; i < GetModelManager().GetAnimator().parameterCount; i++)
            {
                AnimatorControllerParameter acp = GetModelManager().GetAnimator().GetParameter(i);
                switch (GetModelManager().GetAnimator().GetParameter(i).type)
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

        void ResetAnimParam()
        {
            foreach (string b in animBoolParamList)
                GetModelManager().GetAnimator().SetBool(b, false);
            foreach (string t in animTriggerParamList)
                GetModelManager().GetAnimator().ResetTrigger(t);
        }

        void DoAttack()
        {
            GetModelManager().GetAnimator().SetBool("IsWalk", false);
            GetModelManager().GetAnimator().SetTrigger("DoAttack");
        }

        #region Attack
        private Stat power;

        //맞게 수정하기
        public void UseProjectile_NormalAttack1()
        {
            Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.ATTACK]).Init(this);

            _projectileBucketManager.InstantablePositioning(1, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power))
                                    .Activate();
        }

        public void UseProjectile_NormalAttack2()
        {
            Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.ATTACK]).Init(this);

            _projectileBucketManager.InstantablePositioning(2, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power))
                                    .Activate();
        }
        #endregion

        void DoWander()
        {
            EQS();

            float range = recognize.CurrentViewRadius * 2;
            float minDistance = recognize.CurrentViewRadius;
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

        void EQS()
        {

        }

        #region FSM Functions
        ////////////////////////////////////////FSM Functions////////////////////////////////////////
        /** Init State */

        void Init_Enter()
        {
            Debug.Log("Aeternus) Init_Enter");

            //Init Settings
            originViewRadius = recognize.CurrentViewRadius;
            SetNavMeshData();
            InitAnimParamList();

            fsm.ChangeState(States.Idle);
        }

        /** Idle State */
        void Idle_Enter()
        {
            Debug.Log("Aeternus) Idle_Enter");
            recognize.CurrentViewRadius = originViewRadius;

            if (!IsMovable) return;
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            transform.DOKill();
        }

        void Idle_Update()
        {
            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.None || recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose)
            {
                if (!IsWandering)
                    fsm.ChangeState(States.Wander);
            }
            else
            {
                float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);
                if (dist <= AttackRange)
                    fsm.ChangeState(States.Attack);
                else
                    fsm.ChangeState(States.Move);
            }
        }

        /**Move State*/
        void Move_Enter()
        {
            Debug.Log("Aeternus) Move_Enter");
            GetModelManager().GetAnimator().SetBool("IsWalk", true);
            // _audioSources[(int)E_MOLLU_AUDIO_INDEX.TODO].Play();
            nav.isStopped = false;
            nav.enabled = true;
        }

        void Move_Update()
        {
            float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);

            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose)
                fsm.ChangeState(States.Idle);
            else if (dist <= AttackRange)
                fsm.ChangeState(States.Attack);

        }

        void Move_FixedUpdate()
        {
            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog || recognize.GetCurrentRecogState() == E_RECOG_TYPE.ReRecog)
            {
                if (IsMovable)
                {
                    transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
                    nav.SetDestination(_objectiveEntity.transform.position);
                }
            }
        }

        /**Wander State*/
        void Wander_Enter()
        {
            System.Random random = new System.Random();
            Debug.Log("Aeternus) Wander_Enter");

            Invoke("DoWander", random.Next(0, wanderingCoolTime + 1));
            SetMoveState(true);
        }

        void Wander_Update()
        {
            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog || recognize.GetCurrentRecogState() == E_RECOG_TYPE.ReRecog)
            {
                CancelInvoke();
                fsm.ChangeState(States.Idle);
            }
            else if (IsWandering && nav.remainingDistance <= nav.stoppingDistance)
                fsm.ChangeState(States.Idle);
        }

        void Wander_FixedUpdate()
        {
            if (IsWandering && IsMovable)
            {
                transform.DOLookAt(wanderPosition, TurnSpeed);
                nav.SetDestination(wanderPosition);
            }
        }

        void Wander_Exit()
        {
            Debug.Log("Aeternus) Exit Wander");
            IsWandering = false;
        }

        /** Attack State */
        void Attack_Enter()
        {
            indicatorOffScreen.style.color = Color.red;
            indicatorOffScreen.arrowStyle.color = Color.red;
            Debug.Log("Aeternus) Attack_Enter");

            if (!IsMovable) return;
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            transform.DOKill();

            DoAttack();
            //애니메이션에 맞게 수정
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Attack].PlayFunctionalActOneShotWithDuration(1.0f);
        }

        void Attack_Update()
        {
            if (GetModelManager().GetAnimator().GetBool("IsAttackEnd"))
            {
                indicatorOffScreen.style.color = Color.yellow;
                indicatorOffScreen.arrowStyle.color = Color.yellow;
                fsm.ChangeState(States.Idle);
            }
        }

        void Attack_FixedUpdate()
        {
            float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);
            if (dist <= AttackRange)
            {
                if (IsMovable)
                {
                    transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed * 1.2f);
                }
            }
        }

        void Attack_Exit()
        {
            ResetAnimParam();
        }

        /** Death State */

        void Death_Enter()
        {
            Debug.Log("Aeternus) Death_Enter");
            List<Sophia.Instantiates.ItemObject> itemObjects;
            itemObjects = GetComponent<Sophia.Instantiates.GachaComponent>().InstantiateReward();

            foreach (Sophia.Instantiates.ItemObject itemObject in itemObjects)
            {
                if (itemObject == null) continue;
                itemObject.SetTriggerTime(1f).SetTweenSequence(SetSequnce(itemObject)).Activate();
            }
            Die();
        }

        #endregion

        #region Inherited Functions From Enemy Class
        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(power);
            Debug.Log("Aeternus) power : " + power.GetValueForce());
            StatReferer.SetRefStat(moveSpeed);
            this.Settables.ForEach(E =>
            {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }

        public override EntityStatReferer GetStatReferer() => this.StatReferer;

        protected override void CollectSettable()
        {
            Settables.Add(Life);
            Settables.Add(_projectileBucketManager);
            Settables.Add(_affectorManager);
        }

        public override LifeComposite GetLifeComposite() => this.Life;
        public override RecognizeEntityComposite GetRecognizeComposite() => this.recognize;

        public override bool GetDamaged(DamageInfo damage)
        {
            bool isDamaged = false;
            if (Life.IsDie)
            {
                isDamaged = false;
            }
            else
            {
                if (isDamaged = Life.Damaged(damage))
                {
                    GameManager.Instance.NewFeatureGlobalEvent.OnEnemyHitEvent.Invoke();
                }
            }
            if (Life.IsDie)
            {
                fsm.ChangeState(States.Death);
            }
            return isDamaged;
        }

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        public override string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;

        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();

        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);

        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);
        #endregion

        #region Move

        private Stat moveSpeed;

        public bool GetMoveState() => IsMovable;

        public void SetMoveState(bool movableState)
        {
            IsMovable = movableState;
            if (IsMovable)
            {
                nav.enabled = true;
                nav.isStopped = false;
            }
            else
            {
                if (!IsMovable)
                    return;

                nav.isStopped = true;
                nav.enabled = false;
                transform.DOKill();
            }
        }

        public void MoveTick()
        {
            //Currently using Nav.SetDestination
            throw new System.NotImplementedException();
        }

        public UniTask Turning(Vector3 forwardingVector)
        {
            //Currently using DoTween.DoLookAt
            throw new System.NotImplementedException();
        }

        #endregion

        public Sequence SetSequnce(Sophia.Instantiates.ItemObject itemObject)
        {
            Sequence mySequence = DOTween.Sequence();
            System.Random random = new System.Random();
            Vector3 EndPosForward = transform.right;
            var randomAngle = 0;
            Vector3[] rotateMatrix = new Vector3[] {
                new Vector3(Mathf.Cos(randomAngle), 0 , Mathf.Sin(randomAngle)),
                new Vector3(0, 1 , 0),
                new Vector3(-Mathf.Sin(randomAngle), 0 , Mathf.Cos(randomAngle))
            };
            Vector3 retatedVec = Vector3.zero + Vector3.up;
            retatedVec += EndPosForward.x * rotateMatrix[0];
            retatedVec += EndPosForward.y * rotateMatrix[1];
            retatedVec += EndPosForward.z * rotateMatrix[2];
            Tween jumpTween = itemObject.transform.DOLocalJump(retatedVec + transform.position, 10, 1, 1).SetEase(Ease.OutBounce);
            return mySequence.Append(jumpTween);
        }
        [SerializeField] protected List<FMODAudioSource> _audioSources;
    }

}