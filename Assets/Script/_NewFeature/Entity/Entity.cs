using UnityEngine;

namespace Feature_NewData
{
    public abstract class Entity : MonoBehaviour, ILifeAccessable
    {
        public LifeComposite Life;
        

#region Life Accessable

        public abstract void GetDamaged(int damage);
        public abstract void GetDamaged(int damage, VFXObject vfx);
        public abstract void Die();
        public LifeComposite GetLifeComposite()
        {
            return this.Life;
        }
#endregion
    }
}