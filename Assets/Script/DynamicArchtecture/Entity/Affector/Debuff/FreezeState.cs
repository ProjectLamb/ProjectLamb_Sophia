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
    IEntityAddressable entityAddressable;
    IVisuallyInteractable visuallyInteractable;

    EntityData entityData;

    public FreezeState(GameObject _target) {
        debuffData = GlobalModifierResources.Instance.debuffDatas[(int)E_DebuffState.Freeze];
        entityAddressable = _target.GetComponent<IEntityAddressable>();
        entityData = entityAddressable.GetEntityData();
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
        entityData.MoveSpeed *= 0.01f;
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime);
        entityData.MoveSpeed *= 0.01f;
    }
    
    IEnumerator VisualActivate(){
        visuallyInteractable.Interact(this.debuffData);
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime);
        visuallyInteractable.Revert();
    }
}