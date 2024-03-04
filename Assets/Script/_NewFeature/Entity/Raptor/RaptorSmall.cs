using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Sophia.Composite;

namespace Sophia.Entitys
{
    public class RaptorSmall : Raptor
    {
        #region Serial Member
        [SerializeField] private float RushRange;
        [SerializeField] private float RushForce;
        [SerializeField] private float RushTime;

        #endregion
        CoolTimeComposite rushTimer;
        private bool IsRush;
        // Start is called before the first frame update

        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            rushTimer.TickRunning();
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void SetReadyRush() => IsRush = true;
        public void SetUnReadyRush() => IsRush = false;

        public override void SetNavMeshData()
        {
            base.SetNavMeshData();
            _nav.stoppingDistance = RushRange;
        }

        public void UseProjectile_DashAttack()
        {
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.JUMP]).Init(this);
            useProjectile.SetPositioningType(serialProjectileInstantiateData._positioningType)
                            .SetDurateTimeByRatio(serialProjectileInstantiateData._DurateTimeByRatio);

            _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.JUMP, useProjectile)
                                    .SetProjectilePower((int)GetStat(E_NUMERIC_STAT_TYPE.Power).GetValueForce())
                                    .Activate();
        }

        public void GetBuff(Sophia.DataSystem.Modifiers.Affector affector)
        {
            this._affectorManager.Affect(affector);
        }

        void DoRush()
        {
            GetModelManger().GetAnimator().SetTrigger("DoRush");
        }

        #region FSM Functions

        /**Init State*/
        void Init_Enter()
        {
            Debug.Log("Init_Enter");

            //Init Settings
            SetNavMeshData();
            InitAnimParamList();
            rushTimer = new CoolTimeComposite(RushTime, 1)
            .AddBindingAction(SetReadyRush)
            .AddOnFinishedEvent(SetUnReadyRush);

            fsm.ChangeState(States.Idle);
        }

        /**Idle State*/
        void Idle_Enter()
        {
            Debug.Log("Idle_Enter");
            
        }

        void Idle_Update()
        {
            if (Recognize.GetCurrentRecogState() == E_RECOG_TYPE.None || Recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose)
            {
                if (!IsWandering)
                    fsm.ChangeState(States.Wander);
            }
            else
            {
                if (Recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog || Recognize.GetCurrentRecogState() == E_RECOG_TYPE.ReRecog)
                {
                    float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);

                    if (dist <= AttackRange)
                        fsm.ChangeState(States.Attack);
                    else if (dist <= RushRange)
                        fsm.ChangeState(States.Tap);
                    else
                        fsm.ChangeState(States.Chase);
                }
            }
        }

        /** Wander State */
        void Wander_Enter()
        {
            System.Random random = new System.Random();
            Debug.Log("Wander_Enter");

            Invoke("DoWander", random.Next(0, wanderingCoolTime + 1));
            SetMoveState(true);
        }

        void Wander_Update()
        {
            if (Recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog || Recognize.GetCurrentRecogState() == E_RECOG_TYPE.ReRecog)
            {
                CancelInvoke();
                fsm.ChangeState(States.Idle);
            }
            else if (IsWandering && _nav.remainingDistance <= _nav.stoppingDistance)
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
                _nav.SetDestination(wanderPosition);
            }
        }

        void Wander_Exit()
        {
            IsWandering = false;
            this.GetModelManger().GetAnimator().SetBool("IsWalk", false);
        }

        /**Chase State*/
        void Chase_Enter()
        {
            Debug.Log("Chase_Enter");
            this.GetModelManger().GetAnimator().SetBool("IsWalk", true);
            SetMoveState(true);
        }

        void Chase_Update()
        {
            float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);

            if (Recognize.GetCurrentRecogState() == E_RECOG_TYPE.Lose)
                fsm.ChangeState(States.Idle);
            else if (dist <= AttackRange)
                fsm.ChangeState(States.Attack);
            else if (dist <= RushRange)
            {
                fsm.ChangeState(States.Tap);
            }
        }

        void Chase_FixedUpdate()
        {
            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
            _nav.SetDestination(_objectiveEntity.transform.position);
        }

        void Chase_Exit()
        {
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
            if (this.GetModelManger().GetAnimator().GetBool("IsAttackEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Attack_Exit()
        {
            GetModelManger().GetAnimator().SetBool("IsAttackEnd", false);
            ResetAnimParam();
        }

        /**Tap State*/
        void Tap_Enter()
        {
            Debug.Log("Tap_Enter");

            SetMoveState(false);

            DoRush();
        }

        void Tap_Update()
        {
            if (this.GetModelManger().GetAnimator().GetBool("IsTapEnd"))
            {
                fsm.ChangeState(States.Rush);
            }
        }

        void Tap_FixedUpdate()
        {
            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed / 2);
        }

        void Tap_Exit()
        {
            GetModelManger().GetAnimator().SetBool("IsTapEnd", false);
            ResetAnimParam();
        }

        /**Rush State*/
        void Rush_Enter()
        {
            Debug.Log("Rush_Enter");

            rushTimer.ActionStart();
            UseProjectile_DashAttack();
            entityRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            SetMoveState(true);
        }

        void Rush_Update()
        {
            if (!IsRush)
            {
                GetModelManger().GetAnimator().SetTrigger("DoRushQuit");
            }
            if (GetModelManger().GetAnimator().GetBool("IsRushEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Rush_FixedUpdate()
        {
            if (IsRush)
            {
                entityRigidbody.AddForce(transform.forward * RushForce, ForceMode.Acceleration);
            }
            else
            {
                //서서히 감소시키는 코드 넣기
                entityRigidbody.velocity = Vector3.one;
                entityRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        void Rush_Exit()
        {
            GetModelManger().GetAnimator().SetBool("IsRushEnd", false);
            _nav.enabled = false;
            entityRigidbody.velocity = Vector3.zero;
            entityRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            ResetAnimParam();
        }

        /**Death State*/
        void Death_Enter()
        {
            Debug.Log("Death_Enter");
            Die();
        }

        #endregion
    }
}
