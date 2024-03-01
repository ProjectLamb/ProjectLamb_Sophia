using UnityEngine;
using Sophia_Carriers;
using DG.Tweening;
using Sophia;

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
    public ScriptableObjEnemyData ScriptableED; // private SerialBaseEntityData       _baseEntityData;
    protected EntityData BaseEnemyData;  // StatReferer
    [SerializeField] protected EntityData FinalData; // StatReferer

    public override void ResetData() //❌
    {
        FinalData = BaseEnemyData;
    }

    public override ref EntityData GetFinalData() { return ref this.FinalData; } // ❌
    public override EntityData GetOriginData() { return this.BaseEnemyData; } // ❌

    public UnityEngine.AI.NavMeshAgent nav; // 
    public Transform objectiveTarget;   // public  Entity                     _objectiveEntity;
    public bool isRecog;                // public RecognizeEntityComposite RecognizeEntity {get; private set;}
    public bool isDie;                  // public LifeComposite Life {get; private set;}
    public bool isOffensive;            // !isOffensive = Defensive
    int offensiveRate;                  // Factory 
    public float spawnRate;

    public Projectile[] projectiles;    // private ProjectileObject[]         _attckProjectiles;

    public ImageGenerator imageGenerator; 
    public Stage stage;                 // public Stage CurrentStage {get; private set;}
    //public MobGenerator mobGenerator;
    public Animator animator;           //        [SerializeField] private Animator _modelAnimator;
    public AnimEventInvoker animEventInvoker; // MonsterLove
    public ParticleSystem DieParticle;  //private VisualFXObject             _dieParticleRef;

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
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damageAmount = _amount;
        damageInfo.damageRatio = 1;
        Life.Damaged(damageInfo);
        if (Life.IsDie) { Die(); }
    }

    public override void GetDamaged(int _amount, VFXObject _vfx)
    {
        if (Life.IsDie == true) { return; }
                DamageInfo damageInfo = new DamageInfo();
        damageInfo.damageAmount = _amount;
        damageInfo.damageRatio = 1;
        Life.Damaged(damageInfo);
        if (Life.IsDie) { Die(); }
        visualModulator.InteractByVFX(_vfx);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnDestroy() {
        Life.OnDamaged -= Generate;
    }

    protected virtual void NavMeshSet()
    {
        nav.speed = FinalData.MoveSpeed;
        nav.acceleration = nav.speed * 1.5f;
        nav.updateRotation = false;
    }

    protected virtual void Freeze()
    {
        nav.enabled = false;
        transform.DOKill();
        entityRigidbody.velocity = Vector3.zero;
        entityRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    protected virtual void UnFreeze()
    {
        nav.enabled = true;
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

        //TA_escatrgot
        Life = new Sophia.Composite.LifeComposite(FinalData.MaxHP);
        Life.OnDamaged += Generate;

        isRecog = false;
        objectiveTarget = GameManager.Instance?.PlayerGameObject?.transform;
        isDie = false;

        isOffensive = false;
        if (Random.Range(0, 10) < offensiveRate)
            isOffensive = true;

        NavMeshSet();
    }

    public void Generate(DamageInfo info) {
        imageGenerator.GenerateImage(info.GetAmount());
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
