using UnityEngine;
using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.DataSystem.Modifiers;
using System.Xml.Serialization;
using System;

namespace Sophia.Instantiates.Skills
{
    public abstract class SkillAbstractConcrete : Skill
    {
        protected string name;
        protected string description;
        protected Sprite icon;
        public CoolTimeComposite TimerComposite { get; protected set; }
        public override CoolTimeComposite GetCoolTimeComposite() => this.TimerComposite;
        public Entitys.Player ownerEntity { get; protected set; }

        public SkillAbstractConcrete(in SerialUserInterfaceData userInterfaceData)
        {
            name = userInterfaceData._name;
            description = userInterfaceData._description;
            icon = userInterfaceData._icon;
        }

        #region User Interface

        public override string GetName() => name;
        public override string GetDescription() => description;
        public override Sprite GetSprite() => icon;

        #endregion

        #region NonMonobehaviour Update

        bool IsUpdatorBinded = false;
        public override bool GetUpdatorBind() => IsUpdatorBinded;

        public override void AddToUpdater()
        {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }
        public override void RemoveFromUpdator()
        {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        public override void LateTick() { return; }
        public override void FrameTick() { TimerComposite.TickRunning(); }
        public override void PhysicsTick() { return; }


        #endregion
        public override void Use()
        {
            if (!TimerComposite.GetIsReadyToUse()) return;
            TimerComposite.ActionStart();
        }
    }

    namespace Neutral
    {
        public class Barrier : SkillAbstractConcrete
        {
            #region Member

            private DataSystem.Atomics.BarrierAtomics barrier;
            private DataSystem.Atomics.AudioAtomics audio;
            private DataSystem.Atomics.VisualFXAtomics visualFX;

            #endregion
            public Barrier(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter
            public Barrier SetBarrierData(in SerialAffectorData affectorData)
            {
                audio = new DataSystem.Atomics.AudioAtomics(in affectorData._audioData);
                barrier = new DataSystem.Atomics.BarrierAtomics(affectorData._barrierData._barrierRatio);
                visualFX = new DataSystem.Atomics.VisualFXAtomics(E_AFFECT_TYPE.None, in affectorData._visualData);
                return this;
            }
            public Barrier SetOwnerEntity(Entitys.Player entity)
            {
                ownerEntity = entity;
                return this;
            }
            
            #endregion
            public async void Activate()
            {
                barrier?.Invoke(ownerEntity);
                visualFX?.Invoke(ownerEntity);
                audio.Invoke(ownerEntity);
                await UniTask.Delay(5 * 1000);
                barrier?.Revert(ownerEntity);
                visualFX?.Revert(ownerEntity);
                audio.Revert(ownerEntity);
            }
        }

        public class MoveFaster : SkillAbstractConcrete
        {
            #region Member

            private DataSystem.Modifiers.ConcreteAffector.MoveFasterAffect moveFasterAffect;

            #endregion

            public MoveFaster(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter
            public MoveFaster SetMoveFasterAffect(in SerialAffectorData affectorData)
            {
                moveFasterAffect = new DataSystem.Modifiers.ConcreteAffector.MoveFasterAffect(affectorData);
                return this;
            }
            public MoveFaster SetOwnerEntity(Entitys.Player entity)
            {
                ownerEntity = entity;
                return this;
            }

            #endregion
            public void Activate()
            {
                ownerEntity.GetDashAbility().RecoverOneDash();
                ownerEntity.Affect(moveFasterAffect);
            }
        }

        public class WeaponStun : SkillAbstractConcrete
        {

            #region Member
            private ExtrasModifier<Entitys.Entity> extrasModifier;
            private IFunctionalCommand<Entitys.Entity> stunAffectCommand;
            private DataSystem.Extras<Entitys.Entity> extrasRef;

            #endregion

            public WeaponStun(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public WeaponStun SetStunData(in SerialAffectorData AffectData)
            {
                stunAffectCommand = new DataSystem.Functional.AtomFunctions.ConveyAffectCommand.FactoryStunAffectCommand(in AffectData);
                extrasModifier = new ExtrasModifier<Entitys.Entity>(stunAffectCommand, E_EXTRAS_PERFORM_TYPE.Start, E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect);
                return this;
            }
            public WeaponStun SetOwnerEntity(Entitys.Player entity)
            {
                ownerEntity = entity;
                extrasRef = ownerEntity.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect);
                return this;
            }

