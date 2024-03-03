using Sophia.Instantiates;
using TMPro;
using UnityEngine;

namespace Sophia.DataSystem.Atomics
{
    public class LayerChangeAtomics {
        public readonly GameObject gameObjectRef;
        public readonly int prevLayer;
        public readonly int changeLayer;
        
        public LayerChangeAtomics(GameObject gameObject, LayerMask layerMask) {
            gameObjectRef = gameObject;
            prevLayer = gameObjectRef.layer;
            changeLayer = layerMask.value;
        }
        public void Invoke(IVisualAccessible modelAccessible) => modelAccessible.GetModelManger().GetModelObject().layer = changeLayer;
        public void Revert(IVisualAccessible modelAccessible) => modelAccessible.GetModelManger().GetModelObject().layer = prevLayer;
    }

    public class VisualFXAtomics {
        public readonly E_AFFECT_TYPE AffectType;
        public readonly VisualFXObject visualFX;

        public VisualFXAtomics(E_AFFECT_TYPE affectType,in SerialVisualData serialVisualAffectData) {
            AffectType = affectType;
            visualFX = serialVisualAffectData._visualFxRef;
        }

        public void Invoke(IVisualAccessible visualAccessible) {
            VisualFXObject concreteVisualFX = VisualFXObjectPool.GetObject(visualFX).Init();
            visualAccessible.GetVisualFXBucket().InstantablePositioning(concreteVisualFX).Activate();
        }
        public void Revert(IVisualAccessible visualAccessible) {
            visualAccessible.GetVisualFXBucket().RemoveInstantableFromBucket(AffectType);
        }
    }
}
