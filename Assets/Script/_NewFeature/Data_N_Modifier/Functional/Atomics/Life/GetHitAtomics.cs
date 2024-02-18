using Sophia.Composite;

namespace Sophia.DataSystem.Atomics
{
    public class GetHitAtomics {
        public readonly DamageInfo baseTickDamage;
        public readonly float intervalTime;
        public GetHitAtomics(in SerialTickDamageData damageAffectData) {
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