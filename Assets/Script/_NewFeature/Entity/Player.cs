using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FMODPlus;
using Cysharp.Threading.Tasks;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.Instantiates;
    using Sophia.DataSystem.Referer;
    using Sophia.DataSystem.Modifiers;
    using Sophia.UserInterface;

    public class Player : Entity, IMovementAccessible, IAffectManagerAccessible, IInstantiatorAccessible
    {

#region SerializeMember 
//      [SerializeField] private ModelManger  _modelManger;
//      [SerializeField] private VisualFXBucket  _visualFXBucket;
        [SerializeField] private SerialBasePlayerData       _basePlayerData;
        [SerializeField] private ProjectileBucketManager    _projectileBucketManager;
        [SerializeField] private WeaponManager              _weaponManager;
        [SerializeField] private EquipmentManager           _equipmentManager;
        [SerializeField] private AffectorManager            _affectorManager;
        [SerializeField] private SkillManager               _skillManager;

#endregion

#region Members
//      [HideInInspector] public Collider entityCollider;
//      [HideInInspector] public Rigidbody entityRigidbody;
//      [HideInInspector] protected List<IDataSettable> Settables = new();

        private LifeComposite Life;
        private MovementComposite Movement;
        private DashSkill DashSkillAbility;
        private Stat Power;
        private Extras<int> GearcoinExtras;
        private int mPlayerWealth;
        public event UnityAction<int> OnWealthChangeEvent;
        public int PlayerWealth {
            get { return mPlayerWealth; }
            set {
                mPlayerWealth = value;
                OnWealthChangeEvent.Invoke(mPlayerWealth);
            }
        }

        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(Power);
            ExtrasReferer.SetRefExtras<int>(GearcoinExtras);
            this.Settables.ForEach(E => {
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
            StatReferer     = new PlayerStatReferer();
            ExtrasReferer   = new PlayerExtrasReferer();

            Life = new LifeComposite(_basePlayerData.MaxHp, _basePlayerData.Defence);
            Movement = new MovementComposite(entityRigidbody, _basePlayerData.MoveSpeed);
            DashSkillAbility = new DashSkill(this.entityRigidbody, Movement.GetMovemenCompositetData, _basePlayerData.DashForce);
            Power = new Stat(_basePlayerData.Power, 
                E_NUMERIC_STAT_TYPE.Power, 
                E_STAT_USE_TYPE.Natural, 
                OnPowerUpdated 
            );
            GearcoinExtras = new Extras<int>(
                E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered,
                () => {Debug.Log("기어 획득");}
            );
            _affectorManager.Init(_basePlayerData.Tenacity);
        }

        protected override void Start()
        {
            base.Start();
            Life.SetDependUI(InGameScreenUI.Instance._playerHealthBarUI);
            Life.OnDamaged += InGameScreenUI.Instance._hitCanvasShadeScript.Invoke;

            InGameScreenUI.Instance._playerWealthBarUI.SetPlayer(this);
            
            DashSkillAbility.SetDependUI(InGameScreenUI.Instance._playerStaminaBarUI);
            DashSkillAbility.Timer.AddOnUseEvent(() => {
                this.GetModelManger().EnableTrail();
                StartCoroutine(actionDelay(this.GetModelManger().DisableTrail));
            });
            DashSkillAbility.SetAudioSource(DashSource);
        }
#endregion

#region Life Accessible

        public override LifeComposite GetLifeComposite() => this.Life;

        public override bool GetDamaged(DamageInfo damage)
        {
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            isDamaged = Life.Damaged(damage);
            if (Life.IsDie) { Die(); }
            return isDamaged;
        }

        public override bool Die()
        {
            throw new System.NotImplementedException();
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

        public MovementComposite GetMovementComposite() => this.Movement;
        public bool GetMoveState() => this.Movement.IsMovable;
        
        public void SetMoveState(bool movableState) => this.Movement.SetMovableState(movableState);

        public void OnMove(InputValue _value)
        {
            Vector2 move = _value.Get<Vector2>();
            Movement.SetInputVector(move);
        }

        public void MoveTick()
        {
            if(DashSkillAbility.GetIsDashState()) return;
            // GetAnimator().SetFloat("Move", this.entityRigidbody.velocity.magnitude);
            if (!Movement.IsBorder(this.transform)) {
                Movement.MoveTick(this.transform);
            }
        }

        public async UniTask Turning() { await Movement.Turning(transform, Input.mousePosition); }
        //public void TurningWithCallback(UnityAction action) => Movement.TurningWithCallback(transform,Input.mousePosition,action).Forget();

#endregion

#region Dash
        public DashSkill GetDashAbility() => DashSkillAbility;
        public FMODAudioSource DashSource;
        
        public void Dash() => DashSkillAbility.Use();/*m*/

#endregion

        IEnumerator actionDelay(UnityAction action) {
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
            action.Invoke(); 
        }

#region Weapon Handler

        public ProjectileBucketManager GetProjectileBucketManager() => _projectileBucketManager;
        public void OnPowerUpdated() { Debug.Log("공격력 변경"); }

        public async void Attack()
        {
            try
            {
                await Turning();
                _weaponManager.Use();
            }
            catch (OperationCanceledException)
            {

            }
        }

#endregion

#region Skill Handler 

        public SkillManager GetSkillManager() => this._skillManager;
        public void CollectSkill(Skill skill, KeyCode key) => this._skillManager.Collect(skill, key);
        public void DropSkill(KeyCode key) => this._skillManager.Drop(key);
        public void Use(KeyCode key) => this._skillManager.GetSkillByKey(key)?.Use();

#endregion

#region Equip Handler

        public EquipmentManager GetEquipmentManager() => this._equipmentManager;
        public void EquipEquipment(Equipment equipment) => this._equipmentManager.Equip(equipment);
        public void DropEquipment(Equipment equipment) => this._equipmentManager.Drop(equipment);

#endregion

#region Affect Handler

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

#endregion

    }
}