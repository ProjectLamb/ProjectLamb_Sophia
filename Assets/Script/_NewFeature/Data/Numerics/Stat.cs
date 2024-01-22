using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;


namespace Sophia.DataSystem
{
    using Numerics;
    using Modifiers;

    public class Stat
    {
        public readonly float BaseValue;
        public readonly E_STAT_USE_TYPE UseType;
        public readonly E_NUMERIC_STAT_TYPE NumericType;
        private float value;
        private bool isDirty = false;
        public readonly UnityEvent OnStatChanged;

        private readonly List<StatModifier> statModifierList = new();

        public Stat(float baseValue, E_NUMERIC_STAT_TYPE NumericType, E_STAT_USE_TYPE UseType, UnityAction statChangedHandler)
        {
            value = BaseValue = baseValue;
            this.NumericType = NumericType;
            this.UseType = UseType;
            if(statChangedHandler != null) {
                OnStatChanged = new UnityEvent();
                OnStatChanged.AddListener(statChangedHandler);
            }
        }

        public Stat(float baseValue, E_NUMERIC_STAT_TYPE numericType, E_STAT_USE_TYPE useType) : this(baseValue, numericType,useType, null){}

#region Getter

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
            if (stat.UseType != E_STAT_USE_TYPE.Natural )
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

        public float GetValueForce() {
            RecalculateStat();
            return value;
        }

#endregion

        public void AddModifier(StatModifier StatModifier)
        {
            statModifierList.Add(StatModifier);
            statModifierList.OrderBy(calc => calc.Order);
            isDirty = true;
        }

        public void RemoveModifier(StatModifier StatModifier)
        {
            statModifierList.Remove(StatModifier);
            isDirty = true;
        }

        public void ResetModifiers()
        {
            statModifierList.Clear();
            isDirty = true;
        }

        public void RecalculateStat()
        {
            if(isDirty == false) return;

            value = BaseValue;
            float Adder = 0;
            float Multiplier = 1.0f;

            statModifierList.ForEach((calc) => {
                if(!calc.StatType.Equals(this.NumericType)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.StatType.ToString()} != {this.NumericType.ToString()}");
                CalculateWithUseAndCalcType(this.UseType, calc, ref Adder, ref Multiplier);
            });
            if(Multiplier <= 0) {Multiplier = 0;}
            value = value + Adder;
            value = value * Multiplier;

            if(value <= 0) {value = 0;}
            else {value = (float) Math.Round(value, 3);}
            isDirty = false;

            OnStatChanged.Invoke();
        }

        public void CalculateWithUseAndCalcType(E_STAT_USE_TYPE useType, StatModifier calc, ref float adder, ref float multiplier) {
                switch(useType) {
                    case E_STAT_USE_TYPE.Natural:  {
                        if(calc.CalcType == E_STAT_CALC_TYPE.Add)        {adder += calc.Value; return;}
                        else if(calc.CalcType == E_STAT_CALC_TYPE.Mul)   {multiplier += calc.Value; return;}
                        break;
                    }
                    case E_STAT_USE_TYPE.Ratio:  {
                        if(calc.CalcType == E_STAT_CALC_TYPE.Add)        {adder += calc.Value; return;}
                        else if(calc.CalcType == E_STAT_CALC_TYPE.Mul)   {multiplier += calc.Value; return;}
                        break;
                    }
                    case E_STAT_USE_TYPE.Percentage:  {
                        if(calc.CalcType == E_STAT_CALC_TYPE.Add)        {adder += calc.Value; return;}
                        else if(calc.CalcType == E_STAT_CALC_TYPE.Mul)   {multiplier += calc.Value; return;}
                        break;
                    }
                }
        }
    }
}