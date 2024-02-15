using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Numerics;
    public class ProjectileBucket : MonoBehaviour
    {        

#region Serialize
        [SerializeField] private SerialBaseInstantiatorData _baseInstantiatorData;
        [SerializeField] private Entity _ownerRef;
        [SerializeField] private float _bucketScale = 1f;

#endregion

#region Member

        public Stat InstantiableDurateLifeTimeMultiplyRatio {get; protected set;}
        public Stat InstantiableSizeMultiplyRatio {get; protected set;}
        public Stat InstantiableForwardingSpeedMultiplyRatio {get; protected set;}

        public Extras<object> AttackExtras {get; protected set;}
        public Extras<object> CreatedExtras {get; protected set;}
        public Extras<object> TriggerdExtras {get; protected set;}
        public Extras<object> ReleasedExtras {get; protected set;}
        public Extras<object> ForwardingExtras {get; protected set;}

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
        
#endregion

#region Getter
        public float GetBucketSize() => _bucketScale * transform.lossyScale.z;
#endregion

        public ProjectileObject ActivateInstantable(ProjectileObject _instantiatable, Vector3 _offset)
        {
            ProjectileObject instantiatedProjectile = ProjectilePool.Instance.ProPool[_instantiatable.gameObject.name].Get();
            instantiatedProjectile.Init(_ownerRef);
            
            Vector3     offset       = _offset;
            Vector3     position     = transform.position;
            Quaternion  forwardAngle = GetForwardingAngle(instantiatedProjectile.transform.rotation);
            instantiatedProjectile.transform.position = position;
            instantiatedProjectile.transform.rotation = forwardAngle;

            
            switch (instantiatedProjectile.PositioningType)
            {
                case E_INSTANTIATE_POSITION_TYPE.Inner   :
                {
                    instantiatedProjectile.transform.SetParent(transform);
                    instantiatedProjectile.SetScaleOverrideByRatio(transform.localScale.z);
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
            
            instantiatedProjectile.transform.position += offset * GetBucketSize();
            instantiatedProjectile.SetDurateTimeByRatio(InstantiableDurateLifeTimeMultiplyRatio.GetValueForce())
                                .SetScaleMultiplyByRatio(GetBucketSize())
                                .SetScaleMultiplyByRatio(InstantiableSizeMultiplyRatio.GetValueForce())
                                .SetForwardingSpeedByRatio(InstantiableForwardingSpeedMultiplyRatio.GetValueForce())
                                .SetOnProjectileCreatedEvent(OnCreated)
                                .SetOnProjectileForwardingEvent(OnTriggerd)
                                .SetOnProjectileReleasedEvent(OnReleased)
                                .SetOnProjectileTriggerdEvent(OnForwarding);
            
            return instantiatedProjectile;
        }

        public ProjectileObject ActivateInstantable(ProjectileObject _instantiatable)
        {
            ProjectileObject instantiatedProjectile = ProjectilePool.Instance.ProPool[_instantiatable.gameObject.name].Get();
            instantiatedProjectile.Init(_ownerRef);
            
            Vector3     offset       = _instantiatable.transform.position;
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
            
            instantiatedProjectile.transform.position += offset * this.GetBucketSize();
            instantiatedProjectile.SetDurateTimeByRatio(InstantiableDurateLifeTimeMultiplyRatio.GetValueForce())
                                .SetScaleOverrideByRatio(this.GetBucketSize())
                                .SetScaleMultiplyByRatio(InstantiableSizeMultiplyRatio.GetValueForce())
                                .SetForwardingSpeedByRatio(InstantiableForwardingSpeedMultiplyRatio.GetValueForce())
                                .SetOnProjectileCreatedEvent(OnCreated)
                                .SetOnProjectileForwardingEvent(OnTriggerd)
                                .SetOnProjectileReleasedEvent(OnReleased)
                                .SetOnProjectileTriggerdEvent(OnForwarding);
            
            return instantiatedProjectile;
        }

        private Quaternion GetForwardingAngle(Quaternion instantiatorQuaternion)
        {
            return Quaternion.Euler(transform.eulerAngles + instantiatorQuaternion.eulerAngles);
        }

        private Transform GetTransformParent(Transform instantiatorTransform)
        {
            instantiatorTransform.SetParent(this.transform);
            return instantiatorTransform;
        }
    }
}