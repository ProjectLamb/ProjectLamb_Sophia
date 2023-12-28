using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Feature_NewData;

public class TEST_EnemyStub : MonoBehaviour, Feature_NewData.ILifeAccessable, IDamagable, IDieable
{
    public LifeManager Life;

    [SerializeField] public VisualModulator visualModulator;

    private void Awake()
    {
        Life = new LifeManager(100);
        //이걸 실행하면 Monobehaviour가 실행되는건지, 아니면 LifeManager에서 실행되는건가?
    }

    private void Start()
    {
    }

    public void Die()
    {
        Destroy(gameObject, 0.5f);
    }

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
    }

    public LifeManager GetLifeManager()
    {
        return this.Life;
    }
}