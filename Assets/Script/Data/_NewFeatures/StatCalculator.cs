using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Analytics;

namespace Feature_NewData
{
    public enum E_STAT_CALC_TYPE
    {
        MulBefore_1 = 1, Add_2, MulAfter_3
    }

    public class StatCalculator
    {
        public readonly float Value;
        public readonly E_STAT_CALC_TYPE ClacType;
        public readonly int Order;

        public StatCalculator(float value, E_STAT_CALC_TYPE Type)
        {
            Value = value;
            ClacType = Type;
            Order = (int)Type;
        }
    }
}