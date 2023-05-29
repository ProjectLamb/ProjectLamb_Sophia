using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour, IEntityAddressable{
    public Collider entityCollider;
    public Rigidbody entityRigidbody;
    public VisualModulator visualModulator;
    public GameObject model;
    public ParticleSystem DieParticle;


    protected virtual void Awake(){
        TryGetComponent<Collider>(out entityCollider);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        TryGetComponent<VisualModulator>(out visualModulator);
        model ??= transform.GetChild(0).Find("modle").gameObject;
    }

    public virtual EntityData GetEntityData(){return null;}
    public virtual void GetDamaged(int _amount){}
    public virtual void GetDamaged(int _amount, GameObject particle){}
    public virtual void Die(){}
    public virtual void AsyncAffectHandler(E_AffectorType type, List<IEnumerator> _Coroutine){}
    public virtual void AffectHandler(List<UnityAction> _Action){}
}