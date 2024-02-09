
using System.Text;
namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite;
    using UnityEngine;

    public static class CalculateDamageCommands
    {
        public class Dodge : IFunctionalCommand<DamageInfo>
        {
            public SerialDamageConverterData converterData;
            public System.Random random;
            public Dodge(SerialDamageConverterData serialDamageConverterData) {
                random = new System.Random();
                converterData = serialDamageConverterData;
            }
            public void Invoke(ref DamageInfo referer)
            {
                if(converterData._activatePercentage <= random.Next(101)) return;
                referer.dodgeDamage = true;
                referer.damageAmount = 0;
                referer.damageRatio = 0;
            }

            #region UI Access
            public string GetName() =>"회피 한다";
            public string GetDescription() => "회피 한다";
            public Sprite GetSprite() => null;
            #endregion
        }

        public class LuckyHit : IFunctionalCommand<DamageInfo>
        {
            public SerialDamageConverterData converterData;
            public System.Random random;
            public LuckyHit(SerialDamageConverterData serialDamageConverterData) {
                random = new System.Random();
                converterData = serialDamageConverterData;
            }
            public void Invoke(ref DamageInfo referer)
            {
                referer.criticalDamage = converterData._criticalDamage;
                referer.damageRatio = converterData._damageRatio;
            }

            #region UI Access
            public string GetName() =>"럭키 치명타";
            public string GetDescription() => "럭키 치명타";
            public Sprite GetSprite() => null;
            #endregion
        }
    }
}