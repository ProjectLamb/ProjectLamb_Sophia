using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Poison", menuName = "ScriptableObject/EntityAffector/Debuff/Poison", order = int.MaxValue)]
public class PoisonState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
    // affectorStruct.affectorType;
    // affectorStruct.AsyncAffectorCoroutine;
    // affectorStruct.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    public float durationTime;
    public  Material    skin;
    public  VFXObject   vfx;
    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.affectorStruct.affectorType = E_StateType.Poisend;
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
        this.affectorStruct.AsyncAffectorCoroutine.Add(DotDamage());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AffectHandler(this.affectorStruct);
    }

    IEnumerator DotDamage(){
        float passedTime = 0;
        float tenacity =this.targetEntity.GetEntityData().Tenacity;
        float dotDamageDurateTime = durationTime * (1 - tenacity);
        while(dotDamageDurateTime > passedTime){
            passedTime += 0.5f;
            this.targetEntity.GetDamaged((int)(this.ownerEntity.GetEntityData().Power * 0.25f));
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    IEnumerator VisualActivate(){
        float tenacity =this.targetEntity.GetEntityData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);
        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.Revert(this.affectorStruct.affectorType);
    }
}