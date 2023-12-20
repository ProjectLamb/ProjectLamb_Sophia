using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections.Editor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Feature_NewData
{
    public enum E_STAT_USE_TYPE
    {
        Natural, Ratio
    }

    [System.Serializable]
    public struct StatData {
        public float BaseValue;
        public E_STAT_USE_TYPE UseType;
    }

    public class Stat
    {
        public readonly float BaseValue;
        public readonly E_STAT_USE_TYPE UseType;
        private float value;
        private bool isDirty = false;
        public readonly UnityEvent OnStatChanged;

        private readonly List<StatCalculator> statCalculatorList = new();

        public Stat(float baseValue, E_STAT_USE_TYPE UseType, UnityAction statChangedHandler)
        {
            value = BaseValue = baseValue;
            this.UseType = UseType;
            if(statChangedHandler != null) {
                OnStatChanged = new UnityEvent();
                OnStatChanged.AddListener(statChangedHandler);
            }
        }

        public Stat(float baseValue, E_STAT_USE_TYPE useType) : this(baseValue, useType, null){}


        public static implicit operator int(Stat stat)
        {
            if (stat.UseType == E_STAT_USE_TYPE.Natural)
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
        
        // 모든것은 float로 관리하려고 했지만. MaxStamina는 무조건 Int형이 되야 한다.
        public int GetValueByNature() {
            if(UseType.Equals(E_STAT_USE_TYPE.Natural)) {
                RecalculateStat();
                return (int)Math.Clamp(value, 0, int.MaxValue);
            }
            throw new System.Exception("Ratio 형식을 Natural로 리턴 불가.");
        }

        public float GetValueByRatio() {
            if(UseType.Equals(E_STAT_USE_TYPE.Ratio)) {
                RecalculateStat();
                return Math.Clamp(value, 0.0f, 100);
            }
            throw new System.Exception("Natural 형식을 Ratio로 리턴 불가.");
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
            float Adder = 0;
            float Multiplier = 1.0f;

            statCalculatorList.ForEach((calc) => {
                switch(calc.ClacType) {
                    case E_STAT_CALC_TYPE.Add:  {
                        Adder += calc.Value;
                        break;
                    }
                    case E_STAT_CALC_TYPE.Mul:  {
                        Multiplier *= calc.Value;
                        break;
                    }
                }
                value = (value + Adder) * Multiplier;
            });

            if(value <= 1.001f) {value = 1;}
            else {value = (float) Math.Round(value, 3);}
            isDirty = false;

            OnStatChanged.Invoke();
        }
    }
}