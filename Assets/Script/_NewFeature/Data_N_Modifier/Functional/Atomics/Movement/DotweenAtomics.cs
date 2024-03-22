using UnityEngine;
using DG.Tweening;

namespace Sophia.DataSystem.Atomics {
    public class TweenJumpTransformAtomics {
        private float forceAmount;
        private float airingTime;
        public TweenJumpTransformAtomics(float force, float durateTime) {
            forceAmount = force;
            airingTime  = durateTime;
        }
        public void Invoke(Entitys.Entity entityRef) {
            GameObject entityModel = entityRef.GetModelManger().GetModelObject();
            entityModel.transform.DOLocalJump(UnityEngine.Vector3.zero, forceAmount, 1, airingTime);
        } 
    }
}