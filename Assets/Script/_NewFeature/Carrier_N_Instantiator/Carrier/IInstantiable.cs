using System;
using UnityEngine;

namespace Feature_NewData
{
    // T : 캐리어 이거나, 프로젝타일 이거나, VFXObject
    // U : Entity이거나 Carrier이거나 

    public interface IInstantiable<T, U> {

        public T    Clone();

        public U    GetOwner();

        public T    Init(U owner);
        public T    InitByObject(U owner, object[] objects);
        public T    SetPositionAndForwarding(Transform transform, Quaternion instantiatorAngle);
        public T    SetScale(float sizeRatio);
        public T    SetDurateTime(float time);

        public void Activate();
        public void DeActivate();
        public void Release();

        public bool CheckIsCloned();
        public bool CheckIsSameOwner(U owner);
    
    }
    /*********************************************************************************

    public interface IAffectable
    {
        public void AffectHandler(AffectorPackage affectorPackage);
        protected void AffectToTarget(Collider target);
    }

    *********************************************************************************/
}