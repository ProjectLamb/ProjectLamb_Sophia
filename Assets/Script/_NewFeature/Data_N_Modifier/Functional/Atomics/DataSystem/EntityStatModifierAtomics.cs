using System;
using System.Collections;
using System.Collections.Generic;

namespace Sophia.DataSystem.Atomics
{
    public class EntityStatModifyAtomics {
        public readonly Dictionary<E_NUMERIC_STAT_TYPE, Modifiers.StatModifier> StatModifiers = new();
        public EntityStatModifyAtomics(in SerialStatCalculateDatas SerialStatCalculateDatas) {
            foreach (E_NUMERIC_STAT_TYPE statType in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE)))
            {
                SerialStatModifierDatas statValue = SerialStatCalculateDatas.GetModifierDatas(statType);
                if (statValue.calType != E_STAT_CALC_TYPE.None)
                {
                    StatModifiers.Add(statType, new Modifiers.StatModifier(statValue.amount, statValue.calType, statType));
                }
            }
        }

        public void Invoke(IDataAccessible dataAccessible) {
            foreach (var modifier in StatModifiers)
            {
                Stat statRef = dataAccessible.GetStatReferer().GetStat(modifier.Key);
                statRef.AddModifier(modifier.Value);
                statRef.RecalculateStat();
            }
        }
        
        public void Revert(IDataAccessible dataAccessible) {
            foreach (var modifier in StatModifiers)
            {
                Stat statRef = dataAccessible.GetStatReferer().GetStat(modifier.Key);
                statRef.RemoveModifier(modifier.Value);
                statRef.RecalculateStat();
            }
        }
    }
}