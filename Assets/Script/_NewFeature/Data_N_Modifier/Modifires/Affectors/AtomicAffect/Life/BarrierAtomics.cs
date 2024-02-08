namespace Sophia.DataSystem.Modifiers
{
    using Sophia.Composite;

    class BarrierAtomics {
        private LifeComposite lifeRef; 
        private float prevBarrierAmount;
        private float barrierAmount;
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