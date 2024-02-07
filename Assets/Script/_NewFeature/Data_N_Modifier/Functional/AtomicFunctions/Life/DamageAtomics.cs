namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite;
    using Sophia.Entitys;
    public class DamageAtomics {
        private LifeComposite lifeRef; 
        private float damAmount;
        public DamageAtomics(LifeComposite life, float amount) {
            lifeRef = life;
            damAmount = amount;
        }
        public void Invoke() {
            lifeRef.Damaged(damAmount);
        }
    }
}