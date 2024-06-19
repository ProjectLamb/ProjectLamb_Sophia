using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using DG.Tweening;
using Sophia.Composite;
using Sophia.DataSystem.Referer;
using Sophia.DataSystem;
using Sophia.DataSystem.Modifiers;
using Cysharp.Threading.Tasks;
using FMODPlus;

namespace Sophia.Entitys
{
    // public enum E_MOLLU_AUDIO_INDEX {
    //     // 사운드 추가해야 함
    //     TODO
    // }
    public class Mollu : Enemy, IMovable
    {
        #region Public
        public ParticleSystem LeftRocketParticle;
        public ParticleSystem RightRocketParticle;
        public int AttackRange;
        public int TurnSpeed;   //Stat으로 관리 가능성
        #endregion

        #region Private
        private LifeComposite Life { get; set; }
        private Vector3 wanderPosition;
        List<string> animBoolParamList;
        List<string> animTriggerParamList;
        private bool IsWandering;
        bool isFirstAttack = true;

        private float originViewRadius;
        private NavMeshAgent nav;

        ParticleSystem.MainModule _leftRocketParticleMain;
        ParticleSystem.MainModule _rightRocketParticleMain;
        ParticleSystem.MinMaxCurve _rocketStartLifetimeAtStop;
        ParticleSystem.MinMaxCurve _rocketStartLifetimeAtMove;
        ParticleSystem.MinMaxCurve _rocketStartSpeedAtStop;
        ParticleSystem.MinMaxCurve _rocketStartSpeedAtMove;
        ParticleSystem.MinMaxCurve _rocketStartSizeAtStop;
        ParticleSystem.MinMaxCurve _rocketStartSizeAtMove;
        ParticleSystem.MinMaxGradient _rocketStartColorAtStop;
        ParticleSystem.MinMaxGradient _rocketStartColorAtMove;
        Vector3 _lastPos;
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
            Threat,
            Attack,
            Death,
        }

        private object NullRef = null;

        StateMachine<States> fsm;
        protected override void Awake()
        {
            base.Awake();

            power = new Stat(_baseEntityData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural, () => { Debug.Log("Mollu) 공격력 수치 변경"); });
            moveSpeed = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural, () => { Debug.Log("Mollu) 이동속도 수치 변경"); });

