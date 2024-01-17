using UnityEngine;

namespace Sophia.Entitys
{
    using Sophia.DataSystem.Numerics;
    using Sophia.DataSystem;
    using Sophia.Composite.RenderModels;
    using Sophia.Instantiates;
    using Sophia.Composite;
    public abstract class Entity : MonoBehaviour, ILifeAccessable, IStatAccessable
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

        public abstract Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        
        /***
        이거.. 내가 하면서도 제대로 하는건지 모르겠네
        제네릭 타입을 런타임에 정해줘서 반환이 가능하다는건가?? 
        이게 가능하다고? 흠..
        */
        public abstract Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType);

        [ContextMenu("Get Stats Info")]
        public abstract string GetStatsInfo();

#endregion

#region Affector Accessable
        public abstract AffectorComposite GetAffectorComposite();
#endregion
    }
}