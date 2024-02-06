using System;
using System.Threading;
using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    using Cysharp.Threading.Tasks;
    using Sophia.DataSystem.Modifiers.Affector;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    public class AffectConveyCommand
    {
        protected E_AFFECT_TYPE AffectType;
        protected SerialAffectorData AffectData;
        protected Entitys.Entity OwnerRef;
        protected Entitys.Entity FixedTarget;
        
        public AffectConveyCommand(Entitys.Entity owner, SerialAffectorData serialAffectorData) {
            this.OwnerRef = owner;
            this.AffectData = serialAffectorData;
        }

        public void SendForEach(ref Entity entity) {
            
        }
        public void SendForSelf() {

        }
    }
}