
using System.Threading;

namespace Sophia.DataSystem
{
    using System.Numerics;
    using Sophia.Composite;
    using Sophia.Composite.NewTimer;
    using Sophia.DataSystem.Functional;
    using Sophia.Entitys;

    namespace Modifiers
    {
        namespace ConcreteEquipment
        {
            public class LightFlash_009 : Equipment
            {
                public ExtrasModifier<DamageInfo> floatReferenceExtrasModifier;
                public LightFlash_009(SerialEquipmentData equipmentData) : base(equipmentData)
                {
                    floatReferenceExtrasModifier = new ExtrasModifier<DamageInfo>(
                        new CalculateDamageCommands.Dodge(equipmentData._extrasCalculateDatas.OnDamaged[0]._damageConverterData),
                        E_EXTRAS_PERFORM_TYPE.Start,
                        E_FUNCTIONAL_EXTRAS_TYPE.Damaged
                    );
                }

                public override void Invoke(IDataAccessible dataAccessible)
                {
                    base.Invoke(dataAccessible);
                    Extras<DamageInfo> extrasRef = dataAccessible.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.Damaged);
                    extrasRef.AddModifier(floatReferenceExtrasModifier);
                    extrasRef.RecalculateExtras();
                }

                public override void Revert(IDataAccessible dataAccessible)
                {
                    base.Revert(dataAccessible);
                    Extras<DamageInfo> extrasRef = dataAccessible.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.Damaged);
                    extrasRef.RemoveModifier(floatReferenceExtrasModifier);
                    extrasRef.RecalculateExtras();
                }
            }
        }
    }
}