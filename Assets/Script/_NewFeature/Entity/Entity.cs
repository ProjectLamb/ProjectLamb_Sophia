using UnityEngine;
using Sophia.DataSystem;
using Sophia.DataSystem.Numerics;
using Sophia.Instantiates;
using Sophia.Composite;

namespace Sophia.Entitys
{
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

#region Stat Accessable

        public abstract Stat GetStat(E_NUMERIC_STAT_TYPE numericType);

        [ContextMenu("Get Stats Info")]
        public abstract string GetStatsInfo();

#endregion
    }
}