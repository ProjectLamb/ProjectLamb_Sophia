using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    using Sophia.Entitys;
    using Unity.VisualScripting;

    public class ResizeScaleAtomics {
        public Transform transformRef;

        public ResizeScaleAtomics(Transform transform) {
            transformRef = transform;
        }
        public void Invoke() {}
        public void Revert() {}
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
        private Entity       entityRef;
        private Transform    targetTransformRef;
        private Vector3    targetVector;
        private float intervalTimeAmount;
        private float forceAmount;


        public RigidGradualAtomics(Entity entity, Transform transform, float force, float intervalTime) {
            entityRef = entity;
            targetTransformRef = transform;
            forceAmount = force;
            intervalTimeAmount = intervalTime;

        }

        public RigidGradualAtomics(Entity entity, Vector3 vector, float force, float intervalTime) {
            entityRef = entity;
            targetVector = vector;
            forceAmount = force;
            intervalTimeAmount = intervalTime;
        }

        public void Invoke() {
            entityRef.entityRigidbody.velocity = Vector3.zero;
        }
        public void Run() {
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
        public void Revert() {
            entityRef.entityRigidbody.velocity = Vector3.zero;
        }
    }

    public class RigidImpulseAtomics {
        private Entity       entityRef;
        private Transform    targetTransformRef;
        private Vector3    targetVector;
        private float forceAmount;


        public RigidImpulseAtomics(Entity entity, Transform transform, float force) {
            entityRef = entity;
            targetTransformRef = transform;
            forceAmount = force;

        }

        public RigidImpulseAtomics(Entity entity, Vector3 vector, float force) {
            entityRef = entity;
            targetVector = vector;
            forceAmount = force;
        }

        public void Invoke() {
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