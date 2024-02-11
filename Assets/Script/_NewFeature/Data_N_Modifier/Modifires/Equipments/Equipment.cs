using UnityEngine;

using System;
using System.Collections.Generic;
using Sophia.Composite;
using Sophia.DataSystem.Functional;


namespace Sophia.DataSystem.Modifiers
{
    public class Equipment : IUserInterfaceAccessible
    {
        public readonly string Name;
        public readonly string Description;
        public readonly Sprite Icon;
        public readonly Dictionary<E_NUMERIC_STAT_TYPE, StatModifier> StatModifiers = new();

        public Equipment(SerialEquipmentData equipmentData) {
            Name            = equipmentData._equipmentName;
            Description     = equipmentData._description;
            Icon            = equipmentData._icon;

            foreach (E_NUMERIC_STAT_TYPE statType in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE)))
            {
                SerialStatModifireDatas statValue = equipmentData._statCalculateDatas.GetModifireDatas(statType);
                if (statValue.calType != E_STAT_CALC_TYPE.None)
                {
                    StatModifiers.Add(statType, new StatModifier(statValue.amount, statValue.calType, statType));
                }
            }

            // foreach(E_FUNCTIONAL_EXTRAS_TYPE funcType in Enum.GetValues(typeof(E_FUNCTIONAL_EXTRAS_TYPE))) {
            //     SerialExtrasModifireDatas extrasValue = equipmentData._extraDatas.GetModifireDatas(funcType);
            //     if(funcType....)
            //     
            //     
            // }
        }

        public virtual void Invoke(IDataAccessible dataAccessible) {
            foreach (var modifier in StatModifiers)
            {
                Stat statRef = dataAccessible.GetStat(modifier.Key);
                statRef.AddModifier(modifier.Value);
                statRef.RecalculateStat();
            }
        }
        public virtual void Revert(IDataAccessible dataAccessible) {
            foreach (var modifier in StatModifiers)
            {
                Stat statRef = dataAccessible.GetStat(modifier.Key);
                statRef.RemoveModifier(modifier.Value);
                statRef.RecalculateStat();
            }
        }

        public string GetName() => Name;
        public string GetDescription() => Description;
        public Sprite GetSprite() => Icon;
    }
}