using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Analytics;

namespace Sophia.DataSystem.Modifires
{
    using Sophia.DataSystem.Numerics;
    public class StatCalculator
    {
        public readonly E_STAT_CALC_TYPE CalcType = E_STAT_CALC_TYPE.None;
        public readonly E_NUMERIC_STAT_TYPE StatType = E_NUMERIC_STAT_TYPE.None;
        public readonly float Value;
        public readonly int Order;

        public StatCalculator(float value, E_STAT_CALC_TYPE calType, E_NUMERIC_STAT_TYPE statType)
        {
            Value = value;
            CalcType = calType;
            StatType = statType;
            Order = (int)calType;
        }
    }
}