using System;
using UnityEngine;
using FMODPlus;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.Instantiates;
    using Sophia.DataSystem.Modifiers.Affector;
    using Sophia.DataSystem.Referer;
    using Sophia.DataSystem.Modifiers;

    public class Player : Entity, IMovable
    {

#region SerializeMember 
        //      [SerializeField] private ModelManger  _modelManger;
        //      [SerializeField] private VisualFXBucket  _visualFXBucket;
        [SerializeField] private SerialBasePlayerData   _basePlayerData;
        [SerializeField] private WeaponManager          _weaponManager;
        [SerializeField] private ProjectileBucket       _projectileBucket;
        [SerializeField] private EquipmentManager       _equipmentManager;
        [SerializeField] public  Wealths                _PlayerWealth;

#endregion

#region Members
        //      [HideInInspector] public Collider entityCollider;
        //      [HideInInspector] public Rigidbody entityRigidbody;

        public PlayerStatReferer StatReferer            { get; private set; }
        public PlayerExtrasReferer ExtrasReferer        { get; private set; }
        public LifeComposite Life                       { get; private set; }
        public MovementComposite Movement               { get; private set; }
        public AffectorManager AffectHandler   { get; private set; }
        public DashSkill DashSkillAbility               { get; private set; }
        public Stat Power                               { get; private set; }

#endregion

#region Life Accessible

        public override LifeComposite GetLifeComposite() => this.Life;

        public override bool GetDamaged(int damage)
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
            if (DashSkillAbility.GetIsDashState(GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed))) return;
            // GetAnimator().SetFloat("Move", this.entityRigidbody.velocity.magnitude);
            if (!Movement.IsBorder(this.transform)) Movement.MoveTick(this.transform);
        }

        public async UniTask Turning() { await Movement.Turning(transform, Input.mousePosition); }
        //public void TurningWithCallback(UnityAction action) => Movement.TurningWithCallback(transform,Input.mousePosition,action).Forget();

#endregion

#region Dash
        public FMODAudioSource DashSource;

        public void Dash() => DashSkillAbility.Use();/*m*/

#endregion
        private void Awake()
        {
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);

            StatReferer = new PlayerStatReferer();
            ExtrasReferer = new PlayerExtrasReferer();
            Life = new LifeComposite(_basePlayerData.MaxHp, _basePlayerData.Defence);
            Movement = new MovementComposite(entityRigidbody, _basePlayerData.MoveSpeed);
            DashSkillAbility = new DashSkill(this.entityRigidbody, Movement.GetMovemenCompositetData);
            Power = new Stat(_basePlayerData.Power,
                                            E_NUMERIC_STAT_TYPE.Power,
                                            E_STAT_USE_TYPE.Natural,
                                            OnPowerUpdated
                                        );
            AffectHandler = new AffectorManager(_basePlayerData.Tenacity);
        }
        public void OnPowerUpdated() { throw new System.NotImplementedException(); }
        private void Start()
        {
            StatReferer.SetRefStat(Life.MaxHp);
            StatReferer.SetRefStat(Life.Defence);

            ExtrasReferer.SetRefExtras<float>(Life.HitExtras);
            ExtrasReferer.SetRefExtras<float>(Life.DamagedExtras);
            ExtrasReferer.SetRefExtras<object>(Life.DeadExtras);

            StatReferer.SetRefStat(Movement.MoveSpeed);
            ExtrasReferer.SetRefExtras<Vector3>(Movement.MoveExtras);
            ExtrasReferer.SetRefExtras<object>(Movement.IdleExtras);

            StatReferer.SetRefStat(DashSkillAbility.MaxStamina);
            StatReferer.SetRefStat(DashSkillAbility.StaminaRestoreSpeed);
            ExtrasReferer.SetRefExtras<object>(DashSkillAbility.DashExtras);

            StatReferer.SetRefStat(AffectHandler.Tenacity);

            StatReferer.SetRefStat(Power);

            StatReferer.SetRefStat(_projectileBucket.InstantiableDurateLifeTimeMultiplyRatio);
            StatReferer.SetRefStat(_projectileBucket.InstantiableSizeMultiplyRatio);
            StatReferer.SetRefStat(_projectileBucket.InstantiableForwardingSpeedMultiplyRatio);

            StatReferer.SetRefStat(_weaponManager.PoolSize);
            StatReferer.SetRefStat(_weaponManager.AttackSpeed);
            StatReferer.SetRefStat(_weaponManager.MeleeRatio);

            DashSkillAbility.SetAudioSource(DashSource);

            ExtrasReferer.SetRefExtras<Entity>(new Extras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected, () => { Debug.Log("Affect Extras Changed"); }));
        }
        public async void Attack()
        {
            try
            {
                await Turning();
                _weaponManager.GetCurrentWeapon().Use(this);
            }
            catch (OperationCanceledException)
            {

            }
        }
        public void Skill() { throw new System.NotImplementedException(); }

#region Weaponmanager
        public WeaponManager GetWeaponManager() => this._weaponManager;

#endregion

#region Affect Handler

        public override AffectorManager GetAffectorManager() => this.AffectHandler;
        public override void ModifiedByAffector(Affector affector) => this.AffectHandler.ModifiyByAffector(affector);

#endregion


#region Equip Handler

        public EquipmentManager GetEquipmentManager() => this._equipmentManager;
        public void Equip(Equipment equipment) => this._equipmentManager.Equip(equipment);
        public void Drop(Equipment equipment) => this._equipmentManager.Drop(equipment);

#endregion

    }
}

