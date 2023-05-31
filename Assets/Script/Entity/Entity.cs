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
    public Collider     entityCollider;
    public Rigidbody    entityRigidbody;
    public GameObject   model;
    public VisualModulator visualModulator;
<<<<<<< HEAD
    public GameObject model;
    public ParticleSystem DieParticle;
=======
    
    public Dictionary<E_StateType, AffectorStruct> affectorStacks;

>>>>>>> TA_Escatrgot_AffectorManager
    protected virtual void Awake(){
        TryGetComponent<Collider>(out entityCollider);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        model ??= transform.GetChild(0).Find("modle").gameObject;
        affectorStacks = new Dictionary<E_StateType, AffectorStruct>();
    }

    public virtual EntityData GetEntityData(){return null;}
    public virtual void GetDamaged(int _amount){}
    public virtual void GetDamaged(int _amount, GameObject _vfx){}
    public virtual void Die(){}
    public virtual void AffectHandler(AffectorStruct _affectorStruct){}
}