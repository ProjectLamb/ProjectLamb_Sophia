using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Sophia_Carriers;

/// <summary>
/// 적 클래스 <br/>
/// * IDieAble : 죽는 Action , 인터페이스로 동작을 구현<br/>
/// * IDamagable : 맞는 Action , 인터페이스로 동작을 구현
/// </summary>
public class Enemy : Entity
{
    /* 아래 4줄은 절때 활성화 하지마라. 상속받은 Entity에 이미 정의 되어 있다. */
    //public Collider entityCollider;
    //public Rigidbody entityRigidbody;
    //public VisualModulator visualModulator;
    //public GameObject model;

    [field: SerializeField]
    public ScriptableObjEnemyData ScriptableED;
    private EntityData BaseEnemyData;
    private EntityData FinalData;

    public override void ResetData()
    {
        FinalData = BaseEnemyData;
    }

    public override ref EntityData GetFinalData() { return ref this.FinalData; }
    public override EntityData GetOriginData() { return this.BaseEnemyData; }

    public UnityEngine.AI.NavMeshAgent nav;
    public Transform objectiveTarget;
    public bool chase;
    public bool isDie;

    public Projectile[] projectiles;

    public ImageGenerator imageGenerator;
    public Animator animator;
    public AnimEventInvoker animEventInvoker;
    public ParticleSystem DieParticle;

    public override void Die()
    {
        GameManager.Instance.GlobalEvent.OnEnemyDieEvent.ForEach(E => E.Invoke());
        isDie = true;
        chase = false;
        entityRigidbody.velocity = Vector3.zero;
        gameObject.transform.parent.parent.GetComponent<StageGenerator>().CurrentMobCount--;
        Invoke("DestroySelf", 0.5f);
    }

    public override void GetDamaged(int _amount){
        if(isDie == true) {return;}
        GameManager.Instance.GlobalEvent.OnEnemyHitEvent.ForEach(E => E.Invoke());
        FinalData.HitStateRef.Invoke(ref _amount);
        imageGenerator.GenerateImage(_amount);
        CurrentHealth -= _amount;
        if (CurrentHealth <= 0) { this.Die(); }
    }

    public override void GetDamaged(int _amount, VFXObject _vfx){
        if(isDie == true) {return;}
        GameManager.Instance.GlobalEvent.OnEnemyHitEvent.ForEach(E => E.Invoke());
        FinalData.HitStateRef.Invoke(ref _amount);
        imageGenerator.GenerateImage(_amount);
        CurrentHealth -= _amount;
        visualModulator.InteractByVFX(_vfx);
        if (CurrentHealth <= 0) { this.Die(); }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected override void Awake()
    {
        /*아래 3줄은 절때 활성화 하지마라. base.Awake() 에서 이미 이걸 하고 있다.*/
        //TryGetComponent<VisualModulator>(out visualModulator);
        //TryGetComponent<Rigidbody>(out entityRigidbody);
        //TryGetComponent<Collider>(out entityCollider);
        base.Awake();

        TryGetComponent<UnityEngine.AI.NavMeshAgent>(out nav);

        this.model.TryGetComponent<Animator>(out animator);
        this.model.TryGetComponent<AnimEventInvoker>(out animEventInvoker);

        DieParticle.GetComponent<VFXObject>().OnDestroyEvent.AddListener(DestroySelf);

        BaseEnemyData = new EntityData(ScriptableED);
        FinalData = BaseEnemyData;
        CurrentHealth = FinalData.MaxHP;

        chase = false;
        objectiveTarget = GameManager.Instance?.PlayerGameObject?.transform;
        isDie = false;
    }

    private void Start()
    {
        nav.speed = FinalData.MoveSpeed;
    }

    private void FixedUpdate()
    {
        /***************************/
        if(GameManager.Instance?.GlobalEvent.IsGamePaused == true){return;}
        /***************************/
        if (chase)
        {
            nav.SetDestination(objectiveTarget.position);
        }
    }

    private void Update()
    {
        /***************************/
        if(GameManager.Instance?.GlobalEvent.IsGamePaused == true){return;}
        /***************************/
        if (chase) { nav.enabled = true; }
        else { nav.enabled = false; }
        nav.speed = FinalData.MoveSpeed;
    }
}
