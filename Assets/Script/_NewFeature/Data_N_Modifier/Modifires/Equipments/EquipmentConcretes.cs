
namespace Sophia.DataSystem
{
    using System.Collections.Generic;
    using Sophia.Entitys;
    using Sophia.DataSystem.Functional.AtomFunctions;
    
    namespace Modifiers.ConcreteEquipment
    {
        public static class FactoryConcreteEquipment {
            public static Equipment GetEquipmentByID(in SerialEquipmentData equipmentData, Entitys.Entity entity) {
                Equipment equipmentRes = null;
                switch(equipmentData._equipmentID) {
                    case 9 :  {equipmentRes = new Equipment_009_LightFlash(equipmentData);          break;}
                    case 12 : {equipmentRes = new Equipment_012_YellowLegoBrick(equipmentData);     break;}
                    case 13 : {equipmentRes = new Equipment_013_PinkDumbell(equipmentData);         break;}
                    case 14 : {equipmentRes = new Equipment_014_CommunistsHammer(equipmentData, entity);    break;}
                    default : {equipmentRes = new Equipment(equipmentData); break;}
                }
                return equipmentRes;
            }
        }
        
        public class Equipment_009_LightFlash : Equipment
        {
            readonly SerialDamageConverterData DamageConverterData;
            readonly List<ExtrasModifier<DamageInfo>> DamageExtrasModifiers = new();
            public Equipment_009_LightFlash(SerialEquipmentData equipmentData) : base(equipmentData)
            {
                DamageConverterData = new SerialDamageConverterData {
                    _damageRatio = 0,
                    _damageHandleType = DamageHandleType.Dodge,
                    _hitType = HitType.None
                };

                ExtrasModifier<DamageInfo> ExtrasModifier = new ExtrasModifier<DamageInfo> (
                    new Functional.AtomFunctions.CalculateDamageCommand.DodgeHit(in DamageConverterData, 100),
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

        public class Equipment_012_YellowLegoBrick : Equipment
        {
            readonly SerialDamageConverterData DamageConverterData;
            readonly List<ExtrasModifier<DamageInfo>> DamageExtrasModifiers = new();
            public Equipment_012_YellowLegoBrick(SerialEquipmentData equipmentData) : base(equipmentData)
            {
                DamageConverterData = new SerialDamageConverterData {
                    _damageRatio = 5,
                    _damageHandleType = DamageHandleType.None,
                    _hitType = HitType.Critical
                };
                ExtrasModifier<DamageInfo> ExtrasModifier = new ExtrasModifier<DamageInfo> (
                    new CalculateDamageCommand.HardHit(in DamageConverterData, 100),
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

        public class Equipment_013_PinkDumbell : Equipment
        {
            public UnityEngine.Vector3 OriginScale;
            public Player PlayerEntity;
            public Equipment_013_PinkDumbell(SerialEquipmentData equipmentData) : base(equipmentData)
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

        public class Equipment_014_CommunistsHammer : Equipment
        {
            readonly List<ExtrasModifier<Entity>> ConveyAffectExtrasModifiers = new();
            public Equipment_014_CommunistsHammer(SerialEquipmentData equipmentData, Entitys.Entity entity) : base(equipmentData)
            {
                ExtrasModifier<Entity> ExtrasModifier = new ExtrasModifier<Entity>(
                    new ConveyAffectCommand.FactoryKnockbackAffectCommand(
                        in equipmentData._extrasCalculateDatas.OnConveyAffect._affectData, 
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
    }
}
