using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Feature_NewData;
using Feature_NewData.Numerics;

public class TEST_EnemyStub : MonoBehaviour, Feature_NewData.ILifeAccessable, IStatAccessable ,IDamagable, IDieable
{
    [HideInInspector] public Collider entityCollider;
    [HideInInspector] public Rigidbody entityRigidbody;

    public LifeComposite Life {get; private set;}
    public EntityStatReferer StatReferer {get; private set;}

    [SerializeField] private ModelManger  modelManger;
    [SerializeField] private VisualFXManager  visualFXManager;

#region Life Accessable
    public LifeComposite GetLifeComposite() => this.Life;

    public void GetDamaged(int damage)
    {
        if (Life.IsDie) { return; }
        Life.Damaged(damage);
        if(Life.IsDie) {Die();}
    }

    public void GetDamaged(int damage, VFXObject vfx)
    {
        if (Life.IsDie) { return; }
        Life.Damaged(damage);
        if(Life.IsDie) {Die();}
        visualFXManager.VFXInstantiator(vfx);
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

        Life = new LifeComposite(100);
        StatReferer = new EntityStatReferer();
        StatReferer.SetRefStat(Life.MaxHp);
        StatReferer.SetRefStat(Life.Defence);
        //이걸 실행하면 Monobehaviour가 실행되는건지, 아니면 LifeComposite에서 실행되는건가?
    }

}