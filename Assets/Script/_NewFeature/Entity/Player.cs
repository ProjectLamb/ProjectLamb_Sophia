using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FMODPlus;
using Cysharp.Threading.Tasks;

using Sophia.Composite;
using Sophia.DataSystem;
using Sophia.DataSystem.Numerics;
using Sophia.Instantiates;

namespace Sophia.Entitys
{
    public class Player : Entity, IModelAccessable {

#region SerializeMembeer 
        [SerializeField] private SerialBasePlayerData _basePlayerData;
//      [SerializeField] private ModelManger  _modelManger;
//      [SerializeField] private VisualFXBucket  _visualFXBucket;

#endregion

#region Members
//      [HideInInspector] public Collider entityCollider;
//      [HideInInspector] public Rigidbody entityRigidbody;

        public PlayerStatReferer    StatReferer         {get; private set;}
        public LifeComposite        Life                {get; private set;}
        public MovementComposite    Movement            {get; private set;}
        public DashSkill            DashSkillAbility    {get; private set;}
        public Wealths PlayerWealth;
        
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

#region Stat Accessable

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        [ContextMenu("Get Stats Info")]
        public override string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

#endregion

#region Model Accessable

        public void ChangeSkin(Material skin) { _modelManger.ChangeSkin(skin); }
        public void RevertSkin() { _modelManger.RevertSkin(); }
        public Animator GetAnimator() { return _modelManger.GetAnimator(); }

#endregion

#region Movement

        public MovementComposite GetMovementComposite() => this.Movement;

        public void OnMove(InputAction.CallbackContext context)
        {
           Vector2 input = context.ReadValue<Vector2>();
           Debug.Log(input);
           Movement.GetInputVector(input);
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
    
    [ContextMenu("TEST_MOD_DASHSTATS")]
    public void TEST_MOD_DASHSTATS() {
        // 앞으로 아이템을 먹었을때 실행되는 연산이랑 동일하다.
        DashSkillAbility.MaxStamina.AddCalculator(new StatCalculator(1, E_STAT_CALC_TYPE.Add));
        DashSkillAbility.MaxStamina.RecalculateStat();
    }
    
    public void Dash()
    {
        DashSkillAbility.UseDashSkill(Movement.ForwardingVector, GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed));/*m*/
    }

#endregion

        private void Awake() {
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);
        
            StatReferer         = new PlayerStatReferer();
            Life                = new LifeComposite(_basePlayerData.MaxHp, _basePlayerData.Defence);
            Movement            = new MovementComposite(entityRigidbody, _basePlayerData.MoveSpeed);
            DashSkillAbility    = new DashSkill(this.entityRigidbody);
        }

        private void Start(){
           StatReferer.SetRefStat(Life.MaxHp);
           StatReferer.SetRefStat(Life.Defence);
           StatReferer.SetRefStat(Movement.MoveSpeed);
           StatReferer.SetRefStat(DashSkillAbility.MaxStamina);
           StatReferer.SetRefStat(DashSkillAbility.StaminaRestoreSpeed);

           DashSkillAbility.SetAudioSource(DashSource);
        }

        public void Attack() {}
        public void Skill() {}
    }    
}