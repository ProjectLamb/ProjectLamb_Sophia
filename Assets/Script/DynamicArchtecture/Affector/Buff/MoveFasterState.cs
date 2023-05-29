using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveFaster", menuName = "ScriptableObject/EntityAffector/Buff/MoveFaster", order = int.MaxValue)]
public class MoveFasterState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
    // affectorStruct.affectorType;
    // affectorStruct.AsyncAffectorCoroutine;
    // affectorStruct.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    public float durationTime;
    public Material skin;
    public VFXObject vfx;

    public override void Init(Entity _owner, Entity _target) {
        base.Init(_owner, _target);
        this.affectorStruct.affectorType = E_StateType.MoveSpeedUp;
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorStruct.AsyncAffectorCoroutine.Add(Boost());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AffectHandler(affectorStruct);
    }

    IEnumerator Boost(){
        float originMoveSpeed = this.ownerEntity.GetEntityData().MoveSpeed;
        this.ownerEntity.GetEntityData().MoveSpeed *= 1.2f; 
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        this.ownerEntity.GetEntityData().MoveSpeed = originMoveSpeed;
    }

    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetEntityData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.Revert(this.affectorStruct.affectorType);
    }
}