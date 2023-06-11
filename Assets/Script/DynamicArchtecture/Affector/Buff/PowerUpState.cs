using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObject/EntityAffector/Buff/PowerUp", order = int.MaxValue)]
public class PowerUpState : EntityAffector {
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

    private int originPower;
    public PowerUpState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = E_StateType.PowerUp;
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorStruct.AsyncAffectorCoroutine.Add(Boost());
    }
    public override EntityAffector Init(Entity _owner, Entity _target) {
        EntityAffector EAInstance = base.Init(_owner, _target);
        PowerUpState Instance = new PowerUpState(EAInstance);
        Instance.durationTime = this.durationTime;
        Instance.skin = this.skin;
        Instance.vfx = this.vfx;
        Instance.Ratio = this.Ratio;
        Instance.isInitialized  = true;
        return Instance;
    }

    //public virtual void Modifiy(){
    //    if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
    //    targetEntity.AffectHandler(affectorStruct);
    //}

    IEnumerator Boost(){
        originPower = this.ownerEntity.GetOriginData().Power;
        this.ownerEntity.GetFinalData().Power = (int)(originPower * Ratio + 0.5f);
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        this.ownerEntity.GetFinalData().Power = originPower;
    }
    IEnumerator VisualActivate(){
        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorStruct.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorStruct.affectorType);
    }
}