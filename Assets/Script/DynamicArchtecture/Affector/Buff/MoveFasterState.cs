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

    private float originMoveSpeed;

    public MoveFasterState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = E_StateType.MoveSpeedUp;
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorStruct.AsyncAffectorCoroutine.Add(Boost());
    }

    public override EntityAffector Init(Entity _owner, Entity _target) {
        EntityAffector EAInstance = base.Init(_owner, _target);
        MoveFasterState Instance = new MoveFasterState(EAInstance);
        Instance.durationTime = this.durationTime;
        Instance.skin = this.skin;
        Instance.vfx = this.vfx;
        Instance.isInitialized  = true;
        return Instance;
    }

    IEnumerator Boost(){
        originMoveSpeed = this.ownerEntity.GetOriginData().MoveSpeed;
        this.ownerEntity.GetFinalData().MoveSpeed *= 1.2f; 
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        this.ownerEntity.GetFinalData().MoveSpeed = originMoveSpeed;
    }

    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetFinalData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorStruct.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorStruct.affectorType);
    }
}