using System.Collections.Generic;

namespace Feature_NewData
{
    public class Equipment_001 {
        public readonly StatCalculator MaxHPCalculator = new StatCalculator(10f, E_STAT_CALC_TYPE.Add);
        public readonly StatCalculator MoveSpeedCalculator = new StatCalculator(0.95f, E_STAT_CALC_TYPE.Add);
    }
}