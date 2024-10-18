using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using Sophia.Composite;
using HUDIndicator;

namespace Sophia.Entitys
{
    public class RaptorSmall : Raptor
    {
        #region public
        public IndicatorOffScreen indicatorOffScreen;
        public Image attackIndicatorImage;

        #endregion

        #region Serial Member
        [SerializeField] private float RushRange;
        [SerializeField] private float RushTime;

        #endregion

        float originDrag;

        #region Rush Member
        CoolTimeComposite rushTimer;
        float rushDistance;
        float currentRushTime;
        Vector3 rushDestination;
        Ray rushRay;
        RaycastHit hit;
        NavMeshHit navHit;
        private bool IsRush;
        Sophia.Instantiates.ProjectileObject rushProjectileObject;
        #endregion
        // Start is called before the first frame update

        protected override void Awake()
        {
            base.Awake();
            originDrag = entityRigidbody.drag;
        }
        protected override void Start()
        {
            base.Start();
            CurrentInstantiatedStage.mobGenerator.AddMob(this.gameObject);
            transform.parent = CurrentInstantiatedStage.transform.GetChild((int)Stage.STAGE_CHILD.MOB);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            rushTimer.TickRunning();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void SetReadyRush()
        {
            IsRush = true;
            gameObject.layer = LayerMask.NameToLayer("DashEnemy");
        }
        public void SetUnReadyRush()
        {
            IsRush = false;
            gameObject.layer = LayerMask.NameToLayer("Entity");
        }

        public override void SetNavMeshData()
        {
            base.SetNavMeshData();
            nav.stoppingDistance = RushRange;
        }

        public void UseProjectile_DashAttack()
        {
            rushProjectileObject = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.JUMP]).Init(this);
            rushProjectileObject.SetPositioningType(serialProjectileInstantiateData._positioningType);

            _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.JUMP, rushProjectileObject)
                                    .SetProjectilePower((int)GetStat(E_NUMERIC_STAT_TYPE.Power).GetValueForce())
                                    .Activate();
        }

        public void GetBuff(Sophia.DataSystem.Modifiers.Affector affector)
        {
            this._affectorManager.Affect(affector);
        }

        void DoRush()
        {
            GetModelManager().GetAnimator().SetTrigger("DoRush");
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
            rushDistance = RushRange * 1.5f;

            Sophia.Instantiates.VisualFXObject visualFX = VisualFXObjectPool.GetObject(_spawnParticleRef).Init();
            GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();

            fsm.ChangeState(States.Idle);
        }

        /**Idle State*/
        void Idle_Enter()
        {
            Debug.Log("Idle_Enter");
            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Idle].Play();
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

                    // if (dist <= AttackRange) //근접 공격
                    //     fsm.ChangeState(States.Attack);
                    // else 
                    if (dist <= RushRange)
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
            else if (IsWandering && nav.remainingDistance <= nav.stoppingDistance)
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Wander_FixedUpdate()
        {
            if (IsWandering && isMovable)
            {
                this.GetModelManager().GetAnimator().SetBool("IsWalk", true);
                transform.DOLookAt(wanderPosition, TurnSpeed);
                nav.SetDestination(wanderPosition);
            }
        }

        void Wander_Exit()
        {
            IsWandering = false;
            this.GetModelManager().GetAnimator().SetBool("IsWalk", false);
        }

        /**Chase State*/
        void Chase_Enter()
        {
            Debug.Log("Chase_Enter");
            this.GetModelManager().GetAnimator().SetBool("IsWalk", true);
            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Found].Play();
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
            if (isMovable)
            {
                transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
                nav.SetDestination(_objectiveEntity.transform.position);
            }
        }

        void Chase_Exit()
        {
            this.GetModelManager().GetAnimator().SetBool("IsWalk", false);
        }

        /**Attack State*/
        void Attack_Enter()
        {
            Debug.Log("Attack_Enter");

            nav.SetDestination(transform.position);
            nav.isStopped = true;

            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed / 2);
            DoAttack();
        }

        void Attack_Update()
        {
            if (this.GetModelManager().GetAnimator().GetBool("IsAttackEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Attack_Exit()
        {
            GetModelManager().GetAnimator().SetBool("IsAttackEnd", false);
            ResetAnimParam();
        }

        /**Tap State*/
        void Tap_Enter()
        {
            Debug.Log("Tap_Enter");
            StartCoroutine(FadeIn());
            GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Attack].PlayFunctionalActOneShotWithDuration(2.8f);   //Animation Clip Length + 0.6f
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            currentRushTime = RushTime;

            DoRush();
        }

        void Tap_Update()
        {
            rushRay = new Ray(transform.position, transform.forward);

            if (this.GetModelManager().GetAnimator().GetBool("IsTapEnd"))
            {
                fsm.ChangeState(States.Rush);
            }
        }

        void Tap_FixedUpdate()
        {
            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed / 2);
            indicatorOffScreen.style.color = Color.red;
            indicatorOffScreen.arrowStyle.color = Color.red;
            if (NavMesh.SamplePosition(rushRay.GetPoint(rushDistance), out navHit, rushDistance, NavMesh.AllAreas))
            {
                rushDestination = navHit.position;
                currentRushTime = RushTime * (Vector3.Distance(rushDestination, transform.position) / rushDistance);
            }
        }

        void Tap_Exit()
        {
            GetModelManager().GetAnimator().SetBool("IsTapEnd", false);
            ResetAnimParam();
        }

        /**Rush State*/
        void Rush_Enter()
        {
            Debug.Log("Rush_Enter");
            attackIndicatorImage.gameObject.SetActive(false);
            transform.DOMove(rushDestination, currentRushTime).SetEase(Ease.OutQuad);
            rushTimer.ActionStart();
            UseProjectile_DashAttack();
        }

        void Rush_Update()
        {
            if (!IsRush)
            {
                indicatorOffScreen.style.color = Color.yellow;
                indicatorOffScreen.arrowStyle.color = Color.yellow;
                GetModelManager().GetAnimator().SetTrigger("DoRushQuit");
                Destroy(rushProjectileObject);
            }
            if (GetModelManager().GetAnimator().GetBool("IsRushEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Rush_Exit()
        {
            GetModelManager().GetAnimator().SetBool("IsRushEnd", false);
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            entityRigidbody.drag = originDrag;
            entityRigidbody.velocity = Vector3.zero;
            ResetAnimParam();
        }

        /**Death State*/
        void Death_Enter()
        {
            Debug.Log("Death_Enter");
            Die();
        }

        #endregion

        #region FadeEffect
        IEnumerator FadeIn()
        {
            attackIndicatorImage.gameObject.SetActive(true);
            Color fadeColor = attackIndicatorImage.color;
            fadeColor.a = 0;

            while (fadeColor.a < 1f)
            {
                fadeColor.a += 0.05f;
                attackIndicatorImage.color = fadeColor;
                yield return new WaitForSecondsRealtime(0.05f);
            }

        }

        #endregion
    }
}
