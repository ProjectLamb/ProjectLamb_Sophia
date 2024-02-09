using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.Instantiates;

namespace Sophia
{
    public interface IDamagable {
        public bool GetDamaged(DamageInfo damage);
    }
    
    public interface IDieable {
        public bool Die();

    }

    public interface ILifeAccessible : IDamagable, IDieable{
        public LifeComposite GetLifeComposite();
    }

    public interface IMovable {
        public MovementComposite GetMovementComposite();
        public bool GetMoveState();
        public void SetMoveState(bool movableState);
        public void MoveTick();
        public UniTask Turning();
    }

    public interface IAffectable {
        public AffectorManager GetAffectorManager();
        public void Affect(DataSystem.Modifiers.Affector affector);
        public void Recover(DataSystem.Modifiers.Affector affector);
    }
}