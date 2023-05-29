using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//EntityModifier의 핸들러들이다.
//EntityModifier가 전달하는것은 AsyncAffectorStructure가 된다.
public class AffectorManager : MonoBehaviour, IAffectable {
    
    public Dictionary<E_AffectorType, List<IEnumerator>> affectorStacks;
    private void Awake() {
        affectorStacks = new Dictionary<E_AffectorType, List<IEnumerator>>();
    }
    public void AsyncAffectHandler(E_AffectorType type, List<IEnumerator> _Coroutine){
        if(affectorStacks.ContainsKey(type).Equals(false)){ 
            affectorStacks.Add(type, _Coroutine); 
        }
        else {
            StopAffector(affectorStacks[type]);
        }
        affectorStacks[type] = _Coroutine;
        StartAffector(affectorStacks[type]);
    }
    public void AffectHandler(List<UnityAction> _Action) {
        _Action.ForEach((E) => E.Invoke());
    }

    public void StopAffector(List<IEnumerator> corutines){
        foreach(IEnumerator coroutine in corutines){
            StopCoroutine(coroutine);
        }
    }
    public void StartAffector(List<IEnumerator> corutines){
        foreach(IEnumerator coroutine in corutines){
            StartCoroutine(coroutine);
        }
    }
}