using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;
using Sophia.DataSystem.Modifiers;
using Sophia.Composite;
using Sophia.DataSystem;
using Sophia.DataSystem.Referer;

namespace Sophia.Entitys
{
    public abstract class Boss : Enemy, IRecogStateAccessible
    {
        #region SerailizeMember

        [SerializeField] protected RecognizeEntityComposite recognize;
        protected LifeComposite Life { get; set; }
        protected int phase = 1;

        #endregion

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

        }

        #region Inherit

        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);

        public override bool Die()
        {
            Life.Died(); return true;
        }

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();

        public abstract override bool GetDamaged(DamageInfo damage);

        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);

        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;

        public override LifeComposite GetLifeComposite() => this.Life;

        public override RecognizeEntityComposite GetRecognizeComposite() => this.recognize;
        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        public override EntityStatReferer GetStatReferer() => this.StatReferer;

        public override string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

        protected override void CollectSettable()
        {
            Settables.Add(Life);
            Settables.Add(_projectileBucketManager);
            Settables.Add(_affectorManager);
        }

        protected abstract override void SetDataToReferer();

        #endregion
    }
}