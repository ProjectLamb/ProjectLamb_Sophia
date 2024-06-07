using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using DG.Tweening;
using Sophia.DataSystem;
using Sophia.Composite;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;
using Sophia.UserInterface;
using FMODPlus;
using System.Runtime.Remoting.Contexts;

namespace Sophia.Entitys
{
    public enum E_ELDERONE_AUDIO_INDEX
    {
        AttackBoth, AttackOne, Swing, Death, FootStep, PhaseTransition, Dirt
    }
    public class ElderOne : Boss, IMovable
    {
        List<string> animBoolParamList;
        List<string> animTriggerParamList;

        [Header("Stats")]
        public int attackRange = 30;
        public int attackCount = 3;
        public float attackInterval = 0.75f;

        private int turnSpeed = 2;
        private bool isPhaseChanged = false;
        private bool dontTurn = false;
        private float currentAttackTimer;

        [SerializeField] private bool IsInvincible;
        [SerializeField] FMODAudioSource[] _audioSource;
        [SerializeField] private Material emissionMaterial;
        private NavMeshAgent nav;
        #region Move
        private bool isMovable;
        #endregion

        #region Rush
        private bool isRushOnce = false;
        private int rushRange = 75;
        private int rushDistance = 200;
        private int rushStopDistance = 15;
        private float rushTime = 2.5f;
        private float currentRushTime;
        Vector3 rushDestination;
        Ray rushRay;
        NavMeshHit navHit;
        LayerMask RushStopMask;
        #endregion

        #region Skill
        private bool isWalkReady = false;
        private bool isWalkReturn = false;
        private bool isSkillOnce = false;
        #endregion

        #region VFX
        [SerializeField] GameObject barrierVFX;
        #endregion

        // Start is called before the first frame update
        public enum States
        {
            Init,
            Idle,
            Move,
            Attack,
            SkillWalk,
            SkillPhase,
            Rush,
            Death,
        }
        StateMachine<States> fsm;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            recognize = new RecognizeEntityComposite(this.gameObject, this._fOVData);

            power = new Stat(_baseEntityData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural, () => { Debug.Log("공격력 수치 변경"); });
            moveSpeed = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural, () => { Debug.Log("이동속도 수치 변경"); });

            _affectorManager.Init(_baseEntityData.Tenacity);

            animBoolParamList = new List<string>();
            animTriggerParamList = new List<string>();

            TryGetComponent<NavMeshAgent>(out nav);
            _objectiveEntity = GameManager.Instance.PlayerGameObject.GetComponent<Entitys.Entity>();

            RushStopMask = LayerMask.GetMask("Wall");

