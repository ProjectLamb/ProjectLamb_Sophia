namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite;
    using Sophia.Entitys;
    public class DamageAtomics {
        public readonly float baseTickDamage;
        public readonly float tickDamageRatio;
        public readonly float intervalTime;
        public DamageAtomics(SerialTickDamageAffectData damageAffectData) {
            baseTickDamage  = damageAffectData._baseTickDamage;
            tickDamageRatio = damageAffectData._tickDamageRatio;
            intervalTime    = damageAffectData._intervalTime;
        }
        public void Invoke(ILifeAccessible lifeAccessible) {
            lifeAccessible.GetDamaged((int)(baseTickDamage * tickDamageRatio));
        }

        public void Run(ILifeAccessible lifeAccessible) {
            lifeAccessible.GetDamaged((int)(baseTickDamage * tickDamageRatio));
        }
    }
}