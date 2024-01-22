using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace Sophia.DataSystem
{   
    using Functional;
    using Modifiers;
    using Sophia.Entitys;

    public enum E_EXTRAS_PERFORM_TYPE {
        None, Start, Tick, Exit
    }

    public enum E_FUNCTIONAL_EXTRAS_TYPE
    {
        None = 0,
        ENTITY_TYPE = 10,
            Move, Hit, Damaged, Attack, OwnerAffected, TargetAffected, Dead, Idle, PhyiscTriggered,
        
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

        public void InvokeLists(ref T value) {
            foreach (var item in Start)
            {
                item.Invoke(ref value);
            }
            foreach (var item in Tick)
            {
                item.Invoke(ref value);
            }
            foreach (var item in Exit)
            {
                item.Invoke(ref value);
            }
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
        public readonly UnityActionRef<T> BaseFunctional = (ref T a) => {};
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
            foreach(var action in FunctionalLists.Start) {
                action?.Invoke(ref input);
            }
        }
        
        public void PerformTickFunctionals(ref T input) {
            foreach(var action in FunctionalLists.Tick) {
                action?.Invoke(ref input);
            }
        }

        public void PerformExitFunctionals(ref T input) {
            foreach(var action in  FunctionalLists.Exit) {
                action?.Invoke(ref input);
            }
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

        public void ResetFunctionals()
        {
            FunctionalLists.Start.Clear();
            FunctionalLists.Tick.Clear();
            FunctionalLists.Exit.Clear();
        }


        public void RecalculateExtras()
        {
            if(isDirty == false) return;
            ResetFunctionals();
            FunctionalLists.Start.Add(BaseFunctional);
            FunctionalLists.Tick.Add(BaseFunctional);
            FunctionalLists.Exit.Add(BaseFunctional);

            ExtrasModifierList.Start.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(this.FunctionalType)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Start.Add(calc.Functional);
            });
            ExtrasModifierList.Tick.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(this.FunctionalType)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Tick.Add(calc.Functional);
            });
            ExtrasModifierList.Exit.ForEach((calc) => {
                if(!calc.FunctionalType.Equals(this.FunctionalType)) throw new System.Exception($"칼큘레이터 타겟 타입과 현재 타겟 타입이 다르다! {calc.FunctionalType.ToString()} != {this.FunctionalType.ToString()}");
                FunctionalLists.Exit.Add(calc.Functional);
            });

            isDirty = false;

            OnExtrasChanged.Invoke();
        }
    }    
}