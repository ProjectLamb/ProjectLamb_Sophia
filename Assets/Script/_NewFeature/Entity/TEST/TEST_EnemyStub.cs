using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Feature_NewData;
using Feature_NewData.Numerics;

public class TEST_EnemyStub : MonoBehaviour, ILifeAccessable, IStatAccessable
{
    [HideInInspector] public Collider entityCollider;
    [HideInInspector] public Rigidbody entityRigidbody;

    public LifeComposite Life {get; private set;}
    public EntityStatReferer StatReferer {get; private set;}

    [SerializeField] private ModelManger  modelManger;
    [SerializeField] private VisualFXBucket  VisualFXBucket;

#region Life Accessable
    public LifeComposite GetLifeComposite() => this.Life;

    public void GetDamaged(int damage)
    {
        if (Life.IsDie) { return; }
        Life.Damaged(damage);
        if(Life.IsDie) {Die();}
    }

    public void GetDamaged(int damage, VisualFXObject vfx)
    {
        if (Life.IsDie) { return; }
        Life.Damaged(damage);
        if(Life.IsDie) {Die();}
        VisualFXBucket.ActivateInstantable(this, vfx);
    }
    
    public void Die() => Destroy(gameObject, 0.5f);

#endregion

#region Stat Accessable

    public Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

    [ContextMenu("Get Stats Info")]
    public string GetStatsInfo()
    {
        Debug.Log(this.StatReferer.GetStatsInfo());        
        return this.StatReferer.GetStatsInfo();
    }

#endregion

    private void Awake()
    {
        TryGetComponent<Collider>(out entityCollider);
        TryGetComponent<Rigidbody>(out entityRigidbody);

        Life = new LifeComposite(100000);
        StatReferer = new EntityStatReferer();
        StatReferer.SetRefStat(Life.MaxHp);
        StatReferer.SetRefStat(Life.Defence);
    }

}