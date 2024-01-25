using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using TMPro;

namespace Sophia.Composite{
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers.Affector;
    public class AffectorStackRemoveCommend {
        public readonly SortedSet<E_AFFECT_TYPE> removeCommands = new ();
        Dictionary<E_AFFECT_TYPE, Affector> AffectorStacksRef;

        public AffectorStackRemoveCommend(Dictionary<E_AFFECT_TYPE, Affector> affectorStacks) {
            AffectorStacksRef = affectorStacks;
        }

        public void RemoveAffectorStack() {
            if(removeCommands.Count == 0) {return;}
            foreach(var E in removeCommands) {
                AffectorStacksRef[E] = default;
            }
            removeCommands.Clear();
        }
    }

    public class AffectorHandlerComposite : IUpdatorBindable {
        public Stat Tenacity {get; private set;}
        private Dictionary<E_AFFECT_TYPE, Affector>   AffectorStacks = new Dictionary<E_AFFECT_TYPE, Affector>();
        private AffectorStackRemoveCommend affectorStackRemoveCommend;

        public AffectorHandlerComposite(float baseTenacity) {
            Tenacity = new Stat(baseTenacity,
                E_NUMERIC_STAT_TYPE.Tenacity,
                E_STAT_USE_TYPE.Ratio,
                OnTenacityUpdated
            );
            foreach(E_AFFECT_TYPE E in Enum.GetValues(typeof(E_AFFECT_TYPE))){ AffectorStacks.Add(E, default); }
            affectorStackRemoveCommend = new AffectorStackRemoveCommend(AffectorStacks);

            AddToUpator();
        }

        private void OnTenacityUpdated() {
            Debug.Log("TenacityUpdated");
        }

        public void ModifiyByAffector(Affector affector) {
            E_AFFECT_TYPE stateType = affector.AffectType;
            if(!AffectorStacks.ContainsKey(stateType)) {
                throw new System.Exception("현재 받아온 어펙터는 타입이 존재하지 않음");
            }
            if(AffectorStacks.TryGetValue(stateType, out Affector runnginAffector)) {
                if(runnginAffector != null){
                    runnginAffector.CancleModify();
                    AffectorStacks[stateType] = default;
                }
            }
            AffectorStacks[stateType] = affector;
            AffectorStacks[stateType].SetAccelarationByTenacity(this.Tenacity);
            AffectorStacks[stateType].OnRevert += () => affectorStackRemoveCommend.removeCommands.Add(stateType);
            AffectorStacks[stateType].Modifiy(Tenacity);
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
                // if(affector.Equals(default)) continue;
                affector.Value?.TickRunning();
            }
            affectorStackRemoveCommend.RemoveAffectorStack();
        }

        public void PhysicsTick()
        {
            return;
        }
    }
#endregion
}