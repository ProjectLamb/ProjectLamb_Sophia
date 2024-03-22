using Sophia.Composite;

namespace Sophia.DataSystem.Atomics
{
    public class DamageConverterAtomics {
        public SerialDamageConverterData converterData;
        public DamageConverterAtomics(in SerialDamageConverterData SerialDamageConverterData) {
            converterData = SerialDamageConverterData;
        }
        
        public void Invoke(ref DamageInfo referer) {
            referer.damageHandleType = converterData._damageHandleType;
            referer.damageRatio = converterData._damageRatio;
            referer.hitType = converterData._hitType;
        }
    }
}