using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Analytics;

namespace Sophia.DataSystem.Modifiers
{
    [System.Serializable]
    public struct SerialStatModifireDatas {
        public float amount;
        public E_STAT_CALC_TYPE calType;
    }

    public class StatModifier
    {
        public readonly E_STAT_CALC_TYPE CalcType = E_STAT_CALC_TYPE.None;
        public readonly E_NUMERIC_STAT_TYPE StatType = E_NUMERIC_STAT_TYPE.None;
        public readonly float Value;
        public readonly int Order;

        public StatModifier(float value, E_STAT_CALC_TYPE calType, E_NUMERIC_STAT_TYPE statType)
        {
            Value = value;
            CalcType = calType;
            StatType = statType;
            Order = (int)calType;
        }
    }
}