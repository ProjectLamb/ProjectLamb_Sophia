using Feature_NewData.Numerics;
using UnityEngine;

namespace Feature_NewData
{
    public class Entity : MonoBehaviour, ILifeAccessable, IStatAccessable
    {
#region SerializeMembeer 
        [SerializeField] private ModelManger  modelManger;
        [SerializeField] private VisualFXBucket  VisualFXBucket;
#endregion

#region Members
        [HideInInspector] public Collider entityCollider;
        [HideInInspector] public Rigidbody entityRigidbody;

        public LifeComposite Life {get; private set;}
        public EntityStatReferer StatReferer {get; private set;}

#endregion

#region Life Accessable
        public LifeComposite GetLifeComposite() => this.Life;

        public void GetDamaged(int damage) {
            if (Life.IsDie) { return; }
            Life.Damaged(damage);
            if(Life.IsDie) {Die();}
        }
        public void GetDamaged(int damage, VisualFXObject vfx) {
            if (Life.IsDie) { return; }
            Life.Damaged(damage);
            if(Life.IsDie) {Die();}
        }

        public void Die() => Destroy(gameObject, 0.5f);

#endregion

#region Stat Accessable

        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        [ContextMenu("Get Stats Info")]
        public string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

#endregion
    }
}