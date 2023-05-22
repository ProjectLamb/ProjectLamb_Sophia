using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Sandbag : Enemy
{    
    //public EnemyData enemyData;
    //public EntityData GetEntityData() {return this.enemyData;}
    //PipelineData pipelineData;
    //public PipelineData GetPipelineData(){return this.pipelineData;}

    //public GameObject model;
    //Rigidbody RigidBody;

    //public NavMeshAgent nav;
    //public Transform target;
    //public bool chase;
    //public bool mIsDie;

    //public Projectile[] Projectiles;

    //public ProjectileBucket projectileBucket;
    //Animator animator;
    //AnimEventInvoker animEventInvoker;
    //VisualModulator visualModulator;
    /*********************************************************************************
    *
    * 
    *
    *********************************************************************************/

    protected override void Awake() {
        TryGetComponent<VisualModulator>(out this.visualModulator);
        TryGetComponent<Rigidbody>(out this.rigidBody);
        TryGetComponent<NavMeshAgent>(out this.nav);
        TryGetComponent<Collider>(out collider);
        
        model.TryGetComponent<Animator>(out this.animator);
        model.TryGetComponent<AnimEventInvoker>(out this.animEventInvoker);
        
        this.pipelineData = new PipelineData();        
        this.enemyData.DieParticle.GetComponent<ParticleCallback>().onDestroyEvent.AddListener(DestroySelf);
        this.objectiveTarget = GameManager.Instance.playerGameObject.transform;
        
        //healthBar.pipelineData = this.pipelineData;
    }
    private void Start() {
        Debug.Log("Catch");
        animEventInvoker.animCallback[(int)Enum_AnimState.Attack].AddListener( () => {
            projectileBucket.ProjectileInstantiator(projectiles[0], E_ProjectileType.Attack);
        });
        animEventInvoker.animCallback[(int)Enum_AnimState.Jump].AddListener(() => {
            projectileBucket.ProjectileInstantiator(projectiles[1], E_ProjectileType.Attack);
        });
    }

    public override void GetDamaged(int _amount){
        if(this.isDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        animator.SetTrigger("DoHit");
        if (enemyData.CurHP <= 0) {Die();}
    }

    public override void GetDamaged(int _amount, GameObject particle){
        if(this.isDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        visualModulator.Interact(particle);
        animator.SetTrigger("DoHit");
        if (enemyData.CurHP <= 0) {Die();}
    }

    public override void Die(){ 
        enemyData.DieState.Invoke();
        isDie = true;
        collider.enabled = false;
        animator.SetTrigger("DoDie");
        visualModulator.vfxModulator.VFXInstantiator(enemyData.DieParticle);
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }

    public void AffectHandler(List<UnityAction> _Action) {
        _Action.ForEach(E => E.Invoke());
    }

    public void AsyncAffectHandler(List<IEnumerator> _Coroutine) {
        _Coroutine.ForEach(E => StartCoroutine(E));
    }

    private void Update() {
        if(!mIsDie)transform.LookAt(LookAtTarget);
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