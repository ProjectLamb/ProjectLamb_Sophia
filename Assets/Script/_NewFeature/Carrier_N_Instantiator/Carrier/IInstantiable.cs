using System;
using UnityEngine;

namespace Feature_NewData
{
    // T : 캐리어 이거나, 프로젝타일 이거나, VFXObject
    // U : Entity이거나 Carrier이거나 

    public interface IInstantiable<T, U> {

        public U    GetOwner();
        public bool GetIsInitialized();

        public T    Init(U owner);
        public T    InitByObject(U owner, object[] objects);
        public T    SetScale(float sizeRatio);
        public T    SetDurateTime(float time);

        public void Get();
        public void Activate();
        public void DeActivate();
        public void Release();

        public bool CheckIsSameOwner(U owner);
    }

    public interface IRepositionable<Instantable> {
        public void ActivateInstantable(MonoBehaviour entity, Instantable _carrier);
        public Quaternion   GetForwardingAngle(Quaternion instantiatorQuaternion);
        public Transform    GetTransformParent(Transform parent);

    }
    /*********************************************************************************

    public interface IAffectable
    {
        public void AffectHandler(AffectorPackage affectorPackage);
        protected void AffectToTarget(Collider target);
    }

    *********************************************************************************/
}