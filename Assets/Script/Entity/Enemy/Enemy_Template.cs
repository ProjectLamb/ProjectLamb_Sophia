/*
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
public class Enemy_Template : Enemy
{    
    // public EnemyData enemyData;
    // public EntityData GetEntityData() {return this.enemyData;}
    // public AddingData addingData;
    // public AddingData GetAddingData(){return this.addingData;}
    // public GameObject model;
    // public Rigidbody entityRigidbody;
    // public Collider entityCollider;
    public Transform objectiveTarget;
    public NavMeshAgent nav;
    public bool chase;
    // public Projectile[] projectiles;
    // public ProjectileBucket projectileBucket;
    // public Animator animator;
    // public AnimEventInvoker animEventInvoker;
    // public VisualModulator visualModulator;

    public override void Die()
    {
        enemyData.DieState.Invoke();
        isDie = true;
        chase = false;
        entityRigidbody.velocity = Vector3.zero;
        Invoke("DestroySelf", 1f);
    }

    protected override void Awake() {
        TryGetComponent<VisualModulator>(out visualModulator);
        TryGetComponent<Rigidbody>(out entityRigidbody);
        TryGetComponent<NavMeshAgent>(out nav);
        TryGetComponent<Collider>(out entityCollider);
        
        model.TryGetComponent<Animator>(out animator);
        model.TryGetComponent<AnimEventInvoker>(out animEventInvoker);

        addingData = new AddingData();
        enemyData.DieParticle.GetComponent<ParticleCallback>().onDestroyEvent.AddListener(DestroySelf);

        chase = false;
        objectiveTarget = GameManager.Instance?.playerGameObject?.transform;
        isDie = false;
    }

    protected override void FixedUpdate()
    {
        
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        if (chase) {
            nav.SetDestination(objectiveTarget.position);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        if (chase) { nav.enabled = true;}
        else {nav.enabled = false;}
        nav.speed = (enemyData.MoveSpeed + addingData.MoveSpeed);
    }

    protected override void OnDestroy() {
        if(transform.parent.parent == null) return;
        if(!transform.parent.parent.TryGetComponent<StageGenerator>(out StageGenerator roomGenerator)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
        roomGenerator.DecreaseCurrentMobCount();
    }
}

*/