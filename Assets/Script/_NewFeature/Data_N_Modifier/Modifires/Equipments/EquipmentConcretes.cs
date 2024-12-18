
using Sophia.DB;

namespace Sophia.DataSystem
{
    using System.Collections.Generic;
    using Sophia.Entitys;
    using Sophia.DataSystem.Functional.AtomFunctions;
    
    namespace Modifiers.ConcreteEquipment
    {
        public static class FactoryConcreteEquipment {
            public static Equipment GetEquipmentByID(IEquipmentDataAccessable equipmentData, Entitys.Entity entity) {
                Equipment equipmentRes = null;
                switch(equipmentData.EquipmentID) {
                    case 1005 : {equipmentRes = new Equipment_1005_HorsepowerSender(equipmentData, entity);      break;}
                    case 1008 : {equipmentRes = new Equipment_1008_BullsTrap(equipmentData);     break;}
                    case 1009 : {equipmentRes = new Equipment_1009_LightFlash(equipmentData);        break;}
                    case 1011 : {equipmentRes = new Equipment_1011_YellowLegoBrick(equipmentData);       break;}
                    case 1012 : {equipmentRes = new Equipment_1012_PinkDumbbell(equipmentData);      break;}
                    case 1013 : {equipmentRes = new Equipment_1013_CommunistsHammer(equipmentData, entity);     break;}
                    case 2004 : {equipmentRes = new Equipment_2004_CrudeGoldenBadge(equipmentData, entity);     break;}
                    case 3002 : {equipmentRes = new Equipment_3002_FrozenHelm(equipmentData);     break;}
                    case 3004 : {equipmentRes = new Equipment_3004_DeadlyThorn(equipmentData);     break;}
                    case 4005 : {equipmentRes = new Equipment_4005_MovementDirective(equipmentData);     break;}
                    default : {equipmentRes = new Equipment(equipmentData); break;}
                }
                return equipmentRes;
            }
        }
        public class Equipment_1005_HorsepowerSender : Equipment {
            ExtrasModifier<int> gearcoinExtras;
            StatModifier powerModifier;
            Player playerEntity = null;
            public Equipment_1005_HorsepowerSender(IEquipmentDataAccessable equipmentData, Entitys.Entity player) : base(equipmentData) {
                powerModifier = new StatModifier(0, E_STAT_CALC_TYPE.Add, E_NUMERIC_STAT_TYPE.Power);
                player.GetStat(E_NUMERIC_STAT_TYPE.Power).AddModifier(powerModifier);
                gearcoinExtras = new ExtrasModifier<int>(
                    new ItemTriggeredCommand.GearcoinCollect(player),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered
                );
                playerEntity = player as Player;
            }

            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                Extras<int> extrasRef = dataAccessible.GetExtras<int>(E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered);
                extrasRef.AddModifier(gearcoinExtras);
                extrasRef.RecalculateExtras();
                int currentWealth = playerEntity.PlayerWealth;
                extrasRef.PerformStartFunctionals(ref currentWealth);
            }

            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                Extras<int> extrasRef = dataAccessible.GetExtras<int>(E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered);
                extrasRef.RemoveModifier(gearcoinExtras);
                extrasRef.RecalculateExtras();
            }
        }

        public class Equipment_1008_BullsTrap : Equipment {

            readonly List<ExtrasModifier<Entity>> ConveyAffectExtrasModifiers = new();

            public Equipment_1008_BullsTrap(IEquipmentDataAccessable equipmentData) : base(equipmentData) {
                ExtrasModifier<Entity> extrasModifier = new ExtrasModifier<Entity>(
                    new ConveyAffectCommand.FactoryStunAffectCommand(
                        equipmentData.EquipmentStatExtras.OnConveyAffect._affectData
                    ).SetRandomPercentage(5),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect
                );
                ConveyAffectExtrasModifiers.Add(extrasModifier);
            }

            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                foreach(ExtrasModifier<Entity> modifier in ConveyAffectExtrasModifiers) {
                    Extras<Entity> extrasRef = dataAccessible.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                    extrasRef.AddModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }
            
            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                foreach(ExtrasModifier<Entity> modifier in ConveyAffectExtrasModifiers) {
                    Extras<Entity> extrasRef = dataAccessible.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                    extrasRef.RemoveModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }
        }

        public class Equipment_1009_LightFlash : Equipment
        {
            readonly SerialDamageConverterData DamageConverterData;
            readonly List<ExtrasModifier<DamageInfo>> DamageExtrasModifiers = new();
            public Equipment_1009_LightFlash(IEquipmentDataAccessable equipmentData) : base(equipmentData)
            {
                DamageConverterData = new SerialDamageConverterData {
                    _damageRatio = 0,
                    _damageHandleType = DamageHandleType.Dodge,
                    _hitType = HitType.None
                };

                ExtrasModifier<DamageInfo> ExtrasModifier = new ExtrasModifier<DamageInfo> (
                    new Functional.AtomFunctions.CalculateDamageCommand.DodgeHit(in DamageConverterData)
                                                            .SetRandomPercentage(10),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.Damaged
                );
                DamageExtrasModifiers.Add(ExtrasModifier);
            }

            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                foreach(var modifier in DamageExtrasModifiers) {
                    Extras<DamageInfo> extrasRef = dataAccessible.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.Damaged);
                    extrasRef.AddModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }

            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                foreach(var modifier in DamageExtrasModifiers) {
                    Extras<DamageInfo> extrasRef = dataAccessible.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.Damaged);
                    extrasRef.RemoveModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }
        }

        public class Equipment_1011_YellowLegoBrick : Equipment
        {
            readonly SerialDamageConverterData DamageConverterData;
            readonly List<ExtrasModifier<DamageInfo>> DamageExtrasModifiers = new();
            public Equipment_1011_YellowLegoBrick(IEquipmentDataAccessable equipmentData) : base(equipmentData)
            {
                DamageConverterData = new SerialDamageConverterData {
                    _damageRatio = 5,
                    _damageHandleType = DamageHandleType.None,
                    _hitType = HitType.Critical
                };
                ExtrasModifier<DamageInfo> ExtrasModifier = new ExtrasModifier<DamageInfo> (
                    new CalculateDamageCommand.HardHit(in DamageConverterData)
                                        .SetRandomPercentage(5),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse
                );
                DamageExtrasModifiers.Add(ExtrasModifier);
            }
            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                foreach(var modifier in DamageExtrasModifiers) {
                    Extras<DamageInfo> extrasRef = dataAccessible.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse);
                    extrasRef.AddModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }

            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                foreach(var modifier in DamageExtrasModifiers) {
                    Extras<DamageInfo> extrasRef = dataAccessible.GetExtras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse);
                    extrasRef.RemoveModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }
        }

        public class Equipment_1012_PinkDumbbell : Equipment
        {
            public UnityEngine.Vector3 OriginScale;
            public Player PlayerEntity;
            public Equipment_1012_PinkDumbbell(IEquipmentDataAccessable equipmentData) : base(equipmentData)
            {
            }

            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                PlayerEntity ??= dataAccessible as Player;
                OriginScale = PlayerEntity.GetGameObject().transform.localScale;
                PlayerEntity.GetGameObject().transform.localScale *= 1.2f;
            }
            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                PlayerEntity.GetGameObject().transform.localScale = OriginScale;
            }
        }

        public class Equipment_1013_CommunistsHammer : Equipment
        {
            readonly List<ExtrasModifier<Entity>> ConveyAffectExtrasModifiers = new();
            public Equipment_1013_CommunistsHammer(IEquipmentDataAccessable equipmentData, Entitys.Entity entity) : base(equipmentData)
            {
                ExtrasModifier<Entity> ExtrasModifier = new ExtrasModifier<Entity>(
                    new ConveyAffectCommand.FactoryKnockbackAffectCommand(
                        equipmentData.EquipmentStatExtras.OnConveyAffect._affectData, 
                        entity.GetGameObject().transform
                    ),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect
                );
                ConveyAffectExtrasModifiers.Add(ExtrasModifier);
            }
            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                foreach(ExtrasModifier<Entity> modifier in ConveyAffectExtrasModifiers) {
                    Extras<Entity> extrasRef = dataAccessible.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                    extrasRef.AddModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }
            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                foreach(ExtrasModifier<Entity> modifier in ConveyAffectExtrasModifiers) {
                    Extras<Entity> extrasRef = dataAccessible.GetExtras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                    extrasRef.RemoveModifier(modifier);
                    extrasRef.RecalculateExtras();
                }
            }
        }

        public class Equipment_2004_CrudeGoldenBadge : Equipment {
            ExtrasModifier<object> EnemyDieExtrasModifier;
            Entitys.Entity entityRef;
            private readonly SerialAffectorData serialAffectorData;

            public Equipment_2004_CrudeGoldenBadge(IEquipmentDataAccessable equipmentData, Entitys.Entity entity) : base(equipmentData) {
                entityRef = entity;
                serialAffectorData = equipmentData.EquipmentStatExtras.OnConveyAffect._affectData;
                EnemyDieExtrasModifier = new ExtrasModifier<object>(
                    new GeneralCommand.NoneParameterCommand(MoveFasterAction),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie
                );
            }
            
            public void MoveFasterAction() {
                entityRef.Affect(new ConcreteAffector.MoveFasterAffect(serialAffectorData));
            }

            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                
                Extras<object> extrasRef = dataAccessible.GetExtras<object>(E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie);
                extrasRef.AddModifier(EnemyDieExtrasModifier);
                extrasRef.RecalculateExtras();
            }
            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                
                Extras<object> extrasRef = dataAccessible.GetExtras<object>(E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie);
                extrasRef.RemoveModifier(EnemyDieExtrasModifier);
                extrasRef.RecalculateExtras();
            }
        }

        public class Equipment_3002_FrozenHelm : Equipment
        {
            ExtrasModifier<Entitys.Entity> ConveyAffectExtrasModifier;
            public Equipment_3002_FrozenHelm(IEquipmentDataAccessable equipmentData) : base(equipmentData)
            {
                ConveyAffectExtrasModifier = new ExtrasModifier<Entity>(
                    new ConveyAffectCommand.FactoryColdAffectCommand(equipmentData.EquipmentStatExtras.OnConveyAffect._affectData),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect
                );
            }
            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                
                Extras<Entitys.Entity> extrasRef = dataAccessible.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                extrasRef.AddModifier(ConveyAffectExtrasModifier);
                extrasRef.RecalculateExtras();
            }
            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                
                Extras<Entitys.Entity> extrasRef = dataAccessible.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                extrasRef.RemoveModifier(ConveyAffectExtrasModifier);
                extrasRef.RecalculateExtras();
            }
        }

        public class Equipment_3004_DeadlyThorn : Equipment
        {
            ExtrasModifier<Entitys.Entity> ConveyAffectExtrasModifier;
            public Equipment_3004_DeadlyThorn(IEquipmentDataAccessable equipmentData) : base(equipmentData)
            {
                ConveyAffectExtrasModifier = new ExtrasModifier<Entity>(
                    new ConveyAffectCommand.FactoryBleedAffectCommand(equipmentData.EquipmentStatExtras.OnConveyAffect._affectData),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect
                );
            }
            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                
                Extras<Entitys.Entity> extrasRef = dataAccessible.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                extrasRef.AddModifier(ConveyAffectExtrasModifier);
                extrasRef.RecalculateExtras();
            }
            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                
                Extras<Entitys.Entity> extrasRef = dataAccessible.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                extrasRef.RemoveModifier(ConveyAffectExtrasModifier);
                extrasRef.RecalculateExtras();
            }
        }
        public class Equipment_4005_MovementDirective : Equipment
        {
            ExtrasModifier<Entitys.Entity> ConveyAffectExtrasModifier;
            public Equipment_4005_MovementDirective(IEquipmentDataAccessable equipmentData) : base(equipmentData)
            {
                ConveyAffectExtrasModifier = new ExtrasModifier<Entity>(
                    new ConveyAffectCommand.FactoryExecuteCommand(equipmentData.EquipmentStatExtras.OnConveyAffect._affectData)
                        .SetRandomPercentage(5),
                    E_EXTRAS_PERFORM_TYPE.Start,
                    E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect
                );
            }
            public override void Invoke(IDataAccessible dataAccessible)
            {
                base.Invoke(dataAccessible);
                
                Extras<Entitys.Entity> extrasRef = dataAccessible.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                extrasRef.AddModifier(ConveyAffectExtrasModifier);
                extrasRef.RecalculateExtras();
            }
            public override void Revert(IDataAccessible dataAccessible)
            {
                base.Revert(dataAccessible);
                
                Extras<Entitys.Entity> extrasRef = dataAccessible.GetExtras<Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect);
                extrasRef.RemoveModifier(ConveyAffectExtrasModifier);
                extrasRef.RecalculateExtras();
            }
        }
    }
}
