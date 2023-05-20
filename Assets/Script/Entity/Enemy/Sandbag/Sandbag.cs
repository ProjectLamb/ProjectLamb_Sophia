using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Sandbag : MonoBehaviour, IPipelineAddressable
{    
    public ScriptableObjEntityData scriptableObjEnemyData;
    EnemyData enemyData;
    public EntityData GetEntityData(){return this.enemyData;}

    PipelineData pipelineData;
    public PipelineData GetPipelineData(){return this.pipelineData;}

    public UnityEvent hpChangedEvent;
    public GameObject model;
    public ProjectileBucket projectileBucket;
    public Transform LookAtTarget;
    public Projectile[] Projectiles;

    public HealthBar healthBar;

    Animator animator;
    AnimEventInvoker animEventInvoker;
    VisualModulator visualModulator;
    bool mIsDie;
    /*********************************************************************************
    *
    * 
    *
    *********************************************************************************/

    private void Awake() {
        model.TryGetComponent<Animator>(out animator);
        model.TryGetComponent<AnimEventInvoker>(out animEventInvoker);
        TryGetComponent<VisualModulator>(out visualModulator);
        enemyData = new EnemyData(scriptableObjEnemyData);
        enemyData.DieParticle.GetComponent<ParticleCallback>().onDestroyEvent.AddListener(DestroySelf);
        LookAtTarget = GameManager.Instance.playerGameObject.transform;
        healthBar.pipelineData = this.pipelineData;
    }
    private void Start() {
        Debug.Log("Catch");
        animEventInvoker.animCallback[(int)Enum_AnimState.Attack].AddListener( () => {
            projectileBucket.ProjectileInstantiator(Projectiles[0]);
        });
        animEventInvoker.animCallback[(int)Enum_AnimState.Jump].AddListener(() => {
            projectileBucket.ProjectileInstantiator(Projectiles[1]);
        });
    }

    public void GetDamaged(int _amount){
        if(mIsDie == true) {return;}
        enemyData.CurHP -= _amount;
        animator.SetTrigger("DoHit");
        enemyData.HitState.Invoke();
        if (enemyData.CurHP <= 0) {Die();}
    }

    public void GetDamaged(int _amount, GameObject particle){
        if(mIsDie == true) {return;}
        enemyData.CurHP -= _amount;
        enemyData.HitState.Invoke();
        visualModulator.Interact(particle);
        animator.SetTrigger("DoHit");
        if (enemyData.CurHP <= 0) {Die();}
    }

    public void Die(){ 
        enemyData.DieState.Invoke();
        mIsDie = true;
        GetComponent<Collider>().enabled = false;
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
        enemyData.AttackState.Invoke();
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoAttack");
    }

    [ContextMenu("범위데미지",false, int.MaxValue)]
    void InstantiateProjectiles2(){
        enemyData.AttackState.Invoke();
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoJump");
    }
}