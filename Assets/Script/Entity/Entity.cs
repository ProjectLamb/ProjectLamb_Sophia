using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

using Component = UnityEngine.Component;
using Random = UnityEngine.Random;


public abstract class Entity : MonoBehaviour, IDamagable, IDieable, IAffectable, IEntityDataAddressable
{
    [HideInInspector] public Collider entityCollider;
    [HideInInspector] public Rigidbody entityRigidbody;
    [SerializeField] public GameObject model;
    [SerializeField] public VisualModulator visualModulator;
    [SerializeField] public CarrierBucket carrierBucket;
    public Sophia.Composite.LifeComposite Life;

    public Dictionary<STATE_TYPE, AffectorPackage> AffectorStacks = new Dictionary<STATE_TYPE, AffectorPackage>();

    protected virtual void Awake()
    {
        TryGetComponent<Collider>(out entityCollider);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        model ??= transform.GetChild(0).Find("model").gameObject;
        foreach (STATE_TYPE E in Enum.GetValues(typeof(STATE_TYPE)))
        {
            AffectorStacks.Add(E, null);
        }
    }
    public abstract ref EntityData GetFinalData();
    public abstract EntityData GetOriginData();
    public abstract void ResetData();
    public abstract void GetDamaged(int _amount);
    public abstract void GetDamaged(int _amount, VFXObject _vfx);
    public abstract void Die();

    public virtual void AffectHandler(AffectorPackage _affectorPackage)
    {
        STATE_TYPE stateType = _affectorPackage.affectorType;
        AffectorPackage returnAS;
        if (AffectorStacks.TryGetValue(stateType, out returnAS))
        {
            returnAS?.AsyncAffectorCoroutine.ForEach(coroutine => { StopCoroutine(coroutine); });
            AffectorStacks[_affectorPackage.affectorType] = null;
        }
        this.AffectorStacks[_affectorPackage.affectorType] = _affectorPackage;
        _affectorPackage.Affector.ForEach((E) => E.Invoke());
        _affectorPackage.AsyncAffectorCoroutine.ForEach(coroutine => { StartCoroutine(coroutine); });
    }
}
