namespace Sophia.DataSystem.Atomics
{
    using Sophia.Composite;

    public class HealAtomics {
        public readonly LifeComposite lifeRef; 
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