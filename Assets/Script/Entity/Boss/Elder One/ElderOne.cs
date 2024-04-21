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
using UnityEngine.SceneManagement;
using FMODPlus;

namespace Sophia.Entitys
{
    public enum E_ELDERONE_AUDIO_INDEX
    {
        AttackBoth, AttackOne, Swing, Death, FootStep, PhaseTransition
    }
    public class ElderOne : Boss, IMovable
    {
        List<string> animBoolParamList;
        List<string> animTriggerParamList;

        [Header("Stats")]
        public int attackRange = 30;
        public int attackCount = 3;

        private int turnSpeed = 2;
        private bool isPhaseChanged = false;
        private bool IsMovable;
        [SerializeField] private bool IsInvincible;
        [SerializeField] FMODAudioSource[] _audioSource;
        private NavMeshAgent nav;

        // Start is called before the first frame update
        public enum States
        {
            Init,
            Idle,
            Move,
            Attack,
            Skill,
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
                GameManager.Instance.DonDestroyObjectReferer.DontDestroyGameManager.AudioManager.audioStateSender._bossPhaseSender[1].SendCommand();
                nav.speed = _baseEntityData.MoveSpeed * 2;
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
                    this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[7]);
                }
                else
                {
                    IsInvincible = true;
                    this.GetModelManager().GetAnimator().SetTrigger(animTriggerParamList[8]);
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
            InGameScreenUI.Instance._fadeUI.AddBindingAction(() =>
            {
                SceneManager.LoadScene("03_Demo_Clear");
            });

            InGameScreenUI.Instance._fadeUI.FadeOut(0.2f, 3f);
        }

        [ContextMenu("PlayFootStepSound")]
        public void PlayFootStepSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.FootStep].Play();
        }
        [ContextMenu("PlaySwingSound")]
        public void PlaySwingSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.Swing].Play();
        }
        [ContextMenu("PlayAttackOneSound")]
        public void PlayAttackOneSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.AttackOne].Play();
        }
        [ContextMenu("PlayAttackBothSound")]
        public void PlayAttackBothSound()
        {
            _audioSource[(int)E_ELDERONE_AUDIO_INDEX.AttackBoth].Play();
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
        public void UseProjectile_JumpAttack()
        {
            Sophia.Instantiates.ProjectileObject useProjectile = ProjectilePool.GetObject(_attckProjectiles[(int)ANIME_STATE.JUMP]).Init(this);

            _projectileBucketManager.InstantablePositioning((int)ANIME_STATE.JUMP, useProjectile)
                                    .SetProjectilePower(GetStat(E_NUMERIC_STAT_TYPE.Power) * 2)
                                    .Activate();
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

            fsm.ChangeState(States.Idle);
        }

        /** Idle State */
        void Idle_Enter()
        {
            Debug.Log("Idle_Enter");
            ResetAnimParam();
            SetMoveState(false);
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
            //Skill
            if (this.GetModelManager().GetAnimator().GetInteger("attackCount") == attackCount)
                DoSkill(phase);
            //Normal Attack
            else
                DoAttack(phase);

            SetMoveState(false);

            transform.DOLookAt(_objectiveEntity.transform.position, turnSpeed / 2);
        }

        void Attack_Update()
        {
            if (this.GetModelManager().GetAnimator().GetBool("IsAttackEnd"))
            {
                if (this.GetModelManager().GetAnimator().GetCurrentAnimatorStateInfo(0).IsTag("skill"))
                    fsm.ChangeState(States.Skill);
                else
                    fsm.ChangeState(States.Idle);
            }
        }

        void Attack_End()
        {
            this.GetModelManager().GetAnimator().SetBool("IsAttackEnd", false);
        }

        void Skill_Enter()
        {
            Debug.Log("Skill_Enter");
            IsInvincible = false;
            if (!this.GetModelManager().GetAnimator().GetCurrentAnimatorStateInfo(0).IsTag("walk"))
            {
                SetMoveState(false);
                transform.DOKill();
                nav.SetDestination(transform.position);
            }
        }

        void Skill_FixedUpdate()
        {
            if (this.GetModelManager().GetAnimator().GetCurrentAnimatorStateInfo(0).IsTag("walk"))
            {
                transform.DOLookAt(_objectiveEntity.transform.position, turnSpeed);
                nav.SetDestination(_objectiveEntity.transform.position);
            }
        }

        void Skill_Update()
        {
            if (this.GetModelManager().GetAnimator().GetBool("IsSkillEnd"))
            {
                if (this.GetModelManager().GetAnimator().GetCurrentAnimatorStateInfo(0).IsTag("walk"))
                    fsm.ChangeState(States.Skill);
                else
                    fsm.ChangeState(States.Idle);
            }
        }

        void Skill_Exit()
        {
            this.GetModelManager().GetAnimator().SetBool("IsSkillEnd", false);
        }

        /** Death State */
        void Death_Enter()
        {
            Debug.Log("Death_Enter");
            Die();
        }

        void Death_Update()
        {
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
        public bool GetMoveState() => IsMovable;

        public void SetMoveState(bool movableState)
        {
            nav.enabled = true;

            IsMovable = movableState;
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