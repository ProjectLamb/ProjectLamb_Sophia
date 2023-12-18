using System.Collections.Generic;

namespace Feature_NewData
{
    public class Equipment_003 {
        public readonly StatCalculator MaxHpCalculator = new StatCalculator(0.9f, E_STAT_CALC_TYPE.Add);
        public readonly StatCalculator PowerCalculator = new StatCalculator(15, E_STAT_CALC_TYPE.Add);
    }
}