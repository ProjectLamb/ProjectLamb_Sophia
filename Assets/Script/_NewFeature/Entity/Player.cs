using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FMODPlus;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.Instantiates;
    using Sophia.DataSystem.Referer;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Modifiers.ConcreteEquipment;
    using Sophia.UserInterface;
    using Sophia.Composite.RenderModels;
    using Unity.Cinemachine;

    public class Player : Entity, IMovementAccessible, IAffectManagerAccessible, IInstantiatorAccessible
    {

        #region SerializeMember 
        [Header("None")]
        //      [SerializeField] private ModelManger  _modelManger;
        //      [SerializeField] private VisualFXBucket  _visualFXBucket;
        [SerializeField] private SerialBasePlayerData _basePlayerData;
        [SerializeField] private ProjectileBucketManager _projectileBucketManager;
        [SerializeField] private WeaponManager _weaponManager;
        [SerializeField] private EquipmentManager _equipmentManager;
        [SerializeField] private AffectorManager _affectorManager;
        [SerializeField] private SkillManager _skillManager;
        [SerializeField] private SkillIndicator skillIndicator;

        #endregion

        #region Members
        //      [HideInInspector] public Collider entityCollider;
        //      [HideInInspector] public Rigidbody entityRigidbody;
        //      [HideInInspector] protected List<IDataSettable> Settables = new();

        private LifeComposite Life;
        private MovementComposite Movement;
        private DashSkill DashSkillAbility;
        private LayerMask playerOriginLayer;
        private Stat Power;
        private Extras<int> GearcoinExtras;

        public int mPlayerWealth;
        public event UnityAction<int> OnWealthChangeEvent;
        public int PlayerWealth
        {
            get { return mPlayerWealth; }
            set
            {
                mPlayerWealth = value;
                if (DontDestroyGameManager.Instance != null)
                {
                    DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.Gear = value;
                }
                InGameScreenUI.Instance._playerWealthBarUI.CountingNumber = mPlayerWealth;
                OnWealthChangeEvent.Invoke(value - mPlayerWealth);
            }
        }

        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(Power);
            ExtrasReferer.SetRefExtras<int>(GearcoinExtras);
            this.Settables.ForEach(E =>
            {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }

        protected override void CollectSettable()
        {
            this.Settables.Add(Life);
            this.Settables.Add(Movement);
            this.Settables.Add(DashSkillAbility);
            this.Settables.Add(_projectileBucketManager);
            this.Settables.Add(_weaponManager);
            this.Settables.Add(_affectorManager);
            this.Settables.Add(GameManager.Instance.NewFeatureGlobalEvent);
        }

        protected override void Awake()
        {
            /**/
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);
            playerOriginLayer = gameObject.layer;
            StatReferer = new PlayerStatReferer();
            ExtrasReferer = new PlayerExtrasReferer();

            // Load Health
            // 데이터 로딩 타이밍에 의한 문제가 있을 경우 Start로 옮기기
            if (DontDestroyGameManager.Instance != null)
            {
                if (DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.Health != _basePlayerData.MaxHp)
                {
                    Life = new LifeComposite(_basePlayerData.MaxHp, DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.Health, _basePlayerData.Defence);
                }
                else
                {
                    Life = new LifeComposite(_basePlayerData.MaxHp, _basePlayerData.Defence);
                }
                DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.IsDied = false;
            }
            else
            {
                Life = new LifeComposite(_basePlayerData.MaxHp, _basePlayerData.Defence);
            }

            Movement = new MovementComposite(this.transform, this.entityRigidbody, _basePlayerData.MoveSpeed);
            DashSkillAbility = new DashSkill(this.entityRigidbody, Movement.GetMovemenCompositetData, _basePlayerData.DashForce);
            Power = new Stat(_basePlayerData.Power,
                E_NUMERIC_STAT_TYPE.Power,
                E_STAT_USE_TYPE.Natural,
                OnPowerUpdated
            );
            GearcoinExtras = new Extras<int>(
                E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered,
                () => { Debug.Log("기어 획득"); }
            );

            _affectorManager.Init(_basePlayerData.Tenacity);
            _playerIdleBehaivour.InitByData(this);
        }

        protected override void Start()
        {
            base.Start();

            Life.SetDependUI(InGameScreenUI.Instance._playerHealthBarUI);
            Life.OnDamaged += InGameScreenUI.Instance._hitCanvasShadeScript.Invoke;
            Life.OnHpUpdated += OnHealthUpdated;

            // Hit Audio 
            Life.OnHit += HitSource.Play;
            Life.OnEnterDie += DeathSource.Play;

            InGameScreenUI.Instance._playerWealthBarUI.SetPlayer(this);

            DashSkillAbility.SetDependUI(InGameScreenUI.Instance._playerStaminaBarUI);
            DashSkillAbility.Timer.AddOnUseEvent(() =>
            {
                this.GetModelManager().EnableTrail();
                StartCoroutine(actionDelay(DashEnd));
            });
            DashSkillAbility.SetAudioSource(DashSource);

            OnWealthChangeEvent.Invoke(mPlayerWealth);

            PlayerController.AllowInput(this.name);

            if (DontDestroyGameManager.Instance != null)
                LoadPlayerData();
        }

        #endregion

        #region Life Accessible

        public FMODAudioSource HitSource;
        public FMODAudioSource DeathSource;
        public override LifeComposite GetLifeComposite() => this.Life;

        public override bool GetDamaged(DamageInfo damage)
        {
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            isDamaged = Life.Damaged(damage);
            if (isDamaged) { GetModelManager().GetAnimator().SetTrigger("GetDamaged"); }
            if (Life.IsDie) { Die(); }
            return isDamaged;
        }

        public override bool Die()
        {
            PlayerController.DisallowInput(this.name);

            if (DontDestroyGameManager.Instance != null)
            {
                //Restart Logic
                DontDestroyGameManager.Instance.SaveLoadManager.ResetData();
                DontDestroyGameManager.Instance.SaveLoadManager.Data.IsTutorial = false;
                DontDestroyGameManager.Instance.SaveLoadManager.Data.CutSceneSaveData.IsSkipStory = true;
                DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.IsDied = true;
            }

            GetMovementComposite().SetMoveState(false);
            entityCollider.enabled = false;
            _modelManager.GetAnimator().SetTrigger("Die");

            InGameScreenUI.Instance._fadeUI.AddBindingAction(() =>
            {
                DontDestroyGameManager.Instance.AudioManager.audioStateSender._stopSender.SendCommand();
                SceneManager.LoadScene("01_Loading");
            });
            InGameScreenUI.Instance._fadeUI.FadeOut(0.02f, 1.0f);
            //OnDieEvent.Invoke();

            return true;
        }

        private void OnHealthUpdated(float input)
        {
            DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.Health = Life.CurrentHealth;
            if (Life.CurrentHealth <= Life.MaxHp / 100 * 20)
            {
                if (InGameScreenUI.Instance._lowHPCanvasShadeScript.IsRepeating)
                    return;
                    
                InGameScreenUI.Instance._lowHPCanvasShadeScript.Repeat();
            }
            else
            {
                InGameScreenUI.Instance._lowHPCanvasShadeScript.UnRepeat();
            }
        }

        #endregion

        #region Data Accessible

        public override EntityStatReferer GetStatReferer() => this.StatReferer;

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        [ContextMenu("GetStatsInfo")]
        public void GetStatsInfoDebug()
        {
            Debug.Log(StatReferer.GetStatsInfo());
        }

        public override string GetStatsInfo()
        {
            return this.StatReferer.GetStatsInfo();
        }

        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;

        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);

        #endregion

        #region Movement
        [SerializeField] public PlayerIdleBehaivour _playerIdleBehaivour;
        public FMODAudioSource IdleRestAudio;
        public MovementComposite GetMovementComposite() => this.Movement;
        public bool GetMoveState() => this.Movement.IsMovable;

        public void SetMoveState(bool movableState) => this.Movement.SetMoveState(movableState);

        public Vector2 MoveInput;
        public void OnMove(InputValue _value)
        {
            MoveInput = _value.Get<Vector2>();
            Movement.SetInputVector(MoveInput);
        }

        public void MoveTick()
        {
            if (DashSkillAbility.GetIsDashState()) return;
            if (!Sophia.PlayerAttackAnim.canExitAttack) return;
            // GetAnimator().SetFloat("Move", this.entityRigidbody.velocity.magnitude);
            if (!Movement.IsBorder(this.transform))
            {
                Movement.MoveTick();
                GetModelManager().GetAnimator().SetFloat("Move", entityRigidbody.velocity.magnitude);
            }
        }

        public async UniTask Turning(Vector3 forwardingVector) { await Movement.Turning(Input.mousePosition); }
        //public void TurningWithCallback(UnityAction action) => Movement.TurningWithCallback(transform,Input.mousePosition,action).Forget();

        #endregion

        #region Dash
        public DashSkill GetDashAbility() => DashSkillAbility;
        public FMODAudioSource DashSource;

        public void Dash()
        {
            DashSkillAbility.Use();
        }/*m*/

        public void DashEnd()
        {
            gameObject.layer = playerOriginLayer;
            this.GetModelManager().DisableTrail();
            GameManager.Instance.CameraController.cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = GameManager.Instance.CameraController.OriginCameraDamping;
        }

        #endregion

        IEnumerator actionDelay(UnityAction action)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
            action.Invoke();
        }

        #region Weapon Handler
        public WeaponManager GetWeaponManager() => _weaponManager;
        public ProjectileBucketManager GetProjectileBucketManager() => _projectileBucketManager;
        public void OnPowerUpdated() { Debug.Log("공격력 변경"); }

        public async void Attack()
        {
            try
            {
                if (!GetModelManager().GetAnimator().GetBool("canNextAttack"))
                    return;
                // if(Sophia.PlayerAttackAnim.canExitAttack || Sophia.PlayerAttackAnim.resetAtkTrigger) return;
                await Movement.TurningWithAction(transform, Input.mousePosition, () => GetModelManager().GetAnimator().SetTrigger("DoAttack"), true);

            }
            catch (OperationCanceledException)
            {

            }
        }


        #endregion

        #region Skill Handler 

        public SkillManager GetSkillManager() => this._skillManager;

        public void CollectSkill(Skill skill, KeyCode key)
        {
            _skillManager.Collect(skill, key);
        }

        public void DropSkill(KeyCode key) => this._skillManager.Drop(key);
        public async void Use(KeyCode key)  // Using Skill
        {
            // 만약 스킬 쿨이 돌았다면
            if (_skillManager.GetSkillByKey(key).GetCoolTimeComposite().GetIsReadyToUse())
            {
                // 만약 스킬 인디케이터를 표시해야 하면
                if (this._skillManager.GetSkillByKey(key).GetIsSkillIndicate())
                {
                    await Movement.TurningWithAction(transform, Input.mousePosition, () =>
                    {
                        this._skillManager.GetSkillByKey(key)?.Use();
                    }, false);
                }
                else
                {
                    this._skillManager.GetSkillByKey(key)?.Use();
                }
            }
        }
        public void Indicate(KeyCode key)
        {
            string indicateSkillName = this._skillManager.GetSkillByKey(key)?.GetName().Trim(); // 스킬 이름
            if ((this._skillManager.GetSkillByKey(key)?.GetName() != null) && (_skillManager.GetSkillByKey(key).GetCoolTimeComposite().GetIsReadyToUse()))
            {
                //쿨타임 아닐때
                // if (this._skillManager.GetSkillByKey(key).GetIsSkillIndicate()) // 이전 if문 작동안해서 주석처리
                if (!skillIndicator.IsIndicate)
                {
                    skillIndicator.IsIndicate = true;
                    skillIndicator.changeIndicate(indicateSkillName);
                }
            }
        }
        #endregion

        #region Equip Handler

        public EquipmentManager GetEquipmentManager() => this._equipmentManager;
        public void EquipEquipment(Equipment equipment) => this._equipmentManager.Equip(equipment);
        public void DropEquipment(Equipment equipment)
        {
            // File
            foreach (var item in DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.EquipmentDataList)
            {
                if (equipment.ID == item._equipmentID)
                {
                    DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.EquipmentDataList.Remove(item);
                }
            }

            this._equipmentManager.Drop(equipment);
        }

        #endregion

        #region Affect Handler

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

        #endregion

        #region Load Data Handler

        public void LoadPlayerData()
        {
            GlobalSaveLoadManager saveLoadManager = DontDestroyGameManager.Instance.SaveLoadManager;
            if (saveLoadManager != null)
            {
                //기어
                PlayerWealth = saveLoadManager.Data.PlayerData.Gear;

                //부품
                //번호를 토대로 장착
                if (saveLoadManager.Data.PlayerData.EquipmentDataList.Count > 0)
                {
                    foreach (var item in saveLoadManager.Data.PlayerData.EquipmentDataList)
                    {
                        SerialEquipmentData serialEquipmentData = item;

                        Debug.Log(FactoryConcreteEquipment.GetEquipmentByID(serialEquipmentData, GetComponent<Player>()).Name);
                        EquipEquipment(FactoryConcreteEquipment.GetEquipmentByID(serialEquipmentData, GetComponent<Player>()));
                    }
                }

                // //스킬
                // foreach (var item in saveLoadManager.Data.PlayerData.SkillDataDic)
                // {
                //     if (item.Value != null)
                //         CollectSkill(item.Value, item.Key);
                // }
            }
        }

        #endregion

    }
}