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
    [HideInInspector] public GameObject model;
    [SerializeField] public VisualModulator visualModulator;
    [SerializeField] public CarrierBucket carrierBucket;
    [SerializeField] public int mCurrentHealth;
    public int CurrentHealth
    {
        get { return mCurrentHealth; }
        set
        {
            mCurrentHealth = value;
            if (mCurrentHealth < 0) { mCurrentHealth = 0; }
        }
    }

    public Dictionary<STATE_TYPE, AffectorStruct> affectorStacks;

    protected virtual void Awake()
    {
        TryGetComponent<Collider>(out entityCollider);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        model ??= transform.GetChild(0).Find("modle").gameObject;
        affectorStacks = new Dictionary<STATE_TYPE, AffectorStruct>();
    }
    public abstract ref EntityData GetFinalData();
    public abstract EntityData GetOriginData();
    public abstract void ResetData();
    public abstract void GetDamaged(int _amount);
    public abstract void GetDamaged(int _amount, VFXObject _vfx);
    public abstract void Die();
    public virtual void AffectHandler(AffectorStruct _affectorStruct)
    {

        if (affectorStacks.ContainsKey(_affectorStruct.affectorType).Equals(false))
        {
            affectorStacks.Add(_affectorStruct.affectorType, _affectorStruct);
        }
        else
        {
            affectorStacks[_affectorStruct.affectorType].AsyncAffectorCoroutine.ForEach(coroutine => { StopCoroutine(coroutine); });
            this.affectorStacks.Remove(_affectorStruct.affectorType);
            this.affectorStacks.Add(_affectorStruct.affectorType, _affectorStruct);
        }

        Debug.Log($"AsyncAffectorCoroutine 개수:  {_affectorStruct.AsyncAffectorCoroutine.Count}");
        _affectorStruct.Affector.ForEach((E) => E.Invoke());
        _affectorStruct.AsyncAffectorCoroutine.ForEach(coroutine => { StartCoroutine(coroutine); });
    }
}