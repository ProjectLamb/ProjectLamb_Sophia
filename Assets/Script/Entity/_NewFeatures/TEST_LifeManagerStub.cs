using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Feature_NewData;

public class TEST_LifeManagerStub : MonoBehaviour, IDamagable, IDieable
{
    private EnemyLifeManager life;

    [SerializeField] public VisualModulator visualModulator;

    [SerializeField] public DamageTextInstantiator TextInstantiator;

    private void Awake()
    {
        life = new EnemyLifeManager(100);
        //이걸 실행하면 Monobehaviour가 실행되는건지, 아니면 LifeManager에서 실행되는건가?
    }

    private void OnEnable()
    {
        life.AddOnExitDieEvent(() => { Invoke("DestroySelf", 0.5f); });
    }

    private void Start()
    {
        life.AddOnDamageEvent((float value) => { TextInstantiator.Generate(value); });
        // life.AddOnDamageEvent((float value) => { TextInstantiator.Generate(new List<float>{1,2,3,4,5}); });
    }

    public void AffectHandler(AffectorPackage affectorPackage)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        life.Die();
    }

    public void GetDamaged(int damage)
    {
        if (life.IsDie) { return; }
        life.GetDamaged(damage);
    }

    public void GetDamaged(int damage, VFXObject vfx)
    {
        if (life.IsDie) { return; }
        life.GetDamaged(damage);
        visualModulator.InteractByVFX(vfx);
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    /*
    public override void Die()
    {
        base.Die();
        stage.mobGenerator.CurrentMobCount--;
        Invoke("DestroySelf", 0.5f);
    }
    */
    // Start is called before the first frame update
    /*
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(objectiveTarget);
        nav.SetDestination(objectiveTarget.position);
    }
    */
}