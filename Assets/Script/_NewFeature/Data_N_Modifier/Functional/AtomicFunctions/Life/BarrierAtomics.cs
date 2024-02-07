namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite;
    using Sophia.Entitys;

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