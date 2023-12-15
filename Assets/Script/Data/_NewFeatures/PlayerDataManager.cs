namespace Feature_NewData
{

    public class PlayerDataManager {
        public readonly PlayerData BasePlayerData;
        public PlayerData CalculatedPlayerData;


        /*
        개선해야 할 구조. 무기의 데이터를 가져오는것은 뭐지?
        */

        public SkillData BaseSkillData;
        public SkillData OnUsePostSkillAddingData;

        public EntityData TemporalAddingData; //Player를 레퍼드한다.


        public void RecalculateNonTemoralsCalculayed(NonTemporalCalculatedAddings addings){
            // CalculatedPlayerData.SetNumeric(
            //     (BasePlayerData) (BasePlayerData.GetNumeric() + addings.playerFixedAmounts)
            //     +
            //     (BasePlayerData) (BasePlayerData.GetNumeric() * addings.playerReferingRatioAmounts)
            // );

            // CalculatedPlayerData.SetFunctional(BasePlayerData.GetFunctional() + addings.playerAdditionalFunctional);
        }
        

        public void RecalculateNonTemoralsOnUsePostSkill(NonTemporalOnUsePostSkillAddings skillAddings){
            // OnUsePostSkillAddingData.SetNumeric(BaseSkillData.GetNumeric * skillAddings.SkillReferingRatioAmount);
            // OnUsePostSkillAddingData.SetFunctional(BaseSkillData.GetFunctional + skillAddings.SkillAdditionalFunctionals);
        }
        
        public void RecalculateTemporals(TemporalAddings temporalAddings){
            // CalculatedPlayerData.SetNumeric(BasePlayerData.GetNumeric() * temporalAddings.playerReferingRatioAmounts;); 
            // CalculatedPlayerData.SetFunctional(BasePlayerData.GetFunctional() * temporalAddings.playerReferingRatioAmounts);
        }

        public PlayerData GetPlayerBaseData() {
            // return this.GetPlayerBaseData;
            throw new NotImplementedException();
        }

        public PlayerData GetCalculatedData() {
            // return this.GetCalculatedData;
            throw new NotImplementedException();
        }

    }
}