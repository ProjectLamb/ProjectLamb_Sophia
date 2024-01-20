using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sophia.Composite;
using Sophia.DataSystem;
using Sophia.DataSystem.Numerics;
using Sophia.Instantiates;
using Sophia.DataSystem.Functional;

namespace Sophia.Entitys
{
    public class TEST_EnemyStub : Entity
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

        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType)
        {
            return ExtrasReferer.GetExtras<T>(functionalType);
        }
        
        public AffectorComposite affectorComposite {get; private set;}

        public override AffectorComposite GetAffectorComposite() => this.affectorComposite;

        #endregion


        #region Life Accessable

        public override LifeComposite GetLifeComposite() => this.Life;

        public override void GetDamaged(int damage)
        {
            if (Life.IsDie) { return; }
            Life.Damaged(damage);
            if (Life.IsDie) { Die(); }
        }

        public override void GetDamaged(int damage, VisualFXObject vfx)
        {
            if (Life.IsDie) { return; }
            Life.Damaged(damage);
            if (Life.IsDie) { Die(); }
            /*기존 코드는 Actiavete의 책임이 있었는데 지금은 그냥 객체 리턴을 하므로 엄연히 활성화 단계는 함수 호출부에서 해야 할것이다*/
            _visualFXBucket.ActivateInstantable(this, vfx)?.Activate();
        }

        public override void Die() => Destroy(gameObject, 0.5f);

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

        private void Awake()
        {
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);

            StatReferer = new EntityStatReferer();
            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            affectorComposite = new AffectorComposite();
        }

        private void Start()
        {
            StatReferer.SetRefStat(Life.MaxHp);
            StatReferer.SetRefStat(Life.Defence);
        }
    }
}
