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
    using Sophia.DataSystem.Referer;
    using Sophia.DataSystem.Modifiers;
    using System.Collections.Generic;

    public class Player : Entity, IMovementAccessible, IAffectManagerAccessible
    {

#region SerializeMember 
//      [SerializeField] private ModelManger  _modelManger;
//      [SerializeField] private VisualFXBucket  _visualFXBucket;
        [SerializeField] private SerialBasePlayerData       _basePlayerData;
        [SerializeField] private ProjectileBucketManager    _projectileBucketManager;
        [SerializeField] private WeaponManager              _weaponManager;
        [SerializeField] private EquipmentManager           _equipmentManager;
        [SerializeField] private AffectorManager            _affectorManager;
        [SerializeField] public  Wealths                    _PlayerWealth;

#endregion

#region Members
//      [HideInInspector] public Collider entityCollider;
//      [HideInInspector] public Rigidbody entityRigidbody;
//      [HideInInspector] protected List<IDataSettable> Settables = new();

        private LifeComposite Life;
        private MovementComposite Movement;
        private DashSkill DashSkillAbility;
        private Stat Power;

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
            if (DashSkillAbility.GetIsDashState(GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed))) return;
            // GetAnimator().SetFloat("Move", this.entityRigidbody.velocity.magnitude);
            if (!Movement.IsBorder(this.transform)) 
                Movement.MoveTick(this.transform);
        }

        public async UniTask Turning() { await Movement.Turning(transform, Input.mousePosition); }
        //public void TurningWithCallback(UnityAction action) => Movement.TurningWithCallback(transform,Input.mousePosition,action).Forget();

#endregion

#region Dash
        
        public FMODAudioSource DashSource;
        
        public void Dash() => DashSkillAbility.Use();/*m*/

#endregion

        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(Power);
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
        }

        protected override void Awake()
        {
            /**/
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);
            StatReferer = new PlayerStatReferer();
            ExtrasReferer = new PlayerExtrasReferer();

            Life = new LifeComposite(_basePlayerData.MaxHp, _basePlayerData.Defence);
            Movement = new MovementComposite(entityRigidbody, _basePlayerData.MoveSpeed);
            DashSkillAbility = new DashSkill(this.entityRigidbody, Movement.GetMovemenCompositetData);
            Power = new Stat(_basePlayerData.Power, E_NUMERIC_STAT_TYPE.Power, E_STAT_USE_TYPE.Natural, OnPowerUpdated );
            _affectorManager.Init(_basePlayerData.Tenacity);
        
        }
        
        protected override void Start()
        {
            base.Start();
            DashSkillAbility.SetAudioSource(DashSource);
        }

#region Weapon Hanlder

        public void OnPowerUpdated() { Debug.Log("공격력 변경"); }

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

#endregion

#region Skill Handler

        public void Skill() { throw new System.NotImplementedException(); }

#endregion

#region Equip Handler

        public EquipmentManager GetEquipmentManager() => this._equipmentManager;
        public void Equip(Equipment equipment) => this._equipmentManager.Equip(equipment);
        public void Drop(Equipment equipment) => this._equipmentManager.Drop(equipment);

#endregion

#region Affect Handler

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

#endregion

    }
}

