using Sophia.Instantiates;
using UnityEngine;

namespace Sophia.DataSystem.Functional {
    public class VisualFXCommand : IFunctionalToggleCommand<Entitys.Entity>
    {
        private Entitys.Entity OwnerRef;
        private VisualFXObject VisualFXRef;
        private E_AFFECT_TYPE AffectType;
        private float IntervalTime;
        public VisualFXCommand(E_AFFECT_TYPE affectType, SerialVisualAffectData serialVisualAffectData) {
            AffectType = affectType;
            VisualFXRef = serialVisualAffectData._visualFxRef;
            IntervalTime  = serialVisualAffectData._intervalTime;
        }

        public string GetName() => "파티클 소환";
        public string GetDescription() => "대상의 VisualBucket위치에 파티클을 생성합니다";
        public Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }

        public void Invoke(ref Entitys.Entity referer)
        {
            VisualFXObject concreteVisualFX = VisualFXObjectPool.GetObject(VisualFXRef, OwnerRef);
            referer.GetVisualFXBucket().InstantablePositioning(concreteVisualFX).Activate();
        
        }

        public void Revert(ref Entitys.Entity referer)
        {
            referer.GetVisualFXBucket().RemoveInstantableFromBucket(AffectType);
        }
    }
}

/*
private VisualFXCommand     FunctionalVisualFXCommand;

public PoisonedAffect SetVisualFXObject(SerialVisualAffectData serialVisualAffectData)
{
    if(FunctionalVisualFXCommand != null) return this;

    FunctionalVisualFXCommand = new VisualFXCommand(E_AFFECT_TYPE.Poisoned, serialVisualAffectData);
    void VFXOn() => FunctionalVisualFXCommand.Invoke(ref TargetRef);
    void VFXOff() => FunctionalVisualFXCommand.Revert(ref TargetRef);

    Timer.OnStart += VFXOn;
    Timer.OnFinished += VFXOff;
    return this;
}
*/