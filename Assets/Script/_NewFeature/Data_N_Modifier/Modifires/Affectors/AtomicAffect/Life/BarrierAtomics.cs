namespace Sophia.DataSystem.Modifiers
{
    using Sophia.Composite;

    class BarrierAtomics {
        public readonly LifeComposite lifeRef; 
        public readonly float prevBarrierAmount;
        public readonly float barrierAmount;
        public BarrierAtomics(LifeComposite life, float amount) {
            lifeRef = life;
            barrierAmount = amount;
        }
        public void Invoke() {
            lifeRef.SetBarrier(barrierAmount);
        }

        public void Revert() {
            lifeRef.SetBarrier(0);
        }
    }
}