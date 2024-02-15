
using System.Text;
namespace Sophia.DataSystem.Functional.AtomFunctions
{
    using Sophia.Composite;
    using UnityEngine;
    
    public static class CalculateDamageCommand
    {
        public class DodgeHit : IFunctionalCommand<DamageInfo>, IRandomlyActivatable
        {
            public SerialDamageConverterData converterData;
            public System.Random random;
            public DodgeHit(in SerialDamageConverterData serialDamageConverterData) {
                random = new System.Random();
                converterData = serialDamageConverterData;
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

            public bool GetIsActivated() => converterData._activatePercentage <= random.Next(101);
        }

        public class CriticalHit : IFunctionalCommand<DamageInfo>, IRandomlyActivatable
        {
            public SerialDamageConverterData converterData;
            public System.Random random;
            public CriticalHit(in SerialDamageConverterData serialDamageConverterData) {
                random = new System.Random();
                converterData = serialDamageConverterData;
            }
            public void Invoke(ref DamageInfo referer)
            {
                if(GetIsActivated()) return;
                referer.hitType = HitType.Critical;
                referer.damageRatio *= converterData._damageRatio;
            }

#region UI Access

            public string GetName() =>"럭키 치명타";
            public string GetDescription() => "럭키 치명타";
            public Sprite GetSprite() => null;

#endregion
            public bool GetIsActivated() => converterData._activatePercentage <= random.Next(100);
        }
    }
}