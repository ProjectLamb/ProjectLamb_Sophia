using UnityEngine;
using UnityEngine.Events;
using FMODPlus;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Numerics;
    using Sophia.DataSystem.Functional;
    using Sophia.Instantiates;
    using Sophia.DataSystem.Modifiers.Affector;

    public class Player : Entity {

#region SerializeMembeer 
        [SerializeField] private SerialBasePlayerData _basePlayerData;
//      [SerializeField] private ModelManger  _modelManger;
//      [SerializeField] private VisualFXBucket  _visualFXBucket;
        [SerializeField] private WeaponManager        _weaponManager;
        [SerializeField] private ProjectileBucket     _projectileBucket;
        [SerializeField] public  Wealths              _PlayerWealth;

#endregion

#region Members
//      [HideInInspector] public Collider entityCollider;
//      [HideInInspector] public Rigidbody entityRigidbody;

        public PlayerStatReferer    StatReferer         {get; private set;}
        public PlayerExtrasReferer  ExtrasReferer         {get; private set;}
        public LifeComposite        Life                {get; private set;}
        public MovementComposite    Movement            {get; private set;}
        public AffectorHandlerComposite    AffectHandler            {get; private set;}
        public DashSkill            DashSkillAbility    {get; private set;}
        public Stat                 Power               {get; private set;}
        
#endregion

#region Life Accessable

        public override LifeComposite GetLifeComposite() => this.Life;

        public override void GetDamaged(int damage) {
            if (Life.IsDie) { return; }
            Life.Damaged(damage);
            if(Life.IsDie) {Die();}
        }
        public override void GetDamaged(int damage, VisualFXObject vfx) {
            if (Life.IsDie) { return; }
            Life.Damaged(damage);
            if(Life.IsDie) {Die();}

            _visualFXBucket.ActivateInstantable(this, vfx)?.Activate();
        }

        public override void Die() {
            throw new System.NotImplementedException();
        }

#endregion

#region Data Accessable
        public override EntityStatReferer GetStatReferer() => StatReferer;
        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        [ContextMenu("Get Stats Info")]
        public override string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;
        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);
#endregion

#region Movement

        public MovementComposite GetMovementComposite() => this.Movement;
        public void OnMove(InputValue _value)
        {
            Vector2 move = _value.Get<Vector2>();
            Movement.SetInputVector(move);
        }

        public void MoveTick() 
        {
            if(DashSkillAbility.GetIsDashState(GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed))) return;
            // GetAnimator().SetFloat("Move", this.entityRigidbody.velocity.magnitude);
            if(!Movement.IsBorder(this.transform)) Movement.Tick(this.transform);
        }

        public void Turning() => Movement.Turning(transform,Input.mousePosition).Forget();
        public void TurningWithCallback(UnityAction action) => Movement.TurningWithCallback(transform,Input.mousePosition,action).Forget();

#endregion

#region Dash
    public FMODAudioSource DashSource;
    
    public void Dash() => DashSkillAbility.Use();/*m*/

#endregion

#region Weaponmanager
        public WeaponManager GetWeaponManager() => this._weaponManager;

#endregion
        private void Awake() {
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);
        
            StatReferer         = new PlayerStatReferer();
            ExtrasReferer       = new PlayerExtrasReferer();
            Life                = new LifeComposite(_basePlayerData.MaxHp, _basePlayerData.Defence);
            Movement            = new MovementComposite(entityRigidbody, _basePlayerData.MoveSpeed);
            DashSkillAbility    = new DashSkill(this.entityRigidbody, Movement.GetMovemenCompositetData);
            Power               = new Stat(_basePlayerData.Power,
                                            E_NUMERIC_STAT_TYPE.Power,
                                            E_STAT_USE_TYPE.Natural,
                                            OnPowerUpdated
                                        );
            AffectHandler = new AffectorHandlerComposite(_basePlayerData.Tenacity);
        }
        public void OnPowerUpdated() {throw new System.NotImplementedException();}
        private void Start(){
           StatReferer.SetRefStat(Life.MaxHp);
           StatReferer.SetRefStat(Life.Defence);
           StatReferer.SetRefStat(Movement.MoveSpeed);
           StatReferer.SetRefStat(DashSkillAbility.MaxStamina);
           StatReferer.SetRefStat(DashSkillAbility.StaminaRestoreSpeed);
           StatReferer.SetRefStat(AffectHandler.Tenacity);

           StatReferer.SetRefStat(Power);
                      
           StatReferer.SetRefStat(_projectileBucket.InstantiableDurateLifeTimeMultiplyRatio);
           StatReferer.SetRefStat(_projectileBucket.InstantiableSizeMultiplyRatio);
           StatReferer.SetRefStat(_projectileBucket.InstantiableForwardingSpeedMultiplyRatio);

           StatReferer.SetRefStat(_weaponManager.PoolSize);
           StatReferer.SetRefStat(_weaponManager.AttackSpeed);
           StatReferer.SetRefStat(_weaponManager.MeleeRatio);

           DashSkillAbility.SetAudioSource(DashSource);

        
           ExtrasReferer.SetRefExtras<Entity>(new Extras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected, ()=>{Debug.Log("Affect Extras Changed");}));
        }
        public void Attack() { _weaponManager.GetCurrentWeapon().Use(this); }
        public void Skill() {throw new System.NotImplementedException();}

#region Affect Handler

        public override AffectorHandlerComposite GetAffectorHandlerComposite() => this.AffectHandler;
        public override void ModifiedByAffector(Affector affector) => this.AffectHandler.ModifiyByAffector(affector);

#endregion

    }    
}