namespace Sophia.DataSystem.Atomics
{
    using Sophia.Composite;

    public class BarrierAtomics {
        public readonly float barrierAmountRatio;
        private int prevBarrierAmount;
        private int calculatedAmount;
        public BarrierAtomics(float amountRatio) {
            barrierAmountRatio = amountRatio;
        }

        public void Invoke(ILifeAccessible life) {
            prevBarrierAmount = life.GetLifeComposite().CurrentBarrier;
            calculatedAmount = (int)(life.GetLifeComposite().MaxHp.GetValueForce() * barrierAmountRatio);
            life.GetLifeComposite().BarrierCoverd(calculatedAmount);
        }

        public void Revert(ILifeAccessible life) {
            life.GetLifeComposite().SetBarrier(prevBarrierAmount);
        }
    }
}