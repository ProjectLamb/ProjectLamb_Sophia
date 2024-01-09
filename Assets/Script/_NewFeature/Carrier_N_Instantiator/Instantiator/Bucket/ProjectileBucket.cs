using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Numerics;
    public class ProjectileBucket : MonoBehaviour, IInstantiator<Projectile>
    {        

#region Serialize
        [SerializeField] private SerialBaseInstantiatorData _baseInstantiatorData;
        [SerializeField] private Entity _ownerRef;
        [SerializeField] private float _bucketScale = 1f;

#endregion

        public Stat InstantiableDurateLifeTimeMultiplyRatio {get; protected set;}
        public Stat InstantiableSizeMultiplyRatio {get; protected set;}
        public Stat InstantiableForwardingSpeedMultiplyRatio {get; protected set;}

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
            InstantiableDurateLifeTimeMultiplyRatio = new Stat(_baseInstantiatorData.InstantiableDurateLifeTimeMultiplyRatio,
                E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio,
                E_STAT_USE_TYPE.Ratio, OnDurateLifeTime
            );
            InstantiableSizeMultiplyRatio = new Stat(_baseInstantiatorData.InstantiableSizeMultiplyRatio,
                E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio,
                E_STAT_USE_TYPE.Ratio, OnRatioSize
            );
            InstantiableForwardingSpeedMultiplyRatio = new Stat(_baseInstantiatorData.InstantiableForwardingSpeedMultiplyRatio,
                E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio,
                E_STAT_USE_TYPE.Ratio, OnForwardingSpeed
            );

            OnCreated       ??= () => {};
            OnTriggerd      ??= () => {};
            OnReleased      ??= () => {};
            OnForwarding    ??= () => {};
        }
        
#endregion

        public Projectile ActivateInstantable(Entity entityRef, Projectile _instantiatable)
        {
            Projectile instantiatedProjectile = ProjectilePool.Instance.ProPool[_instantiatable.gameObject.name].Get();
            instantiatedProjectile.Init(entityRef);
            
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
            instantiatedProjectile.SetDurateTimeByRatio(InstantiableDurateLifeTimeMultiplyRatio.GetValueForce())
                                .SetScaleByRatio(InstantiableSizeMultiplyRatio.GetValueForce())
                                .SetForwardingSpeedByRatio(InstantiableForwardingSpeedMultiplyRatio.GetValueForce())
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