            recognize = new RecognizeEntityComposite(this.gameObject, this._fOVData);
            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);

            _affectorManager.Init(_baseEntityData.Tenacity);
            _objectiveEntity = GameManager.Instance.PlayerGameObject.GetComponent<Entity>();

            animBoolParamList = new List<string>();
            animTriggerParamList = new List<string>();

            _leftRocketParticleMain = LeftRocketParticle.main;
            _rightRocketParticleMain = RightRocketParticle.main;
            _rocketStartLifetimeAtStop = _leftRocketParticleMain.startLifetime;
            _rocketStartLifetimeAtMove = _rocketStartLifetimeAtStop.constant / 2;
            _rocketStartSpeedAtStop = _leftRocketParticleMain.startSpeed.constant;
            _rocketStartSpeedAtMove = _rocketStartSpeedAtStop.constant + 20;
            _rocketStartSizeAtStop = _leftRocketParticleMain.startSize;
            _rocketStartSizeAtMove = _rocketStartSizeAtStop.constant + 5;
            _rocketStartColorAtStop = _leftRocketParticleMain.startColor;
            _rocketStartColorAtMove = new ParticleSystem.MinMaxGradient(Color.red);

            _lastPos = transform.position;

            TryGetComponent<NavMeshAgent>(out nav);

            fsm = new StateMachine<States>(this);
            fsm.ChangeState(States.Init);
        }

        protected override void Start()
        {
            base.Start();

            Life.OnDamaged += OnMolluHit;
            Life.OnEnterDie += OnMolluEnterDie;
            Life.OnExitDie += OnMolluExitDie;
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

        void FixedUpdate()
        {
            // Check entity is moving or not
            if (transform.position != _lastPos)
            {  // Moving
                // _leftRocketParticleMain.startLifetime = _rocketStartLifetimeAtMove;
                _leftRocketParticleMain.startSpeed = _rocketStartSpeedAtMove;
                _leftRocketParticleMain.startSize = _rocketStartSizeAtMove;
                _leftRocketParticleMain.startColor = _rocketStartColorAtMove;

                // _rightRocketParticleMain.startLifetime = _rocketStartLifetimeAtMove;
                _rightRocketParticleMain.startSpeed = _rocketStartSpeedAtMove;
                _rightRocketParticleMain.startSize = _rocketStartSizeAtMove;
                _rightRocketParticleMain.startColor = _rocketStartColorAtMove;
            }
            else
            {  // Stationary
                // _leftRocketParticleMain.startLifetime = _rocketStartLifetimeAtStop;
                _leftRocketParticleMain.startSpeed = _rocketStartSpeedAtStop;
                _leftRocketParticleMain.startSize = _rocketStartSizeAtStop;
                _leftRocketParticleMain.startColor = _rocketStartColorAtStop;

                // _rightRocketParticleMain.startLifetime = _rocketStartLifetimeAtStop;
                _rightRocketParticleMain.startSpeed = _rocketStartSpeedAtStop;
                _rightRocketParticleMain.startSize = _rocketStartSizeAtStop;
                _rightRocketParticleMain.startColor = _rocketStartColorAtStop;
            }
            _lastPos = transform.position;

            fsm.Driver.FixedUpdate.Invoke();
        }

        public void OnMolluHit(DamageInfo damageInfo)
        {
            // GetModelManager().GetAnimator().SetTrigger("DoHit");
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Damaged].PlayFunctionalActOneShotWithDuration(0.3f);
            GameManager.Instance.NewFeatureGlobalEvent.EnemyHit.PerformStartFunctionals(ref GlobalHelper.NullRef);
        }

        public void OnMolluEnterDie()
        {
            // GetModelManager().GetAnimator().SetTrigger("DoDie");
            // _audioSources[(int)E_MOLLU_AUDIO_INDEX.Death].Play();

            //VFX
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Dead].PlayFunctionalActOneShotWithDuration(0.5f);
            Sophia.Instantiates.VisualFXObject visualFX = VisualFXObjectPool.GetObject(_dieParticleRef).Init();
            GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();

            CurrentInstantiatedStage.mobGenerator.RemoveMob(gameObject);
            GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformStartFunctionals(ref GlobalHelper.NullRef);
            SetMoveState(false);
            entityCollider.enabled = false;

        }

        public void OnMolluExitDie()
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
            if (isFirstAttack)
            {
                GetModelManager().GetAnimator().SetTrigger("DoFirstAttack1");
                isFirstAttack = false;
            }
            else
            {
                GetModelManager().GetAnimator().SetTrigger("DoAttack1");
            }
        }

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
            //Environmental Query System
        }

        #region Attack
        private Stat power;

        public void UseProjectile_NormalAttack()
        {
            Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.ATTACK]).Init(this);

            _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.ATTACK, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power))
                                    .Activate();
        }
        #endregion

        #region FSM Functions
        ////////////////////////////////////////FSM Functions////////////////////////////////////////
        /** Init State */
        void Init_Enter()
        {
            Debug.Log("Mollu) Init_Enter");

            //Init Settings
            originViewRadius = recognize.CurrentViewRadius;
            SetNavMeshData();
            InitAnimParamList();

            fsm.ChangeState(States.Idle);
        }

        /** Idle State */
        void Idle_Enter()
        {
            Debug.Log("Mollu) Idle_Enter");
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
                if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog)
                {
                    fsm.ChangeState(States.Threat);
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
        }

        /**Threat State*/
        // TODO) 플레이어 발견시 이펙트 추가 예정 (예시 : 머리에서 빨간 빛이 나옴)
        void Threat_Enter()
        {
            Debug.Log("Mollu) Threat_Enter");

            if (!IsMovable) return;
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            transform.DOKill();
            recognize.CurrentViewRadius *= 3;
            // GetModelManger().GetAnimator().SetTrigger("DoThreat");

            // _audioSources[(int)E_MOLLU_AUDIO_INDEX.TODO].Play();
        }

        void Threat_Update()
        {
            // if (GetModelManger().GetAnimator().GetBool("IsThreatEnd")) {
            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose)
                fsm.ChangeState(States.Idle);
            else
            {
                float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);

                if (dist <= AttackRange)
                    fsm.ChangeState(States.Attack);
                else
                    fsm.ChangeState(States.Move);
            }
            // }
        }

        void Threat_FixedUpdate()
        {
            if (IsMovable)
            {
                transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
            }
        }

        void Threat_Exit()
        {
            // GetModelManger().GetAnimator().SetBool("IsThreatEnd", false);
        }

        /**Move State*/
        void Move_Enter()
        {
            Debug.Log("Mollu) Move_Enter");
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

        void Move_Exit()
        {
            // _audioSources[(int)E_MOLLU_AUDIO_INDEX.TODO].Stop();
        }

        /**Wander State*/
        void Wander_Enter()
        {
            System.Random random = new System.Random();
            Debug.Log("Mollu) Wander_Enter");

            Invoke("DoWander", random.Next(0, wanderingCoolTime + 1));
            SetMoveState(true);
        }

        void Wander_Update()
        {
            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog || recognize.GetCurrentRecogState() == E_RECOG_TYPE.ReRecog)
            {
                CancelInvoke();
                fsm.ChangeState(States.Threat);
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
            Debug.Log("Mollu) Exit Wander");

            IsWandering = false;
        }

        /**Attack State*/
        // 움직임은 멈추되, 시선은 따라가도록 처리
        void Attack_Enter()
        {
            Debug.Log("Mollu) Attack_Enter");

            if (!IsMovable) return;
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            transform.DOKill();

            DoAttack();
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Attack].PlayFunctionalActOneShotWithDuration(1.5f);
        }

        void Attack_Update()
        {
            if (GetModelManager().GetAnimator().GetBool("IsAttackEnd"))
            {
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
            else
            {
                isFirstAttack = true;
            }
        }

        void Attack_Exit()
        {
            ResetAnimParam();
        }

        /**Death State*/
        void Death_Enter()
        {
            Debug.Log("Mollu) Death_Enter");
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
            Debug.Log("Mollu) power : " + power.GetValueForce());
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
            var randomAngle = random.Next(-180, 180);
            Vector3[] rotateMatrix = new Vector3[] {
                new Vector3(Mathf.Cos(randomAngle), 0 , Mathf.Sin(randomAngle)),
                new Vector3(0, 1 , 0),
                new Vector3(-Mathf.Sin(randomAngle), 0 , Mathf.Cos(randomAngle))
            };
            Vector3 retatedVec = Vector3.zero + Vector3.up;
            retatedVec += EndPosForward.x * rotateMatrix[0];
            retatedVec += EndPosForward.y * rotateMatrix[1];
            retatedVec += EndPosForward.z * rotateMatrix[2];
            var randomDist = (float)random.NextDouble() * 7;
            var randomForce = (float)random.NextDouble();
            var randomTime = (float)(random.NextDouble() * 2 + 0.5);
            Debug.Log(retatedVec * randomDist);
            Tween jumpTween = itemObject.transform.DOLocalJump((retatedVec * randomDist) + transform.position, randomForce * 25, 1, randomTime).SetEase(Ease.OutBounce);
            return mySequence.Append(jumpTween);
        }
        [SerializeField] protected List<FMODAudioSource> _audioSources;
    }
}