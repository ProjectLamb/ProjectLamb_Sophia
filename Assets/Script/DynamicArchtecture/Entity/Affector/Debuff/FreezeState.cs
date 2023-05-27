using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Freeze", menuName = "ScriptableObject/EntityAffector/Debuff/Freeze", order = int.MaxValue)]
public class FreezeState : EntityAffector {
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
    //protected List<IEnumerator> AsyncAffectorCoroutine;
    //protected List<UnityAction> Affector;
    //protected Entity targetEntity //protected Entity ownerEntity;

    public float durationTime;
    public Material skin;

    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(SetSlow());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }

    IEnumerator SetSlow(){
        float originMoveSpeed = this.targetEntity.GetEntityData().MoveSpeed;
        float tenacity =this.targetEntity.GetEntityData().Tenacity;
        float slowDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.GetEntityData().MoveSpeed = 0;
        yield return YieldInstructionCache.WaitForSeconds(slowDurateTime);
        this.targetEntity.GetEntityData().MoveSpeed = originMoveSpeed;
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetEntityData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin, visualDurateTime);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.Revert();
    }
}