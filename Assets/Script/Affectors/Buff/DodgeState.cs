using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DodgeState : EntityAffector{
    
    public float durationTime;
    public Material skin;
    public VFXObject vfx;
    
    public DodgeState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.INVINCIBLE;

        this.affectorPackage.AsyncAffectorCoroutine.Add(VisualActivate());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        DodgeState Instance = new DodgeState(EAInstance);
        Instance.durationTime = this.durationTime;
        Instance.skin = this.skin;
        Instance.vfx = this.vfx;
        Instance.isInitialized  = true;
        return Instance;
    }

    public void Dodge(ref float _amount){
        _amount = 0;
    }
    
    IEnumerator VisualActivate(){
        //this.targetEntity.visualModulator.InteractByMaterial(skin);
        //this.targetEntity.visualModulator.InteractByVFX(Entity owner,vfx);
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        //this.targetEntity.visualModulator.RevertByMaterial(this.affectorPackage.affectorType);
        //this.targetEntity.visualModulator.RevertByVFX(this.affectorPackage.affectorType);
    }
}