using UnityEngine;
using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.DataSystem.Modifiers;
using System;

namespace Sophia.Instantiates.Skills {
    public enum E_SKILL_INDEX {
        None, ProjectileInstantiation, ImpulseForward, AddExcution, AddDamageInfo, AddStunConveyer, Barrier, MoveFaster, PowerUp
    }

    public class FactoryConcreteSkill {
        public static Skill GetSkillByID(E_SKILL_INDEX index, Entitys.Player player, 
            in SerialUserInterfaceData userInterfaceData, 
            in SerialAffectorData affectorData, 
            in SerialOnDamageExtrasModifierDatas damageExtrasModifierData, 
            in SerialOnConveyAffectExtrasModifierDatas conveyAffectExtrasModifierData, 
            in SerialProjectileInstantiateData projectileInstantiateData
        )
        {
                switch (index) {
                case E_SKILL_INDEX.ProjectileInstantiation : {
                    return new ProjectileInstantiationSkill(in userInterfaceData)
                                    .SetInstantiationData(in projectileInstantiateData)
                                    .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.ImpulseForward       : {
                    return new ImpulseForwardSkill(in userInterfaceData)
                                    .SetOwnerEntity(player);

                }
                case E_SKILL_INDEX.AddExcution          : {
                    return new AddExcutionSkill(in userInterfaceData)
                                     .SetExcuteData(in conveyAffectExtrasModifierData)
                                     .SetOwnerEntity(player);

                }
                case E_SKILL_INDEX.AddDamageInfo        : {
                    return new AddDamageInfoSkill(in userInterfaceData)
                                     .SetDamageInfoData(in damageExtrasModifierData)
                                     .SetOwnerEntity(player);

                }
                case E_SKILL_INDEX.AddStunConveyer      : {
                    return new AddStunConveyerSkill(in userInterfaceData)
                                        .SetStunData(affectorData)
                                        .SetOwnerEntity(player);

                }
                case E_SKILL_INDEX.Barrier              : {
                    return new BarrierSkill(in userInterfaceData)
                                        .SetBarrierData(affectorData)
                                        .SetOwnerEntity(player);

                }
                case E_SKILL_INDEX.MoveFaster           : {
                    return new MoveFasterSkill(in userInterfaceData)
                                        .SetMoveFasterAffect(in affectorData)
                                        .SetOwnerEntity(player);

                }
                case E_SKILL_INDEX.PowerUp              : {
                    return new PowerUpSkill(in userInterfaceData)
                                        .SetPowerUpAffect(in affectorData)
                                        .SetOwnerEntity(player);

                }
                default                 : {
                    throw new System.Exception("인덱스가 None 이거나 없는 값을 주입함");
                }
            }
        
        }
    }

    public class ProjectileInstantiationSkill : Skill  {
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        ProjectileBucket instantiatorRef;
        
        SerialProjectileInstantiateData projectileInstantiateData;
        
        public Entitys.Entity ownerEntity;
        public ProjectileInstantiationSkill(in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;
  
            TimerComposite = TimerComposite = new CoolTimeComposite(1f, 1)
                                    .AddBindingAction(Activate);
        }
#region Setter
        public ProjectileInstantiationSkill SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData) {
            projectileInstantiateData = serialProjectileInstantiateData;
            return this;
        }
        
