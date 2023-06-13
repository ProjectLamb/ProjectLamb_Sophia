using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarrierState : EntityAffector {
//  [SerializeField]    public AffectorStruct affectorStruct;
//  public bool  isInitialized;    
//  [HideInInspector]   public Entity targetEntity;
//  [HideInInspector]   public Entity ownerEntity;
    public float DurationTime;
    public float Ratio;
    public Barrier      barrierEnttiy;
    public Barrier      InstantBarrierEntity;

    public BarrierState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = E_StateType.Barrier;
        this.affectorStruct.AsyncAffectorCoroutine.Add(BarrierGenerate());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        BarrierState Instance = new BarrierState(EAInstance);
        Instance.barrierEnttiy = this.barrierEnttiy;
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
        barrierEnttiy.CurrentHealth = (int)(( (float)(this.ownerEntity.GetFinalData().MaxHP) * Ratio) + 0.5f);
        barrierEnttiy.DurationTime  = DurationTime;
        InstantBarrierEntity = GameObject.Instantiate(barrierEnttiy, ownerEntity.projectileBucket.transform);
        yield return YieldInstructionCache.WaitForSeconds(DurationTime);
        InstantBarrierEntity?.Die();
    }
}