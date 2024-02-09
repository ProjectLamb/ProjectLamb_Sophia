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
        public List<ExtrasModifier<DamageInfo>> DamageExtrasModifiers = new();

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
            ExtrasModifier<DamageInfo> DamageExtrasModifier = new ExtrasModifier<DamageInfo> (
                new CalculateDamageCommands.Dodge(equipmentData._extrasCalculateDatas.OnDamaged[0]._damageConverterData),
                E_EXTRAS_PERFORM_TYPE.Start,
                E_FUNCTIONAL_EXTRAS_TYPE.Damaged
            );
            DamageExtrasModifiers.Add(DamageExtrasModifier);
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
                Stat statRef = dataAccessible.GetStatReferer().GetStat(modifier.Key);
                statRef.AddModifier(modifier.Value);
                statRef.RecalculateStat();
            }
            foreach(var modifier in DamageExtrasModifiers) {
                Extras<DamageInfo> extrasRef = dataAccessible.GetExtrasReferer().GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.Damaged);
                extrasRef.AddModifier(modifier);
                extrasRef.RecalculateExtras();
            }
        }
        public virtual void Revert(IDataAccessible dataAccessible) {
            foreach (var modifier in StatModifiers)
            {
                Stat statRef = dataAccessible.GetStatReferer().GetStat(modifier.Key);
                statRef.RemoveModifier(modifier.Value);
                statRef.RecalculateStat();
            }
            foreach(var modifier in DamageExtrasModifiers) {
                Extras<DamageInfo> extrasRef = dataAccessible.GetExtrasReferer().GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.Damaged);
                extrasRef.RemoveModifier(modifier);
                extrasRef.RecalculateExtras();
            }
        }

        public string GetName() => Name;
        public string GetDescription() => Description;
        public Sprite GetSprite() => Icon;
    }
}