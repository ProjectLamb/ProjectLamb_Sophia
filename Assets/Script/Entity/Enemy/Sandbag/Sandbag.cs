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

    public EnemyData enemyData;
    public override EntityData GetEntityData() {return this.enemyData;}
    
    public Transform objectiveTarget;

    public bool IsDie;

    public Projectile[] projectiles;

    public ProjectileBucket projectileBucket;
    Animator animator;
    AnimEventInvoker animEventInvoker;
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
        
        this.enemyData.DieParticle.GetComponent<ParticleCallback>().onDestroyEvent.AddListener(DestroySelf);
        this.objectiveTarget = GameManager.Instance.playerGameObject.transform;
        
    }
    private void Start() {
        animEventInvoker.animCallback[(int)Enum_AnimState.Attack].AddListener( () => {
            projectileBucket.ProjectileInstantiator(projectiles[0]);
        });
        animEventInvoker.animCallback[(int)Enum_AnimState.Jump].AddListener(() => {
            projectileBucket.ProjectileInstantiator(projectiles[1]);
        });
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
        visualModulator.vfxModulator.VFXInstantiator(enemyData.DieParticle);
    }
    
    public override void AffectHandler(List<UnityAction> _Action) {
        _Action.ForEach(E => E.Invoke());
    }
    
    public override void AsyncAffectHandler(List<IEnumerator> _Coroutine) {
        _Coroutine.ForEach(E => StartCoroutine(E));
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
}