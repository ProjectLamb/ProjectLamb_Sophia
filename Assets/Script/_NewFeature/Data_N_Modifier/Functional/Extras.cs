using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace Sophia.DataSystem
{   
    using Modifiers;

    public class FunctionalPerforms<T> {
        public List<IFunctionalCommand<T>> Start;
        public List<IFunctionalCommand<T>> Tick;
        public List<IFunctionalCommand<T>> Exit;

        public FunctionalPerforms() {
            Start = new List<IFunctionalCommand<T>>();
            Tick = new List<IFunctionalCommand<T>>();
            Exit = new List<IFunctionalCommand<T>>();
        }

        public void InvokeStartTime(ref T value) {
            foreach (var command in Start)   command.Invoke(ref value);
        }
        
        public void InvokeTickTime(ref T value){
            foreach (var command in Tick)    command.Invoke(ref value);
        }

        public void InvokeExitTime(ref T value){
            foreach (var command in Exit)    command.Invoke(ref value);
        }
    }

    public class ExtrasModifierPerforms<T> {
        public List<ExtrasModifier<T>> Start;
        public List<ExtrasModifier<T>> Tick;
        public List<ExtrasModifier<T>> Exit;

        public ExtrasModifierPerforms() {
            Start = new List<ExtrasModifier<T>>();
            Tick = new List<ExtrasModifier<T>>();
            Exit = new List<ExtrasModifier<T>>();
        }
    }

    public class Extras<T> {
        public readonly E_FUNCTIONAL_EXTRAS_TYPE FunctionalType;
        private readonly FunctionalPerforms<T> FunctionalLists = new FunctionalPerforms<T>();

        public static implicit operator FunctionalPerforms<T> (Extras<T> extras) {
            extras.RecalculateExtras();
            return extras.FunctionalLists;
        }

        private bool isDirty = false;
        
        public readonly UnityEvent OnExtrasChanged;

        private ExtrasModifierPerforms<T> ExtrasModifierList = new ExtrasModifierPerforms<T>();

        public Extras(E_FUNCTIONAL_EXTRAS_TYPE FunctionalType, UnityAction extrasChangeHandler)
        {
            this.FunctionalType = FunctionalType;

            if(extrasChangeHandler != null) {
                OnExtrasChanged = new UnityEvent();
                OnExtrasChanged.AddListener(extrasChangeHandler);
            }
        }

        public void PerformStartFunctionals(ref T input){
            if(isDirty) {RecalculateExtras();}
            FunctionalLists.InvokeStartTime(ref input);
        }
        
        public void PerformTickFunctionals(ref T input) {
            if(isDirty) {RecalculateExtras();}
            FunctionalLists.InvokeTickTime(ref input);
        }

        public void PerformExitFunctionals(ref T input) {
            if(isDirty) {RecalculateExtras();}
            FunctionalLists.InvokeExitTime(ref input);
        }

        public void AddModifier(ExtrasModifier<T> extrasModifier) {

            switch(extrasModifier.PerfType) {
                case E_EXTRAS_PERFORM_TYPE.Start : 
                {
                    ExtrasModifierList.Start.Add(extrasModifier);
                    ExtrasModifierList.Start.OrderBy(calc => calc.Order);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Tick : 
                {
                    ExtrasModifierList.Tick.Add(extrasModifier);
                    ExtrasModifierList.Tick.OrderBy(calc => calc.Order);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Exit : 
                {
                    ExtrasModifierList.Exit.Add(extrasModifier);
                    ExtrasModifierList.Exit.OrderBy(calc => calc.Order);
                    break;
                }
                default : { throw new System.Exception("수행 타입이 None임"); }
            }
            isDirty = true;
        }

        public void RemoveModifier(ExtrasModifier<T> extrasModifier)
        {
            switch(extrasModifier.PerfType) {
                case E_EXTRAS_PERFORM_TYPE.Start : 
                {
                    ExtrasModifierList.Start.Remove(extrasModifier);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Tick : 
                {
                    ExtrasModifierList.Tick.Remove(extrasModifier);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Exit : 
                {
                    ExtrasModifierList.Exit.Remove(extrasModifier);
                    break;
                }
                default : { throw new System.Exception("수행 타입이 None임"); }
            }
            isDirty = true;
        }

        void ResetFunctionals()
        {
            FunctionalLists.Start.Clear();
            FunctionalLists.Tick.Clear();
            FunctionalLists.Exit.Clear();
        }

        public void RecalculateExtras()
        {
            if(isDirty == false) return;
            ResetFunctionals();

            ExtrasModifierList.Start.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(this.FunctionalType)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Start.Add(calc.Value);
            });
            ExtrasModifierList.Tick.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(this.FunctionalType)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Tick.Add(calc.Value);
            });
            ExtrasModifierList.Exit.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(this.FunctionalType)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Exit.Add(calc.Value);
            });

            isDirty = false;

            OnExtrasChanged?.Invoke();
        }
    }
}