using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Feature_NewData
{
    /*********************************************************************************
    *
    * Data Redefiner
    * 데이터 변경을 하는 놈
    *
    *********************************************************************************/

    #region Data Redefiner Database
    
    public class EntityFixedAmounts {
        public  int             FixedMaxHp; 
        public  int             FixedDefence;
        public  int             FixedPower;
        public  int             FixedTenacity;
    }
    public class EntityReferingRatioAmounts {
        public  float           ReferingRatioMaxHp; 
        public  float           ReferingRatioDefence;
        public  float           ReferingRatioPower;
        public  float           ReferingRatioAttackSpeed;
        public  float           ReferingRatioMoveSpeed;
        public  float           ReferingRatioTenacity;
    }
    
    public class EntityAdditionalFunctionals {
        public  UnityAction     AddedMove;
        public  UnityAction     AddedDamaged;
        public  UnityAction     AddedAttack;
        public  UnityAction     AddedAffected;
        public  UnityAction     AddedDead;
        public  UnityAction     AddedStanding;
        public  UnityAction     AddedPhyiscTriggered;
    }
    
    public class PlayerFixedAmounts {
        public  int             FixedMaxHp; 
        public  int             FixedDefence;
        public  int             FixedPower;
        public  int             FixedTenacity;
        public  int             FixedMaxStamina;
        public  int             FixedLuck;

    }
    public class PlayerReferingRatioAmounts {
        public  float           ReferingRatioMaxHp; 
        public  float           ReferingRatioDefence;
        public  float           ReferingRatioPower;
        public  float           ReferingRatioAttackSpeed;
        public  float           ReferingRatioMoveSpeed;
        public  float           ReferingRatioTenacity;
        public  float           ReferingRatioStaminaRestoreSpeed;
    }
    public class PlayerAdditionalFunctionals {
        public  UnityAction     AddedMove;
        public  UnityAction     AddedDamaged;
        public  UnityAction     AddedAttack;
        public  UnityAction     AddedAffected;
        public  UnityAction     AddedDead;
        public  UnityAction     AddedStanding;
        public  UnityAction     AddedPhyiscTriggered;
        public  UnityAction     AddedSkill;
    }


    public class InstantiatorReferingRatioAmounts {
        public float ReferingRatioMaxDistance;
        public float ReferingRatioMaxDuration;
        public float ReferingRatioSize;
        public float ReferingRatioSpeed;
    }
    public class InstantiatorAdditionalFunctionals {
        public UnityAction AddedCreated;
        public UnityAction AddedInteracted;
        public UnityAction AddedReleased;
        public UnityAction AddedForwarding;
    }

    public class WeaponReferingRatioAmount {
        public float MaxProjectilePoolSize;
        public float RestoreSpeed;
        public float MeleeDamageRatio;
        public float RangeDamageRatio;
        public float TechDamageRatio;
    }
    public class WeaponAdditionalFunctionals {
        public UnityAction AddedWeaponUse;
        public UnityAction AddedWeaponChange;
        public UnityAction AddedProjectileRestore;
    }

    public class SkillReferingRatioAmonts{
        public float ReferingRatioEfficienceMultiplyer;
        public float ReferingRatioAccecerate;
    }

    public class SkillAddtionalFunctionals {
        public UnityAction AddedUse;
        public UnityAction AddedSkillChange;
        public UnityAction AddedSkillRefilled;
    }

    public class NonTemporalCalculatedAddings {
        public PlayerFixedAmounts               playerFixedAmounts;
        public PlayerReferingRatioAmounts       playerReferingRatioAmounts;
        public PlayerAdditionalFunctionals      playerAdditionalFunctional;
    }

    public class NonTemporalOnUsePostWeaponAddings
    {
        public WeaponReferingRatioAmount        weaponReferingRatioAmount;
        public WeaponAdditionalFunctionals      weaponAdditionalFunctionals;
    }
    public class NonTemporalOnUsePostSkillAddings {
        public SkillReferingRatioAmonts         skillReferingRatioAmonts;
        public SkillAddtionalFunctionals        skillAddtionalFunctionals;
    }

    public class TemporalAddings {
        public EntityReferingRatioAmounts  entityReferingRatioAmounts;
        public EntityAdditionalFunctionals entityAdditionalFunctionals;
    }

#endregion
}