            #endregion
            public async void Activate()
            {
                extrasRef.AddModifier(extrasModifier);
                extrasRef.RecalculateExtras();
                await UniTask.Delay(5 * 1000);
                extrasRef.RemoveModifier(extrasModifier);
                extrasRef.RecalculateExtras();
            }
        }

        public class WeaponAdditionalDamage : SkillAbstractConcrete
        {

            #region Member

            private ExtrasModifier<DamageInfo> extrasModifier;
            private IFunctionalCommand<DamageInfo> damageInfoCommand;
            private DataSystem.Extras<DamageInfo> extrasRef;

            #endregion

            public WeaponAdditionalDamage(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public WeaponAdditionalDamage SetDamageInfoData(in SerialOnDamageExtrasModifierDatas modifierDatas)
            {
                damageInfoCommand = new DataSystem.Functional.AtomFunctions.CalculateDamageCommand.MulHit(modifierDatas._damageConverterData);
                extrasModifier = new ExtrasModifier<DamageInfo>(
                    damageInfoCommand, modifierDatas._performType, modifierDatas._extrasType
                );
                return this;
            }
            public WeaponAdditionalDamage SetOwnerEntity(Entitys.Player entity)
            {
                ownerEntity = entity;
                extrasRef = ownerEntity.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse);
                return this;
            }

            #endregion
            public async void Activate()
            {
                extrasRef.AddModifier(extrasModifier);
                extrasRef.RecalculateExtras();
                await UniTask.Delay(5 * 1000);
                extrasRef.RemoveModifier(extrasModifier);
                extrasRef.RecalculateExtras();
            }
        }

        public class PowerUp : SkillAbstractConcrete
        {
            #region Member
            private DataSystem.Modifiers.ConcreteAffector.PowerUpAffect powerUpAffect;
            #endregion
            public PowerUp(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter
            public PowerUp SetPowerUpAffect(in SerialAffectorData affectorData)
            {
                powerUpAffect = new DataSystem.Modifiers.ConcreteAffector.PowerUpAffect(affectorData);
                return this;
            }
            public PowerUp SetOwnerEntity(Entitys.Player entity)
            {
                ownerEntity = entity;
                return this;
            }

            #endregion
            public void Activate()
            {
                ownerEntity.Affect(powerUpAffect);
            }
        }

        public class Lava : SkillAbstractConcrete
        {
            #region Member

            private ProjectileBucket instantiatorRef;
            private SerialProjectileInstantiateData projectileInstantiateData;

            #endregion

            public Lava(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public Lava SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData)
            {
                projectileInstantiateData = serialProjectileInstantiateData;
                return this;
            }

            public Lava SetOwnerEntity(Entitys.Player player)
            {
                ownerEntity = player;
                return this;
            }


            #endregion
            public void Activate()
            {
                ProjectileObject useProjectile = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
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

        }

        public class BlackWhiteHole : SkillAbstractConcrete
        {
            #region Member

            private ProjectileBucket instantiatorRef;
            private SerialProjectileInstantiateData projectileInstantiateData;

            #endregion

            public BlackWhiteHole(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public BlackWhiteHole SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData)
            {
                projectileInstantiateData = serialProjectileInstantiateData;
                return this;
            }

            public BlackWhiteHole SetOwnerEntity(Entitys.Player player)
            {
                ownerEntity = player;
                return this;
            }


            #endregion
            public void Activate()
            {
                ProjectileObject useProjectile = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
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

        }

    }
    namespace Melee
    {
        public class DoubleShot : SkillAbstractConcrete
        {
            #region Member

            private ProjectileBucket instantiatorRef;
            private SerialProjectileInstantiateData projectileInstantiateData;

            #endregion

            public DoubleShot(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public DoubleShot SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData)
            {
                projectileInstantiateData = serialProjectileInstantiateData;
                return this;
            }

            public DoubleShot SetOwnerEntity(Entitys.Player player)
            {
                ownerEntity = player;
                return this;
            }


