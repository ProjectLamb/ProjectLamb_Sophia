using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Composite;

    public class ProjectileBucket : MonoBehaviour
    {        

#region SerializeMember
        [SerializeField] private float _bucketScale = 1f;

#endregion

#region Member
        
        private ProjectileBucketManager projectileBucketManagerRef;

#endregion

#region Getter

        public float GetBucketSize() => _bucketScale * transform.lossyScale.z;

#endregion

#region Setter
        public void SetProjectileBucketManamger(ProjectileBucketManager projectileBucketManager) => projectileBucketManagerRef = projectileBucketManager;
#endregion

        public ProjectileObject InstantablePositioning(ProjectileObject instantiatedProjectile, Vector3 _positionOffset, Vector3 _rotateOffset)
        {
            Vector3     offset       = _positionOffset;
            Vector3     position     = transform.position;
            Quaternion  forwardAngle = GetForwardingAngle(instantiatedProjectile.transform.rotation);
            Quaternion  rotateAngle  = Quaternion.Euler(_rotateOffset);

            instantiatedProjectile.transform.position = position;
            instantiatedProjectile.transform.rotation = Quaternion.Euler(forwardAngle.eulerAngles + rotateAngle.eulerAngles);

            
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
            instantiatedProjectile.SetDurateTimeByRatio(projectileBucketManagerRef.InstantiableDurateLifeTimeMultiplyRatio.GetValueForce())
                                .SetScaleMultiplyByRatio(GetBucketSize())
                                .SetScaleMultiplyByRatio(projectileBucketManagerRef.InstantiableSizeMultiplyRatio.GetValueForce())
                                .SetForwardingSpeedByRatio(projectileBucketManagerRef.InstantiableForwardingSpeedMultiplyRatio.GetValueForce())
                                .SetOnProjectileCreatedEvent(projectileBucketManagerRef.OnCreated)
                                .SetOnProjectileForwardingEvent(projectileBucketManagerRef.OnTriggerd)
                                .SetOnProjectileReleasedEvent(projectileBucketManagerRef.OnReleased)
                                .SetOnProjectileTriggerdEvent(projectileBucketManagerRef.OnForwarding);
            
            return instantiatedProjectile;
        }

        public ProjectileObject InstantablePositioning(ProjectileObject instantiatable) => InstantablePositioning(instantiatable, Vector3.zero, Vector3.zero);

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