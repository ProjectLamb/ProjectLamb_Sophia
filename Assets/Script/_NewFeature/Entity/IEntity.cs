using Sophia.Composite;
using Sophia.Instantiates;

namespace Sophia
{
    public interface IDamagable {
        public void GetDamaged(int damage);
        public void GetDamaged(int damage, VisualFXObject vfx);
    }
    
    public interface IDieable {
        public void Die();

    }

    public interface ILifeAccessible : IDamagable, IDieable{
        public LifeComposite GetLifeComposite();
    }
}