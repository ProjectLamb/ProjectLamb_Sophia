using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Stern", menuName = "ScriptableObject/EntityAffector/Debuff/Stern", order = int.MaxValue)]
public class SternState : EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
    //protected List<IEnumerator> AsyncAffectorCoroutine;
    //protected List<UnityAction> Affector;
    //protected Entity targetEntity //protected Entity ownerEntity;

    public float durationTime;
    public Material skin;
    public ParticleSystem particles;

    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(SetStern());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
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

        this.targetEntity.visualModulator.InteractByMaterial(skin, visualDurateTime);
        this.targetEntity.visualModulator.InteractByParticle(particles, visualDurateTime);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.Revert();
    }
}