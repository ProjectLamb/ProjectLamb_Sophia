using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Critical", menuName = "ScriptableObject/EntityAffector/Buff/Critical", order = int.MaxValue)]
public class CriticalState : EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
    //protected List<IEnumerator> AsyncAffectorCoroutine;
    //protected List<UnityAction> Affector;
    //protected Entity targetEntity 
    //protected Entity ownerEntity;
    
    public UnityAction activateTrigger;

    public override void Init(Entity _owner, Entity _target)
    {
        base.Init(_owner, _target);
        //this.AsyncAffectorCoroutine.Add();
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    
    //아;; 이거 이벤트 엄청 꼬이겠네. 
    // 엔티티 Hit에 관련된 버그가 있으면 이거 중심으로 디버그 하자.
    // 일단 Projectile에 넣어버리면, 맞았을떄 발생이 되는걸로 생각할 수 있다.
    // 딱봐도 UI랑 겹치는 엄청난 버그가 생기겠지만 HitTrigger을 넘겨받을 수 없다면 이렇게라도 해야겠다
    IEnumerator CriticalAttack(){
        int originPower = this.ownerEntity.GetEntityData().Power;
        this.ownerEntity.GetEntityData().Power *= 5;
        //어떤 Entity가 맞기 전까지는 활성화가 안꺼진다.
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }
}