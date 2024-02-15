
namespace Sophia.DataSystem.Modifiers
{
    using Sophia.Composite;
    public class HoldAtomics {
        public HoldAtomics() {
        }        
        public void Invoke(IMovable movable) => movable.SetMoveState(false);
        public void Revert(IMovable movable) => movable.SetMoveState(true);
    }
}