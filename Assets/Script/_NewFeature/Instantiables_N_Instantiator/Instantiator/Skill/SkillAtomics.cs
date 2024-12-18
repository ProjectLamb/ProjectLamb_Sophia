using UnityEngine;
using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.DataSystem.Modifiers;
using System;
using Sophia.DB;

namespace Sophia.Instantiates.Skills {
    public enum E_SKILL_INDEX {
        _Neutral_ = 0,
            Barrier, MoveFaster, WeaponStun, WeaponAdditionalDamage, PowerUp, Lava, BlackWhiteHole,
        _Melee_ = 100,
            DoubleShot, Piercing, RotateSlash, ThrowSlash, DashSlash
    }

    public class FactoryConcreteSkill {
        public static Skill GetSkillByID(E_SKILL_INDEX index, Entitys.Player player, 
            ISkillDataAccessable skillData
        )
        {
            switch (index) {
                case E_SKILL_INDEX.Barrier : {
                    return new Neutral.Barrier(index, skillData.SkillUserInterfaceData)
                                .SetBarrierData(skillData.SkillAffectorData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.MoveFaster : {
                    return new Neutral.MoveFaster(index, skillData.SkillUserInterfaceData)
                                .SetMoveFasterAffect(skillData.SkillAffectorData)
                                .SetOwnerEntity(player)
;
                }
                case E_SKILL_INDEX.WeaponStun : {
                    return new Neutral.WeaponStun(index, skillData.SkillUserInterfaceData)
                                .SetStunData(skillData.SkillConveyAffectModifierData._affectData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.WeaponAdditionalDamage : {
                    return new Neutral.WeaponAdditionalDamage(index, skillData.SkillUserInterfaceData)
                                .SetDamageInfoData(skillData.SkillDamageModifierData)
                                .SetOwnerEntity(player)
                                .SetAudioData(skillData.SkillActivatedAudioData);
                }
                case E_SKILL_INDEX.PowerUp : {
                    return new Neutral.PowerUp(index, skillData.SkillUserInterfaceData)
                                .SetPowerUpAffect(skillData.SkillAffectorData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.Lava : {
                    return new Neutral.Lava(index, skillData.SkillUserInterfaceData)
                                .SetInstantiationData(skillData.SkillProjectileInstantiateData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.BlackWhiteHole : {
                    return new Neutral.BlackWhiteHole(index, skillData.SkillUserInterfaceData)
                                .SetInstantiationData(skillData.SkillProjectileInstantiateData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.DoubleShot : {
                    return new Melee.DoubleShot(index, skillData.SkillUserInterfaceData)
                                .SetInstantiationData(skillData.SkillProjectileInstantiateData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.Piercing : {
                    return new Melee.Piercing(index, skillData.SkillUserInterfaceData)
                                .SetInstantiationData(skillData.SkillProjectileInstantiateData)
                                .SetPhysics(skillData.SkillAffectorData)
                                .SetOwnerEntity(player);
                }
                case E_SKILL_INDEX.RotateSlash : {
                    return new Melee.RotateSlash(index, skillData.SkillUserInterfaceData)
                                .SetInstantiationData(skillData.SkillProjectileInstantiateData)
                                .SetOwnerEntity(player);

  
                }
                case E_SKILL_INDEX.ThrowSlash : {
                    return new Melee.ThrowSlash(index, skillData.SkillUserInterfaceData)
                                .SetInstantiationData(skillData.SkillProjectileInstantiateData)
                                .SetOwnerEntity(player);

  
                }
                case E_SKILL_INDEX.DashSlash : {
                    return new Melee.DashSlash(index, skillData.SkillUserInterfaceData)
                                .SetInstantiationData(skillData.SkillProjectileInstantiateData)
                                .SetPhysics(skillData.SkillAffectorData)
                                .SetOwnerEntity(player);
                }
                default : {
                    throw new System.Exception("올바르지 않은 인덱스 접근");
                }
            }
            }
        
    }
}