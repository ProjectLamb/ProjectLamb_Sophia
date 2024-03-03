
using System.Text;
namespace Sophia.DataSystem.Functional.AtomFunctions
{
    using Sophia.Composite;
    using UnityEngine;
    
    public static class CalculateDamageCommand
    {
        public class DodgeHit : IFunctionalCommand<DamageInfo>, IRandomlyActivatable<DamageInfo>
        {
            private Atomics.DamageConverterAtomics damageConverter;

            public DodgeHit(in SerialDamageConverterData serialDamageConverterData) {
                damageConverter = new Atomics.DamageConverterAtomics(serialDamageConverterData);
            }

            public void Invoke(ref DamageInfo referer)
            {
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.damageHandleType = DamageHandleType.Dodge;
                referer.damageAmount = 0;
                referer.damageRatio = 0;
            }

#region UI Access

            public string GetName() =>"회피 한다";
            public string GetDescription() => "회피 한다";
            public Sprite GetSprite() => null;

#endregion

#region Randomly Activate
            private System.Random random;
            private int percentage;
            private bool IsRandomlyActivate = false;

            public IFunctionalCommand<DamageInfo> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }

        public class HardHit : IFunctionalCommand<DamageInfo>, IRandomlyActivatable<DamageInfo>
        {
            private SerialDamageConverterData converterData;

            public HardHit(in SerialDamageConverterData serialDamageConverterData) {
            }

            public void Invoke(ref DamageInfo referer)
            {
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.damageRatio *= 5;
            }

#region UI Access

            public string GetName() =>"럭키 치명타";
            public string GetDescription() => "럭키 치명타";
            public Sprite GetSprite() => null;

#endregion

#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<DamageInfo> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }

        public class MulHit : IFunctionalCommand<DamageInfo>, IRandomlyActivatable<DamageInfo>
        {
            private SerialDamageConverterData converterData;

            public MulHit(in SerialDamageConverterData serialDamageConverterData) {
                converterData = serialDamageConverterData;
            }
            public void Invoke(ref DamageInfo referer)
            {
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.damageRatio *= converterData._damageRatio;
            }

#region UI Access

            public string GetName() =>"럭키 치명타";
            public string GetDescription() => "럭키 치명타";
            public Sprite GetSprite() => null;

#endregion


#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<DamageInfo> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }
    }
}