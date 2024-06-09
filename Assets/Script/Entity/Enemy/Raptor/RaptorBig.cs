using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sophia.Composite;
using NUnit.Framework;
using UnityEngine.AI;
using System.Linq;
using Sophia.Instantiates;

namespace Sophia.Entitys
{
    public class RaptorBig : Raptor
    {
        #region Serial Member
        [SerializeField] GameObject RaptorSmall;

        [Header("Raptor Settings")]
        [SerializeField] private float EscapeRange;
        [SerializeField] private int spawnRaptorAmount = 3;
        [SerializeField] private SerialAffectorData serialAffectorData;
        #endregion

        #region Member
        CoolTimeComposite howlingTimer;
        List<RaptorSmall> raptorSmallList;
        private float howlingCoolTime = 5f;
        Vector3 EscapePosition;

        #endregion
        protected override void Awake()
        {
            base.Awake();
            raptorSmallList = new List<RaptorSmall>();
            EscapeRange = _fOVData.viewRadius / 2;
        }
        private bool isReadyHowling = true;
        public void SetReadyHowling() => isReadyHowling = true;
        public void SetUnreadyHowling() => isReadyHowling = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            howlingTimer.TickRunning();
        }

        public override void SetNavMeshData()
        {
            base.SetNavMeshData();
            _nav.stoppingDistance = AttackRange;
        }

        void DoHowl()
        {
            System.Random random = new System.Random();
            int currentRaptorCount = 0;
            GetModelManager().GetAnimator().SetTrigger("DoHowl");
            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Howling].Play();

            foreach (RaptorSmall raptor in raptorSmallList)
            {
                if (raptor != null)
                {
                    currentRaptorCount++;
                }
            }

            if (currentRaptorCount == 0)
                InstantiateRaptorSmall(random.Next(spawnRaptorAmount - 1, spawnRaptorAmount + 2));
            else
            {
                DoBuff();
            }
        }

        void InstantiateRaptorSmall(int amount)
        {
            float range = AttackRange;
            for (int i = 0; i < amount; i++)
            {
                GameObject instance;
                Vector3 spawnPosition = transform.position;
                Vector3 randomVector = Random.insideUnitSphere * range;
                NavMeshHit hit;

                randomVector += transform.position;

                if (NavMesh.SamplePosition(randomVector, out hit, range, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position;
                }
                instance = Instantiate(RaptorSmall, spawnPosition, Quaternion.identity);
                CurrentInstantiatedStage.mobGenerator.AddMob(instance);
                raptorSmallList.Add(instance.GetComponent<RaptorSmall>());
                raptorSmallList.Last().CurrentInstantiatedStage = CurrentInstantiatedStage;
            }
        }

        void DoBuff()
        {
            foreach (RaptorSmall raptorSmall in raptorSmallList)
            {
                if (raptorSmall != null)
                    raptorSmall.GetBuff(new Sophia.DataSystem.Modifiers.ConcreteAffector.PowerUpAffect(in serialAffectorData));
            }
        }
        void DoEscape()
        {
            Vector3 escapeVector = 3 * (transform.position - _objectiveEntity.transform.position);
            NavMeshHit hit;

            escapeVector += transform.position;

            if (NavMesh.SamplePosition(escapeVector, out hit, EscapeRange, NavMesh.AllAreas))
            {
                EscapePosition = hit.position;
            }
            else
            {
                EscapePosition = transform.position;
            }
        }

        #region FSM Functions

        /** Init State */
        void Init_Enter()
        {
            Debug.Log("Init_Enter");

            //Init Settings
            originViewRadius = Recognize.CurrentViewRadius;
            SetNavMeshData();
            InitAnimParamList();
            howlingTimer = new CoolTimeComposite(howlingCoolTime, 1)
                    .AddBindingAction(SetUnreadyHowling)
                    .AddOnFinishedEvent(SetReadyHowling);

            fsm.ChangeState(States.Idle);
        }

        /** Idle State */
        void Idle_Enter()
        {
            Debug.Log("Idle_Enter");
            Recognize.CurrentViewRadius = originViewRadius;
            _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Idle].Play();
            _nav.SetDestination(transform.position);
            _nav.isStopped = true;
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

                    // if (dist <= AttackRange)
                    //     fsm.ChangeState(States.Attack);
                    // else
                    if (dist <= EscapeRange)
                        fsm.ChangeState(States.Escape);
                    else
                    {
                        if (isReadyHowling)
                            fsm.ChangeState(States.Howl);
                    }
                }
            }
        }

        void Idle_FixedUpdate()
        {
            if (Recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog)
            {
                _audioSources[(int)E_RAPTOR_AUDIO_INDEX.Found].Play();
            }
            if (Recognize.GetCurrentRecogState() == E_RECOG_TYPE.FirstRecog || Recognize.GetCurrentRecogState() == E_RECOG_TYPE.ReRecog)
            {
                if (isMovable)
                {
                    transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
                }
            }
        }

        /**Escape State*/
        void Escape_Enter()
        {
            Debug.Log("Escape_Enter");

            this.GetModelManager().GetAnimator().SetBool("IsEscape", true);
            SetMoveState(true);
        }
        void Escape_Update()
        {
            DoEscape();

            float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);

            // if (dist <= AttackRange)
            //     fsm.ChangeState(States.Attack);
            // else
            if (isReadyHowling)
                fsm.ChangeState(States.Howl);
            else if (dist > EscapeRange)
                fsm.ChangeState(States.Idle);
        }

        void Escape_FixedUpdate()
        {
            if (isMovable)
            {
                _nav.SetDestination(EscapePosition);
                transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
            }
        }

        void Escape_Exit()
        {
            _nav.speed = MoveSpeed.GetValueForce();
            this.GetModelManager().GetAnimator().SetBool("IsEscape", false);
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
            if (IsWandering && isMovable)
            {
                this.GetModelManager().GetAnimator().SetBool("IsWalk", true);
                transform.DOLookAt(wanderPosition, TurnSpeed);
                _nav.SetDestination(wanderPosition);
            }
        }

        void Wander_Exit()
        {
            IsWandering = false;
            this.GetModelManager().GetAnimator().SetBool("IsWalk", false);
        }

        /**Attack State*/
        void Attack_Enter()
        {
            Debug.Log("Attack_Enter");

            _nav.SetDestination(transform.position);
            _nav.isStopped = true;
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

        /**Howl State*/
        void Howl_Enter()
        {
            Debug.Log("Howl_Enter");

            _nav.SetDestination(transform.position);
            _nav.isStopped = true;
            transform.DOLookAt(_objectiveEntity.transform.position, TurnSpeed);
            DoHowl();
        }

        void Howl_Update()
        {
            if (GetModelManager().GetAnimator().GetBool("IsHowlEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Howl_Exit()
        {
            GetModelManager().GetAnimator().SetBool("IsHowlEnd", false);

            if (!howlingTimer.GetIsReadyToUse())
                return;
            howlingTimer.ActionStart();
        }

        /**Death State*/
        void Death_Enter()
        {
            Debug.Log("Death_Enter");

            List<Sophia.Instantiates.ItemObject> itemObjects;
            itemObjects = GetComponent<Sophia.Instantiates.GachaComponent>().InstantiateReward();

            foreach (ItemObject itemObject in itemObjects)
            {
                if(itemObject == null) continue;
                itemObject.SetTriggerTime(1f).SetTweenSequence(SetSequnce(itemObject)).Activate();
            }
            Die();
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
        private void OnDestroy()
        {
            howlingTimer.RemoveOnFinishedEvent(SetReadyHowling);
        }
    }

}
