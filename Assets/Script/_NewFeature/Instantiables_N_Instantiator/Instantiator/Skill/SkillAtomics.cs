using UnityEngine;
using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.DataSystem.Modifiers;
using System;

namespace Sophia.Instantiates.Skills {
    public enum E_SKILL_INDEX {
        _Neutral_ = 0,
            Barrier, MoveFaster, WeaponStun, WeaponAdditionalDamage, PowerUp, Lava, BlackWhiteHole,
        _Melee_ = 100,
            DoubleShot, Piercing, RotateSlash, ThrowSlash, DashSlash
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
                case E_SKILL_INDEX.Barrier : {
                    return new Neutral.Barrier(in userInterfaceData)
                                .SetBarrierData(in affectorData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.MoveFaster : {
                    return new Neutral.MoveFaster(in userInterfaceData)
                                .SetMoveFasterAffect(in affectorData)
                                .SetOwnerEntity(player)
;
                }
                case E_SKILL_INDEX.WeaponStun : {
                    return new Neutral.WeaponStun(in userInterfaceData)
                                .SetStunData(in conveyAffectExtrasModifierData._affectData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.WeaponAdditionalDamage : {
                    return new Neutral.WeaponAdditionalDamage(in userInterfaceData)
                                .SetDamageInfoData(in damageExtrasModifierData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.PowerUp : {
                    return new Neutral.PowerUp(in userInterfaceData)
                                .SetPowerUpAffect(in affectorData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.Lava : {
                    return new Neutral.Lava(in userInterfaceData)
                                .SetInstantiationData(in projectileInstantiateData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.BlackWhiteHole : {
                    return new Neutral.BlackWhiteHole(in userInterfaceData)
                                .SetInstantiationData(in projectileInstantiateData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.DoubleShot : {
                    return new Melee.DoubleShot(in userInterfaceData)
                                .SetInstantiationData(in projectileInstantiateData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.Piercing : {
                    return new Melee.Piercing(in userInterfaceData)
                                .SetInstantiationData(in projectileInstantiateData)
                                .SetPhysics(in affectorData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.RotateSlash : {
                    return new Melee.RotateSlash(in userInterfaceData)
                                .SetInstantiationData(in projectileInstantiateData)
                                .SetOwnerEntity(player);

  
                }
                case E_SKILL_INDEX.ThrowSlash : {
                    return new Melee.ThrowSlash(in userInterfaceData)
                                .SetInstantiationData(in projectileInstantiateData)
                                .SetOwnerEntity(player);

  
                }
                case E_SKILL_INDEX.DashSlash : {
                    return new Melee.DashSlash(in userInterfaceData)
                                .SetInstantiationData(in projectileInstantiateData)
                                .SetPhysics(in affectorData)
                                .SetOwnerEntity(player);
                }
                default : {
                    throw new System.Exception("올바르지 않은 인덱스 접근");
                }
            }
            }
        
    }
}