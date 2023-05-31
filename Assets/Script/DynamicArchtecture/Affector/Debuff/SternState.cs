using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Stern", menuName = "ScriptableObject/EntityAffector/Debuff/Stern", order = int.MaxValue)]
public class SternState : EntityAffector{
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

    public SternState(EntityAffector _eaData) {
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = E_StateType.Stern;
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorStruct.AsyncAffectorCoroutine.Add(SetStern());
    }

    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        SternState Instance = new SternState(EAInstance);
        Instance.durationTime = this.durationTime;
        Instance.skin = this.skin;
        Instance.vfx = this.vfx;
        Instance.isInitialized  = true;
        return Instance;
    }

    IEnumerator SetStern(){
        float originMoveSpeed = this.targetEntity.GetEntityData().MoveSpeed;
        float tenacity =this.targetEntity.GetEntityData().Tenacity;
        float sternDurateTime = durationTime * (1 - tenacity);
        this.targetEntity.GetEntityData().MoveSpeed = 0;
        yield return YieldInstructionCache.WaitForSeconds(sternDurateTime);
        this.targetEntity.GetEntityData().MoveSpeed = originMoveSpeed;
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetEntityData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);
        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorStruct.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorStruct.affectorType);
    }
}