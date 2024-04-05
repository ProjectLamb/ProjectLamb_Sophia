using System;
using System.Collections.Generic;
using System.Text;
using Sophia.DataSystem.Modifiers;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.DataSystem.Referer
{
    public class EntityStatReferer : IStatAccessible
    {
        protected SortedList<E_NUMERIC_STAT_TYPE, Stat> Stats = new();
        protected StringBuilder stringBuilder = new StringBuilder();

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
            stringBuilder.Clear();
            foreach(var stat in Stats) {
                if(stat.Value != null) stringBuilder.AppendLine(stat.Key.ToString() + " : " + stat.Value.GetValueForce());
            }
            return stringBuilder.ToString();
        }

        public void AddListenerToStats(UnityAction unityAction) {
            foreach(var stat in Stats) {
                if(stat.Value != null) stat.Value.OnStatChanged.AddListener(unityAction);
            }
        }
        public void RemoveListenerToStats(UnityAction unityAction) {
            foreach(var stat in Stats) {
                if(stat.Value != null) stat.Value.OnStatChanged.RemoveListener(unityAction);
            }
        }
    }

    public class PlayerStatReferer : EntityStatReferer
    {
        public PlayerStatReferer() : base() {
            this.Stats.Add(E_NUMERIC_STAT_TYPE.MaxStamina,                               null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed,                      null);
            this.Stats.Add(E_NUMERIC_STAT_TYPE.DashForce,                                null);
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