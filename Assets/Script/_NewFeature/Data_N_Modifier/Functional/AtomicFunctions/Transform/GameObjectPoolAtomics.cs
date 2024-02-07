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
        private VisualFXObject VisualFXRef;
        private E_AFFECT_TYPE AffectType;
        private Entity OwnerRef;

        public VisualFXAtomics(E_AFFECT_TYPE affectType, Entity owner, SerialVisualAffectData serialVisualAffectData) {
            AffectType = affectType;
            VisualFXRef = serialVisualAffectData._visualFxRef;
            OwnerRef = owner;
        }

        public void Invoke() {
            VisualFXObject concreteVisualFX = VisualFXObjectPool.GetObject(VisualFXRef);
            OwnerRef.GetVisualFXBucket().InstantablePositioning(concreteVisualFX).Activate();
        }
        public void Revert() {
            OwnerRef.GetVisualFXBucket().RemoveInstantableFromBucket(AffectType);
        }
    }
}
