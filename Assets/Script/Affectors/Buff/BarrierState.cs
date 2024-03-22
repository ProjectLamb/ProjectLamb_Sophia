using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

[System.Serializable]
public class BarrierState : EntityAffector {
//  [SerializeField]    public AffectorPackage affectorPackage;
//  public bool  isInitialized;    
//  [HideInInspector]   public Entity targetEntity;
//  [HideInInspector]   public Entity targetEntity;
    public float                        DurationTime;
    public float                        Ratio;
    public BarrierProjectile            Barrier;
    [HideInInspector] 
    private Carrier                     instantBarrier;

    public BarrierState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.targetEntity    = _eaData.targetEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.BARRIER;
        this.affectorPackage.AsyncAffectorCoroutine.Add(BarrierGenerate());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        BarrierState Instance = new BarrierState(EAInstance);
        Instance.Barrier = this.Barrier;
        Instance.isInitialized  = true;
        Instance.DurationTime = this.DurationTime;
        Instance.Ratio = this.Ratio;
    
        return Instance;
    }
//  public virtual void Modifiy(){
//      if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
//      targetEntity.AffectHandler(affectorPackage);
//  }

    public override void SetValue(List<float> objects)
    {
        DurationTime    = objects[0];
        Ratio           = objects[1];
    }
    IEnumerator BarrierGenerate(){
        float passedTime = 0;
        int BarrierHealth = (int)(( (float)(this.targetEntity.GetFinalData().MaxHP) * Ratio) + 0.5f);
        instantBarrier = targetEntity.carrierBucket.CarrierInstantiatorByObjects(targetEntity, Barrier, new object[]{DurationTime, BarrierHealth});
        while(DurationTime > passedTime ){
            passedTime += Time.fixedDeltaTime;
            if(!instantBarrier.IsActivated) {instantBarrier.DestroySelf(); yield break;}
            yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
        }
        instantBarrier.DestroySelf();
    }
}