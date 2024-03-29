using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InvincibleState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorPackage affectorPackage;
    // affectorPackage.affectorType;
    // affectorPackage.AsyncAffectorCoroutine;
    // affectorPackage.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    public float DurationTime;
    public Material skin;
    public VFXObject vfx;

    private int originLayer;

    public InvincibleState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.ownerEntity = _eaData.ownerEntity;
        this.targetEntity = _eaData.targetEntity;
        this.affectorPackage.affectorType = STATE_TYPE.INVINCIBLE;
        this.affectorPackage.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorPackage.AsyncAffectorCoroutine.Add(Boost());
    }
    public override EntityAffector Init(Entity _owner, Entity _target) {
        EntityAffector EAInstance = base.Init(_owner, _target);
        InvincibleState Instance = new InvincibleState(EAInstance);
        Instance.DurationTime = this.DurationTime;
        Instance.skin = this.skin;
        Instance.vfx = this.vfx;
        Instance.isInitialized  = true;
        return Instance;
    }

    IEnumerator Boost(){
        originLayer = this.ownerEntity.gameObject.layer;
        this.ownerEntity.gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return YieldInstructionCache.WaitForSeconds(DurationTime);
        this.ownerEntity.gameObject.layer = originLayer;
    }
    IEnumerator VisualActivate(){
        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX( vfx);
        yield return YieldInstructionCache.WaitForSeconds(DurationTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorPackage.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorPackage.affectorType);
    }
}

/*
체력 보호막
화염 영역 생성하기
잃은 체력?
*/