using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

/// <summary>
/// VFXObject가 생성되는 버켓이다. 주로 Entity내 자식오브젝트로 포함됨 <br/>
/// > Method : 파괴되는 타이밍
/// </summary>
public class VFXBucket : MonoBehaviour {
    public Dictionary<STATE_TYPE, VFXObject>    VisualStacks = new Dictionary<STATE_TYPE, VFXObject>();
    public UnityAction<STATE_TYPE>              OnDestroyHandler;
    public float BucketScale = 1;
    private void Awake() {
        InitializeVisualStacksByEnums(Enum.GetValues(typeof(STATE_TYPE)));
        OnDestroyHandler = (STATE_TYPE type) => {RemoveStateByType(type);};
    }
    
    private void InitializeVisualStacksByEnums(Array _stateTypes){
        foreach(STATE_TYPE E in _stateTypes){ VisualStacks.Add(E, null); }
    }
    
    private void RemoveStateByType(STATE_TYPE _type){ VisualStacks[_type] = null; }
    
    public void VFXInstantiator(VFXObject _vfx){
        switch(_vfx.BucketStaking){
            case BUCKET_STACKING_TYPE.NONE_STACK : 
            {
                STATE_TYPE stateType = _vfx.AffectorType;
                if(VisualStacks.TryGetValue(stateType, out VFXObject value)){
                    //null이 아니라면 더 쌓을 수 없으므로 리턴
                    if(value != null){return;}
                }

                VFXObject vfxObject = Instantiate(_vfx, transform);
                vfxObject.OnDestroyActionByState += OnDestroyHandler;
                vfxObject.SetScale(BucketScale);
                VisualStacks[stateType] = vfxObject;
                VisualStacks[stateType].DestroyVFX();
                break;
            }
            case BUCKET_STACKING_TYPE.STACK : 
            {
                VFXObject vfxObject = Instantiate(_vfx, transform);
                vfxObject.SetScale(BucketScale);
                vfxObject.DestroyVFX();
                break;
            }
        }
    }
    public GameObject VFXGameObjectInstantiator(GameObject _obj){
        //재작성하기
        return null;
    }

    public void RevertVFX(STATE_TYPE _stateType){
        if(VisualStacks.TryGetValue(_stateType, out VFXObject value)){
            if(value == null) {return;}
            VisualStacks[_stateType].DestroyVFXForce();
            VisualStacks[_stateType] = null;
        }
    }
    public Quaternion GetForwardingAngle(Transform _useCarrier){
        return Quaternion.Euler(transform.eulerAngles + _useCarrier.transform.eulerAngles);
    }
}