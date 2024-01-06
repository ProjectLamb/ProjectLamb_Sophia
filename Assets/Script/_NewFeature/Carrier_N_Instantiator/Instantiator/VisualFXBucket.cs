using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using TMPro;

namespace Feature_NewData
{
    public class VisualFXBucket : MonoBehaviour, IRepositionable<VisualFXObject>
    {
        private Dictionary<E_AFFECT_TYPE, VisualFXObject>   VisualStacks = new Dictionary<E_AFFECT_TYPE, VisualFXObject>();
        private UnityAction<E_AFFECT_TYPE>                  OnDestroyHandler;
        
        [SerializeField]  private float BucketScale = 1f;

        private void Awake() {
            
            foreach(E_AFFECT_TYPE E in Enum.GetValues(typeof(E_AFFECT_TYPE))){ VisualStacks.Add(E, null); }
            OnDestroyHandler = (E_AFFECT_TYPE type) => VisualStacks[type] = null;
        }
        /*기존 코드는 Actiavete의 책임이 있었는데 지금은 그냥 객체 리턴을 하므로 엄연히 활성화 단계는 함수 호출부에서 해야 할것이다*/
        public VisualFXObject ActivateInstantable(MonoBehaviour entity, VisualFXObject _instantiable)
        {
            VisualFXObject instantiatedVFX = VisualFXObjectPool.Instance.VFXPool[_instantiable.gameObject.name].Get();
            instantiatedVFX.Init(null);
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
                        //null이 아니라면 더 쌓을 수 없으므로 리턴
                        if(value != null){return null;}
                    }
                    instantiatedVFX.AddOnReleaseEvent(() => OnDestroyHandler.Invoke(instantiatedVFX.AffectType));
                    VisualStacks[stateType] = instantiatedVFX;
                    break;
                }
                case E_INSTANTIATE_STACKING_TYPE.Stack : 
                {
                    break;
                }
            }
            
            instantiatedVFX.transform.position += offset * transform.localScale.z;
            instantiatedVFX.SetScaleByRatio(BucketScale);
            return instantiatedVFX;
        }

        public Quaternion GetForwardingAngle(Quaternion instantiatorQuaternion)
        {
            return Quaternion.Euler(transform.eulerAngles + instantiatorQuaternion.eulerAngles);
        }

        public Transform GetTransformParent(Transform instantiatorTransform)
        {
            instantiatorTransform.SetParent(this.transform);
            return instantiatorTransform;
        }
    }
}