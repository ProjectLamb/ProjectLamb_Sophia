using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SternState : DebuffState{
    DebuffData debuffData;
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IPipelineAddressable pipelineAddressable;
    IVisuallyInteractable visuallyInteractable;

    public SternState(GameObject _target) {
        debuffData = GlobalModifierResources.Instance.debuffDatas[(int)E_DebuffState.Stern];
        pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
        visuallyInteractable = _target.GetComponent<IVisuallyInteractable>();
        this.addingData = pipelineAddressable.GetAddingData();
        this.entityData   = pipelineAddressable.GetEntityData();
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(SetStern());
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }

    IEnumerator SetStern(){
        this.addingData.MoveSpeed = -entityData.MoveSpeed;
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime * (1 - this.addingData.Tenacity));
        this.addingData.MoveSpeed = 0;
    }
    
    IEnumerator VisualActivate(){
        visuallyInteractable.Interact(this.debuffData);
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime * (1 - this.addingData.Tenacity));
        visuallyInteractable.Revert();
    }
}