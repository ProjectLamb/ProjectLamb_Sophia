namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite;
    using Sophia.Entitys;
    public class HealAtomics {
        private LifeComposite lifeRef; 
        float healAmount;
        public HealAtomics(LifeComposite life, float amount) {
            lifeRef = life;
            healAmount = amount;
        }
        public void Invoke() {
            lifeRef.Healed(healAmount);
        }
    } 
}