using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenceState :  EntityAffector{
    public float VisualDurationTime;
    public VFXObject vfx;
    public float Ratio;
    
    public DefenceState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.DEFENCE;

        this.affectorPackage.AsyncAffectorCoroutine.Add(VisualActivate());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        DefenceState Instance = new DefenceState(EAInstance);
        Instance.VisualDurationTime = this.VisualDurationTime;
        Instance.vfx = this.vfx;
        Instance.Ratio = this.Ratio;
        Instance.isInitialized  = true;
        return Instance;
    }

    public void Defence(ref float _amount){
        _amount += (float)(_amount * Ratio); // 디펜스의 반대는 더 많이 맞는다는것으로
    }

    IEnumerator VisualActivate(){
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(VisualDurationTime);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorPackage.affectorType);
    }
}