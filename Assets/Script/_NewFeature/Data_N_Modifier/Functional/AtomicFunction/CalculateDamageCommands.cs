
using System.Text;
namespace Sophia.DataSystem.Functional.AtomFunctions
{
    using Sophia.Composite;
    using UnityEngine;
    
    public static class CalculateDamageCommand
    {
        public class DodgeHit : IFunctionalCommand<DamageInfo>, IRandomlyActivatable
        {
            private Atomics.DamageConverterAtomics damageConverter;
            private System.Random random;
            private float percentage;

            public DodgeHit(in SerialDamageConverterData serialDamageConverterData, float activatePercentage) {
                damageConverter = new Atomics.DamageConverterAtomics(serialDamageConverterData);
                random = new System.Random();
                percentage = activatePercentage;
            }
            public void Invoke(ref DamageInfo referer)
            {
                if(GetIsActivated()) return;
                referer.damageHandleType = DamageHandleType.Dodge;
                referer.damageAmount = 0;
                referer.damageRatio = 0;
            }

#region UI Access

            public string GetName() =>"회피 한다";
            public string GetDescription() => "회피 한다";
            public Sprite GetSprite() => null;

#endregion

            public bool GetIsActivated() => percentage <= random.Next(100);
        }

        public class HardHit : IFunctionalCommand<DamageInfo>, IRandomlyActivatable
        {
            private SerialDamageConverterData converterData;
            private System.Random random;
            private float percentage;
            public HardHit(in SerialDamageConverterData serialDamageConverterData, float activatePercentage) {
                random = new System.Random();
                percentage = activatePercentage;
            }
            public void Invoke(ref DamageInfo referer)
            {
                if(GetIsActivated()) return;
                referer.damageRatio *= 5;
            }

#region UI Access

            public string GetName() =>"럭키 치명타";
            public string GetDescription() => "럭키 치명타";
            public Sprite GetSprite() => null;

#endregion
            public bool GetIsActivated() => percentage <= random.Next(100);
        }
    }
}