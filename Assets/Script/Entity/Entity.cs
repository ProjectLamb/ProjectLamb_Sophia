using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

public abstract class Entity : MonoBehaviour, IEntityAddressable {
    public Collider         entityCollider;
    public Rigidbody        entityRigidbody;
    public GameObject       model;
    public VisualModulator  visualModulator;
    private int mCurrentHealth; 
    public int CurrentHealth {
        get {return mCurrentHealth;}
        set {
            mCurrentHealth = value;
            if(mCurrentHealth < 0) {mCurrentHealth = 0;}
        }
    }

    public Dictionary<E_StateType, AffectorStruct> affectorStacks;

    protected virtual void Awake(){
        TryGetComponent<Collider>(out entityCollider);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        model ??= transform.GetChild(0).Find("modle").gameObject;
        affectorStacks = new Dictionary<E_StateType, AffectorStruct>();
    }
    public abstract ref EntityData GetFinalData();
    public abstract     EntityData GetOriginData();
    public abstract void ResetData();
    public abstract void GetDamaged(int _amount);
    public abstract void GetDamaged(int _amount, VFXObject _vfx);
    public abstract void Die();
    public virtual void AffectHandler(AffectorStruct _affectorStruct) {
        if(affectorStacks.ContainsKey(_affectorStruct.affectorType).Equals(false)){ 
            affectorStacks.Add(_affectorStruct.affectorType, _affectorStruct);
        }
        else {
            foreach(IEnumerator coroutine in this.affectorStacks[_affectorStruct.affectorType].AsyncAffectorCoroutine){
                StopCoroutine(coroutine);
            }
            this.affectorStacks.Remove(_affectorStruct.affectorType);
            this.affectorStacks.Add(_affectorStruct.affectorType, _affectorStruct);
        }
        _affectorStruct.Affector.ForEach((E) => E.Invoke());
        Debug.Log($"AsyncAffectorCoroutine 개수:  {_affectorStruct.AsyncAffectorCoroutine.Count}");
        foreach(IEnumerator coroutine in _affectorStruct.AsyncAffectorCoroutine){
            StartCoroutine(coroutine);
        }
    }
}