            fsm = new StateMachine<States>(this);
            fsm.ChangeState(States.Init);
        }

        protected override void Start()
        {
            base.Start();

            Life.OnEnterDie += OnElderOneEnterDie;
            Life.OnExitDie += OnElderOneExitDie;

            InitAnimParamList();
        }

        void Update()
        {
            if (!isPhaseChanged)
                CheckPhase();

            fsm.Driver.Update.Invoke();
        }

        void FixedUpdate()
        {
            fsm.Driver.FixedUpdate.Invoke();
        }
        [ContextMenu("Die")]
        public void ForceDie()
        {
            fsm.ChangeState(States.Death);
        }

        void InitAnimParamList()
        {
            for (int i = 0; i < this.GetModelManager().GetAnimator().parameterCount; i++)
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

        void ResetAnimParam()
        {
            foreach (string b in animBoolParamList)
                this.GetModelManager().GetAnimator().SetBool(b, false);
            foreach (string t in animTriggerParamList)
                this.GetModelManager().GetAnimator().ResetTrigger(t);
        }

        void SetNavMeshData()
        {
            nav.speed = moveSpeed.GetValueForce();
            nav.acceleration = nav.speed * 1.5f;
            nav.updateRotation = false;
            nav.stoppingDistance = attackRange;
        }

        void CheckPhase()
        {
            if (Life.CurrentHealth <= Life.MaxHp / 2)
            {
                phase = 2;
                //GameManager.Instance.DonDestroyObjectReferer.DontDestroyGameManager.AudioManager.audioStateSender._bossPhaseSender[1].SendCommand();
                nav.speed = _baseEntityData.MoveSpeed * 1.5f;
                this.GetModelManager().GetAnimator().SetFloat("MoveSpeed", 1.5f);
                attackInterval /= 2;
                rushTime /= 2;
                emissionMaterial.SetColor("_EmissionColor", Color.red * 12f);
                isPhaseChanged = true;
            }
        }

        void PlayRandomIdleAnimation(int idleAmount)
        {
            int random = Random.Range(0, idleAmount);
            switch (random)
            {
                case 0:
                    this.GetModelManager().GetAnimator().ResetTrigger(animTriggerParamList[0]);
                    this.GetModelManager().GetAnimator().ResetTrigger(animTriggerParamList[1]);
                    break;
                case 1:
                    this.GetModelManager().GetAnimator().ResetTrigger(animTriggerParamList[1]);
                    this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[0]);
                    break;
                case 2:
                    this.GetModelManager().GetAnimator().ResetTrigger(animTriggerParamList[0]);
                    this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[1]);
                    break;
                default:
                    ResetAnimParam();
                    break;
            }
        }

        void DoAttack(int phase)
        {
            for (int i = 2; i <= 5; i++)
                this.GetModelManager().GetAnimator().ResetTrigger(animTriggerParamList[i]);

            if (phase == 1)
            {
                int random = Random.Range(0, 3);
                switch (random)
                {
                    case 0:
                        this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[2]);
                        break;
                    case 1:
                        this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[3]);
                        break;
                    case 2:
                        this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[4]);
                        break;
                }
            }
            else if (phase == 2)
            {
                this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[5]);
            }
            else
            {
                Debug.Log("Normal Attack Error");
            }
        }

        void DoSkill(int phase)
        {
            for (int i = 2; i < animTriggerParamList.Count; i++)
                this.GetModelManager().GetAnimator().ResetTrigger(animTriggerParamList[i]);

            if (phase == 1)
            {
                this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[6]);
            }
            else if (phase == 2)
            {
                if (this.GetModelManager().GetAnimator().GetInteger("phaseSkill") % 2 == 0)
                {
                    fsm.ChangeState(States.SkillWalk);
                }
                else    //2Phase
                {
                    fsm.ChangeState(States.SkillPhase);
                    attackRange = 30;
                    dontTurn = true;
                }
            }
        }

        void OnElderOneEnterDie()
        {
            Sophia.Instantiates.VisualFXObject visualFX = VisualFXObjectPool.GetObject(_dieParticleRef).Init();
            GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.Death].Play();
            GameManager.Instance.DonDestroyObjectReferer.DontDestroyGameManager.AudioManager.audioStateSender._bossPhaseSender[2].SendCommand();
            CurrentInstantiatedStage.mobGenerator.RemoveMob(this.gameObject);
        }
        public override bool Die()
        {
            Life.Died();
            return true;
        }

        public void DisableModel()
        {
            _modelManager.gameObject.SetActive(false);
            entityCollider.enabled = false;
        }

        void OnElderOneExitDie()
        {
            Invoke("DisableModel", 0.5f);
        }

        public void PlayFootStepSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.FootStep].Play();
        }
        public void PlaySwingSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.Swing].Play();
        }
        public void PlayAttackOneSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.AttackOne].Play();
        }
        public void PlayAttackBothSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.AttackBoth].Play();
        }
        public void PlayDirtSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.Dirt].Play();
        }

        #region Attack
        private Stat power;
        [SerializeField] protected Instantiates.ProjectileObject[] _attckProjectileDirection;

        public void UseProjectile_NormalAttack()
        {
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.ATTACK]).Init(this);

            _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.ATTACK, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power))
                                    .Activate();
        }
        public void UseProjectile_LeftAttack()
        {
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectileDirection[0]).Init(this);

            _projectileBucketManager.InstantablePositioning(4, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power))
                                    .Activate();
        }
        public void UseProjectile_RightAttack()
        {
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectileDirection[1]).Init(this);

            _projectileBucketManager.InstantablePositioning(4, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power))
                                    .Activate();
        }
        public void UseProjectile_JumpAttack()
        {
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.JUMP]).Init(this);

            _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.JUMP, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power) * 2)
                                    .Activate();
        }

        public void UseProjectile_WaveAttack()
        {
            StartCoroutine(CoWaveAttack());
        }

        IEnumerator CoWaveAttack()
        {
            for (int i = 0; i < 5; i++)
            {
                Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[3]).Init(this);

                _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.JUMP, useProjectile)
                                        .SetProjectilePower((int)(GetStat(E_NUMERIC_STAT_TYPE.Power) * (4 - i)))
                                        .SetScaleMultiplyByRatio(i + 1)
                                        .Activate();
                yield return new WaitForSeconds(0.25f);
            }
        }

        #endregion

        ////////////////////////////////////////FSM Functions////////////////////////////////////////
        /** Init State */
        void Init_Enter()
        {
            Debug.Log("Init_Enter");
            //Init Settings
            InitAnimParamList();
            SetNavMeshData();
            emissionMaterial.SetColor("_EmissionColor", Color.black * 0f);

            fsm.ChangeState(States.Idle);
        }

        /** Idle State */
        void Idle_Enter()
        {
            Debug.Log("Idle_Enter");
            ResetAnimParam();
            nav.SetDestination(transform.position);
            nav.isStopped = true;
        }

        void Idle_Update()
        {
            if (this.recognize.GetCurrentRecogState() == E_RECOG_TYPE.None)
            {
                PlayRandomIdleAnimation(3);
            }
            //If HasTarget
            else
            {
                float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);
                if (dist <= attackRange)
                    fsm.ChangeState(States.Attack);
                else if (dist >= rushRange)
                    fsm.ChangeState(States.Rush);
                else
                    fsm.ChangeState(States.Move);
            }
        }

        /** Move State */
        void Move_Enter()
        {
            Debug.Log("Move_Enter");
            this.GetModelManager().GetAnimator().SetBool("IsWalk", true);
            SetMoveState(true);
        }

        void Move_Update()
        {
            switch (recognize.GetCurrentRecogState())
            {
                case E_RECOG_TYPE.None:
                    {
                        fsm.ChangeState(States.Idle);
                        break;
                    }
                case E_RECOG_TYPE.FirstRecog:
                    {
                        float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);
                        if (dist <= attackRange)
                            fsm.ChangeState(States.Attack);
                        else if (dist >= rushRange)
                            fsm.ChangeState(States.Rush);
                        break;
                    }
                case E_RECOG_TYPE.Lose:
                    {
                        break;
                    }
                case E_RECOG_TYPE.ReRecog:
                    {
                        float dist = Vector3.Distance(transform.position, _objectiveEntity.transform.position);
                        if (dist <= attackRange)
                            fsm.ChangeState(States.Attack);
                        else if (dist >= rushRange)
                            fsm.ChangeState(States.Rush);
                        break;
                    }
            }
        }

        void Move_FixedUpdate()
        {
            transform.DOLookAt(_objectiveEntity.transform.position, turnSpeed);
            nav.SetDestination(_objectiveEntity.transform.position);
        }
        void Move_Exit()
        {
            this.GetModelManager().GetAnimator().SetBool("IsWalk", false);
        }

        /** Attack State */
        void Attack_Enter()
        {
            Debug.Log("Attack_Enter");
            dontTurn = false;
            //Skill
            if (this.GetModelManager().GetAnimator().GetInteger("attackCount") == attackCount)
                DoSkill(phase);
            //Normal Attack
            else
                DoAttack(phase);

            nav.SetDestination(transform.position);
            nav.isStopped = true;

            currentAttackTimer = 0;
            if (!dontTurn)
                transform.DOLookAt(_objectiveEntity.transform.position, turnSpeed / 2);
        }

        void Attack_Update()
        {
            if (this.GetModelManager().GetAnimator().GetBool("IsAttackEnd"))
            {
                currentAttackTimer += Time.deltaTime;

                if (currentAttackTimer >= attackInterval)
                    fsm.ChangeState(States.Idle);
            }
        }

        void Attack_Exit()
        {
            this.GetModelManager().GetAnimator().SetBool("IsAttackEnd", false);
            if (phase == 2 && this.GetModelManager().GetAnimator().GetInteger("attackCount") == attackCount && this.GetModelManager().GetAnimator().GetInteger("phaseSkill") % 2 != 0)
            {
                attackRange *= 2;
            }
        }

        void SkillWalk_Enter()
        {
            Debug.Log("SkillWalk_Enter");

            this.SetMoveState(true);
            nav.stoppingDistance /= 2;
            this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[7]);
        }

        void SkillWalk_Update()
        {
            if (this.GetModelManager().GetAnimator().GetBool("IsAttackEnd"))
            {
                isWalkReady = true;
                this.GetModelManager().GetAnimator().SetBool("IsAttackEnd", false);
            }
            if (this.GetModelManager().GetAnimator().GetBool("IsSkillWalkEnd"))
            {
                isWalkReady = false;
                transform.DOKill();
                nav.SetDestination(transform.position);
                this.GetModelManager().GetAnimator().SetBool("IsSkillWalkEnd", false);
            }
            if (this.GetModelManager().GetAnimator().GetBool("IsSkillEnd"))
            {
                fsm.ChangeState(States.Idle);
                this.GetModelManager().GetAnimator().SetBool("IsSkillEnd", false);
            }
        }

        void SkillWalk_FixedUpdate()
        {
            if (isWalkReady)
            {
                this.SetMoveState(true);
                transform.DOLookAt(_objectiveEntity.transform.position, turnSpeed);
                nav.SetDestination(_objectiveEntity.transform.position);
            }
        }

        void SkillWalk_Exit()
        {
            nav.stoppingDistance = attackRange;
            this.GetModelManager().GetAnimator().SetBool("IsAttackEnd", false);
            this.GetModelManager().GetAnimator().SetBool("IsSkillWalkEnd", false);
            this.GetModelManager().GetAnimator().SetBool("IsSkillEnd", false);
        }

        void SkillPhase_Enter()
        {
            Debug.Log("SkillPhase_Enter");

            transform.DOKill();
            SetMoveState(true);
            recognize.CurrentViewAngle = 0;
            nav.stoppingDistance = 0;
            isWalkReturn = true;
            this.GetModelManager().GetAnimator().SetBool("IsWalk", true);
            transform.DOLookAt(CurrentInstantiatedStage.transform.position, turnSpeed);
            nav.SetDestination(CurrentInstantiatedStage.transform.position);
        }

        void SkillPhase_Update()
        {
            if (this.GetModelManager().GetAnimator().GetBool("IsAttackEnd"))
            {
                IsInvincible = false;
                this.GetModelManager().GetAnimator().SetBool("IsAttackEnd", false);
            }

            if (isWalkReturn)
            {
                Debug.Log(transform.position);
                if ((transform.position.x >= nav.destination.x - 1f && transform.position.x <= nav.destination.x + 1f) &&
                 (transform.position.z >= nav.destination.z - 1f && transform.position.z <= nav.destination.z + 1f))
                {
                    if (!isSkillOnce)
                    {
                        barrierVFX?.SetActive(true);
                        this.GetModelManager().GetAnimator().SetBool("IsWalk", false);
                        IsInvincible = true;
                        nav.SetDestination(transform.position);
                        nav.isStopped = true;
                        transform.DOLookAt(_objectiveEntity.transform.position, turnSpeed);
                        this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[8]);
                        isSkillOnce = true;
                    }
                }
            }

            if (this.GetModelManager().GetAnimator().GetBool("IsSkillEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void SkillPhase_FixedUpdate()
        {
            if (isWalkReturn)
            {
                SetMoveState(true);
                nav.SetDestination(CurrentInstantiatedStage.transform.position);
            }
        }

        void SkillPhase_Exit()
        {
            recognize.CurrentViewAngle = 360;
            isWalkReturn = false;
            isSkillOnce = false;
            nav.stoppingDistance = attackRange;
            this.GetModelManager().GetAnimator().SetBool("IsWalk", false);
        }

        void Rush_Enter()
        {
            Debug.Log("Rush_Enter");
            this.GetModelManager().GetAnimator().SetTrigger("DoRush");
            nav.SetDestination(transform.position);
            nav.isStopped = true;

            if(GameManager.Instance.CameraController != null)
            {
                GameManager.Instance.CameraController.SwitchCamera(1);
            }
        }

        void Rush_Update()
        {
            if (this.GetModelManager().GetAnimator().GetBool("IsRush"))
            {
                if (!isRushOnce)
                {
                    transform.DOMove(rushDestination, rushTime).SetEase(Ease.InQuad);
                    isRushOnce = true;
                }

                if (Physics.Raycast(transform.position, transform.forward, rushStopDistance, RushStopMask) || transform.position == rushDestination)
                {
                    Debug.Log("RushStop");
                    _objectiveEntity.transform.GetComponent<Player>().SetMoveState(true);
                    _objectiveEntity.transform.parent = null;
                    transform.DOKill();
                    this.GetModelManager().GetAnimator().SetBool("IsRush", false);
                }
            }
            else
            {
                rushRay = new Ray(transform.position, transform.forward);
            }

            if (this.GetModelManager().GetAnimator().GetBool("IsRushEnd"))
            {
                fsm.ChangeState(States.Idle);
            }
        }

        void Rush_FixedUpdate()
        {
            if (!this.GetModelManager().GetAnimator().GetBool("IsRush"))
            {
                transform.DOLookAt(_objectiveEntity.transform.position, turnSpeed / 2);

                if (NavMesh.SamplePosition(rushRay.GetPoint(rushDistance), out navHit, rushDistance, NavMesh.AllAreas))
                {
                    rushDestination = navHit.position;
                    currentRushTime = rushTime * (Vector3.Distance(rushDestination, transform.position) / rushDistance);
                    //GameObject.Find("Cube").transform.position = rushDestination;
                }
            }
        }

        void Rush_Exit()
        {
            this.GetModelManager().GetAnimator().SetBool("IsRushEnd", false);
            isRushOnce = false;

            if(GameManager.Instance.CameraController != null)
            {
                GameManager.Instance.CameraController.SwitchCamera(0);
            }
        }

        /** Death State */
        void Death_Enter()
        {
            Sophia.UserInterface.InGameScreenUI.Instance._storyFadePanel.SetTransparent();
            Sophia.UserInterface.InGameScreenUI.Instance._storyFadePanel.WaitAfterBoss();
            Debug.Log("Death_Enter");
            Die();
        }

        void Death_Update()
        {
            if (!TextManager.Instance.IsStory && !StoryManager.Instance.IsBossClear && Sophia.UserInterface.InGameScreenUI.Instance._storyFadePanel.IsWaitOver)
            {
                Sophia.UserInterface.InGameScreenUI.Instance._fadeUI.FadeOut(0.05f, 2f);
                Sophia.UserInterface.InGameScreenUI.Instance._fadeUI.AddBindingAction(() => UnityEngine.SceneManagement.SceneManager.LoadScene("05_Demo_Clear"));
                StoryManager.Instance.IsBossClear = true;
            }
            //check animation end
        }

        public override bool GetDamaged(DamageInfo damage)
        {
            bool isDamaged = false;

            if (IsInvincible) return isDamaged;

            // if (Life.IsDie) { isDamaged = false; }
            // else
            // {
            //     if (isDamaged = Life.Damaged(damage)) { GameManager.Instance.GlobalEvent.OnEnemyHitEvent.ForEach(Event => Event.Invoke()); }
            // }
            if (isDamaged = Life.Damaged(damage)) { GameManager.Instance.GlobalEvent.OnEnemyHitEvent.ForEach(Event => Event.Invoke()); }
            if (Life.IsDie) { fsm.ChangeState(States.Death); }
            return isDamaged;
        }

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

        #region Move
        private Stat moveSpeed;
        public bool GetMoveState() => isMovable;

        public void SetMoveState(bool movableState)
        {
            nav.enabled = true;

            isMovable = movableState;
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

        private void OnCollisionEnter(Collision other)
        {
            if (!this.GetModelManager().GetAnimator().GetBool("IsRush"))
            {
                return;
            }

            if (other.transform.tag == "Player")
            {
                other.transform.GetComponent<Player>().SetMoveState(false);
                other.transform.parent = this.transform;
            }
        }

        #endregion
    }
}