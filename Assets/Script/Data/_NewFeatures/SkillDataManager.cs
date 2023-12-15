using System;

namespace Feature_NewData
{

    public class SkillDataManager {
        public SkillData BaseSkillData;
        public SkillData OnUsePostSkillAddingData;

        public void RecalculateNonTemoralsOnUsePostSkill(NonTemporalOnUsePostSkillAddings skillAddings){
            // OnUsePostSkillAddingData.SetNumeric(BaseSkillData.GetNumeric * skillAddings.SkillReferingRatioAmount);
            // OnUsePostSkillAddingData.SetFunctional(BaseSkillData.GetFunctional + skillAddings.SkillAdditionalFunctionals);
        }


        public SkillData GetSkillData() {
            // return BaseSkillData + OnUsePostSkillAddingData;
            throw new NotImplementedException();
        }
    }
}