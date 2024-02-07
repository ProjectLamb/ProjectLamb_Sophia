using UnityEngine;

using System;
using System.Collections.Generic;
using Sophia;
using Sophia.DataSystem;
using Sophia.DataSystem.Referer;
using Sophia.DataSystem.Modifiers;
using Sophia.DataSystem.Functional;

namespace Sophia.DataSystem.Modifiers
{
    [System.Serializable]
    public struct SerialEquipmentData {
        [SerializeField] public string _equipmentName;
        [SerializeField] public string _description;
        [SerializeField] public Sprite _icon;
        [SerializeField] public SerialCalculateDatas _calculateDatas;
    }

    public class Equipment : IUserInterfaceAccessible
    {
        public readonly string Name;
        public readonly string Description;
        public readonly Sprite Icon;
        public readonly Dictionary<E_NUMERIC_STAT_TYPE, StatModifier> StatModifiers = new();
        //public readonly Dictionary<E_FUNCTIONAL_EXTRAS_TYPE, ExtrasModifier> ExtrasModifiers = new();

        public Equipment(SerialEquipmentData equipmentData) {
            Name            = equipmentData._equipmentName;
            Description     = equipmentData._description;
            Icon            = equipmentData._icon;

            foreach (E_NUMERIC_STAT_TYPE statType in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE)))
            {
                SerialStatModifireDatas statValue = equipmentData._calculateDatas.GetModifireDatas(statType);
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

        public string GetName() => Name;
        public string GetDescription() => Description;
        public Sprite GetSprite() => Icon;
    }
}