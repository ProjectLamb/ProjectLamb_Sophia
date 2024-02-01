using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.Instantiates;

namespace Sophia
{
    public interface IDamagable {
        public void GetDamaged(int damage);
    }
    
    public interface IDieable {
        public void Die();

    }

    public interface ILifeAccessible : IDamagable, IDieable{
        public LifeComposite GetLifeComposite();
    }

    public interface IMovable {
        public bool GetMoveState();
        public void SetMoveState(bool movableState);
        public void MoveTick();
        public UniTask Turning();
    }
}