using Sophia.Composite;

namespace Sophia.DataSystem.Modifiers
{
    

    public class DamageAtomics {
        public readonly DamageInfo baseTickDamage;
        public readonly float intervalTime;
        public DamageAtomics(SerialTickDamageAffectData damageAffectData) {
            baseTickDamage  = damageAffectData._baseTickDamage;
            intervalTime    = damageAffectData._intervalTime;
        }

        public void Invoke(ILifeAccessible lifeAccessible) {
            lifeAccessible.GetDamaged(baseTickDamage);
        }

        public void Run(ILifeAccessible lifeAccessible) {
            lifeAccessible.GetDamaged(baseTickDamage);
        }
    }
}