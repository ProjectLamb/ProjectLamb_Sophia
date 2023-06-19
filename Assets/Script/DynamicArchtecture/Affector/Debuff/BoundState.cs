using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoundedState : EntityAffector {
        /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
    // affectorStruct.affectorType;
    // affectorStruct.AsyncAffectorCoroutine;
    // affectorStruct.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    
    public float DurationTime;
    public BoundedState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = STATE_TYPE.BOUNDED;
        this.affectorStruct.AsyncAffectorCoroutine.Add(SetBounded());
    }

    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        BoundedState Instance = new BoundedState(EAInstance);
        Instance.isInitialized  = true;
        Instance.DurationTime = this.DurationTime;
        return Instance;
    }
    
    IEnumerator SetBounded(){
        Debug.Log("실행됨");
        float originMoveSpeed = this.targetEntity.GetOriginData().MoveSpeed;
        float tenacity =this.targetEntity.GetFinalData().Tenacity;
        float BoundedDurateTime = DurationTime * (1 - tenacity);

        this.targetEntity.GetFinalData().MoveSpeed = 0f;
        yield return YieldInstructionCache.WaitForSeconds(BoundedDurateTime);
        this.targetEntity.GetFinalData().MoveSpeed = originMoveSpeed;
    }
}