using UnityEngine;
using DG.Tweening;

namespace Sophia.DataSystem.Modifiers
{
    using Sophia.Entitys;
    

    public class ResizeScaleAtomics {
        private Vector3 originScale;
        public float afterScaleAmountRef;

        public ResizeScaleAtomics(float scaleAmount, Entity entity) {
            originScale = entity.GetModelManger().GetModelObject().transform.localScale;
            afterScaleAmountRef = scaleAmount;
        }
        public void Invoke(IVisualAccessible modelAccessible) {
            modelAccessible.GetModelManger().GetModelObject().transform.localScale = modelAccessible.GetModelManger().GetModelObject().transform.localScale * afterScaleAmountRef;
        }
        public void Revert(IVisualAccessible modelAccessible) {
            modelAccessible.GetModelManger().GetModelObject().transform.localScale = originScale;
        }
    }

    public class TeleportAtomics {
        public readonly Transform transformRef;
        public readonly Vector3 teleportPos;
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
        public readonly Transform    OwnerTransformRef;
        public readonly Vector3    targetVector;
        public readonly float forceAmount;


        public RigidImpulseAtomics(Transform transform, float force) {
            OwnerTransformRef = transform;
            forceAmount = force;

        }

        public RigidImpulseAtomics(Vector3 vector, float force) {
            targetVector = vector;
            forceAmount = force;
        }

        public void Invoke(Entity entityRef) {
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
    
    public class TweenJumpTransformAtomics {
        private float forceAmount;
        private float airingTime;
        public TweenJumpTransformAtomics(float force, float durateTime) {
            forceAmount = force;
            airingTime  = durateTime;
        }
        public void Invoke(Entity entityRef) {
            GameObject entityModel = entityRef.GetModelManger().GetModelObject();
            entityModel.transform.DOLocalJump(UnityEngine.Vector3.zero, forceAmount, 1, airingTime);
        } 
    }
}