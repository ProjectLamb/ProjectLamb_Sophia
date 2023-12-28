using UnityEngine;

namespace Feature_NewData
{
    public abstract class Entity : MonoBehaviour, ILifeAccessable
    {
        public LifeManager Life;
        

#region Life Accessable

        public abstract void GetDamaged(int damage);
        public abstract void GetDamaged(int damage, VFXObject vfx);
        public abstract void Die();
        public LifeManager GetLifeManager()
        {
            return this.Life;
        }
#endregion
    }
}