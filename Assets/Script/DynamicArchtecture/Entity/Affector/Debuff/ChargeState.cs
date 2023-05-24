using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargeState : DebuffState{
    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IEntityAddressable entityAddressable;
    IVisuallyInteractable visuallyInteractable;

    public ChargeState(Player _attackOwner) {        
        this.addingData = _attackOwner.equipmentManager.AddingData; //Debuff State 참고 // 앞으로 Debuff 대신 모듈레이터로
        this.entityData   = _attackOwner.playerData;
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        this.AsyncAffectorCoroutine.Add(ChargeAttack(_attackOwner.JustAttack));
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    
    IEnumerator ChargeAttack(UnityAction _attackState){
        bool holding = false;
        if(Input.GetMouseButton(0)){
            holding = true;
            float holdTime = 0;
            while(holdTime < 1f){
                if(Input.GetMouseButtonUp(0)){
                    Debug.Log("공격 실패");
                    holding = false;
                    break;
                }
                Debug.Log(holdTime * 100f);
                holdTime += Time.deltaTime;
                yield return null;
            }
            if(holding == true){
                Debug.Log("충전공격 준비 완료");
                yield return new WaitUntil(()=>{return Input.GetMouseButtonUp(0);});
                for(int i = 0 ; i < 4; i++){
                    _attackState.Invoke();
                    yield return YieldInstructionCache.WaitForSeconds(0.125f);
                }
            }
        }
    }
}