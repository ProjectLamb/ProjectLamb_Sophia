using UnityEngine;
using DG.Tweening;

namespace Sophia.DataSystem.Atomics
{
    public class RigidGradualAtomics {
        public readonly Transform    OwnerTransformRef;
        public readonly Vector3    targetVector;
        public readonly float intervalTimeAmount;
        public readonly float forceAmount;

        public RigidGradualAtomics(Transform transform, float force, float intervalTime) {
            OwnerTransformRef = transform;
            forceAmount = force;
            intervalTimeAmount = intervalTime;

        }

        public RigidGradualAtomics(Vector3 vector, float force, float intervalTime) {
            targetVector = vector;
            forceAmount = force;
            intervalTimeAmount = intervalTime;
        }

        public void Invoke(Entitys.Entity entityRef) {
            entityRef.entityRigidbody.velocity = Vector3.zero;
        }
        public void Run(Entitys.Entity entityRef) {
            if(OwnerTransformRef != null) {
                entityRef.entityRigidbody.AddForce(
                    Vector3.Normalize(targetVector) * forceAmount * intervalTimeAmount, 
                    ForceMode.VelocityChange
                );
            }
            else {
                entityRef.entityRigidbody.AddForce(
                    Vector3.Normalize(OwnerTransformRef.position) * forceAmount * intervalTimeAmount, 
                    ForceMode.VelocityChange
                );
            }
        }
        public void Revert(Entitys.Entity entityRef) {
            entityRef.entityRigidbody.velocity = Vector3.zero;
        }
    }

    public class RigidImpulseAtomics {
        public readonly Transform   OwnerTransformRef;
        public readonly Vector3     targetVector;
        public readonly float forceAmount;


        public RigidImpulseAtomics(Transform transform, float force) {
            OwnerTransformRef = transform;
            forceAmount = force;
        }

        public RigidImpulseAtomics(Vector3 vector, float force) {
            targetVector = vector;
            forceAmount = force;
        }

        public void Invoke(Entitys.Entity entityRef) {
            if(OwnerTransformRef != null) {
                Vector3 vector3 = Vector3.Normalize(entityRef.GetGameObject().transform.position - OwnerTransformRef.position);
                entityRef.entityRigidbody.AddForce(
                    vector3 * forceAmount,
                    ForceMode.Impulse
                );
            }
            else {
                entityRef.entityRigidbody.AddForce(
                    Vector3.Normalize(targetVector) * forceAmount,
                    ForceMode.Impulse
                );
            }
        }
    }
}