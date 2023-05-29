using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ChargeAttack", menuName = "ScriptableObject/EntityAffector/Owner/ChargeAttack", order = int.MaxValue)]
public class ChargeState : EntityAffector {
    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        //사실 임시로 한거지만 
        // 플레이어의 공격 매커니즘을 바꾸는 효과는 플레이어에게 정의해야하거나, 무기의 FSM을 정의하는게 맞아보인다.
        // 종원이에게 FSM 방식 작성하게 요청하기
        Player downcastPlayer = _owner as Player;
        this.AsyncAffectorCoroutine.Add(ChargeAttack(downcastPlayer.JustAttack));
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AsyncAffectHandler(this.affectorType,this.AsyncAffectorCoroutine);
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