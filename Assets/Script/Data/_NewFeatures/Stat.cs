using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections.Editor;

namespace Feature_NewData
{
    public enum E_STAT_USE_TYPE
    {
        Fixed, Ratio
    }

    public class Stat
    {
        public readonly float BaseValue;
        public readonly E_STAT_USE_TYPE UseType;
        private float value;
        private bool isDirty = false;

        private readonly List<StatCalculator> statCalculatorList = new();

        public Stat(float baseValue, E_STAT_USE_TYPE UseType)
        {
            value = BaseValue = baseValue;
            this.UseType = UseType;
        }

        public static implicit operator int(Stat stat)
        {
            if (stat.UseType == E_STAT_USE_TYPE.Fixed)
            {
                stat.RecalculateStat();
                return (int)stat.value;
            }
            throw new System.Exception("Ratio 형식을 int로 리턴 불가.");
        }

        public static implicit operator float(Stat stat)
        {
            if (stat.UseType == E_STAT_USE_TYPE.Ratio)
            {
                stat.RecalculateStat();
                return stat.value;
            }
            throw new System.Exception("Value 형식을 float로 리턴 불가.");
        }

        public void AddCalculator(StatCalculator StatCalculator)
        {
            statCalculatorList.Add(StatCalculator);
            statCalculatorList.OrderBy(calc => calc.Order);
            isDirty = true;
        }

        public void RemoveCalculator(StatCalculator StatCalculator)
        {
            statCalculatorList.Remove(StatCalculator);
            isDirty = true;
        }

        public void ResetCalculators()
        {
            statCalculatorList.Clear();
            isDirty = true;
        }

        public void RecalculateStat()
        {
            if(isDirty == false) return;

            value = BaseValue;
            statCalculatorList.ForEach((calc) =>
            {
                if (calc.ClacType == E_STAT_CALC_TYPE.Add_2)
                {
                    this.value += calc.Value;
                }
                else { this.value *= value; }
            }
            );

            isDirty = false;
        }
    }
}