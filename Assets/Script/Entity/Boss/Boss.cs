using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

public class Boss : Entity
{
    [field: SerializeField]
    public ScriptableObjEnemyData ScriptableED;
    protected EntityData BaseEnemyData;
    protected EntityData FinalData;

    public override void ResetData()
    {
        FinalData = BaseEnemyData;
    }

    public override ref EntityData GetFinalData() { return ref this.FinalData; }
    public override EntityData GetOriginData() { return this.BaseEnemyData; }

    public Transform objectiveTarget;
    public bool isDie;

    public Projectile[] projectiles;

    public ImageGenerator imageGenerator;
    public Animator animator;
    public AnimEventInvoker animEventInvoker;
    public ParticleSystem DieParticle;
    public override void GetDamaged(int _amount)
    {
        if(Life.IsDie) {return;}
        Life.Damaged(_amount);
    }
    public override void GetDamaged(int _amount, VFXObject _vfx)
    {
        if(Life.IsDie) {return;}
        Life.Damaged(_amount);
        visualModulator.InteractByVFX(_vfx);
    }
    public override void Die()
    {

    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected override void Awake()
    {
        base.Awake();
        this.model.TryGetComponent<Animator>(out animator);
        this.model.TryGetComponent<AnimEventInvoker>(out animEventInvoker);

        DieParticle.GetComponent<VFXObject>().OnDestroyEvent.AddListener(DestroySelf);

        BaseEnemyData = new EntityData(ScriptableED);
        FinalData = BaseEnemyData;
        Life = new Feature_NewData.LifeManager(FinalData.MaxHP);

        objectiveTarget = GameManager.Instance?.PlayerGameObject?.transform;
        isDie = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
