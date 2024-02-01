using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using UnityEngine.Events;

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

    public interface IPoolAccesable {
        public void SetByPool<T>(IObjectPool<T> pool) where T : MonoBehaviour;
        public void GetByPool();
        public void ReleaseByPool();
        public void SetPoolEvents(UnityAction activated, UnityAction deActivated, UnityAction release);
        public event UnityAction OnActivated;
        public event UnityAction OnRelease;
    }

    public interface IColliderTriggerable {
        public void ColliderTriggerHandle(Collider target);
    }

    public interface IInstantiator<Instantable> {
        /*기존 코드는 Actiavete의 책임이 있었는데 지금은 그냥 객체 리턴을 하므로 엄연히 활성화 단계는 함수 호출부에서 해야 할것이다*/
        public Instantable InstantablePositioning(Instantable _carrier, Vector3 _offset);
        public Instantable InstantablePositioning(Instantable _carrier);
    }

    /*********************************************************************************

    public interface IAffectable
    {
        public void AffectHandler(AffectorPackage affectorPackage);
        protected void AffectToTarget(Collider target);
    }

    *********************************************************************************/
}