using UnityEngine;

namespace Sophia.Entitys
{
    using Sophia.DataSystem.Numerics;
    using Sophia.DataSystem;
    using Sophia.Composite.RenderModels;
    using Sophia.Instantiates;
    using Sophia.Composite;
    using Sophia.DataSystem.Functional;
    using Sophia.DataSystem.Modifiers.Affector;

    public abstract class Entity : MonoBehaviour, ILifeAccessable, IDataAccessable, IVisualAccessable
    {
#region SerializeMembeer 
        [SerializeField] protected ModelManger  _modelManger;
        [SerializeField] protected VisualFXBucket  _visualFXBucket;
#endregion

#region Members
        [HideInInspector] public Collider entityCollider;
        [HideInInspector] public Rigidbody entityRigidbody;

#endregion

#region Life Accessable
        public abstract LifeComposite GetLifeComposite();
        public abstract void GetDamaged(int damage);
        public abstract void GetDamaged(int damage, VisualFXObject vfx);
        public abstract void Die();

#endregion

#region Data Accessable
        public abstract EntityStatReferer GetStatReferer();
        public abstract Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        [ContextMenu("Get Stats Info")]
        public abstract string GetStatsInfo();

        public abstract EntityExtrasReferer GetExtrasReferer();
        public abstract Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType);

#endregion

#region Affector Accessable

        public abstract AffectorHandlerComposite GetAffectorHandlerComposite();
        public abstract void ModifiedByAffector(Affector affector);

        public ModelManger GetModelManger() => this._modelManger;
        public VisualFXBucket GetVisualFXBucket() => this._visualFXBucket;
        
#endregion
    }
}