using UnityEngine;

namespace Sophia.DataSystem.Functional.AtomFunctions
{
    using Sophia.Composite;
    using Sophia.DataSystem.Modifiers;

    public static class ItemTriggeredCommand {
        public class GearcoinCollect : IFunctionalCommand<int>
        {
            Entitys.Entity entityRef;
            StatModifier powerModifier;
            public string GetDescription() => "코인 획득";
            public string GetName() => "코인 획득";
            public Sprite GetSprite() => null;

            public GearcoinCollect(Entitys.Entity entity) {
                entityRef = entity;
            }

            public void Invoke(ref int currentGear)
            {
                if(powerModifier != null) {
                    entityRef.GetStat(E_NUMERIC_STAT_TYPE.Power).RemoveModifier(powerModifier);
                }
                powerModifier = new StatModifier(
                    currentGear / 10,
                    E_STAT_CALC_TYPE.Add,
                    E_NUMERIC_STAT_TYPE.Power
                );
                entityRef.GetStat(E_NUMERIC_STAT_TYPE.Power).AddModifier(powerModifier);
                entityRef.GetStat(E_NUMERIC_STAT_TYPE.Power).RecalculateStat();
            }
        }
    }
}