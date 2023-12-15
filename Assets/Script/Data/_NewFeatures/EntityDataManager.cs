using System;

namespace Feature_NewData
{
    public interface ITemporallyChangable {

    }

    public class EntityDataManager {
        public readonly EntityData BaseEntityData;
        public EntityData TemporalAddingData;
        public EntityData FinalEntityData;

        public void RecalculateTemporals(TemporalAddings temporalAddings){
            // TemporalAddingData.SetNumeric(BaseEntityData.GetNumeric() * temporalAddings.entityReferingRatioAmounts;); 
            // TemporalAddingData.SetFunctional(BaseEntityData.GetFunctional() * temporalAddings.entityReferingRatioAmounts);
        }

        public EntityData GetFinalData() {
            // return this.BaseEntityData + this.TemporalAddingData;
            throw new NotImplementedException();
        }
        public EntityData GetBaseData() {
            // return this.BaseEntityData;
            throw new NotImplementedException();
        }
    }
}

/*
언제 최종 결과물을 계산하는것은 누구인가? 매니저인가?
역시 얘가 맞는것 같긴한데..
*/
