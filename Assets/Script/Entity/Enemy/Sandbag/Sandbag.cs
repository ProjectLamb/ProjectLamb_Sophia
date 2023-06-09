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
    public ScriptableObjEnemyData ScriptableED;
    private EntityData BaseEnemyData;
    private EntityData FinalData;
    public override ref EntityData GetFinalData() {    return ref this.FinalData;}
    public override     EntityData GetOriginData() {return this.BaseEnemyData;}
    
    public override void ResetData(){
        FinalData = BaseEnemyData;
    }

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

        BaseEnemyData = new EntityData(ScriptableED);
        FinalData = BaseEnemyData;
        CurrentHealth = FinalData.MaxHP;

        this.objectiveTarget = GameManager.Instance.playerGameObject.transform;
    }
    private void Start() {
        animEventInvoker.animCallback[(int)E_AnimState.Attack].AddListener( () => {
            projectileBucket.ProjectileInstantiator(this, projectiles[0]);
        });
        animEventInvoker.animCallback[(int)E_AnimState.Jump].AddListener(() => {
            projectileBucket.ProjectileInstantiator(this, projectiles[1]);
        });
        this.FinalData.HitStateRef = (ref int amount) => {imageGenerator.GenerateImage(amount);};
    }

    public override void GetDamaged(int _amount){
        if(IsDie == true) {return;}
        FinalData.HitStateRef.Invoke(ref _amount);
        CurrentHealth -= _amount;
        animator.SetTrigger("DoHit");
        if (CurrentHealth <= 0) {Die();}
    }

    public override void GetDamaged(int _amount, VFXObject _vfx){
        if(IsDie == true) {return;}
        FinalData.HitStateRef.Invoke(ref _amount);
        CurrentHealth -= _amount;
        visualModulator.InteractByVFX(_vfx);
        animator.SetTrigger("DoHit");
        if (CurrentHealth <= 0) {Die();}
    }

    public override void Die(){ 
        FinalData.DieState.Invoke();
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
}