using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
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

    public interface IPoolAccesable<T> where T : class {
        public void SetByPool(IObjectPool<T> pool);
        public void GetByPool();
        public void ReleaseByPool();
    }

    public interface IInstantiable {
        public void Activate();
        public void DeActivate();
    }

    public interface IColliderTriggerable {
        public void ColliderTriggerHandle(Collider target);
    }

    public interface IInstantiator<Instantable> {
        /*기존 코드는 Actiavete의 책임이 있었는데 지금은 그냥 객체 리턴을 하므로 엄연히 활성화 단계는 함수 호출부에서 해야 할것이다*/
        public Instantable ActivateInstantable(Instantable _carrier, Vector3 _offset);
        public Instantable ActivateInstantable(Instantable _carrier);
    }

    /*********************************************************************************

    public interface IAffectable
    {
        public void AffectHandler(AffectorPackage affectorPackage);
        protected void AffectToTarget(Collider target);
    }

    *********************************************************************************/
}