using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

[System.Serializable]
public class BarrierState : EntityAffector {
//  [SerializeField]    public AffectorStruct affectorStruct;
//  public bool  isInitialized;    
//  [HideInInspector]   public Entity targetEntity;
//  [HideInInspector]   public Entity ownerEntity;
    public float                DurationTime;
    public float                Ratio;
    public BarrierProjectile    barrier;
    public Carrier              instantBarrier;

    public BarrierState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = STATE_TYPE.BARRIER;
        this.affectorStruct.AsyncAffectorCoroutine.Add(BarrierGenerate());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        BarrierState Instance = new BarrierState(EAInstance);
        Instance.barrier = this.barrier;
        Instance.isInitialized  = true;
        Instance.DurationTime = this.DurationTime;
        Instance.Ratio = this.Ratio;
    
        return Instance;
    }
//  public virtual void Modifiy(){
//      if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
//      targetEntity.AffectHandler(affectorStruct);
//  }

    public override void SetValue(List<float> objects)
    {
        DurationTime    = objects[0];
        Ratio           = objects[1];
    }
    IEnumerator BarrierGenerate(){
        float passedTime = 0;
        int barrierHealth = (int)(( (float)(this.ownerEntity.GetFinalData().MaxHP) * Ratio) + 0.5f);
        instantBarrier = ownerEntity.carrierBucket.CarrierInstantiatorByObjects(ownerEntity, barrier, BUCKET_POSITION.INNER, new object[]{DurationTime, barrierHealth});
        while(DurationTime > passedTime ){
            passedTime += Time.fixedDeltaTime;
            if(!instantBarrier.IsActivated) {instantBarrier.DestroySelf(); yield break;}
            yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
        }
        instantBarrier.DestroySelf();
    }
}