using System;
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
    public VFXObject DieParticle;
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
        base.Awake();
        model.TryGetComponent<Animator>(out this.animator);
        model.TryGetComponent<AnimEventInvoker>(out this.animEventInvoker);
        
        this.objectiveTarget = GameManager.Instance.playerGameObject.transform;
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

    public override void GetDamaged(int _amount, GameObject _obj){
        if(IsDie == true) {return;}
        enemyData.HitStateRef.Invoke(ref _amount);
        enemyData.CurHP -= _amount;
        visualModulator.InteractByGameObject(_obj);
        animator.SetTrigger("DoHit");
        if (enemyData.CurHP <= 0) {Die();}
    }

    public override void Die(){ 
        enemyData.DieState.Invoke();
        IsDie = true;
        this.entityCollider.enabled = false;
        animator.SetTrigger("DoDie");
        visualModulator.InteractByVFX(DieParticle);
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
    
    public override void AffectHandler(AffectorStruct affectorStruct){
        if(affectorStacks.ContainsKey(affectorStruct.affectorType).Equals(false)){ 
            affectorStacks.Add(affectorStruct.affectorType, affectorStruct);
        }
        else {
            foreach(IEnumerator coroutine in affectorStacks[affectorStruct.affectorType].AsyncAffectorCoroutine){
                StopCoroutine(coroutine);
            }
        }
        affectorStacks[affectorStruct.affectorType].Affector.ForEach((E) => E.Invoke());
        affectorStacks[affectorStruct.affectorType] = affectorStruct;
        foreach(IEnumerator coroutine in affectorStacks[affectorStruct.affectorType].AsyncAffectorCoroutine){
            StartCoroutine(coroutine);
        }
    }
}