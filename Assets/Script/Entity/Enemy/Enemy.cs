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
public class Enemy : MonoBehaviour, IEntityAddressable
{
    /**/
    [field : SerializeField]
    public EnemyData enemyData;
    public EntityData GetEntityData() {return this.enemyData;}

    public GameObject model;
    public Rigidbody entityRigidbody;
    public Collider entityCollider;

    public UnityEngine.AI.NavMeshAgent nav;
    public Transform objectiveTarget;
    public bool chase;
    public bool isDie;

    public Projectile[] projectiles;

    public ProjectileBucket projectileBucket;
    public Animator animator;
    public AnimEventInvoker animEventInvoker;
    public VisualModulator visualModulator;

    public virtual void Die()
    {
        enemyData.DieState.Invoke();
        isDie = true;
        chase = false;
        entityRigidbody.velocity = Vector3.zero;
        Invoke("DestroySelf", 0.5f);
    }

    public virtual void GetDamaged(int _amount){
        if(isDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        if (enemyData.CurHP <= 0) {this.Die();}
    }
    public virtual void GetDamaged(int _amount, GameObject particle){
        if(isDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        visualModulator.Interact(particle);
        if (enemyData.CurHP <= 0) {this.Die();}
    }
    public void AffectHandler(List<UnityAction> _action){
        _action.ForEach(E => E.Invoke());
    }

    public void AsyncAffectHandler(List<IEnumerator> _coroutine){
        _coroutine.ForEach(E => StartCoroutine(E));
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }


    protected virtual void Awake()
    {
        TryGetComponent<VisualModulator>(out visualModulator);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        TryGetComponent<UnityEngine.AI.NavMeshAgent>(out nav);
        TryGetComponent<Collider>(out entityCollider);
        
        model.TryGetComponent<Animator>(out animator);
        model.TryGetComponent<AnimEventInvoker>(out animEventInvoker);

        enemyData.DieParticle.GetComponent<ParticleCallback>().onDestroyEvent.AddListener(DestroySelf);

        chase = false;
        objectiveTarget = GameManager.Instance?.playerGameObject?.transform;
        isDie = false;
    }

    protected virtual void FixedUpdate()
    {
        /***************************/
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        /***************************/
        if (chase) {
            nav.SetDestination(objectiveTarget.position);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        /***************************/
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        /***************************/
        if (chase) { nav.enabled = true;}
        else {nav.enabled = false;}
        nav.speed = enemyData.MoveSpeed;
    }

    //protected virtual void OnDestroy() {
    //    if(transform.parent.parent == null) return;
    //    if(!transform.parent.parent.TryGetComponent<StageGenerator>(out StageGenerator roomGenerator)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
    //    roomGenerator.DecreaseCurrentMobCount();
    //}
}
