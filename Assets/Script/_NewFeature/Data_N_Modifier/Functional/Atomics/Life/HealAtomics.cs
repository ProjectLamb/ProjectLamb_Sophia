namespace Sophia.DataSystem.Atomics
{
    using Sophia.Composite;

    public class HealAtomics {
        public readonly LifeComposite lifeRef; 
        int healAmount;
        public HealAtomics(LifeComposite life, int amount) {
            lifeRef = life;
            healAmount = amount;
        }
        public void Invoke() {
            lifeRef.Healed(healAmount);
        }
    } 
}