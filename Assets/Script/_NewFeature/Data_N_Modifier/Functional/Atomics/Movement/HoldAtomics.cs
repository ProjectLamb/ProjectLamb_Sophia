
namespace Sophia.DataSystem.Atomics
{
    public class HoldAtomics {
        public void Invoke(IMovable movable) => movable.SetMoveState(false);
        public void Revert(IMovable movable) => movable.SetMoveState(true);
    }
}