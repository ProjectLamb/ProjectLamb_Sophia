using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ChargeState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    private Player downcastPlayer;
    private bool    holding;
    private float   holdTime;
    public ChargeState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.AsyncAffectorCoroutine.Add(ChargeAttack(downcastPlayer.JustAttack));
    } 
    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        ChargeState Instance = new ChargeState(EAInstance);
        //사실 임시로 한거지만 
        // 플레이어의 공격 매커니즘을 바꾸는 효과는 플레이어에게 정의해야하거나, 무기의 FSM을 정의하는게 맞아보인다.
        // 종원이에게 FSM 방식 작성하게 요청하기
        downcastPlayer = _owner as Player;
        Instance.isInitialized  = true;
        return Instance;
    }
    
    IEnumerator ChargeAttack(UnityAction _attackState){
        holding = false;
        if(Input.GetMouseButton(0)){
            holding = true; holdTime = 0;
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