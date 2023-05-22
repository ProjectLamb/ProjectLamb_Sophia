using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecutionState : DebuffState{
    DebuffData debuffData;
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IPipelineAddressable pipelineAddressable;
    IVisuallyInteractable visuallyInteractable;

    public ExecutionState(GameObject _target) {
        debuffData = GlobalModifierResources.Instance.debuffDatas[(int)E_DebuffState.Execution];
        pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
        visuallyInteractable = _target.GetComponent<IVisuallyInteractable>();

        this.pipelineData = pipelineAddressable.GetPipelineData();
        this.entityData   = pipelineAddressable.GetEntityData();
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        this.Affector.Add(Execution);
        this.Affector.Add(GameManager.Instance.globalEvent.HandleTimeSlow);
        this.Affector.Add(Camera.main.GetComponent<CameraEffect>().HandleZoomIn);
        this.AsyncAffectorCoroutine.Add(VisualActivate());
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AffectHandler(this.Affector);
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }

    public void Execution (){
        pipelineAddressable.GetDamaged(1234567890);
    }
    
    IEnumerator VisualActivate(){
        visuallyInteractable.Interact(this.debuffData);
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime * (1 - this.pipelineData.Tenacity));
        visuallyInteractable.Revert();
    }
}