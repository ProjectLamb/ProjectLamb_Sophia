using System;
using System.Threading;
using UnityEngine;

namespace Sophia.DataSystem.Functional {
    using Cysharp.Threading.Tasks;
    using Sophia.Instantiates;
    public class SkinCommand : IFunctionalToggleCommand<Entitys.Entity>
    {
        private CancellationTokenSource CtsRef;
        private Entitys.Entity OwnerRef;
        private Material MaterialRef;
        private float IntervalTime;

        public SkinCommand(SerialSkinAffectData serialSkinAffectData, CancellationTokenSource cts) {
            MaterialRef = serialSkinAffectData._materialRef;
            IntervalTime = serialSkinAffectData._intervalTime;
            CtsRef = cts;
        }
        
        public void Invoke(ref Entitys.Entity referer)
        {
            try
            {
                if (referer == null) return;
                referer.GetModelManger().ChangeSkin(CtsRef.Token, MaterialRef).Forget();
            }
            catch (OperationCanceledException)
            {

            }
        }

        public void Revert(ref Entitys.Entity referer)
        {
            try
            {
                if (referer == null) return;
                referer.GetModelManger().RevertSkin(CtsRef.Token).Forget();
            }
            catch (OperationCanceledException)
            {

            }
        }

        public string GetName() => "스킨";
        public string GetDescription() => "대상의 Material을 변경시킵니다.";

        public Sprite GetSprite()
        {
            throw new NotImplementedException();
        }
    }
}

/*
private SkinCommand         FunctionalSkinCommand;

public ...Affect SetMaterial(SerialSkinAffectData serialSkinAffectData)
{
    if(FunctionalSkinCommand != null) return this;

    FunctionalSkinCommand = new SkinCommand(serialSkinAffectData);
    void MeshOn() => FunctionalSkinCommand.Invoke(ref TargetRef);
    void MeshOff() => FunctionalSkinCommand.Revert(ref TargetRef);

    Timer.OnStart += MeshOn;
    Timer.OnFinished += MeshOff;
    return this;
}
*/
