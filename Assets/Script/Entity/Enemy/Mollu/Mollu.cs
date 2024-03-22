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
using Sophia_Carriers;
using UnityEngine.Rendering;
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

            TryGetComponent<NavMeshAgent>(out nav);

            fsm = new StateMachine<States>(this);
            fsm.ChangeState(States.Init);
        }

        protected override void Start()
        {
            base.Start();

            Life.OnEnterDie += OnMolluEnterDie;
            Life.OnExitDie += OnMolluExitDie;

            InitAnimParamList();
        }

        // Update is called once per frame
        void Update()
        {
            fsm.Driver.Update.Invoke();
        }

        void FixedUpdate()
        {
            fsm.Driver.FixedUpdate.Invoke();
        }

        public void OnMolluEnterDie()
        {
            CurrentInstantiatedStage.mobGenerator.RemoveMob(gameObject);
        }

        public void OnMolluExitDie()
        {
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

        public override bool Die() { Life.Died(); return true; }

        void InitAnimParamList()
        {
            for (int i = 0; i < GetModelManger().GetAnimator().parameterCount; i++)
            {
                AnimatorControllerParameter acp = GetModelManger().GetAnimator().GetParameter(i);
                switch (GetModelManger().GetAnimator().GetParameter(i).type)
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
                GetModelManger().GetAnimator().SetBool(b, false);
            foreach (string t in animTriggerParamList)
                GetModelManger().GetAnimator().ResetTrigger(t);
        }

        void DoAttack()
        {
            if (isFirstAttack) {
                GetModelManger().GetAnimator().SetTrigger("DoFirstAttack1");
                isFirstAttack = false;
            } else {
                GetModelManger().GetAnimator().SetTrigger("DoAttack1");
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

            if (NavMesh.SamplePosition(randomVector, out hit, range, NavMesh.AllAreas)) {
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

            fsm.ChangeState(States.Idle);
        }

        /** Idle State */
        void Idle_Enter()
        {
            Debug.Log("Mollu) Idle_Enter");
            recognize.CurrentViewRadius = originViewRadius;

            if(!IsMovable) return;
            nav.isStopped = true;
            nav.enabled = false;
            transform.DOKill();
        }

        void Idle_Update()
        {
            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.None || recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose) {
                if (!IsWandering)
                    fsm.ChangeState(States.Wander);
            } else {
                if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog) {
                    fsm.ChangeState(States.Threat);
                } else {
                    float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);
                    if (dist <= AttackRange)
                        fsm.ChangeState(States.Attack);
                    else
                        fsm.ChangeState(States.Move);
                }
            }
        }

        /**Threat State*/
        void Threat_Enter()
        {
            Debug.Log("Mollu) Threat_Enter");

            if(!IsMovable) return;
            nav.isStopped = true;
            nav.enabled = false;
            transform.DOKill();
            recognize.CurrentViewRadius *= 2;
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
            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
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
                transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
                nav.SetDestination(_objectiveEntity.transform.position);
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
            if (IsWandering)
            {
                transform.DOLookAt(wanderPosition, TurnSpeed);
                nav.SetDestination(wanderPosition);
            }
        }

        void Wander_Exit()
        {
            IsWandering = false;
        }

        /**Attack State*/
        // 움직임은 멈추되, 시선은 따라가도록 처리
        void Attack_Enter()
        {
            Debug.Log("Mollu) Attack_Enter");

            if(!IsMovable) return;
            nav.isStopped = true;
            nav.enabled = false;
            transform.DOKill();

            DoAttack();
        }
        void Attack_Update()
        {
            if (GetModelManger().GetAnimator().GetBool("IsAttackEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Attack_FixedUpdate()
        {
            float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);
            if (dist <= AttackRange) {
                transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed * 2);
            } else {
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
            if (Life.IsDie) { isDamaged = false; }
            else
            {
                if (isDamaged = Life.Damaged(damage)) {
                    GameManager.Instance.NewFeatureGlobalEvent.OnEnemyHitEvent.Invoke();
                }
            }
            if (Life.IsDie) { fsm.ChangeState(States.Death); }
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
            if (IsMovable) {
                nav.enabled = true;

                nav.isStopped = false;
            }
            else
            {

                if(!IsMovable)return;
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

        public UniTask Turning()
        {
            //Currently using DoTween.DoLookAt
            throw new System.NotImplementedException();
        }

        #endregion
        [SerializeField] protected List<FMODAudioSource> _audioSources;
    }
}