            #endregion
            public async void Activate()
            {
                ProjectileObject useProjectile1 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
                instantiatorRef.InstantablePositioning(useProjectile1, Vector3.zero, Vector3.forward * 30)
                                .SetInstantiateType(projectileInstantiateData._InstantiateType)
                                .SetScaleMultiplyByRatio(projectileInstantiateData._ScaleMultiplyByRatio)
                                .SetDurateTimeByRatio(projectileInstantiateData._DurateTimeByRatio)
                                .SetSimulateSpeedByRatio(projectileInstantiateData._SimulateSpeed)
                                .SetForwardingSpeedByRatio(projectileInstantiateData._ForwardingSpeedByRatio)
                                .SetProjectilePower(projectileInstantiateData._ProjectilePower)
                                .SetAffectType(projectileInstantiateData._AffectType)
                                .SetIntervalData(in projectileInstantiateData._intervalData)
                                .Activate();
                
                await UniTask.Delay(100); 

                ProjectileObject useProjectile2 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef.InstantablePositioning(useProjectile2, Vector3.zero, Vector3.forward * -30)
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

        }

        public class Piercing : SkillAbstractConcrete
        {
            #region Member

            private DataSystem.Atomics.DashAtomics DashAtomics;
            private ProjectileBucket instantiatorRef;
            private SerialProjectileInstantiateData projectileInstantiateData;
            private SerialPhysicsData PhysicsData;
            #endregion

            public Piercing(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public Piercing SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData)
            {
                projectileInstantiateData = serialProjectileInstantiateData;
                return this;
            }

            public Piercing SetPhysics(in SerialAffectorData affectorData) {
                PhysicsData = affectorData._physicsData;
                return this;
            }

            public Piercing SetOwnerEntity(Entitys.Player player)
            {
                ownerEntity = player;
                DashAtomics = new DataSystem.Atomics.DashAtomics(
                    player.entityRigidbody, 
                    player.GetMovementComposite().GetMovemenCompositetData,
                    player.GetStat(E_NUMERIC_STAT_TYPE.DashForce).GetValueForce
                );
                TimerComposite.AddOnUseEvent(async () => {
                    player.GetMovementComposite().SetMovableState(false);
                    await UniTask.Delay(500); 
                    player.GetMovementComposite().SetMovableState(true);
                });
                return this;
            }

            #endregion
            public void Activate()
            {
                ProjectileObject useProjectile1 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
                instantiatorRef.InstantablePositioning(useProjectile1, Vector3.zero, Vector3.forward * 15)
                                .SetInstantiateType(projectileInstantiateData._InstantiateType)
                                .SetScaleMultiplyByRatio(projectileInstantiateData._ScaleMultiplyByRatio)
                                .SetDurateTimeByRatio(projectileInstantiateData._DurateTimeByRatio)
                                .SetSimulateSpeedByRatio(projectileInstantiateData._SimulateSpeed)
                                .SetForwardingSpeedByRatio(projectileInstantiateData._ForwardingSpeedByRatio)
                                .SetProjectilePower(projectileInstantiateData._ProjectilePower)
                                .SetAffectType(projectileInstantiateData._AffectType)
                                .SetIntervalData(in projectileInstantiateData._intervalData)
                                .Activate();
                DashAtomics.Invoke();
            }

        }
    
        public class RotateSlash : SkillAbstractConcrete
        {
            #region Member

            private ProjectileBucket instantiatorRef;
            private SerialProjectileInstantiateData projectileInstantiateData;

            #endregion

            public RotateSlash(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public RotateSlash SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData)
            {
                projectileInstantiateData = serialProjectileInstantiateData;
                return this;
            }

            public RotateSlash SetOwnerEntity(Entitys.Player player)
            {
                ownerEntity = player;
                return this;
            }

            #endregion
            public void Activate()
            {
                ProjectileObject useProjectile1 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
                instantiatorRef.InstantablePositioning(useProjectile1)
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
        }

        public class ThrowSlash : SkillAbstractConcrete {
            #region Member

            private ProjectileBucket instantiatorRef;
            private SerialProjectileInstantiateData projectileInstantiateData;

            #endregion

            public ThrowSlash(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(15f, 1);
                TimerComposite.AddBindingAction(Activate);
            }

            #region Setter

            public ThrowSlash SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData)
            {
                projectileInstantiateData = serialProjectileInstantiateData;
                return this;
            }

            public ThrowSlash SetOwnerEntity(Entitys.Player player)
            {
                ownerEntity = player;
                return this;
            }

