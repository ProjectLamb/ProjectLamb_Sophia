using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Invincible", menuName = "ScriptableObject/EntityAffector/Buff/Invincible", order = int.MaxValue)]
public class InvincibleState : EntityAffector {
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
    public float Ratio;

    private int originLayer;

    public InvincibleState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.ownerEntity = _eaData.ownerEntity;
        this.targetEntity = _eaData.targetEntity;
        this.affectorStruct.affectorType = E_StateType.Invincible;
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorStruct.AsyncAffectorCoroutine.Add(Boost());
    }
    public override EntityAffector Init(Entity _owner, Entity _target) {
        EntityAffector EAInstance = base.Init(_owner, _target);
        InvincibleState Instance = new InvincibleState(EAInstance);
        Instance.durationTime = this.durationTime;
        Instance.skin = this.skin;
        Instance.vfx = this.vfx;
        Instance.Ratio = this.Ratio;
        Instance.isInitialized  = true;
        return Instance;
    }

    IEnumerator Boost(){
        originLayer = this.ownerEntity.gameObject.layer;
        this.ownerEntity.gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        this.ownerEntity.gameObject.layer = originLayer;
    }
    IEnumerator VisualActivate(){
        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorStruct.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorStruct.affectorType);
    }
}

/*
체력 보호막
화염 영역 생성하기
잃은 체력?
*/