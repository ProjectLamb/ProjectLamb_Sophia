using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BurnState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorPackage affectorPackage;
    // affectorPackage.affectorType;
    // affectorPackage.AsyncAffectorCoroutine;
    // affectorPackage.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    public float DurationTime;
    public float Damage;
    public  Material    skin;
    public  VFXObject   vfx;
    public BurnState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.BURN;
        this.affectorPackage.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorPackage.AsyncAffectorCoroutine.Add(DotDamage());
    }
    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        BurnState Instance = new BurnState(EAInstance);
        Instance.DurationTime = this.DurationTime;
        Instance.skin = this.skin;
        Instance.vfx = this.vfx;
        Instance.isInitialized  = true;
        return Instance;
    }

    public override void SetValue(List<float> objects)
    {
        DurationTime     = objects[0];
        Damage           = objects[1];
    }

    IEnumerator DotDamage(){
        float passedTime = 0;
        float tenacity =this.targetEntity.GetFinalData().Tenacity;
        float dotDamageDurateTime = DurationTime * (1 - tenacity);
        while(dotDamageDurateTime > passedTime){
            passedTime += 0.5f;
            this.targetEntity.GetDamaged((int)Damage);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    IEnumerator VisualActivate(){
        float tenacity =this.targetEntity.GetFinalData().Tenacity;
        float visualDurateTime = DurationTime * (1 - tenacity);
        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX( vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorPackage.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorPackage.affectorType);
    }
}