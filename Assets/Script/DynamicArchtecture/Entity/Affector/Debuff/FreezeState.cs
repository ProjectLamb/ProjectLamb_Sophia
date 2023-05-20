using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FreezeState : DebuffState{
    DebuffData debuffData;
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IPipelineAddressable pipelineAddressable;
    IVisuallyInteractable visuallyInteractable;

    PipelineData pipelineData;

    public FreezeState(GameObject _target) {
        debuffData = GlobalModifierResources.Instance.debuffDatas[(int)E_DebuffState.Freeze];
        pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
        pipelineData = pipelineAddressable.GetPipelineData();
        visuallyInteractable = _target.GetComponent<IVisuallyInteractable>();
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(SetSlow());
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }

    IEnumerator SetSlow(){
        pipelineData.MoveSpeed *= 0.01f;
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime);
        pipelineData.MoveSpeed *= 0.01f;
    }
    
    IEnumerator VisualActivate(){
        visuallyInteractable.Interact(this.debuffData);
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime);
        visuallyInteractable.Revert();
    }
}