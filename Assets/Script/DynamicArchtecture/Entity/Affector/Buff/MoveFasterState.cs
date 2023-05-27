using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveFaster", menuName = "ScriptableObject/EntityAffector/Buff/MoveFaster", order = int.MaxValue)]
public class MoveFasterState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
    //protected List<IEnumerator> AsyncAffectorCoroutine;
    //protected List<UnityAction> Affector;
    //protected Entity targetEntity 
    //protected Entity ownerEntity;
    public float durationTime;
    public Material skin;
    public ParticleSystem particles;

    public override void Init(Entity _owner, Entity _target) {
        base.Init(_owner, _target);
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(Boost());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);

    }

    IEnumerator Boost(){
        float originMoveSpeed = this.ownerEntity.GetEntityData().MoveSpeed;
        this.ownerEntity.GetEntityData().MoveSpeed *= 1.2f; 
        yield return YieldInstructionCache.WaitForSeconds(durationTime);
        this.ownerEntity.GetEntityData().MoveSpeed = originMoveSpeed;
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