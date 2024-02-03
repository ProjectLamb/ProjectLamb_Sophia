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
        [SerializeField] public SerialModifireDatas MaxHp;
        [SerializeField] public SerialModifireDatas Defence;
        [SerializeField] public SerialModifireDatas MoveSpeed;
        [SerializeField] public SerialModifireDatas Accecerate;
        [SerializeField] public SerialModifireDatas Tenacity;
        [SerializeField] public SerialModifireDatas MaxStamina;
        [SerializeField] public SerialModifireDatas StaminaRestoreSpeed;
        [SerializeField] public SerialModifireDatas Luck;
        [SerializeField] public SerialModifireDatas Power;
        [SerializeField] public SerialModifireDatas InstantiableDurateLifeTimeMultiplyRatio;
        [SerializeField] public SerialModifireDatas InstantiableSizeMultiplyRatio;
        [SerializeField] public SerialModifireDatas InstantiableForwardingSpeedMultiplyRatio;
        [SerializeField] public SerialModifireDatas PoolSize;
        [SerializeField] public SerialModifireDatas AttackSpeed;
        [SerializeField] public SerialModifireDatas MeleeRatio;
        [SerializeField] public SerialModifireDatas RangerRatio;
        [SerializeField] public SerialModifireDatas TechRatio;
        [SerializeField] public SerialModifireDatas EfficienceMultiplyer;
        [SerializeField] public SerialModifireDatas CoolDownSpeed;

        public SerialModifireDatas GetModifireDatas(E_NUMERIC_STAT_TYPE numericType)
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
                default : {return new SerialModifireDatas{amount = -1, calType = 0};}
            }
        }
    }

    public class EntityStatReferer : IStatAccessible
    {
        private SortedList<E_NUMERIC_STAT_TYPE, Stat> Stats = new();

        public EntityStatReferer() {
            Stats.Add(E_NUMERIC_STAT_TYPE.MaxHp, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Defence, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.MoveSpeed, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Accecerate, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Tenacity, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Power, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio, default);
        }

        public void SetRefStat(Stat statRef) {
            if(statRef.NumericType == E_NUMERIC_STAT_TYPE.None) throw new System.Exception($"참조 스텟이 초기화되지 않음");

            if(Stats.TryGetValue(statRef.NumericType, out Stat stat)) {
                stat = statRef;
            }

            else {throw new System.Exception($"이 Entity 멤버에는 {statRef.NumericType.ToString()} 없음");}
        }
        
        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType) {
            if(numericType == E_NUMERIC_STAT_TYPE.None) {throw new System.Exception($"NoneStat은 가져올 수 없음");}

            if(Stats.TryGetValue(numericType, out Stat stat)) {
                return stat;
            }
            
            else {throw new System.Exception($"이 Entity 멤버에는 {numericType.ToString()} 없음");}
        }

        public string GetStatsInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach(E_NUMERIC_STAT_TYPE key in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE)))
            {
                stringBuilder.AppendFormat("{0} : {1:N}", key.ToString(), Stats[key]);
            }

            return stringBuilder.ToString();
        }
    }

    public class PlayerStatReferer : EntityStatReferer
    {
        private SortedList<E_NUMERIC_STAT_TYPE, Stat> Stats = new();
        public PlayerStatReferer() {
            Stats.Add(E_NUMERIC_STAT_TYPE.MaxHp,                                    default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Defence,                                  default);
            Stats.Add(E_NUMERIC_STAT_TYPE.MoveSpeed,                                default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Accecerate,                               default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Tenacity,                                 default);
            Stats.Add(E_NUMERIC_STAT_TYPE.MaxStamina,                               default);
            Stats.Add(E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed,                      default);
            Stats.Add(E_NUMERIC_STAT_TYPE.CoolDownSpeed,                            default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Power,                                    default);
            Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio,  default);
            Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio,            default);
            Stats.Add(E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio, default);
            Stats.Add(E_NUMERIC_STAT_TYPE.PoolSize,                                 default);
            Stats.Add(E_NUMERIC_STAT_TYPE.AttackSpeed,                              default);
            Stats.Add(E_NUMERIC_STAT_TYPE.MeleeRatio,                               default);
            Stats.Add(E_NUMERIC_STAT_TYPE.RangerRatio,                              default);
            Stats.Add(E_NUMERIC_STAT_TYPE.TechRatio,                                default);
            Stats.Add(E_NUMERIC_STAT_TYPE.EfficienceMultiplyer,                     default);
            Stats.Add(E_NUMERIC_STAT_TYPE.Luck,                                     default);
        }
    }
}