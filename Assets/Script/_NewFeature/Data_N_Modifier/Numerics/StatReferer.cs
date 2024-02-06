using System;
using System.Collections.Generic;
using System.Text;
using Sophia.DataSystem.Modifiers;
using UnityEngine;

namespace Sophia.DataSystem.Numerics
{
    [System.Serializable]
    public struct SerialBaseEntityData{
    
    [Header("Life ")]
        public float MaxHp;
        public float Defence;
    
    [Header("Move")]
        public float MoveSpeed;
        public float Accecerate;
    
    [Header("Affect")]
        public float Tenacity;
    
    [Header("Instantiator")]
        public float Power;

        public float InstantiableDurateLifeTimeMultiplyRatio;
        public float InstantiableSizeMultiplyRatio;
        public float InstantiableForwardingSpeedMultiplyRatio;
    }
    
    [System.Serializable]
    public struct SerialBasePlayerData{
        [Header("Life ")]
        public float MaxHp;
        public float Defence;
        [Header("Move")]
        public float MoveSpeed;
        public float Accecerate;
        [Header("Affect")]
        public float Tenacity;

        [Header("Dash")]
        public float MaxStamina;
        public float StaminaRestoreSpeed;

        [Header("Ohter")]
        public float Luck;

        [Header("Instantiator")]
        public float Power;
    }
    
    [System.Serializable]
    public struct SerialBaseInstantiatorData {
        [Header("Instantiator/Comman")]
        public float InstantiableDurateLifeTimeMultiplyRatio;
        public float InstantiableSizeMultiplyRatio;
        public float InstantiableForwardingSpeedMultiplyRatio;
    }
    
    [System.Serializable]
    public struct SerialBaseWeaponData {
        [Header("Instantiator/Weapon")]
        public float PoolSize;
        public float AttackSpeed;
        public float MeleeRatio;
        public float RangerRatio;
        public float TechRatio;
    }
    
    [System.Serializable]
    public struct SerialBaseSkillData {
        [Header("Instantiator/Skill")]
        public float EfficienceMultiplyer;
        public float CoolDownSpeed;
    }

    [System.Serializable]
    public struct Wealths
    {
        public int Gear;
        public int Frag;
    }
    
    [System.Serializable]
    public struct SerialCalculateDatas
    {
        [SerializeField] public SerialStatModifireDatas MaxHp;
        [SerializeField] public SerialStatModifireDatas Defence;
        [SerializeField] public SerialStatModifireDatas MoveSpeed;
        [SerializeField] public SerialStatModifireDatas Accecerate;
        [SerializeField] public SerialStatModifireDatas Tenacity;
        [SerializeField] public SerialStatModifireDatas MaxStamina;
        [SerializeField] public SerialStatModifireDatas StaminaRestoreSpeed;
        [SerializeField] public SerialStatModifireDatas Luck;
        [SerializeField] public SerialStatModifireDatas Power;
        [SerializeField] public SerialStatModifireDatas InstantiableDurateLifeTimeMultiplyRatio;
        [SerializeField] public SerialStatModifireDatas InstantiableSizeMultiplyRatio;
        [SerializeField] public SerialStatModifireDatas InstantiableForwardingSpeedMultiplyRatio;
        [SerializeField] public SerialStatModifireDatas PoolSize;
        [SerializeField] public SerialStatModifireDatas AttackSpeed;
        [SerializeField] public SerialStatModifireDatas MeleeRatio;
        [SerializeField] public SerialStatModifireDatas RangerRatio;
        [SerializeField] public SerialStatModifireDatas TechRatio;
        [SerializeField] public SerialStatModifireDatas EfficienceMultiplyer;
        [SerializeField] public SerialStatModifireDatas CoolDownSpeed;

