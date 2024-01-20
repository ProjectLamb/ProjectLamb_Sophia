using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using TMPro;

namespace Sophia.Composite{
    using Sophia.DataSystem.Affector;
    public class AffectorComposite : IUpdatorBindable {
        private Dictionary<E_AFFECT_TYPE, Affector>   AffectorStacks = new Dictionary<E_AFFECT_TYPE, Affector>();
        private UnityAction<E_AFFECT_TYPE>            OnDestroyHandler;

        public AffectorComposite() {
            foreach(E_AFFECT_TYPE E in Enum.GetValues(typeof(E_AFFECT_TYPE))){ AffectorStacks.Add(E, null); }
            OnDestroyHandler = (E_AFFECT_TYPE type) => AffectorStacks[type] = null;
        }

        public void HandleModifiy(Affector affector) {
            E_AFFECT_TYPE stateType = affector.AffectType;
            if(!AffectorStacks.ContainsKey(stateType)) {
                throw new System.Exception("현재 받아온 어펙터는 타입이 존재하지 않음");
            }
            if(AffectorStacks.TryGetValue(stateType, out Affector runnginAffector)) {
                if(runnginAffector != null){
                    runnginAffector.Revert();
                    OnDestroyHandler.Invoke(stateType);
                }
            }
            AffectorStacks[stateType] = affector;
        }

#region  Updator Implements
        bool IsUpdatorBinded = false;
        public bool GetUpdatorBind() => IsUpdatorBinded;
        
        public void AddToUpator()
        {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }

        public void RemoveFromUpdator()
        {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public void LateTick() {
            return;
        }

        public void FrameTick() {
            foreach(var affector in AffectorStacks) {
                affector.Value?.TickRunning();
            }
        }

        public void PhysicsTick()
        {
            return;
        }
    }
#endregion
}