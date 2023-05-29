using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Poison", menuName = "ScriptableObject/EntityAffector/Debuff/Poison", order = int.MaxValue)]
public class PoisonState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/ 
    // protected List<IEnumerator> AsyncAffectorCoroutine;x
    // protected List<UnityAction> Affector;x
    // protected Entity targetEntity //protected Entity ownerEntity;

    public float durationTime;
    public Material skin;
    public ParticleSystem particles;

    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(DotDamage());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }

    IEnumerator DotDamage() {
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
        this.targetEntity.visualModulator.InteractByMaterial(skin, visualDurateTime);
        this.targetEntity.visualModulator.InteractByParticle(particles, visualDurateTime);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.Revert();
    }
}