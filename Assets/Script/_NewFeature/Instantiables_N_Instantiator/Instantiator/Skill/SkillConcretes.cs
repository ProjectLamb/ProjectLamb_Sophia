using UnityEngine;
using Cysharp.Threading.Tasks;
using Sophia.Composite;
using System.Threading;
using Sophia.DataSystem.Modifiers;

namespace Sophia.Instantiates.Skills {
    
    [System.Serializable]
    public struct SerialUserInterfaceData {
        [SerializeField] public string _name;
        [SerializeField] public string _description;
        [SerializeField] public Sprite _icon;
    }

    public class AddStunConveyerSkill : IUserInterfaceAccessible, IUpdatorBindable {
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite timer {get; private set;}
        public float durateTime = 5f;
        private ExtrasModifier<Entitys.Entity> extrasModifier;
        private IFunctionalCommand<Entitys.Entity> StunAffectCommand;
        
        private Entitys.Entity ownerEntity;          
        private DataSystem.Extras<Entitys.Entity> extrasRef;
        
        public AddStunConveyerSkill( in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            timer = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(ActivateStunConveyer);
        }

#region Setter

        public AddStunConveyerSkill SetStunData(in SerialAffectorData AffectData) {
            StunAffectCommand = new DataSystem.Functional.AtomFunctions.ConveyAffectCommand.FactoryStunAffectCommand(in AffectData);
            extrasModifier = new ExtrasModifier<Entitys.Entity>(StunAffectCommand, E_EXTRAS_PERFORM_TYPE.Start, E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect);
            return this;
        }
        public AddStunConveyerSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            extrasRef = ownerEntity.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect);
            return this;
        }

#endregion

#region User Interface
        
        public string GetName()         => name;
        public string GetDescription()  => description;
        public Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public bool GetUpdatorBind() => IsUpdatorBinded;

        public void AddToUpator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public void LateTick() {return;}
        public void FrameTick() {timer.TickRunning();}
        public void PhysicsTick() {return;}

#endregion
        public async void ActivateStunConveyer()  {
            extrasRef.AddModifier(extrasModifier);
            extrasRef.RecalculateExtras();
            await UniTask.Delay((int)durateTime * 1000);
            extrasRef.RemoveModifier(extrasModifier);
            extrasRef.RecalculateExtras();
        }
        public void Use() {
            if(!timer.GetIsReadyToUse()) return;
            timer.ActionStart();
        }
    }

    public class BarrierSkill : IUserInterfaceAccessible, IUpdatorBindable
    {
#region Member
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite timer {get; private set;}
        public float durateTime = 5f;

        private DataSystem.Atomics.BarrierAtomics           barrier;
        private DataSystem.Atomics.VisualFXAtomics          visualFX;
        private DataSystem.Atomics.MaterialChangeAtomics    materialChange;

        private Entitys.Entity ownerEntity;

        public BarrierSkill( in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            timer = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(ActivateBarrier);
        }
#endregion

#region Setter

        public BarrierSkill SetBarrierData(in SerialBarrierData serialBarrierData) {
            barrier = new DataSystem.Atomics.BarrierAtomics(serialBarrierData._barrierRatio);
            return this;
        }
        public BarrierSkill SetVisualFxData(in SerialVisualData serialVisualData) {
            visualFX = new DataSystem.Atomics.VisualFXAtomics(E_AFFECT_TYPE.None, in serialVisualData);
            return this;
        }
        public BarrierSkill SetMaterialData(in SerialSkinData serialSkinData) {
            materialChange = new DataSystem.Atomics.MaterialChangeAtomics(in serialSkinData);
            return this;
        }
        public BarrierSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            return this;
        }

#endregion

#region User Interface
        
        public string GetName()         => name;
        public string GetDescription()  => description;
        public Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public bool GetUpdatorBind() => IsUpdatorBinded;

        public void AddToUpator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public void LateTick() {return;}
        public void FrameTick() {timer.TickRunning();}
        public void PhysicsTick() {return;}

#endregion

        public async void ActivateBarrier()  {
            barrier?.Invoke(ownerEntity);
            visualFX?.Invoke(ownerEntity);
            materialChange?.Invoke(ownerEntity);
            await UniTask.Delay((int)durateTime * 1000);
            barrier?.Revert(ownerEntity);
            visualFX?.Revert(ownerEntity);
            materialChange?.Revert(ownerEntity);
        }

        public void Use() {
            if(!timer.GetIsReadyToUse()) return;
            timer.ActionStart();
        }
    }
    
    public class MoveFasterSkill : IUserInterfaceAccessible, IUpdatorBindable
    {

#region Member
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite timer {get; private set;}
        public float durateTime = 5f;
        private DataSystem.Modifiers.ConcreteAffector.MoveFasterAffect moveFasterAffect;
        private Entitys.Entity ownerEntity;

        public MoveFasterSkill( in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            timer = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(ActivateMoveFaster);
        }
#endregion

#region Setter

        public MoveFasterSkill SetMoveFasterAffect(in SerialAffectorData affectorData) {
            moveFasterAffect = new DataSystem.Modifiers.ConcreteAffector.MoveFasterAffect(affectorData);
            return this;
        }
        public MoveFasterSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            return this;
        }


#endregion

#region User Interface
        
        public string GetName()         => name;
        public string GetDescription()  => description;
        public Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public bool GetUpdatorBind() => IsUpdatorBinded;

        public void AddToUpator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public void LateTick() {return;}
        public void FrameTick() {timer.TickRunning();}
        public void PhysicsTick() {return;}

#endregion
        public void ActivateMoveFaster() {
            ownerEntity.Affect(moveFasterAffect);
        }

        public void Use() {
            if(!timer.GetIsReadyToUse()) return;
            timer.ActionStart();
        }
    }

    public class PowerUpSkill : IUserInterfaceAccessible, IUpdatorBindable {
#region Member
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite timer {get; private set;}
        private DataSystem.Modifiers.ConcreteAffector.PowerUpAffect powerUpAffect;
        private Entitys.Entity ownerEntity;

        public PowerUpSkill(in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            timer = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(ActivatePowerUp);
        }

#endregion
#region Setter
        public PowerUpSkill SetPowerUpAffect(in SerialAffectorData affectorData) {
            powerUpAffect = new DataSystem.Modifiers.ConcreteAffector.PowerUpAffect(affectorData);
            return this;
        }
        public PowerUpSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            return this;
        }

#endregion
#region User Interface

        public string GetName()         => name;
        public string GetDescription()  => description;
        public Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public bool GetUpdatorBind() => IsUpdatorBinded;

        public void AddToUpator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public void LateTick() {return;}
        public void FrameTick() {timer.TickRunning();}
        public void PhysicsTick() {return;}

#endregion
        public void ActivatePowerUp() {
            ownerEntity.Affect(powerUpAffect);
        }

        public void Use() {
            if(!timer.GetIsReadyToUse()) return;
            timer.ActionStart();
        }
    }
}