            #endregion
            public void Activate()
            {
                ProjectileObject useProjectile1 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
                instantiatorRef.InstantablePositioning(useProjectile1, Vector3.zero, Vector3.right * -30)
                                .SetInstantiateType(projectileInstantiateData._InstantiateType)
                                .SetScaleMultiplyByRatio(projectileInstantiateData._ScaleMultiplyByRatio)
                                .SetDurateTimeByRatio(projectileInstantiateData._DurateTimeByRatio)
                                .SetSimulateSpeedByRatio(projectileInstantiateData._SimulateSpeed)
                                .SetForwardingSpeedByRatio(projectileInstantiateData._ForwardingSpeedByRatio)
                                .SetProjectilePower(projectileInstantiateData._ProjectilePower)
                                .SetAffectType(projectileInstantiateData._AffectType)
                                .SetIntervalData(in projectileInstantiateData._intervalData)
                                .Activate();

                ProjectileObject useProjectile2 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef.InstantablePositioning(useProjectile2, Vector3.zero, Vector3.zero)
                                .SetInstantiateType(projectileInstantiateData._InstantiateType)
                                .SetScaleMultiplyByRatio(projectileInstantiateData._ScaleMultiplyByRatio)
                                .SetDurateTimeByRatio(projectileInstantiateData._DurateTimeByRatio)
                                .SetSimulateSpeedByRatio(projectileInstantiateData._SimulateSpeed)
                                .SetForwardingSpeedByRatio(projectileInstantiateData._ForwardingSpeedByRatio)
                                .SetProjectilePower(projectileInstantiateData._ProjectilePower)
                                .SetAffectType(projectileInstantiateData._AffectType)
                                .SetIntervalData(in projectileInstantiateData._intervalData)
                                .Activate();
                ProjectileObject useProjectile3 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef.InstantablePositioning(useProjectile3, Vector3.zero, Vector3.right * 30)
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
        }
    
        public class DashSlash : SkillAbstractConcrete
        {
            #region Member

            private DataSystem.Atomics.DashAtomics DashAtomics;
            private ProjectileBucket instantiatorRef;
            private SerialProjectileInstantiateData projectileInstantiateData;
            private SerialPhysicsData PhysicsData;
            #endregion

            public DashSlash(in SerialUserInterfaceData userInterfaceData) : base(userInterfaceData)
            {
                TimerComposite = new CoolTimeComposite(5, 3)
                                        .AddBindingAction(Activate);
            }

            #region Setter

            public DashSlash SetInstantiationData(in SerialProjectileInstantiateData serialProjectileInstantiateData)
            {
                projectileInstantiateData = serialProjectileInstantiateData;
                return this;
            }

            public DashSlash SetPhysics(in SerialAffectorData affectorData) {
                PhysicsData = affectorData._physicsData;
                return this;
            }
            public DashSlash SetOwnerEntity(Entitys.Player player)
            {
                ownerEntity = player;
                DashAtomics = new DataSystem.Atomics.DashAtomics(
                    player.entityRigidbody, 
                    player.GetMovementComposite().GetMovemenCompositetData,
                    player.GetStat(E_NUMERIC_STAT_TYPE.DashForce).GetValueForce
                );
                TimerComposite.AddOnUseEvent(async () => {
                    player.GetMovementComposite().SetMovableState(false);
                    await UniTask.Delay(500); 
                    player.GetMovementComposite().SetMovableState(true);
                });
                return this;
            }

            #endregion
            public void Activate()
            {
                ProjectileObject useProjectile1 = ProjectilePool.GetObject(projectileInstantiateData._projectileObjectRefer).Init(ownerEntity);
                instantiatorRef = ownerEntity.GetProjectileBucketManager().GetProjectileBucket(projectileInstantiateData._bucketIndex);
                instantiatorRef.InstantablePositioning(useProjectile1, Vector3.zero, Vector3.forward * 15)
                                .SetInstantiateType(projectileInstantiateData._InstantiateType)
                                .SetScaleMultiplyByRatio(projectileInstantiateData._ScaleMultiplyByRatio)
                                .SetDurateTimeByRatio(projectileInstantiateData._DurateTimeByRatio)
                                .SetSimulateSpeedByRatio(projectileInstantiateData._SimulateSpeed)
                                .SetForwardingSpeedByRatio(projectileInstantiateData._ForwardingSpeedByRatio)
                                .SetProjectilePower(projectileInstantiateData._ProjectilePower)
                                .SetAffectType(projectileInstantiateData._AffectType)
                                .SetIntervalData(in projectileInstantiateData._intervalData)
                                .Activate();
                DashAtomics.Invoke();
            }

        }
    
    }

}
