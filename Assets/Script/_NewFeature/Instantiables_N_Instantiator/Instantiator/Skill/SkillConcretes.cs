using UnityEngine;
using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.DataSystem.Modifiers;
using System.Xml.Serialization;
using System;

namespace Sophia.Instantiates.Skills {
    public abstract class SkillAbstractConcrete : IUserInterfaceAccessible, IUpdatorBindable{
        protected string name;
        protected string description;
        protected Sprite icon;
        public CoolTimeComposite TimerComposite {get; protected set;}
        public Entitys.Entity ownerEntity {get; protected set;}
        
        protected SerialProjectileInstantiateData projectileInstantiateData;
        
        public SkillAbstractConcrete(in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;
        }
#region User Interface
        
        public string GetName()         => name;
        public string GetDescription()  => description;
        public Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public bool GetUpdatorBind() => IsUpdatorBinded;

        public void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public void LateTick() {return;}
        public void FrameTick() {TimerComposite.TickRunning();}
        public void PhysicsTick() {return;}


#endregion
        protected void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }
}