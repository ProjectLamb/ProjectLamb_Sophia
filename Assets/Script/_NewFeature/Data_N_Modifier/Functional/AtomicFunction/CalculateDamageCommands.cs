
using System.Text;
namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite;
    using UnityEngine;
    
    public static class CalculateDamageCommand
    {
        public class DodgeHit : IFunctionalCommand<DamageInfo>
        {
            public SerialDamageConverterData converterData;
            public System.Random random;
            public DodgeHit(SerialDamageConverterData serialDamageConverterData) {
                random = new System.Random();
                converterData = serialDamageConverterData;
            }
            public void Invoke(ref DamageInfo referer)
            {
                if(converterData._activatePercentage <= random.Next(101)) return;
                referer.damageHandleType = DamageHandleType.Dodge;
                referer.damageAmount = 0;
                referer.damageRatio = 0;
            }

            #region UI Access
            public string GetName() =>"회피 한다";
            public string GetDescription() => "회피 한다";
            public Sprite GetSprite() => null;
            #endregion
        }

        public class CriticalHit : IFunctionalCommand<DamageInfo>
        {
            public SerialDamageConverterData converterData;
            public System.Random random;
            public CriticalHit(SerialDamageConverterData serialDamageConverterData) {
                random = new System.Random();
                converterData = serialDamageConverterData;
            }
            public void Invoke(ref DamageInfo referer)
            {
                if(converterData._activatePercentage <= random.Next(101)) return;
                referer.hitType = HitType.Critical;
                referer.damageRatio *= converterData._damageRatio;
            }

            #region UI Access

            public string GetName() =>"럭키 치명타";
            public string GetDescription() => "럭키 치명타";
            public Sprite GetSprite() => null;

            #endregion
        }
    }
}