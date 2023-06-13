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

[System.Serializable]
public class EntityAffector : IModifier {
    [SerializeField]    public AffectorStruct affectorStruct;
    public bool  isInitialized;    
    [HideInInspector]   public Entity targetEntity;
    [HideInInspector]   public Entity ownerEntity;
    public virtual void Modifiy(){
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        targetEntity.AffectHandler(affectorStruct);
    }
    public virtual EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = new EntityAffector();
        EAInstance.affectorStruct = new AffectorStruct();
        EAInstance.ownerEntity = _owner;
        EAInstance.targetEntity = _target;
        return EAInstance;
    }
    public virtual void SetValue(List<float> objects){}
}