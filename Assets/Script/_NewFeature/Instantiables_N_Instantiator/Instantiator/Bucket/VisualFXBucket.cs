using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using TMPro;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    

    public class VisualFXBucket : MonoBehaviour
    {
        [SerializeField]  private float _bucketScale = 1f;

        private Dictionary<E_AFFECT_TYPE, VisualFXObject>   VisualStacks = new Dictionary<E_AFFECT_TYPE, VisualFXObject>();
        private UnityAction<E_AFFECT_TYPE>                  OnDestroyHandler;
        
        public float GetBucketSize() => _bucketScale * transform.lossyScale.z;

        private void Awake() {
            
            foreach(E_AFFECT_TYPE E in Enum.GetValues(typeof(E_AFFECT_TYPE))){ VisualStacks.Add(E, null); }
            OnDestroyHandler = DestroyHander;
        }

        public VisualFXObject InstantablePositioning(VisualFXObject instantiatedVFX)
        {
            Vector3     offset       = instantiatedVFX.transform.position;
            Vector3     position     = transform.position;
            Quaternion  forwardAngle = GetForwardingAngle(instantiatedVFX.transform.rotation);
            instantiatedVFX.transform.position = position;
            instantiatedVFX.transform.rotation = forwardAngle;

            switch (instantiatedVFX.PositioningType)
            {
                case E_INSTANTIATE_POSITION_TYPE.Inner   :
                {
                    instantiatedVFX.transform.SetParent(transform);
                    instantiatedVFX.SetScaleOverrideByRatio(instantiatedVFX.transform.localScale.z);
                    break;
                }
                case E_INSTANTIATE_POSITION_TYPE.Outer  :
                {
                    break;
                }
            }

            switch (instantiatedVFX.StackingType)
            {
                case E_INSTANTIATE_STACKING_TYPE.NonStack : 
                {
                    E_AFFECT_TYPE stateType = instantiatedVFX.AffectType;
                    if(VisualStacks.TryGetValue(stateType, out VisualFXObject value)){
                        if(value != null){
                            if(instantiatedVFX.DEBUG){Debug.Log($"현재 {instantiatedVFX.AffectType} Destroy For 재할당");}
                            value.DeActivate();
                        }
                    }
                    instantiatedVFX.OnRelease += () => OnDestroyHandler.Invoke(instantiatedVFX.AffectType);
                    if(instantiatedVFX.DEBUG){Debug.Log($"현재 {instantiatedVFX.AffectType} 할당");}
                    VisualStacks[stateType] = instantiatedVFX;
                    break;
                }
                case E_INSTANTIATE_STACKING_TYPE.Stack : 
                {
                    break;
                }
            }
            
            instantiatedVFX.transform.position += offset * transform.localScale.z;
            instantiatedVFX.SetScaleMultiplyByRatio(GetBucketSize());
            return instantiatedVFX;
        }

        public void RemoveInstantableFromBucket(E_AFFECT_TYPE affectType) {
            if(VisualStacks.TryGetValue(affectType, out VisualFXObject value)){
                if(value != null){
                    value.DeActivate();
                }
            }
        }

        private Quaternion GetForwardingAngle(Quaternion instantiatorQuaternion)
        {
            return Quaternion.Euler(transform.eulerAngles + instantiatorQuaternion.eulerAngles);
        }

        private Transform GetTransformParent(Transform instantiatorTransform)
        {
            instantiatorTransform.SetParent(this.transform);
            return instantiatorTransform;
        }

        private void DestroyHander(E_AFFECT_TYPE type) {
            VisualStacks[type] = null;
        }
    }
}