        public ProjectileInstantiationSkill SetOwnerEntity(Entitys.Player player) {
            ownerEntity = player; 
            instantiatorRef = player.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
            return this;
        }

#endregion
#region User Interface
        
        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}


#endregion
        public void Activate() {
            ProjectileObject useProjectile = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
            instantiatorRef.InstantablePositioning(useProjectile)
                            .SetInstantiateType(projectileInstantiateData._InstantiateType)
                            .SetScaleMultiplyByRatio(projectileInstantiateData._ScaleMultiplyByRatio)
                            .SetDurateTimeByRatio(projectileInstantiateData._DurateTimeByRatio)
                            .SetSimulateSpeedByRatio(projectileInstantiateData._SimulateSpeed)
                            .SetForwardingSpeedByRatio(projectileInstantiateData._ForwardingSpeedByRatio)
                            .SetProjectilePower(projectileInstantiateData._ProjectilePower)
                            .SetAffectType(projectileInstantiateData._AffectType)
                            .SetIntervalData(in projectileInstantiateData._intervalData)
                            .Activate();
        }

        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }

    public class ImpulseForwardSkill : Skill {
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        public DataSystem.Atomics.DashAtomics DashAtomics;
        public ImpulseForwardSkill(in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;
            TimerComposite = new CoolTimeComposite(15f, 1)
                                    .AddBindingAction(DashAtomics.Invoke);
        }

#region Setter
        public ImpulseForwardSkill SetOwnerEntity(Entitys.Player entity) {
            DashAtomics = new DataSystem.Atomics.DashAtomics(
                entity.entityRigidbody, 
                entity.GetMovementComposite().GetMovemenCompositetData,
                entity.GetStat(E_NUMERIC_STAT_TYPE.DashForce).GetValueForce
            );
            return this;
        }
#endregion
#region User Interface
        
        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}


#endregion

        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }

    public class AddExcutionSkill : Skill
    {
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        private ExtrasModifier<Entitys.Entity> extrasModifier;
        private IFunctionalCommand<Entitys.Entity> conveyAffectCommand;
        private Entitys.Entity ownerEntity;
        private DataSystem.Extras<Entitys.Entity> extrasRef;

        public AddExcutionSkill(in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            TimerComposite = new CoolTimeComposite(15f, 1)
                .AddBindingAction(Activate);
        }
#region Setter

        public AddExcutionSkill SetExcuteData(in SerialOnConveyAffectExtrasModifierDatas modifierDatas) {
            conveyAffectCommand = new DataSystem.Functional.AtomFunctions.ConveyAffectCommand.FactoryExecuteCommand(modifierDatas._affectData);
            extrasModifier = new ExtrasModifier<Entitys.Entity>(
                conveyAffectCommand, modifierDatas._performType, modifierDatas._extrasType
            );
            return this;
        }
        public AddExcutionSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            extrasRef = ownerEntity.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
            return this;
        }

#endregion

#region User Interface
        
        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}


#endregion

        public async void Activate() {
            extrasRef.AddModifier(extrasModifier);
            extrasRef.RecalculateExtras();
            await UniTask.Delay((int)3 * 1000);
            extrasRef.RemoveModifier(extrasModifier);
            extrasRef.RecalculateExtras();
        }
        
        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }

    public class AddDamageInfoSkill : Skill {
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        public float durateTime = 5f;
        private ExtrasModifier<DamageInfo> extrasModifier;
        private IFunctionalCommand<DamageInfo> damageInfoCommand;

        private Entitys.Entity ownerEntity;          
        private DataSystem.Extras<DamageInfo> extrasRef;
        
        public AddDamageInfoSkill(in SerialUserInterfaceData userInterfaceData) {
            name        = userInterfaceData._name;
            description = userInterfaceData._description;
            icon        = userInterfaceData._icon;

            TimerComposite = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(Activate);
        }

#region Setter

        public AddDamageInfoSkill SetDamageInfoData(in SerialOnDamageExtrasModifierDatas modifierDatas) {
            damageInfoCommand = new DataSystem.Functional.AtomFunctions.CalculateDamageCommand.MulHit(modifierDatas._damageConverterData);
            extrasModifier = new ExtrasModifier<DamageInfo>(
                damageInfoCommand, modifierDatas._performType, modifierDatas._extrasType
            );
            return this;
        }
        public AddDamageInfoSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            extrasRef = ownerEntity.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse);
            return this;
        }

#endregion

#region User Interface
        
        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}

