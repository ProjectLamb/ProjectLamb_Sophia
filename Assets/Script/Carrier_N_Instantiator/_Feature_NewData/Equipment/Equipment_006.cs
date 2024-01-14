using System.Collections.Generic;

namespace Feature_NewData{


    public class Equipment_006 {
        public readonly StatCalculator MoveSpeedCalculator = new StatCalculator(1.1f, E_STAT_CALC_TYPE.Add);
        public readonly StatCalculator AttackSpeedCalculator = new StatCalculator(1.05f, E_STAT_CALC_TYPE.Add);
    }
}