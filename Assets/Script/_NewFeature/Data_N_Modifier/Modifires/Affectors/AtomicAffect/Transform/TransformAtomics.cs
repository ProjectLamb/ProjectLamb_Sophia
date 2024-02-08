using UnityEngine;

namespace Sophia.DataSystem.Modifiers
{
    using Sophia.Entitys;
    using Unity.VisualScripting;

    public class ResizeScaleAtomics {
        public Vector3 originScale;
        public float afterScaleAmountRef;

        public ResizeScaleAtomics(float scaleAmount) {
            afterScaleAmountRef = scaleAmount;
        }
        public void Invoke(IVisualAccessible modelAccessible) {
            originScale = modelAccessible.GetModelManger().GetModelObject().transform.localScale;
            modelAccessible.GetModelManger().GetModelObject().transform.localScale = modelAccessible.GetModelManger().GetModelObject().transform.localScale * afterScaleAmountRef;
        }
        public void Revert(IVisualAccessible modelAccessible) {
            modelAccessible.GetModelManger().GetModelObject().transform.localScale = originScale;
        }
    }

    public class TeleportAtomics {
        public Transform transformRef;
        public Vector3 teleportPos;
        public TeleportAtomics(Transform transform, ref Vector3 teleportPos) {
            transformRef = transform;
            this.teleportPos = teleportPos;
        }
        public void Invoke() {
            transformRef.position = teleportPos;
        }
        public void Revert() {
            transformRef.position = teleportPos;
        }
    }

    public class RigidGradualAtomics {
        private Transform    targetTransformRef;
        private Vector3    targetVector;
        private float intervalTimeAmount;
        private float forceAmount;


        public RigidGradualAtomics(Transform transform, float force, float intervalTime) {
            targetTransformRef = transform;
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
            if(targetTransformRef != null) {
                entityRef.entityRigidbody.AddForce(
                    Vector3.Normalize(targetVector) * forceAmount * intervalTimeAmount, 
                    ForceMode.VelocityChange
                );
            }
            else {
                entityRef.entityRigidbody.AddForce(
                    Vector3.Normalize(targetTransformRef.position) * forceAmount * intervalTimeAmount, 
                    ForceMode.VelocityChange
                );
            }
        }
        public void Revert(Entitys.Entity entityRef) {
            entityRef.entityRigidbody.velocity = Vector3.zero;
        }
    }

    public class RigidImpulseAtomics {
        private Transform    targetTransformRef;
        private Vector3    targetVector;
        private float forceAmount;


        public RigidImpulseAtomics(Transform transform, float force) {
            targetTransformRef = transform;
            forceAmount = force;

        }

        public RigidImpulseAtomics(Vector3 vector, float force) {
            targetVector = vector;
            forceAmount = force;
        }

        public void Invoke(Entitys.Entity entityRef) {
            if(targetTransformRef != null) {
                entityRef.entityRigidbody.AddForce(
                    Vector3.Normalize(targetVector) * forceAmount,
                    ForceMode.Impulse
                );
            }
            else {
                entityRef.entityRigidbody.AddForce(
                    Vector3.Normalize(targetTransformRef.position) * forceAmount,
                    ForceMode.Impulse
                );
            }
        }
    }
}