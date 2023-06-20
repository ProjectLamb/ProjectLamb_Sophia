using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

public abstract class Entity : MonoBehaviour, IEntityAddressable
{
    [HideInInspector] public Collider entityCollider;
    [HideInInspector] public Rigidbody entityRigidbody;
    [SerializeField]  public GameObject model;
    [SerializeField]  public VisualModulator visualModulator;
    [SerializeField]  public CarrierBucket carrierBucket;
    [SerializeField]  public int mCurrentHealth;
    public int CurrentHealth
    {
        get { return mCurrentHealth; }
        set
        {
            mCurrentHealth = value;
            if (mCurrentHealth < 0) { mCurrentHealth = 0; }
        }
    }

    public Dictionary<STATE_TYPE, AffectorStruct> AffectorStacks = new Dictionary<STATE_TYPE, AffectorStruct>();

    protected virtual void Awake()
    {
        TryGetComponent<Collider>(out entityCollider);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        model ??= transform.GetChild(0).Find("model").gameObject;
        foreach(STATE_TYPE E in Enum.GetValues(typeof(STATE_TYPE))){
            AffectorStacks.Add(E, null);
        }
    }
    public abstract ref EntityData GetFinalData();
    public abstract EntityData GetOriginData();
    public abstract void ResetData();
    public abstract void GetDamaged(int _amount);
    public abstract void GetDamaged(int _amount, VFXObject _vfx);
    public abstract void Die();
    public virtual void AffectHandler(AffectorStruct _affectorStruct)
    {
        STATE_TYPE stateType = _affectorStruct.affectorType;
        AffectorStruct returnAS;
        if(AffectorStacks.TryGetValue(stateType, out returnAS)){
            returnAS?.AsyncAffectorCoroutine.ForEach(coroutine => { StopCoroutine(coroutine); });
            AffectorStacks[_affectorStruct.affectorType] = null;
        }
        this.AffectorStacks[_affectorStruct.affectorType] = _affectorStruct;
        _affectorStruct.Affector.ForEach((E) => E.Invoke());
        _affectorStruct.AsyncAffectorCoroutine.ForEach(coroutine => { StartCoroutine(coroutine); });
    }
}