using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CriticalState : DebuffState{


    /*********************************************************************************
    * 
    * 리시버들 
    *  
    *********************************************************************************/
    IEntityAddressable entityAddressable;
    IVisuallyInteractable visuallyInteractable;

    public CriticalState(Player _AttackOwner) {        
        this.addingData = _AttackOwner.equipmentManager.AddingData;
        this.entityData   = _AttackOwner.playerData;
        this.AsyncAffectorCoroutine = new List<IEnumerator>();
        this.Affector = new List<UnityAction>();
        
        this.AsyncAffectorCoroutine.Add(CriticalAttack(_AttackOwner.JustAttack));
    }

    public void Modifiy(IAffectable affectableEntity) {
        affectableEntity.AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    
    IEnumerator CriticalAttack(UnityAction _AttackState){
        yield break;
    }
}