using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Entity {
//  [HideInInspector] public Collider entityCollider;
//  [HideInInspector] public Rigidbody entityRigidbody;
//  [HideInInspector] public GameObject model;
//  [SerializeField]  public VisualModulator visualModulator;
//  [SerializeField]  public int mCurrentHealth;
    public  ScriptableObjEntityData ScriptableED;
    private EntityData BaseBarrierData;
    private EntityData FinalData;
    public  ParticleSystem DieParticle;
    private UnityActionRef<int> HitState;
    
//  public int CurrentHealth
//  {
//      get { return mCurrentHealth; }
//      set
//      {
//          mCurrentHealth = value;
//          if (mCurrentHealth < 0) { mCurrentHealth = 0; }
//      }
//  }
    public float DurationTime;

//  public Dictionary<E_StateType, AffectorStruct> affectorStacks;
//  protected virtual void Awake()
//  {
//      TryGetComponent<Collider>(out entityCollider);
//      TryGetComponent<Rigidbody>(out entityRigidbody);
//      model ??= transform.GetChild(0).Find("modle").gameObject;
//      affectorStacks = new Dictionary<E_StateType, AffectorStruct>();
//  }

    public override ref EntityData GetFinalData (){ return ref this.FinalData; }
    public override     EntityData GetOriginData(){ return this.BaseBarrierData; }
    public override void ResetData(){

    }
    public override void GetDamaged(int _amount){
        CurrentHealth -= _amount;
        if (CurrentHealth <= 0) {this.Die();}
    }
    public override void GetDamaged(int _amount, VFXObject _vfx){
        GetDamaged(_amount);
        if (CurrentHealth <= 0) {this.Die();}
    }
    public override void Die(){
        DestroySelf();
    }
    public void DestroySelf(){
        Destroy(gameObject);
    }
    private void OnDestroy() {
        PlayerDataManager.GetEntityData().HitStateRef -= HitState;
    }
    public override void AffectHandler(AffectorStruct _affectorStruct) {return;}
    protected override void Awake() {
        base.Awake();
        BaseBarrierData = new EntityData(ScriptableED);
        FinalData = BaseBarrierData;
    }
    private void Start() {
        HitState += (ref int _amount) => {SetPlayerHitIgnore(ref _amount);};
        PlayerDataManager.GetEntityData().HitStateRef += HitState;
        Invoke("Die", DurationTime);
    }

    private void SetPlayerHitIgnore(ref int _amount){
        _amount = 0;
    }
}