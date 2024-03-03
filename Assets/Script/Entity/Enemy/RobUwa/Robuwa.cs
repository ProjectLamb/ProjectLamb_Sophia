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
        public Stat MoveSpeed;
        public int AttackRange;
        public int TurnSpeed;   //Stat으로 관리 가능성
        public float WanderingCoolTime;
        #endregion

        #region Private
        private LifeComposite Life { get; set; }
        private RecognizeEntityComposite Recog { get; set; }
        private Vector3 wanderPosition;
        List<string> animBoolParamList;
        List<string> animTriggerParamList;
        private bool IsFirstRecog = true;
        private bool IsWandering = false;
        private bool IsMovable = false;
        private float originViewRadius;
        private Animator animator;
        private NavMeshAgent nav;

        #endregion
        // Start is called before the first frame update

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

            animBoolParamList = new List<string>();
            animTriggerParamList = new List<string>();

            fsm = new StateMachine<States>(this);
            fsm.ChangeState(States.Init);
        }

        protected override void Start()
        {
            base.Start();
            animator = GetModelManger().GetAnimator();
            SetNavMeshData();

            Life.OnEnterDie += OnRobuwaEnterDie;
            Life.OnExitDie += OnRobuwaExitDie;

            InitAnimParamList();
            originViewRadius = GetComponent<FieldOfView>().viewRadius;
            IsFirstRecog = true;
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
            nav.speed = _baseEntityData.MoveSpeed;
            nav.acceleration = nav.speed * 1.5f;
            nav.updateRotation = false;
        }

        public override bool Die() { Life.Died(); return true; }

        void InitAnimParamList()
        {
            for (int i = 0; i < animator.parameterCount; i++)
            {
                AnimatorControllerParameter acp = animator.GetParameter(i);
                switch (animator.GetParameter(i).type)
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
                animator.SetBool(b, false);
            foreach (string t in animTriggerParamList)
                animator.ResetTrigger(t);
        }

        void DoAttack()
        {
            switch (animator.GetInteger("attackCount") % 3)
            {
                case 0:
                    animator.SetTrigger("DoAttackLeft");
                    break;
                case 1:
                    animator.SetTrigger("DoAttackRight");
                    break;
                case 2:
                    animator.SetTrigger("DoAttackJump");
                    break;
            }
        }

        void DoWander()
        {
            EQS();

            float range = GetComponent<FieldOfView>().viewRadius * 2;
            float minDistance = GetComponent<FieldOfView>().viewRadius;
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

        // void SetCarrierBucketPosition()
        // {
        //     carrierBucket.transform.position = transform.position + new Vector3(0, carrierBucket.GetComponent<CarrierBucket>().BucketScale / 2, AttackRange);
        // }

        // public void Use(Player player) {
        //     ProjectileObject useProjectile = ProjectilePool.GetObject(ProjectileObject).Init(player);
        //     mInstantiatorRef.InstantablePositioning(useProjectile)
        //                     .SetProjectilePower(player.GetStat(E_NUMERIC_STAT_TYPE.Power))
        //                     .Activate();
        // }

        // public void UseProjectile_NormalAttack()
        // {
        //     Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject().Init(this);

        //     _projectileBucketManager.InstantablePositioning(0, useProjectile)
        //                             .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power))
        //                             .Activate();
        //     //this.carrierBucket.CarrierInstantiatorByObjects(this, projectiles[0], new object[] { _baseEntityData.Power * 1 });
        // }

        #region FSM Functions
        ////////////////////////////////////////FSM Functions////////////////////////////////////////
        /** Init State */
        void Init_Enter()
        {
            Debug.Log("Init_Enter");

            //Init Settings
            //SetCarrierBucketPosition();

            fsm.ChangeState(States.Idle);
        }

        void Idle_Enter()
        {
            Debug.Log("Idle_Enter");
            GetComponent<FieldOfView>().viewRadius = originViewRadius;
            SetMoveState(false);
        }

        void Idle_Update()
        {
            if (!GetComponent<FieldOfView>().IsRecog)
            {
                //Play Idle
                if (!IsWandering)
                    fsm.ChangeState(States.Wander);
            }
            else
            {
                if (IsFirstRecog)
                    fsm.ChangeState(States.Threat);
                else
                {
                    float dist = Vector3.Distance(transform.position, objectiveTarget.position);
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
            GetComponent<FieldOfView>().viewRadius *= 2;
            objectiveTarget = GameManager.Instance.PlayerGameObject.transform;
            animator.SetTrigger("DoThreat");
        }

        void Threat_Update()
        {
            //animator bool 바꾸기
            if (animator.GetBool("IsThreatEnd"))
            {
                IsFirstRecog = false;
                if (!GetComponent<FieldOfView>().IsRecog)
                {
                    fsm.ChangeState(States.Idle);
                }
                else
                {
                    float dist = Vector3.Distance(transform.position, objectiveTarget.position);

                    if (dist <= AttackRange)
                        fsm.ChangeState(States.Attack);
                    else
                        fsm.ChangeState(States.Move);
                }
            }
        }

        void Threat_FixedUpdate()
        {
            transform.DOLookAt(objectiveTarget.position, TurnSpeed);
        }

        void Threat_Exit()
        {
            animator.SetBool("IsThreatEnd", false);
        }

        /**Move State*/
        void Move_Enter()
        {
            Debug.Log("Move_Enter");
            animator.SetBool("IsWalk", true);
            SetMoveState(true);
        }

        void Move_Update()
        {
            float dist = Vector3.Distance(transform.position, objectiveTarget.position);

            if (!GetComponent<FieldOfView>().IsRecog)
                fsm.ChangeState(States.Idle);
            else if (dist <= AttackRange)
                fsm.ChangeState(States.Attack);

        }

        void Move_FixedUpdate()
        {
            if (GetComponent<FieldOfView>().IsRecog)
            {
                transform.DOLookAt(objectiveTarget.position, TurnSpeed);
                nav.SetDestination(objectiveTarget.position);
            }
        }

        void Move_Exit()
        {
            animator.SetBool("IsWalk", false);
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
            if (GetComponent<FieldOfView>().IsRecog)
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
                animator.SetBool("IsWalk", true);
                transform.DOLookAt(wanderPosition, TurnSpeed);
                nav.SetDestination(wanderPosition);
            }
        }

        void Wander_Exit()
        {
            IsWandering = false;
            animator.SetBool("IsWalk", false);
        }

        /**Attack State*/
        void Attack_Enter()
        {
            Debug.Log("Attack_Enter");

            SetMoveState(false);

            transform.DOLookAt(objectiveTarget.position, TurnSpeed / 2);
            DoAttack();
        }

        void Attack_Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
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
            StatReferer.SetRefStat(MoveSpeed);
            this.Settables.ForEach(E =>
            {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }

        public override EntityStatReferer GetStatReferer()
        {
            throw new System.NotImplementedException();
        }

        protected override void CollectSettable()
        {
            Settables.Add(Life);
            Settables.Add(_affectorManager);
        }

        public override LifeComposite GetLifeComposite() => this.Life;
        public override RecognizeEntityComposite GetRecognizeComposite() => this.Recog;

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

        #region IMovable
        public bool GetMoveState() => IsMovable;

        public void SetMoveState(bool movableState)
        {
            IsMovable = movableState;
            nav.enabled = IsMovable;
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