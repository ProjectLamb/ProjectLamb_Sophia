using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace Sophia.DataSystem
{   
    using Functional;
    using Modifires;
    using Sophia.Entitys;

    public enum E_EXTRAS_PERFORM_TYPE {
        None, Start, Tick, Exit
    }

    public enum E_FUNCTIONAL_EXTRAS_TYPE
    {
        None = 0,
        ENTITY_TYPE = 10,
            Move, Hit, Damaged, Attack, Affected, Dead, Idle, PhyiscTriggered,
        
        PLAYER_TYPE = 20,
            Dash, skill,
        WEAPON_TYPE = 30, 
            WeaponUse, ProjectileRestore,
        
        SKILL_TYPE = 40,
            SkillUse, SkillRefilled,

        PROJECTILE_TYPE = 50,
            Created, Triggerd, Released, Forwarding
    }

    public interface IExtrasAccessable {
        // public Extras GetExtras(E_Functional_EXTRAS_MEMBERS FunctionalType);
        // public string GetExtrasInfo();
    }

    public class FunctionalPerforms<T> {
        public List<UnityActionRef<T>> Start;
        public List<UnityActionRef<T>> Tick;
        public List<UnityActionRef<T>> Exit;

        public FunctionalPerforms() {
            Start = new List<UnityActionRef<T>>();
            Tick = new List<UnityActionRef<T>>();
            Exit = new List<UnityActionRef<T>>();
        }
    }

    public class ExtrasCalculatorPerforms<T> {
        public List<ExtrasCalculator<T>> Start;
        public List<ExtrasCalculator<T>> Tick;
        public List<ExtrasCalculator<T>> Exit;

        public ExtrasCalculatorPerforms() {
            Start = new List<ExtrasCalculator<T>>();
            Tick = new List<ExtrasCalculator<T>>();
            Exit = new List<ExtrasCalculator<T>>();
        }
    }

    public class Extras<T> {
        public readonly UnityActionRef<T> BaseFunctional = (ref T a) => {};
        public readonly E_FUNCTIONAL_EXTRAS_TYPE FunctionalType;

        private readonly FunctionalPerforms<T> FunctionalLists = new FunctionalPerforms<T>();
        private bool isDirty = false;
        
        public readonly UnityEvent OnExtrasChanged;

        private ExtrasCalculatorPerforms<T> ExtrasCalculatorList = new ExtrasCalculatorPerforms<T>();

        public Extras(E_FUNCTIONAL_EXTRAS_TYPE FunctionalType, UnityAction extrasChangeHandler)
        {
            this.FunctionalType = FunctionalType;

            if(extrasChangeHandler != null) {
                OnExtrasChanged = new UnityEvent();
                OnExtrasChanged.AddListener(extrasChangeHandler);
            }
        }
        
        public void PerformStartFunctionals(ref T input){
            foreach(var action in FunctionalLists.Start) {
                action.Invoke(ref input);
            }
        }
        
        public void PerformTickFunctionals(ref T input) {
            foreach(var action in FunctionalLists.Tick) {
                action.Invoke(ref input);
            }
        }

        public void PerformExitFunctionals(ref T input) {
            foreach(var action in  FunctionalLists.Exit) {
                action.Invoke(ref input);
            }
        }

        public void AddCalculator(ExtrasCalculator<T> extrasCalculator) {

            switch(extrasCalculator.PerfType) {
                case E_EXTRAS_PERFORM_TYPE.Start : 
                {
                    ExtrasCalculatorList.Start.Add(extrasCalculator);
                    ExtrasCalculatorList.Start.OrderBy(calc => calc.Order);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Tick : 
                {
                    ExtrasCalculatorList.Tick.Add(extrasCalculator);
                    ExtrasCalculatorList.Tick.OrderBy(calc => calc.Order);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Exit : 
                {
                    ExtrasCalculatorList.Exit.Add(extrasCalculator);
                    ExtrasCalculatorList.Exit.OrderBy(calc => calc.Order);
                    break;
                }
                default : { throw new System.Exception("수행 타입이 None임"); }
            }
            isDirty = true;
        }

        public void RemoveCalculator(ExtrasCalculator<T> extrasCalculator)
        {
            switch(extrasCalculator.PerfType) {
                case E_EXTRAS_PERFORM_TYPE.Start : 
                {
                    ExtrasCalculatorList.Start.Remove(extrasCalculator);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Tick : 
                {
                    ExtrasCalculatorList.Tick.Remove(extrasCalculator);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Exit : 
                {
                    ExtrasCalculatorList.Exit.Remove(extrasCalculator);
                    break;
                }
                default : { throw new System.Exception("수행 타입이 None임"); }
            }
            isDirty = true;
        }

        public void ResetCalculators()
        {
            ExtrasCalculatorList.Start.Clear();
            ExtrasCalculatorList.Tick.Clear();
            ExtrasCalculatorList.Exit.Clear();
            isDirty = true;
        }


        public void RecalculateExtras()
        {
            if(isDirty == false) return;
            ResetCalculators();
            FunctionalLists.Start.Add(BaseFunctional);
            FunctionalLists.Tick.Add(BaseFunctional);
            FunctionalLists.Exit.Add(BaseFunctional);

            ExtrasCalculatorList.Start.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(E_EXTRAS_PERFORM_TYPE.Start)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Start.Add(calc.Functional);
            });
            ExtrasCalculatorList.Tick.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(E_EXTRAS_PERFORM_TYPE.Tick)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Tick.Add(calc.Functional);
            });
            ExtrasCalculatorList.Exit.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(E_EXTRAS_PERFORM_TYPE.Exit)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Exit.Add(calc.Functional);
            });

            isDirty = false;

            OnExtrasChanged.Invoke();
        }
    }    
}