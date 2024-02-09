using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Sophia_Carriers;
using Sophia.Composite;

public class Sandbag : Entity
{    
    /* 아래 4줄은 절때 활성화 하지마라. 상속받은 Entity에 이미 정의 되어 있다. */
    //public Collider entityCollider;
    //public Rigidbody entityRigidbody;
    //public VisualModulator visualModulator;
    //public GameObject model;
    
    [SerializeField]
    public ScriptableObjEnemyData    ScriptableED;
    private EntityData               BaseEnemyData;
    private EntityData               FinalData;

    public override ref EntityData GetFinalData() {return ref this.FinalData;}
    public override     EntityData GetOriginData() {return this.BaseEnemyData;}
    public override void ResetData(){ FinalData = BaseEnemyData; }

    public Transform                ObjectiveTarget;
    public bool                     IsDie;
    public Projectile[]             AttackProjectiles;
    public VFXObject                DieParticle;
    public ImageGenerator           DamageBarGenerator;
    private Animator                mAnimator;
    private AnimEventInvoker        mAnimEventInvoker;
    
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
        model.TryGetComponent<Animator>(out this.mAnimator);
        model.TryGetComponent<AnimEventInvoker>(out this.mAnimEventInvoker);

        BaseEnemyData = new EntityData(ScriptableED);
        FinalData = BaseEnemyData;
        Life = new Sophia.Composite.LifeComposite(FinalData.MaxHP);
        // Life.OnHit += FinalData.HitStateRef;
        Life.OnDamaged += (DamageInfo val) => {
                mAnimator.SetTrigger("DoHit");
                GameManager.Instance.GlobalEvent.OnEnemyHitEvent.ForEach(E => E.Invoke());
            };
        Life.OnEnterDie += FinalData.DieState;
        Life.OnEnterDie += () => {
                this.entityCollider.enabled = false;
                mAnimator.SetTrigger("DoDie");
                visualModulator.InteractByVFX(DieParticle);
            };

        this.ObjectiveTarget = GameManager.Instance.PlayerGameObject.transform;
    }

    public void UseProjectile_NormalAttack(){
        this.carrierBucket.CarrierInstantiatorByObjects(this, AttackProjectiles[0], new object[] {FinalData.Power * 1});
    }
    public void UseProjectile_JumpAttack(){
        this.carrierBucket.CarrierInstantiatorByObjects(this, AttackProjectiles[1], new object[] {FinalData.Power * 2});
    }
    private void Start() {
        this.FinalData.HitStateRef = (ref float amount) => {DamageBarGenerator.GenerateImage((int)amount);};
    }

    public override void GetDamaged(int _amount){
        if(Life.IsDie == true) {return;}
        DamageInfo info = new DamageInfo{
            affectType = Sophia.E_AFFECT_TYPE.None,
            damageAmount = _amount,
            damageRatio = 1,
            criticalDamage = false,
            dodgeDamage = false
        };
        Life.Damaged(info);
    }

    public override void GetDamaged(int _amount, VFXObject _vfx){
        if(Life.IsDie == true) {return;}
        DamageInfo info = new DamageInfo{
            affectType = Sophia.E_AFFECT_TYPE.None,
            damageAmount = _amount,
            damageRatio = 1,
            criticalDamage = false,
            dodgeDamage = false
        };
        Life.Damaged(info);

        visualModulator.InteractByVFX( _vfx);
    }

    public override void Die(){ 
        Life.Died();
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }

    private void Update() {
        /***************************/
        if(GameManager.Instance?.GlobalEvent.IsGamePaused == true){return;}
        /***************************/
        if(!IsDie)transform.LookAt(ObjectiveTarget);
    }
    
    [ContextMenu("평타", false, int.MaxValue)]
    void InstantiateProjectiles1(){
        //Find Instantiate On This Animator Events;
        mAnimator.SetTrigger("DoAttack");
    }

    [ContextMenu("범위데미지",false, int.MaxValue)]
    void InstantiateProjectiles2(){
        //Find Instantiate On This Animator Events;
        mAnimator.SetTrigger("DoJump");
    }
}