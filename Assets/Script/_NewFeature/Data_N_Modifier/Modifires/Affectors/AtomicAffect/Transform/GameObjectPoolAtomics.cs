using Sophia.Instantiates;
using TMPro;
using UnityEngine;

namespace Sophia.DataSystem.Modifiers
{
    
    public class LayerChangeAtomics {
        private GameObject gameObjectRef;
        private int prevLayer;
        private int changeLayer;
        
        public LayerChangeAtomics(GameObject gameObject, LayerMask layerMask) {
            gameObjectRef = gameObject;
            prevLayer = gameObjectRef.layer;
            changeLayer = layerMask.value;
        }
        public void Invoke(IVisualAccessible modelAccessible) => modelAccessible.GetModelManger().GetModelObject().layer = changeLayer;
        public void Revert(IVisualAccessible modelAccessible) => modelAccessible.GetModelManger().GetModelObject().layer = prevLayer;
    }

    public class VisualFXAtomics {
        private E_AFFECT_TYPE AffectType;
        private VisualFXObject visualFX;

        public VisualFXAtomics(E_AFFECT_TYPE affectType, SerialVisualAffectData serialVisualAffectData) {
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
