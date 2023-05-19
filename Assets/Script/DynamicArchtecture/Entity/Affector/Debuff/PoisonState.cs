using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoisonState : IModifier {
    DebuffData debuffData;
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IDamagable damagableTarget;
    IVisuallyInteractable visuallyInteractable;
    
    //타겟에 맞는 데이터에 따라서 실행될 어펙터는 다르다.
    //근데 안해도 되는게, 어차피 인터페이스 접근해서 맞는놈만 가지고 작동 시키면 된다.
    // 그렇게 되면 몬스터든, 아니든 상관이 없어진다.
    //List<UnityAction> actions;

    public List<IEnumerator> AsyncAffectorCoroutine;
    public List<UnityAction> Affector;

    public PoisonState(GameObject _target){
        debuffData = GlobalModifierResources.Instance.debuffDatas[(int)E_DebuffState.Poisend];
        damagableTarget = _target.GetComponent<IDamagable>();
        visuallyInteractable = _target.GetComponent<IVisuallyInteractable>();
        AsyncAffectorCoroutine = new List<IEnumerator>();
        Affector = new List<UnityAction>();
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(DotDamage());
    }

    public void Modifiy() {
    }
    //new PoisonState

    IEnumerator DotDamage(){
        float passedTime = 0;
        while(debuffData.durationTime > passedTime){
            passedTime += 0.5f;
            damagableTarget.GetDamaged(debuffData.damageAmount);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    IEnumerator VisualActivate(){
        visuallyInteractable.Interact(this.debuffData);
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime);
        visuallyInteractable.Revert();
    }
}