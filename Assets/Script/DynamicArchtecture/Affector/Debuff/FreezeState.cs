using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Freeze", menuName = "ScriptableObject/EntityAffector/Debuff/Freeze", order = int.MaxValue)]
public class FreezeState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
    // affectorStruct.affectorType;
    // affectorStruct.AsyncAffectorCoroutine;
    // affectorStruct.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;

    public float durationTime;
    public Material skin;
    public FreezeState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = E_StateType.Freeze;
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorStruct.AsyncAffectorCoroutine.Add(SetSlow());
        
    }
    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        FreezeState Instance = new FreezeState(EAInstance);
        Instance.durationTime = this.durationTime;
        Instance.skin = this.skin;
        Instance.isInitialized  = true;
        return Instance;
    }

    IEnumerator SetSlow(){
        Debug.Log("실행됨");
        float originMoveSpeed = this.targetEntity.GetOriginData().MoveSpeed;
        float tenacity =this.targetEntity.GetFinalData().Tenacity;
        float slowDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.GetFinalData().MoveSpeed = 0;
        yield return YieldInstructionCache.WaitForSeconds(slowDurateTime);
        this.targetEntity.GetFinalData().MoveSpeed = originMoveSpeed;
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetFinalData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorStruct.affectorType);
    }
}