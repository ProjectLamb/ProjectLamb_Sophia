using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.Instantiates;
using UnityEngine;
using UnityEngine.AI;

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
        public bool GetMoveState();
        public void SetMoveState(bool movableState);
        public void MoveTick();
        public UniTask Turning(Vector3 forwardingVector);
    }

    public interface IMovementAccessible :IMovable {
        public MovementComposite GetMovementComposite();
    }

    public interface IAffectable {
        public void Affect(DataSystem.Modifiers.Affector affector);
        public void Recover(DataSystem.Modifiers.Affector affector);
    }

    public interface IAffectManagerAccessible : IAffectable {
        public AffectorManager GetAffectorManager();
    }
}