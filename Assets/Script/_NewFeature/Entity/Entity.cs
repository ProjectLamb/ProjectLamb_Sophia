using UnityEngine;
using System.Collections.Generic;

namespace Sophia.Entitys
{
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Composite.RenderModels;
    using Sophia.Instantiates;
    using Sophia.Composite;

    public abstract class Entity : MonoBehaviour, ILifeAccessible, IDataAccessible, IVisualAccessible, IAffectable, IAudioAccessible
    {

#region SerializeMember 
        
        [SerializeField] protected ModelManger          _modelManger;
        [SerializeField] protected EntityAudioManager   _audioManager;
        [SerializeField] protected VisualFXBucket       _visualFXBucket;

#endregion

#region Members
        [HideInInspector] public Collider entityCollider;
        [HideInInspector] public Rigidbody entityRigidbody;
        [HideInInspector] protected List<IDataSettable> Settables = new();

#endregion

        protected abstract void SetDataToReferer();
        
        protected abstract void CollectSettable();

        protected virtual void Awake() {
            TryGetComponent<Collider>(out entityCollider);
            TryGetComponent<Rigidbody>(out entityRigidbody);
            StatReferer     = new EntityStatReferer();
            ExtrasReferer   = new EntityExtrasReferer();
        }

        protected virtual void Start() {
            CollectSettable();
            SetDataToReferer();
        }


#region Life Accessible
        public abstract LifeComposite GetLifeComposite();
        public abstract bool GetDamaged(DamageInfo damage);
        public abstract bool Die();

#endregion

#region Data Accessible

        public EntityStatReferer StatReferer { get; protected set; }
        public EntityExtrasReferer ExtrasReferer { get; protected set; }
        public abstract EntityStatReferer GetStatReferer();
        public abstract Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        public abstract string GetStatsInfo();
        public abstract EntityExtrasReferer GetExtrasReferer();
        public abstract Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType);

#endregion

#region Affector Accessible

        public abstract AffectorManager GetAffectorManager();
        public abstract void Affect(DataSystem.Modifiers.Affector affector);
        public abstract void Recover(DataSystem.Modifiers.Affector affector);

#endregion

#region Visual Accessible

        public ModelManger GetModelManger() => this._modelManger;
        public GameObject GetGameObject() => this._modelManger.GetModelObject();
        public VisualFXBucket GetVisualFXBucket() => this._visualFXBucket;
        
#endregion

#region Audio Handler
        public EntityAudioManager GetAudioManager() => _audioManager;
#endregion
    }
}