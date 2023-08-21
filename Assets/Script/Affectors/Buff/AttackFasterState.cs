using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackFasterState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorPackage affectorPackage;
    // affectorPackage.affectorType;
    // affectorPackage.AsyncAffectorCoroutine;
    // affectorPackage.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    public float DurationTime;
    public float Ratio;
//  public Material skin;
//  public VFXObject vfx;
    private float originAttackSpeed;


    public AttackFasterState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.ATTACK_SPEED_UP;
//      this.affectorPackage.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorPackage.AsyncAffectorCoroutine.Add(Boost());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        AttackFasterState Instance = new AttackFasterState(EAInstance);
        Instance.DurationTime = this.DurationTime;
//      Instance.skin = this.skin;
//      Instance.vfx = this.vfx;
        Instance.Ratio = this.Ratio;
        Instance.isInitialized  = true;
        return Instance;
    }
    public override void SetValue(List<float> objects)
    {
        DurationTime    = objects[0];
        Ratio           = objects[1];
    }

    IEnumerator Boost(){
        originAttackSpeed = this.targetEntity.GetOriginData().AttackSpeed;
        this.targetEntity.GetFinalData().AttackSpeed = originAttackSpeed * Ratio;
        yield return YieldInstructionCache.WaitForSeconds(DurationTime);
        this.targetEntity.GetFinalData().AttackSpeed = originAttackSpeed;
    }

//  IEnumerator VisualActivate(){
//      this.targetEntity.visualModulator.InteractByMaterial(skin);
//      this.targetEntity.visualModulator.InteractByVFX(vfx);
//      yield return YieldInstructionCache.WaitForSeconds(DurationTime);
//      this.targetEntity.visualModulator.RevertByMaterial(this.affectorPackage.affectorType);
//      this.targetEntity.visualModulator.RevertByVFX(this.affectorPackage.affectorType);
//  }
}