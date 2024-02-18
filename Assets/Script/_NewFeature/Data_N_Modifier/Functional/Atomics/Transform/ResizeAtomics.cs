using UnityEngine;
using DG.Tweening;

namespace Sophia.DataSystem.Atomics
{
    
    public class ResizeScaleAtomics {
        private Vector3 originScale;
        public float afterScaleAmountRef;

        public ResizeScaleAtomics(float scaleAmount, Entitys.Entity entity) {
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
}