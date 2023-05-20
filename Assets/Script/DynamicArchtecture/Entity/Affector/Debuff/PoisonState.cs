using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoisonState : DebuffState {
    DebuffData debuffData;
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IPipelineAddressable pipelineAddressable;
    IVisuallyInteractable visuallyInteractable;
    
    //타겟에 맞는 데이터에 따라서 실행될 어펙터는 다르다.
    //근데 안해도 되는게, 어차피 인터페이스 접근해서 맞는놈만 가지고 작동 시키면 된다.
    // 그렇게 되면 몬스터든, 아니든 상관이 없어진다.
    //List<UnityAction> actions;

    public PoisonState(GameObject _target){
        debuffData = GlobalModifierResources.Instance.debuffDatas[(int)E_DebuffState.Poisend];
        pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
        visuallyInteractable = _target.GetComponent<IVisuallyInteractable>();
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(DotDamage());
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    //new PoisonState

    IEnumerator DotDamage(){
        float passedTime = 0;
        while(debuffData.durationTime > passedTime){
            passedTime += 0.5f;
            pipelineAddressable.GetDamaged(debuffData.damageAmount);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    IEnumerator VisualActivate(){
        visuallyInteractable.Interact(this.debuffData);
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime);
        visuallyInteractable.Revert();
    }
}