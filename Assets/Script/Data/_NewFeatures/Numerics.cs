using System.Collections.Generic;
using System.Linq;

namespace Feature_NewData
{
    public enum E_NUMERIC_BASE_STATE_MENEBERS {
        
    }
    
    // FinalStat = [(Natural + NatureAdd?)] * [(Ratio +* RatioMul)]

    // 기본
    // 아이템 합 이후 스탯
    /*

    -------------------------------------------------
    1000 * 1.2
    1200 * 0.8
    960 

    1. 가만히 있는 데이터가 필요하고
    2. 덧셈 증가 하는 데이터를 두고
    3. 비율 증가하는 데이터를 
    -------------------------------------------------

    능력치 변화에 대한 능력치 변화 순서

    BaseStat = [Natural * Ratio] (아이템)
    AfterEquipmentStat = BaseStat * Ratio (버프에 의해서)
    AfterWeaponStat
    
    FinalStat들이 있겠고
    
    FinalFinalStat = FinalStat * (퍼프스텟 적용) 시간에 따라서 변하는 애니깐

    평타 데이지인지 // 스킬 데미지
    
    평타 데이지 = AfterEquipmentStat * 무기 데미지 비율
    스킬 데미지 = AfterEquipmentStat * 무기 데미지 비율 * 스킬 비례댐

    */

    public enum E_NUMERIC_STAT_MEMBERS
    {
        NaturalMaxHp = 100, NaturalDefence, NaturalPower, NaturalAttackSpeed, NaturalMoveSpeed, NaturalTenacity = 105, 
        NaturalMaxStamina, NaturalStaminaRestoreSpeed, NaturalLuck = 108,

        RatioMaxHp = 200, RatioDefence, RatioPower, RatioAttackSpeed, RatioMoveSpeed, RatioTenacity, 
        RatioStaminaRestoreSpeed = 207,

        NaturalDistance = 110, NaturalMaxDuNaturaln, NaturalSize, NaturalSpeed,
        RatioMaxDistance = 210, RatioMaxDuration, RatioSize, RatioSpeed,

        NaturalMaxProjectilePoolSize = 120, NaturalRatioRestoreSpeed, NaturalMeleeDamage, NaturalRangeDamage, NaturalTechDamage,
        RatioMaxProjectilePoolSize = 220, RatioRestoreSpeed, RatioMeleeDamage, RatioRangeDamage, RatioTechDamage,

        RatioEfficienceMultiplyer = 230, RatioAccecerate
    }
    public interface INumericAccessable
    {
        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType);

        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc);
        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc);
        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType);
    }

    public abstract class Numerics : INumericAccessable
    {
        protected readonly Dictionary<E_NUMERIC_STAT_MEMBERS, Stat> numerics = new();

        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return numerics[numericMemberType];
        }
        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            numerics[numericMemberType].AddCalculator(calc);
        }
        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            numerics[numericMemberType].RemoveCalculator(calc);
        }
        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            numerics[numericMemberType].ResetCalculators();
        }
    }
}


/*********************************************************************************
Escatrgot 

SetNumeric(
    E_NUMERIC_STAT_MEMBERS numericMemberType, 
    float value, 
    E_STAT_USE_TYPE useType
);

을 지운 이유.

Numeric의 초기화는 오직 생성자를 통해서 할것이고,
변동은 무조건 Calculator을 옽해서 진행하게 될것이다.

그래서 SetNumeric은 잘못된것이다.

*********************************************************************************/