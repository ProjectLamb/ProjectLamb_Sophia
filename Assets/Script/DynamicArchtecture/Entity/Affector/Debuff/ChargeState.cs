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
    IPipelineAddressable pipelineAddressable;
    IVisuallyInteractable visuallyInteractable;

    public ChargeState(GameObject _AttackOwner) {
        pipelineAddressable = _AttackOwner.GetComponent<IPipelineAddressable>();
        
        this.pipelineData = pipelineAddressable.GetPipelineData();
        this.entityData   = pipelineAddressable.GetEntityData();
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        
        this.AsyncAffectorCoroutine.Add(ChargeAttack());
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    
    IEnumerator ChargeAttack(){
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
                Debug.Log("공격 발사 한대");
                Debug.Log("공격 발사 두대");
                Debug.Log("공격 발사 세대");
                Debug.Log("공격 발사 네대 다 맞았다~");
            }
        }
    }
}