#endregion

        public async void Activate() {
            extrasRef.AddModifier(extrasModifier);
            extrasRef.RecalculateExtras();
            await UniTask.Delay((int)durateTime * 1000);
            extrasRef.RemoveModifier(extrasModifier);
            extrasRef.RecalculateExtras();
        }
        
        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }

    public class AddStunConveyerSkill : Skill {
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        public float durateTime = 5f;
        private ExtrasModifier<Entitys.Entity> extrasModifier;
        private IFunctionalCommand<Entitys.Entity> stunAffectCommand;
        
        private Entitys.Entity ownerEntity;          
        private DataSystem.Extras<Entitys.Entity> extrasRef;
        
        public AddStunConveyerSkill( in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            TimerComposite = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(Activate);
        }

#region Setter

        public AddStunConveyerSkill SetStunData(in SerialAffectorData AffectData) {
            stunAffectCommand = new DataSystem.Functional.AtomFunctions.ConveyAffectCommand.FactoryStunAffectCommand(in AffectData);
            extrasModifier = new ExtrasModifier<Entitys.Entity>(stunAffectCommand, E_EXTRAS_PERFORM_TYPE.Start, E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect);
            return this;
        }
        public AddStunConveyerSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            extrasRef = ownerEntity.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect);
            return this;
        }

#endregion

#region User Interface
        
        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}

#endregion

        public async void Activate()  {
            extrasRef.AddModifier(extrasModifier);
            extrasRef.RecalculateExtras();
            await UniTask.Delay((int)durateTime * 1000);
            extrasRef.RemoveModifier(extrasModifier);
            extrasRef.RecalculateExtras();
        }
        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }

    public class BarrierSkill : Skill
    {
#region Member
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        public float durateTime = 5f;

        private DataSystem.Atomics.BarrierAtomics           barrier;
        private DataSystem.Atomics.VisualFXAtomics          visualFX;
        private DataSystem.Atomics.MaterialChangeAtomics    materialChange;

        private Entitys.Entity ownerEntity;

        public BarrierSkill( in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            TimerComposite = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(Activate);
        }
#endregion

#region Setter

        public BarrierSkill SetBarrierData(in SerialAffectorData affectorData) {
            barrier = new DataSystem.Atomics.BarrierAtomics(affectorData._barrierData._barrierRatio);
            visualFX = new DataSystem.Atomics.VisualFXAtomics(E_AFFECT_TYPE.None, in affectorData._visualData);
            return this;
        }
        public BarrierSkill SetOwnerEntity(Entitys.Entity entity) {
            ownerEntity = entity; 
            return this;
        }

#endregion

#region User Interface
        
        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}

#endregion

        public async void Activate()  {
            barrier?.Invoke(ownerEntity);
            visualFX?.Invoke(ownerEntity);
            materialChange?.Invoke(ownerEntity);
            await UniTask.Delay((int)durateTime * 1000);
            barrier?.Revert(ownerEntity);
            visualFX?.Revert(ownerEntity);
            materialChange?.Revert(ownerEntity);
        }

        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }
    
    public class MoveFasterSkill : Skill
    {

#region Member
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        public float durateTime = 5f;
        private DataSystem.Modifiers.ConcreteAffector.MoveFasterAffect moveFasterAffect;
        private Entitys.Entity ownerEntity;

        public MoveFasterSkill( in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            TimerComposite = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(Activate);
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
        
        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}

#endregion
        public void Activate() {
            ownerEntity.Affect(moveFasterAffect);
        }

        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }

    public class PowerUpSkill : Skill {
#region Member
        private string name;
        private string description;
        private Sprite icon;
        public CoolTimeComposite TimerComposite {get; private set;}
        private DataSystem.Modifiers.ConcreteAffector.PowerUpAffect powerUpAffect;
        private Entitys.Entity ownerEntity;

        public PowerUpSkill(in SerialUserInterfaceData userInterfaceData) {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;

            TimerComposite = new CoolTimeComposite(15f, 1)
                            .AddBindingAction(Activate);
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

        public override string GetName()         => name;
        public override string GetDescription()  => description;
        public override Sprite GetSprite()       => icon;

#endregion

#region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdator() {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator() {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() {return;}
        public override void FrameTick() {TimerComposite.TickRunning();}
        public override void PhysicsTick() {return;}

#endregion
        public void Activate() {
            ownerEntity.Affect(powerUpAffect);
        }

        public override void Use() {
            if(!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }
}