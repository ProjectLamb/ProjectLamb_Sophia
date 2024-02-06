using System;

namespace Sophia.DataSystem.Functional {
    using Sophia.DataSystem.Modifiers;
    using Sophia.Entitys;
    using UnityEngine;

    public class TemporaryModifiyCommand : IFunctionalToggleCommand<Entitys.Entity>
    {
        private Entitys.Entity OwnerRef;
        private StatModifier TemporaryModifier;
        
        public TemporaryModifiyCommand(SerialStatModifireDatas serialModifireDatas, E_NUMERIC_STAT_TYPE statType) {
            TemporaryModifier = new StatModifier(serialModifireDatas.amount, serialModifireDatas.calType, statType);
        }

        public string GetName() => "스텟 변경";
        public string GetDescription() => $"대상의 {TemporaryModifier.StatType}스텟을 {TemporaryModifier.Value}만큼 변경시킵니다.";

        public Sprite GetSprite()
        {
            throw new NotImplementedException();
        }

        public void Invoke(ref Entity referer)
        {
            referer.GetStatReferer().GetStat(TemporaryModifier.StatType).AddModifier(TemporaryModifier);
        }

        public void Revert(ref Entity referer)
        {
            referer.GetStatReferer().GetStat(TemporaryModifier.StatType).RemoveModifier(TemporaryModifier);
        }
    }
}

/*
private TemporaryModifiyCommand FunctionalTemporaryModifiyCommand;

public ...Affect SetModifyData(SerialStatModifireDatas ModifiyData)
{
    if(FunctionalTemporaryModifiyCommand != null) return this;

    FunctionalTemporaryModifiyCommand = new TemporaryModifiyCommand(ModifiyData, E_NUMERIC_STAT_TYPE.MoveSpeed);
    void Apply() => FunctionalTemporaryModifiyCommand.Invoke(ref TargetRef);
    void Unapply() => FunctionalTemporaryModifiyCommand.Revert(ref TargetRef);
    
    Timer.OnStart += Apply;
    Timer.OnFinished += Unapply;
    return this;
}
*/