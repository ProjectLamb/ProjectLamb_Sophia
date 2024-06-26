using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    
    using Cysharp.Threading.Tasks;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Functional;
    public class TEST_EnemyStub : Entity, IMovable
    {
#region SerializeMember 
        [SerializeField] protected SerialBaseEntityData _baseEntityData;
        [SerializeField] public AffectorManager _affectorManager;
//      [SerializeField] protected ModelManger _modelManger;
//      [SerializeField] protected VisualFXBucket _visualFXBucket;
#endregion

#region Members
//      [HideInInspector] public Collider entityCollider;
//      [HideInInspector] public Rigidbody entityRigidbody;
//      [HideInInspector] protected List<IDataSettable> Settables = new();

        public LifeComposite Life { get; private set; }

#endregion

        protected override void SetDataToReferer()
        {
            StatReferer.SetRefStat(MoveSpeed);
            this.Settables.ForEach(E => {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }
        
        protected override void CollectSettable()
        {
            Settables.Add(Life);
            Settables.Add(_affectorManager);
        }

        protected override void Awake()
        {
            base.Awake();
            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            MoveSpeed = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural);
            _affectorManager.Init(_baseEntityData.Tenacity);
        }

        protected override void Start()
        {
            base.Start();
            Life.OnEnterDie += () => {
                object NullRef = null;
                GameManager.Instance.NewFeatureGlobalEvent.EnemyDie.PerformStartFunctionals(ref GlobalHelper.NullRef);
            };
        }

        private void FixedUpdate() {
            MoveTick();
        }


#region Life Accessible

        public override LifeComposite GetLifeComposite() => this.Life;

        public override bool GetDamaged(DamageInfo damage)
        {
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            else {
                if(isDamaged = Life.Damaged(damage)) {
                    GameManager.Instance?.GlobalEvent?.OnEnemyHitEvent?.ForEach(Event => Event.Invoke());
                }
            }
            if (Life.IsDie) { Die(); }
            return isDamaged;
        }

        public override bool Die() {
            Destroy(gameObject, 0.5f);
            return true;
        }

#endregion

#region Stat Accessible

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        [ContextMenu("Get Stats Info")]
        public override string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }


        public override EntityStatReferer GetStatReferer() => StatReferer;
        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;
        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);

#endregion

#region Movement

        public bool IsMovable = false;
        public Stat MoveSpeed;

        public bool GetMoveState() => IsMovable;

        public void SetMoveState(bool movableState) => IsMovable = movableState;

        public void MoveTick() {
            if(IsMovable == false) return;
            Transform targetPos = GameManager.Instance.PlayerGameObject.transform;
            Vector3 ForwardingVector = Vector3.Normalize((targetPos.position - transform.position));
            entityRigidbody.velocity = ForwardingVector * MoveSpeed.GetValueForce() * Time.fixedDeltaTime;
        }

        public UniTask Turning(Vector3 forwardingVector)
        {
            throw new System.NotImplementedException();
        }
        
#endregion
        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);
    }
}
