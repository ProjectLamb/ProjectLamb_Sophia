using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Numerics;
    using Sophia.Instantiates;
    using Sophia.DataSystem.Functional;
    using Sophia.DataSystem.Modifiers.Affector;
    using Cysharp.Threading.Tasks;

    public class TEST_EnemyStub : Entity, IMovable
    {
#region SerializeMembeer 
        [SerializeField] protected SerialBaseEntityData _baseEntityData;
        // [SerializeField] protected ModelManger _modelManger;
        // [SerializeField] protected VisualFXBucket _visualFXBucket;
#endregion

#region Members
        //  [HideInInspector] public Collider entityCollider;
        //  [HideInInspector] public Rigidbody entityRigidbody;

        public LifeComposite Life { get; private set; }
        public EntityStatReferer StatReferer { get; private set; }
        public EntityExtrasReferer ExtrasReferer { get; private set; }
        public AffectorHandlerComposite affectorComposite {get; private set;}

        public override AffectorHandlerComposite GetAffectorHandlerComposite() => this.affectorComposite;
        public override void ModifiedByAffector(Affector affector) => this.affectorComposite.ModifiyByAffector(affector);

#endregion


#region Life Accessible

        public override LifeComposite GetLifeComposite() => this.Life;

        public override void GetDamaged(int damage)
        {
            if (Life.IsDie) { return; }
            Life.Damaged(damage);
            if (Life.IsDie) { Die(); }
        }

        public override void Die() => Destroy(gameObject, 0.5f);

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

        private void Awake()
        {
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);

            StatReferer = new EntityStatReferer();
            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            affectorComposite = new AffectorHandlerComposite(_baseEntityData.Tenacity);
            
            MoveSpeed = new Stat(_baseEntityData.MoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural);
        }

        private void Start()
        {
            StatReferer.SetRefStat(Life.MaxHp);
            StatReferer.SetRefStat(Life.Defence);
            StatReferer.SetRefStat(affectorComposite.Tenacity);
            StatReferer.SetRefStat(MoveSpeed);
        }

        private void FixedUpdate() {
            MoveTick();
        }

#region Movement Test

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

        public UniTask Turning()
        {
            throw new System.NotImplementedException();
        }
#endregion
    }
}
