using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoisonState : IAffectable {
    DebuffData debuffData;
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IDamagable damagableEntity;
    IVisuallyInteractable visuallyInteractable;
    
    //타겟에 맞는 데이터에 따라서 실행될 어펙터는 다르다.
    //근데 안해도 되는게, 어차피 인터페이스 접근해서 맞는놈만 가지고 작동 시키면 된다.
    // 그렇게 되면 몬스터든, 아니든 상관이 없어진다.
    //List<UnityAction> actions;

    public List<IEnumerator> AsyncAffectorCoroutine;
    public List<UnityAction> Affector;

    public PoisonState(IDamagable _owner){
        debuffData = GlobalModifierResources.Instance.debuffDatas[(int)E_DebuffState.Poisend];
        damagableEntity = _owner;
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(DotDamage());
    }

    //new PoisonState

    IEnumerator DotDamage(){
        float passedTime = 0;
        while(debuffData.durationTime > passedTime){
            passedTime += 0.5f;
            damagableEntity.GetDamaged(debuffData.damageAmount);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    IEnumerator VisualActivate(){
        Debug.Log("반짝반짝 거린다.");
        yield return YieldInstructionCache.WaitForSeconds(debuffData.durationTime);
    }
}