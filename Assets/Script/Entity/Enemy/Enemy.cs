using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    
    [field : SerializeField]
    public EnemyData enemyData;

    public UnityEngine.AI.NavMeshAgent nav;
    public Transform objectiveTarget;
    public bool chase;
    public bool isDie;

    public Projectile[] projectiles;

    public ProjectileBucket projectileBucket;
    public ImageGenerator   imageGenerator;
    public Animator animator;
    public AnimEventInvoker animEventInvoker;
    public ParticleSystem DieParticle;

    public override EntityData GetEntityData() {return this.enemyData;}
    public override void Die()
    {
        enemyData.DieState.Invoke();
        isDie = true;
        chase = false;
        entityRigidbody.velocity = Vector3.zero;
        Invoke("DestroySelf", 0.5f);
    }

    public override void GetDamaged(int _amount){
        if(isDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        if (enemyData.CurHP <= 0) {this.Die();}
    }
    public override void GetDamaged(int _amount, GameObject _obj){
        if(isDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        visualModulator.InteractByGameObject(_obj);
        if (enemyData.CurHP <= 0) {this.Die();}
    }

    public void DestroySelf(){
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

        DieParticle.GetComponent<VFXObject>().onDestroyEvent.AddListener(DestroySelf);
        enemyData.DieState += GameManager.Instance.globalEvent.EnemyDie;

        chase = false;
        objectiveTarget = GameManager.Instance?.playerGameObject?.transform;
        isDie = false;
    }

    private void Start() {
        this.enemyData.HitStateRef = (ref int amount) => {imageGenerator.GenerateImage(amount);};
    }

    private void FixedUpdate()
    {
        /***************************/
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        /***************************/
        if (chase) {
            nav.SetDestination(objectiveTarget.position);
        }
    }

    private void Update()
    {
        /***************************/
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        /***************************/
        if (chase) { nav.enabled = true;}
        else {nav.enabled = false;}
        nav.speed = enemyData.MoveSpeed;
    }
    public override void AffectHandler(AffectorStruct affectorStruct){
        if(this.affectorStacks.ContainsKey(affectorStruct.affectorType).Equals(false)){ 
            this.affectorStacks.Add(affectorStruct.affectorType, affectorStruct);
        }
        else {
            foreach(IEnumerator coroutine in this.affectorStacks[affectorStruct.affectorType].AsyncAffectorCoroutine){
                StopCoroutine(coroutine);
            }
            this.affectorStacks.Remove(affectorStruct.affectorType);
            this.affectorStacks.Add(affectorStruct.affectorType, affectorStruct);
        }
        affectorStruct.Affector.ForEach((E) => E.Invoke());
        foreach(IEnumerator coroutine in affectorStruct.AsyncAffectorCoroutine){
            StartCoroutine(coroutine);
        }
    }
}
