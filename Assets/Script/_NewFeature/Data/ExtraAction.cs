using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace Feature_NewData
{   
    public enum E_EXTRA_PERFORM_TYPE {
        Start, Tick, Exit
    }
    
    public class ExtraAction<T> {
        private Dictionary<E_EXTRA_PERFORM_TYPE, UnityActionRef<T>> Extras;

        public List<UnityActionRef<T>> startExtras;
        public List<UnityActionRef<T>> tickExtras;
        public List<UnityActionRef<T>> exitExtras;

        private bool isDirty = false;

        public ExtraAction() {
            Extras.Add(E_EXTRA_PERFORM_TYPE.Start, (ref T rVal) => {});
            Extras.Add(E_EXTRA_PERFORM_TYPE.Tick, (ref T rVal) => {});
            Extras.Add(E_EXTRA_PERFORM_TYPE.Exit, (ref T rVal) => {});
        }

        public static implicit operator Dictionary<E_EXTRA_PERFORM_TYPE, UnityActionRef<T>>(ExtraAction<T> extras)
        {
            extras.RecalculateExtras();
            return extras.Extras;
        }

        public void AddCalculator(UnityActionRef<T> refAction, E_EXTRA_PERFORM_TYPE perfomrType) {
            switch(perfomrType) {
                case E_EXTRA_PERFORM_TYPE.Start     :  { startExtras.Add(refAction); break;}
                case E_EXTRA_PERFORM_TYPE.Tick      :  { tickExtras.Add(refAction); break;}
                case E_EXTRA_PERFORM_TYPE.Exit      :  { exitExtras.Add(refAction); break;}
            }
            isDirty = true;
            throw new System.Exception("Ref 형식 엑스트라를 Void에 넣을 수 없음");
        }

        public void RemoveCalculator(UnityActionRef<T> refAction, E_EXTRA_PERFORM_TYPE perfomrType){
                switch(perfomrType) {
                    case E_EXTRA_PERFORM_TYPE.Start :  { startExtras.Remove(refAction); break; }
                    case E_EXTRA_PERFORM_TYPE.Tick :  { tickExtras.Remove(refAction); break; }
                    case E_EXTRA_PERFORM_TYPE.Exit :  { exitExtras.Remove(refAction); break; }
                }
                isDirty = true;
            throw new System.Exception("Void 형식 엑스트라를 Ref에 넣을 수 없음");
        }


        public void ResetCalculators()
        {
            startExtras.Clear();
            tickExtras.Clear();
            exitExtras.Clear();
            isDirty = true;
        }

        public void RecalculateExtras() {
            if(isDirty == false) return;

                Extras[E_EXTRA_PERFORM_TYPE.Start] = (ref T rValue) => {};
                Extras[E_EXTRA_PERFORM_TYPE.Tick] = (ref T rValue) => {};
                Extras[E_EXTRA_PERFORM_TYPE.Exit] = (ref T rValue) => {};

                startExtras.ForEach((extras) =>  { Extras[E_EXTRA_PERFORM_TYPE.Start] += extras; });
                tickExtras.ForEach((extras) =>  { Extras[E_EXTRA_PERFORM_TYPE.Tick] += extras; });
                exitExtras.ForEach((extras) =>  { Extras[E_EXTRA_PERFORM_TYPE.Exit] += extras; });
                
                isDirty = false;            
            return;
        }
    }
}