        public SerialStatModifireDatas GetModifireDatas(E_NUMERIC_STAT_TYPE numericType)
        {
            switch (numericType)
            {
                case E_NUMERIC_STAT_TYPE.MaxHp                                      : {return MaxHp;}
                case E_NUMERIC_STAT_TYPE.Defence                                    : {return Defence;}
                case E_NUMERIC_STAT_TYPE.MoveSpeed                                  : {return MoveSpeed;}
                case E_NUMERIC_STAT_TYPE.Accecerate                                 : {return Accecerate;}
                case E_NUMERIC_STAT_TYPE.Tenacity                                   : {return Tenacity;}
                case E_NUMERIC_STAT_TYPE.MaxStamina                                 : {return MaxStamina;}
                case E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed                        : {return StaminaRestoreSpeed;}
                case E_NUMERIC_STAT_TYPE.Power                                      : {return Power;}
                case E_NUMERIC_STAT_TYPE.Luck                                       : {return Luck;}
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio    : {return InstantiableDurateLifeTimeMultiplyRatio;}
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio              : {return InstantiableSizeMultiplyRatio;}
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio   : {return InstantiableForwardingSpeedMultiplyRatio;}
                case E_NUMERIC_STAT_TYPE.PoolSize                                   : {return PoolSize;}
                case E_NUMERIC_STAT_TYPE.AttackSpeed                                : {return AttackSpeed;}
                case E_NUMERIC_STAT_TYPE.MeleeRatio                                 : {return MeleeRatio;}
                case E_NUMERIC_STAT_TYPE.RangerRatio                                : {return RangerRatio;}
                case E_NUMERIC_STAT_TYPE.TechRatio                                  : {return TechRatio;}
                case E_NUMERIC_STAT_TYPE.EfficienceMultiplyer                       : {return EfficienceMultiplyer;}
                case E_NUMERIC_STAT_TYPE.CoolDownSpeed                              : {return CoolDownSpeed;}
                default : {return new SerialStatModifireDatas{amount = -1, calType = 0};}
            }
        }
    }

    public class EntityStatReferer : IStatAccessible
    {
        protected SortedList<E_NUMERIC_STAT_TYPE, Stat> Stats = new();

        public EntityStatReferer() {
            this.Stats.Add(E_NUMERIC_STAT_TYPE.MaxHp, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.Defence, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.MoveSpeed, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.Accecerate, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.Tenacity, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.Power, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio, null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio, null);
        }

        public void SetRefStat(Stat statRef) {
            if(statRef.NumericType == E_NUMERIC_STAT_TYPE.None) throw new System.Exception($"참조 스텟이 초기화되지 않음");

            if(Stats.ContainsKey(statRef.NumericType)) {
                Stats[statRef.NumericType] = statRef;
            }
            else {throw new System.Exception($"이 Entity 멤버에는 {statRef.NumericType.ToString()} 없음");}
        }
        
        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType) {
            if(numericType == E_NUMERIC_STAT_TYPE.None) {throw new System.Exception($"NoneStat은 가져올 수 없음");}
            if(Stats.ContainsKey(numericType)) {
                if(Stats.TryGetValue(numericType, out Stat stat)){ return stat; }
                else {throw new System.Exception($"{numericType.ToString()} 값이 NULL 임");}
            }
            else {throw new System.Exception($"이 Entity 멤버에는 {numericType.ToString()} 없음");}
        }

        public string GetStatsInfo()
        {
            StringBuilder stringBuilder = new StringBuilder(1000);

            foreach(E_NUMERIC_STAT_TYPE key in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE)))
            {
                if(key == E_NUMERIC_STAT_TYPE.None || Stats[key] == null){continue;}
                stringBuilder.Append(key.ToString());
                stringBuilder.Append(" : ");
                stringBuilder.Append(Stats[key].GetValueForce().ToString());
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }

    public class PlayerStatReferer : EntityStatReferer
    {
        public PlayerStatReferer() {
            this.Stats.Add(E_NUMERIC_STAT_TYPE.MaxStamina,                               null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed,                      null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.CoolDownSpeed,                            null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.PoolSize,                                 null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.AttackSpeed,                              null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.MeleeRatio,                               null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.RangerRatio,                              null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.TechRatio,                                null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.EfficienceMultiplyer,                     null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.Luck,                                     null);
        }
    }
}