using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BlackHoleState : EntityAffector {
        /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorPackage affectorPackage;
    // affectorPackage.affectorType;
    // affectorPackage.AsyncAffectorCoroutine;
    // affectorPackage.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    
    public float DurationTime;
    public float Direction = 0f;
    public BlackHoleState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.BLACK_HOLE;
        this.affectorPackage.AsyncAffectorCoroutine.Add(BlackHole());
        this.affectorPackage.AsyncAffectorCoroutine.Add(SetSlow());
    }

    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        BlackHoleState Instance = new BlackHoleState(EAInstance);
        Instance.isInitialized  = true;
        Instance.DurationTime   = this.DurationTime;
        Instance.Direction      = this.Direction;
        return Instance;
    }

    IEnumerator BlackHole(){
        float passedTime = 0f;
        float tenacity =this.targetEntity.GetFinalData().Tenacity;
        float blackHoleDurateTime = DurationTime * (1 - tenacity);
        Vector3 dir =  (this.targetEntity.entityRigidbody.position - this.ownerEntity.entityRigidbody.position) * Direction;
        Vector3 originVelocity = this.targetEntity.entityRigidbody.velocity;
        float originMoveSpeed = this.targetEntity.GetOriginData().MoveSpeed;
        while(blackHoleDurateTime > passedTime){
            passedTime += 0.05f;
            dir = (this.targetEntity.entityRigidbody.position - this.ownerEntity.entityRigidbody.position) * Direction;
            this.targetEntity.entityRigidbody.velocity = dir;
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }
        this.targetEntity.entityRigidbody.velocity = originVelocity;
    }
    
    IEnumerator SetSlow(){
        Debug.Log("실행됨");
        float originMoveSpeed = this.targetEntity.GetOriginData().MoveSpeed;
        float tenacity =this.targetEntity.GetFinalData().Tenacity;
        float slowDurateTime = DurationTime * (1 - tenacity);

        this.targetEntity.GetFinalData().MoveSpeed = originMoveSpeed * 0.4f;
        yield return YieldInstructionCache.WaitForSeconds(slowDurateTime);
        this.targetEntity.GetFinalData().MoveSpeed = originMoveSpeed;
    }
    IEnumerator DotDamage(){
        float passedTime = 0;
        float tenacity =this.targetEntity.GetFinalData().Tenacity;
        float dotDamageDurateTime = DurationTime * (1 - tenacity);
        while(dotDamageDurateTime > passedTime){
            passedTime += 0.5f;
            this.targetEntity.GetDamaged((int)(this.ownerEntity.GetFinalData().Power * 0.25f) + 1);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }
}