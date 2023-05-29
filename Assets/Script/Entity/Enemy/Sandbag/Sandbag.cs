using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Sandbag : Entity
{    
    /* 아래 4줄은 절때 활성화 하지마라. 상속받은 Entity에 이미 정의 되어 있다. */
    //public Collider entityCollider;
    //public Rigidbody entityRigidbody;
    //public VisualModulator visualModulator;
    //public GameObject model;
    
    [SerializeField]
    public EnemyData enemyData;
    public override EntityData GetEntityData() {return this.enemyData;}
    
    public Transform objectiveTarget;

    public bool IsDie;

    public Projectile[] projectiles;

    public ProjectileBucket projectileBucket;
    Animator animator;
    AnimEventInvoker animEventInvoker;
    public ImageGenerator imageGenerator;
    /*********************************************************************************
    *
    * 
    *
    *********************************************************************************/

    protected override void Awake() {
        /*아래 4줄을 절대 활성화 시키지 마라*/
        //TryGetComponent<Collider>(out entityCollider);
        //TryGetComponent<Rigidbody>(out entityRigidbody);
        //TryGetComponent<VisualModulator>(out visualModulator);
        //model ??= transform.GetChild(0).Find("modle").gameObject;

        model.TryGetComponent<Animator>(out this.animator);
        model.TryGetComponent<AnimEventInvoker>(out this.animEventInvoker);
        
        this.objectiveTarget = GameManager.Instance.playerGameObject.transform;
        affectorStacks = new Dictionary<E_AffectorType, List<IEnumerator>>();
    }
    private void Start() {
        animEventInvoker.animCallback[(int)Enum_AnimState.Attack].AddListener( () => {
            projectileBucket.ProjectileInstantiator(projectiles[0]);
        });
        animEventInvoker.animCallback[(int)Enum_AnimState.Jump].AddListener(() => {
            projectileBucket.ProjectileInstantiator(projectiles[1]);
        });
        this.enemyData.HitStateRef = (ref int amount) => {imageGenerator.GenerateImage(amount);};
    }

    public override void GetDamaged(int _amount){
        if(IsDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        animator.SetTrigger("DoHit");
        if (enemyData.CurHP <= 0) {Die();}
    }

    public override void GetDamaged(int _amount, GameObject particle){
        if(IsDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        visualModulator.Interact(particle);
        animator.SetTrigger("DoHit");
        if (enemyData.CurHP <= 0) {Die();}
    }

    public override void Die(){ 
        enemyData.DieState.Invoke();
        IsDie = true;
        this.entityCollider.enabled = false;
        animator.SetTrigger("DoDie");
        visualModulator.vfxModulator.VFXInstantiator(this.DieParticle);
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }

    private void Update() {
        /***************************/
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        /***************************/
        if(!IsDie)transform.LookAt(objectiveTarget);
    }
    
    [ContextMenu("평타", false, int.MaxValue)]
    void InstantiateProjectiles1(){
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoAttack");
    }

    [ContextMenu("범위데미지",false, int.MaxValue)]
    void InstantiateProjectiles2(){
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoJump");
    }

        public Dictionary<E_AffectorType, List<IEnumerator>> affectorStacks;
    public override void AsyncAffectHandler(E_AffectorType type, List<IEnumerator> _Coroutine){
        if(affectorStacks.ContainsKey(type).Equals(false)){ 
            affectorStacks.Add(type, _Coroutine); 
        }
        else {
            StopAffector(affectorStacks[type]);
        }
        affectorStacks[type] = _Coroutine;
        StartAffector(affectorStacks[type]);
    }
    public override void AffectHandler(List<UnityAction> _Action) {
        _Action.ForEach((E) => E.Invoke());
    }

    public void StopAffector(List<IEnumerator> corutines){
        foreach(IEnumerator coroutine in corutines){
            StopCoroutine(coroutine);
        }
    }
    public void StartAffector(List<IEnumerator> corutines){
        foreach(IEnumerator coroutine in corutines){
            StartCoroutine(coroutine);
        }
    }
}