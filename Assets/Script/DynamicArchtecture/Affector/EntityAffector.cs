using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*
EntityAffect의 
개선해야할 부분은 다음과 같다
1. AsyncAffector은 언제나 도중에 Stop될 수 있어야 한다.
2. 이미 중복되는 코루틴 인가? 중복이 되면 어떻게 필터링을 할 수 있을지 고민해보자.
*/

public abstract class EntityAffector : ScriptableObject, IModifier {
    protected AffectorStruct affectorStruct;
    protected Entity targetEntity;
    protected Entity ownerEntity;
    protected bool  isInitialized;

    public abstract void Modifiy(IAffectable affectableEntity);

    public virtual void Init(Entity _owner, Entity _target){
        this.isInitialized = true;
        ownerEntity = _owner;
        targetEntity = _target;
        this.affectorStruct = new AffectorStruct();
    }
}