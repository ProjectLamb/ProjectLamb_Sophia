using System;
using UnityEngine;

namespace Sophia.Instantiates
{
    // T : 캐리어 이거나, 프로젝타일 이거나, VFXObject
    // U : Entity이거나 Carrier이거나 
    public enum E_INSTANTIATE_STACKING_TYPE
    {
        NonStack, Stack
    }

    public enum E_INSTANTIATE_POSITION_TYPE
    {
        Inner, Outer
    }

    public interface IInstantiable<T, U> {

        public U    GetOwner();
        public bool GetIsInitialized();

        public T    Init(U owner);
        public T    InitByObject(U owner, object[] objects);
        public T    SetScaleByRatio(float sizeRatio);
        public T    SetDurateTimeByRatio(float time);

        public void Get();
        public void Activate();
        public void DeActivate();
        public void Release();

        public bool CheckIsSameOwner(U owner);
    }

    public interface IRepositionable<Instantable> {
        /*기존 코드는 Actiavete의 책임이 있었는데 지금은 그냥 객체 리턴을 하므로 엄연히 활성화 단계는 함수 호출부에서 해야 할것이다*/
        public Instantable ActivateInstantable(MonoBehaviour entity, Instantable _carrier);
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