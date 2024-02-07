using UnityEngine;

namespace Sophia.Entitys
{
    using Sophia.DataSystem;
    using Sophia.Composite.RenderModels;
    using Sophia.Instantiates;
    using Sophia.Composite;
    using Sophia.DataSystem.Referer;
    using Sophia.DataSystem.Modifiers.Affector;

    public abstract class Entity : MonoBehaviour, ILifeAccessible, IDataAccessible, IVisualAccessible
    {
#region SerializeMember 
        [SerializeField] protected ModelManger  _modelManger;
        [SerializeField] protected VisualFXBucket  _visualFXBucket;
#endregion

#region Members
        [HideInInspector] public Collider entityCollider;
        [HideInInspector] public Rigidbody entityRigidbody;

#endregion

#region Life Accessible
        public abstract LifeComposite GetLifeComposite();
        public abstract bool GetDamaged(int damage);
        public abstract bool Die();

#endregion

#region Data Accessible
        public abstract EntityStatReferer GetStatReferer();
        public abstract Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        
        public abstract string GetStatsInfo();

        public abstract EntityExtrasReferer GetExtrasReferer();
        public abstract Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType);

#endregion

#region Affector Accessible

        public abstract AffectorManager GetAffectorManager();
        public abstract void Affect(DataSystem.Modifiers.NewAffector.Affector affector);
        public abstract void ModifiedByAffector(Affector affector);

#endregion

#region Visual Accessible
        public ModelManger GetModelManger() => this._modelManger;
        public VisualFXBucket GetVisualFXBucket() => this._visualFXBucket;
        
#endregion
    }
}