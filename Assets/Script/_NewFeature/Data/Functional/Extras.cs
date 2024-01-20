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
            Move, Hit, Damaged, Attack, Affected, Dead, Standing, PhyiscTriggered,
        
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

    public class Extras<T> {
        public readonly UnityActionRef<T> BaseFunctional = (ref T a) => {};
        public readonly E_FUNCTIONAL_EXTRAS_TYPE FunctionalType;

        private List<UnityActionRef<T>> StartFunctionalLists;
        private List<UnityActionRef<T>> TickFunctionalLists;
        private List<UnityActionRef<T>> ExitFunctionalLists;
        private bool isDirty = false;
        
        public readonly UnityEvent OnExtrasChanged;

        private readonly List<ExtrasCalculator<T>> StartExtrasCalculatorList = new List<ExtrasCalculator<T>>();
        private readonly List<ExtrasCalculator<T>> TickExtrasCalculatorList = new List<ExtrasCalculator<T>>();
        private readonly List<ExtrasCalculator<T>> ExitExtrasCalculatorList = new List<ExtrasCalculator<T>>();

        public Extras(E_FUNCTIONAL_EXTRAS_TYPE FunctionalType, UnityAction extrasChangeHandler)
        {
            this.FunctionalType = FunctionalType;
            if(extrasChangeHandler != null) {
                OnExtrasChanged = new UnityEvent();
                OnExtrasChanged.AddListener(extrasChangeHandler);
            }
        }
        
        public void PerformStartFunctionals(ref T input){
            foreach(var action in  StartFunctionalLists) {
                action.Invoke(ref input);
            }
        }
        
        public void PerformTickFunctionals(ref T input) {
            foreach(var action in  TickFunctionalLists) {
                action.Invoke(ref input);
            }
        }

        public void PerformExitFunctionals(ref T input) {
            foreach(var action in  ExitFunctionalLists) {
                action.Invoke(ref input);
            }
        }

        public void AddCalculator(ExtrasCalculator<T> extrasCalculator) {

            switch(extrasCalculator.PerfType) {
                case E_EXTRAS_PERFORM_TYPE.Start : 
                {
                    StartExtrasCalculatorList.Add(extrasCalculator);
                    StartExtrasCalculatorList.OrderBy(calc => calc.Order);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Tick : 
                {
                    TickExtrasCalculatorList.Add(extrasCalculator);
                    TickExtrasCalculatorList.OrderBy(calc => calc.Order);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Exit : 
                {
                    ExitExtrasCalculatorList.Add(extrasCalculator);
                    ExitExtrasCalculatorList.OrderBy(calc => calc.Order);
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
                    StartExtrasCalculatorList.Remove(extrasCalculator);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Tick : 
                {
                    TickExtrasCalculatorList.Remove(extrasCalculator);
                    break;
                }
                case E_EXTRAS_PERFORM_TYPE.Exit : 
                {
                    ExitExtrasCalculatorList.Remove(extrasCalculator);
                    break;
                }
                default : { throw new System.Exception("수행 타입이 None임"); }
            }
            isDirty = true;
        }

        public void ResetCalculators()
        {
            StartExtrasCalculatorList.Clear();
            TickExtrasCalculatorList.Clear();
            ExitExtrasCalculatorList.Clear();
            isDirty = true;
        }


        public void RecalculateExtras()
        {
            if(isDirty == false) return;
            ResetCalculators();
            StartFunctionalLists.Add(BaseFunctional);
            TickFunctionalLists.Add(BaseFunctional);
            ExitFunctionalLists.Add(BaseFunctional);

            StartExtrasCalculatorList.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(E_EXTRAS_PERFORM_TYPE.Start)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                StartFunctionalLists.Add(calc.Functional);
            });
            TickExtrasCalculatorList.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(E_EXTRAS_PERFORM_TYPE.Tick)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                TickFunctionalLists.Add(calc.Functional);
            });
            ExitExtrasCalculatorList.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(E_EXTRAS_PERFORM_TYPE.Exit)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                ExitFunctionalLists.Add(calc.Functional);
            });

            isDirty = false;

            OnExtrasChanged.Invoke();
        }

         /***
        이거.. 내가 하면서도 제대로 하는건지 모르겠네
        제네릭 타입을 런타임에 정해줘서 반환이 가능하다는건가?? 
        이게 가능하다고? 흠..
        */

        public static implicit operator Extras<T>(Extras<Vector3> v)
        {
            if(typeof(T) == typeof(Vector3)) {
                v.RecalculateExtras();
                return v;
            }
            throw new System.Exception("벡터타입이 아님");
        }

        public static implicit operator Extras<T>(Extras<float> v)
        {
            if(typeof(T) == typeof(float)) {
                v.RecalculateExtras();
                return v;
            }
            throw new System.Exception("float 타입이 아님");
        }

        public static implicit operator Extras<T>(Extras<object> v)
        {
            if(typeof(T) == typeof(object)) {
                v.RecalculateExtras();
                return v;
            }
            throw new System.Exception("벡터타입이 아님");
        }

        public static implicit operator Extras<T>(Extras<Entity> v)
        {
            if(typeof(T) == typeof(Entity)) {
                v.RecalculateExtras();
                return v;
            }
            throw new System.Exception("엔티티 타입이 아님");
        }
    }    
}