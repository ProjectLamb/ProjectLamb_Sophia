
namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite;
    public class HoldAtomics {
        MovementComposite movementRef;
        public HoldAtomics(MovementComposite movement) {
            movementRef = movement;
        }        
        public void Invoke() => movementRef.SetMovableState(true);
        public void Revert() => movementRef.SetMovableState(false);
    }
}