using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Feature_NewData
{
    public class ProjectileBucket : MonoBehaviour, IRepositionable<Projectile>
    {        

#region Serialize

        [SerializeField] private float BucketScale = 1f;
        [SerializeField] private float MultiplyDurateLifeTime = 1f;
        [SerializeField] private float MultiplyRatioSize = 1f;
        [SerializeField] private float MultiplyForwardingSpeed = 1f;
        [SerializeField] public Entity ownerEntity;

#endregion

        public Stat RatioDurateLifeTime {get; protected set;}
        public Stat RatioSize {get; protected set;}
        public Stat RatioForwardingSpeed {get; protected set;}


        #region Event
        /*
        당연히 Functional로 관리를 해야 하지만 Projectil이 엄연히 다음 이벤트를 가지고 있는것은 사실이니.
        이벤트 주입을 하자.
        */
        
        public event UnityAction OnCreated = null;
        public event UnityAction OnTriggerd = null;
        public event UnityAction OnReleased = null;
        public event UnityAction OnForwarding = null;

        protected void OnDurateLifeTime() {
            throw new System.NotImplementedException();
        }
        protected void OnRatioSize() {
            throw new System.NotImplementedException();
        }
        protected void OnForwardingSpeed() {
            throw new System.NotImplementedException();
        }

        private void Awake() {
            RatioDurateLifeTime = new Stat(MultiplyDurateLifeTime,
                E_NUMERIC_STAT_TYPE.DurateLifeTime,
                E_STAT_USE_TYPE.Ratio, OnDurateLifeTime
            );
            RatioSize = new Stat(MultiplyRatioSize,
                E_NUMERIC_STAT_TYPE.Size,
                E_STAT_USE_TYPE.Ratio, OnRatioSize
            );
            RatioForwardingSpeed = new Stat(MultiplyForwardingSpeed,
                E_NUMERIC_STAT_TYPE.ForwardingSpeed,
                E_STAT_USE_TYPE.Ratio, OnForwardingSpeed
            );

            OnCreated       ??= () => {};
            OnTriggerd      ??= () => {};
            OnReleased      ??= () => {};
            OnForwarding    ??= () => {};

            if(ownerEntity == null) new System.Exception("프로젝타일 버킷의 주인, 현재 엔티티가 지정되지 않음");
        }

        #endregion

        public Projectile ActivateInstantable(MonoBehaviour entity, Projectile _instantiatable)
        {
            Projectile instantiatedProjectile = ProjectilePool.Instance.ProPool[_instantiatable.gameObject.name].Get();
            instantiatedProjectile.Init(ownerEntity);
            
            Vector3     offset       = instantiatedProjectile.transform.position;
            Vector3     position     = transform.position;
            Quaternion  forwardAngle = GetForwardingAngle(instantiatedProjectile.transform.rotation);
            instantiatedProjectile.transform.position = position;
            instantiatedProjectile.transform.rotation = forwardAngle;

            
            switch (instantiatedProjectile.PositioningType)
            {
                case E_INSTANTIATE_POSITION_TYPE.Inner   :
                {
                    instantiatedProjectile.transform.SetParent(transform);   
                    break;
                }
                case E_INSTANTIATE_POSITION_TYPE.Outer  :
                {
                    break;
                }
            }

            switch (instantiatedProjectile.StackingType)
            {
                case E_INSTANTIATE_STACKING_TYPE.NonStack : 
                {
                    break;
                }
                case E_INSTANTIATE_STACKING_TYPE.Stack : 
                {
                    break;
                }
            }
            
            instantiatedProjectile.transform.position += offset * transform.localScale.z;
            instantiatedProjectile.SetDurateTimeByRatio(RatioDurateLifeTime)
                                .SetScaleByRatio(RatioSize)
                                .SetForwardingSpeedByRatio(RatioForwardingSpeed)
                                .SetOnProjectileCreatedEvent(OnCreated)
                                .SetOnProjectileForwardingEvent(OnTriggerd)
                                .SetOnProjectileReleasedEvent(OnReleased)
                                .SetOnProjectileTriggerdEvent(OnForwarding);
            
            return instantiatedProjectile;
        }

        public Quaternion GetForwardingAngle(Quaternion instantiatorQuaternion)
        {
            return Quaternion.Euler(transform.eulerAngles + instantiatorQuaternion.eulerAngles);
        }

        public Transform GetTransformParent(Transform instantiatorTransform)
        {
            instantiatorTransform.SetParent(this.transform);
            return instantiatorTransform;
        }
    }
}