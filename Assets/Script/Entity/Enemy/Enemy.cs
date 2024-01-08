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
    protected EntityData BaseEnemyData;
    [SerializeField] protected EntityData FinalData;

    public override void ResetData()
    {
        FinalData = BaseEnemyData;
    }

    public override ref EntityData GetFinalData() { return ref this.FinalData; }
    public override EntityData GetOriginData() { return this.BaseEnemyData; }

    public UnityEngine.AI.NavMeshAgent nav;
    public Transform objectiveTarget;
    public bool isRecog;
    public bool isDie;
    public bool isOffensive;    //!isOffensive = Defensive
    int offensiveRate;

    public Projectile[] projectiles;

    public ImageGenerator imageGenerator;
    public Stage stage;
    public MobGenerator mobGenerator;
    public Animator animator;
    public AnimEventInvoker animEventInvoker;
    public ParticleSystem DieParticle;

    public override void Die()
    {
        GameManager.Instance.GlobalEvent.OnEnemyDieEvent.ForEach(E => E.Invoke());
        isDie = true;
        Freeze();
        entityCollider.enabled = false;
    }

    public override void GetDamaged(int _amount)
    {
        if (Life.IsDie == true) { return; }
        Life.Damaged(_amount);
    }

    public override void GetDamaged(int _amount, VFXObject _vfx)
    {
        if (Life.IsDie == true) { return; }
        Life.Damaged(_amount);
        visualModulator.InteractByVFX(_vfx);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected virtual void NavMeshSet()
    {
        nav.speed = FinalData.MoveSpeed;
        nav.acceleration = nav.speed * 1.5f;
        nav.updateRotation = false;
    }

    protected virtual void Freeze()
    {
        entityRigidbody.velocity = Vector3.zero;
        entityRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    protected virtual void UnFreeze()
    {
        entityRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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
        Life = new Sophia.Composite.LifeComposite(FinalData.MaxHP);

        isRecog = false;
        objectiveTarget = GameManager.Instance?.PlayerGameObject?.transform;
        isDie = false;

        isOffensive = false;
        if (Random.Range(0, 10) < offensiveRate)
            isOffensive = true;

        NavMeshSet();
    }

    private void Start()
    {

    }

    protected virtual void FixedUpdate()
    {
        /***************************/
        if (GameManager.Instance?.GlobalEvent.IsGamePaused == true) { return; }
        /***************************/
    }

    protected virtual void Update()
    {
        /***************************/
        if (GameManager.Instance?.GlobalEvent.IsGamePaused == true) { return; }
        /***************************/

        /*if (isRecog) { nav.enabled = true; }
        else { nav.enabled = false; }*/
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.PlayerGameObject)
            entityRigidbody.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.PlayerGameObject)
            entityRigidbody.constraints = RigidbodyConstraints.None;
    }*/
}
