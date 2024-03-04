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

namespace Sophia.Entitys
{
    public class Robuwa : Enemy, IMovable
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
        private bool IsWandering = false;
        [SerializeField]
        private bool IsMovable = false;
        private float originViewRadius;
        private NavMeshAgent nav;

        #endregion

        #region Serialize Member
        [SerializeField] protected RecognizeEntityComposite recognize;
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

            power = new Stat(_baseEntityData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural, () => { Debug.Log("공격력 수치 변경"); });
            moveSpeed = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural, () => { Debug.Log("이동속도 수치 변경"); });

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

            Life.OnEnterDie += OnRobuwaEnterDie;
            Life.OnExitDie += OnRobuwaExitDie;

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
        public void OnRobuwaEnterDie()
        {
            CurrentInstantiatedStage.mobGenerator.RemoveMob(this.gameObject);
        }

        public void OnRobuwaExitDie()
        {
            Destroy(gameObject, 0.5f);
        }
        void SetNavMeshData()
        {
            nav.speed = moveSpeed.GetValueForce();
            nav.acceleration = nav.speed * 1.5f;
            nav.updateRotation = false;
            nav.stoppingDistance = AttackRange;
        }

        public override bool Die() { Life.Died(); return true; }

        void InitAnimParamList()
        {
            for (int i = 0; i < this.GetModelManger().GetAnimator().parameterCount; i++)
            {
                AnimatorControllerParameter acp = this.GetModelManger().GetAnimator().GetParameter(i);
                switch (this.GetModelManger().GetAnimator().GetParameter(i).type)
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
                this.GetModelManger().GetAnimator().SetBool(b, false);
            foreach (string t in animTriggerParamList)
                this.GetModelManger().GetAnimator().ResetTrigger(t);
        }

        void DoAttack()
        {
            switch (this.GetModelManger().GetAnimator().GetInteger("attackCount") % 3)
            {
                case 0:
                    this.GetModelManger().GetAnimator().SetTrigger("DoAttackLeft");
                    break;
                case 1:
                    this.GetModelManger().GetAnimator().SetTrigger("DoAttackRight");
                    break;
                case 2:
                    this.GetModelManger().GetAnimator().SetTrigger("DoAttackJump");
                    break;
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
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.ATTACK]).Init(this);

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
            Debug.Log("Init_Enter");

            //Init Settings
            originViewRadius = recognize.CurrentViewRadius;
            SetNavMeshData();

            fsm.ChangeState(States.Idle);
        }

        void Idle_Enter()
        {
            Debug.Log("Idle_Enter");
            recognize.CurrentViewRadius = originViewRadius;
            SetMoveState(false);
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
                    fsm.ChangeState(States.Threat);
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
        void Threat_Enter()
        {
            Debug.Log("Threat Enter");

            SetMoveState(false);
            recognize.CurrentViewRadius *= 2;
            this.GetModelManger().GetAnimator().SetTrigger("DoThreat");
        }

        void Threat_Update()
        {
            if (this.GetModelManger().GetAnimator().GetBool("IsThreatEnd"))
            {
                if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose)
                {
                    fsm.ChangeState(States.Idle);
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

        void Threat_FixedUpdate()
        {
            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
        }

        void Threat_Exit()
        {
            this.GetModelManger().GetAnimator().SetBool("IsThreatEnd", false);
        }

        /**Move State*/
        void Move_Enter()
        {
            Debug.Log("Move_Enter");
            this.GetModelManger().GetAnimator().SetBool("IsWalk", true);
            SetMoveState(true);
        }

        void Move_Update()
        {
            float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);

            if (recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose)
                fsm.ChangeState(States.Idle);
            else if (dist <= AttackRange)
            {
                fsm.ChangeState(States.Attack);
            }

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
            this.GetModelManger().GetAnimator().SetBool("IsWalk", false);
        }

        /**Wander State*/
        void Wander_Enter()
        {
            System.Random random = new System.Random();
            Debug.Log("Wander_Enter");

            Invoke("DoWander", random.Next(0, 4));
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
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Wander_FixedUpdate()
        {
            if (IsWandering)
            {
                this.GetModelManger().GetAnimator().SetBool("IsWalk", true);
                transform.DOLookAt(wanderPosition, TurnSpeed);
                nav.SetDestination(wanderPosition);
            }
        }

        void Wander_Exit()
        {
            IsWandering = false;
            this.GetModelManger().GetAnimator().SetBool("IsWalk", false);
        }

        /**Attack State*/
        void Attack_Enter()
        {
            Debug.Log("Attack_Enter");

            SetMoveState(false);

            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed / 2);
            DoAttack();
        }

        void Attack_Update()
        {
            if (this.GetModelManger().GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Attack_Exit()
        {
            ResetAnimParam();
        }

        /**Death State*/
        void Death_Enter()
        {
            Debug.Log("Death_Enter");
            Die();
        }
        #endregion

        #region Inherited Functions From Enemy Class
        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(power);
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
                if (isDamaged = Life.Damaged(damage)) { GameManager.Instance.GlobalEvent.OnEnemyHitEvent.ForEach(Event => Event.Invoke()); }
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
            if (movableState)
                nav.enabled = true;
            nav.isStopped = !movableState;
            if (!movableState)
            {
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
    }
}