using Sophia.Instantiates;
using TMPro;
using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    using Entitys;
    public class LayerChangeAtomics {
        private GameObject gameObjectRef;
        private int prevLayer;
        private int changeLayer;
        
        public LayerChangeAtomics(GameObject gameObject, LayerMask layerMask) {
            gameObjectRef = gameObject;
            prevLayer = gameObjectRef.layer;
            changeLayer = layerMask.value;
        }
        public void Invoke() => gameObjectRef.layer = changeLayer;
        public void Revert() => gameObjectRef.layer = prevLayer;
    }

    public class VisualFXAtomics {
        private E_AFFECT_TYPE AffectType;
        private VisualFXObject visualFX;

        public VisualFXAtomics(E_AFFECT_TYPE affectType, SerialVisualAffectData serialVisualAffectData) {
            AffectType = affectType;
            visualFX = serialVisualAffectData._visualFxRef;
        }

        public void Invoke(IVisualAccessible visualAccessible) {
            VisualFXObject concreteVisualFX = VisualFXObjectPool.GetObject(visualFX);
            visualAccessible.GetVisualFXBucket().InstantablePositioning(concreteVisualFX).Activate();
        }
        public void Revert(IVisualAccessible visualAccessible) {
            visualAccessible.GetVisualFXBucket().RemoveInstantableFromBucket(AffectType);
        }
    }
}
