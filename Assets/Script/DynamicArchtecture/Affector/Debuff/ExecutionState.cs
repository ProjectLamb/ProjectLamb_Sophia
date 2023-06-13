using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ExecutionState : EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  public AffectorStruct affectorStruct;
//  public Entity targetEntity;
//  public Entity ownerEntity;
//  public bool  isInitialized;
    public float DurationTime;    
    public Material skin;
    public VFXObject vfx;

    //자가 복제를 하는놈이다.
    public ExecutionState(EntityAffector _eaData) {
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorStruct.affectorType = E_StateType.Execution;
        this.affectorStruct.Affector.Add(Execution);
        this.affectorStruct.Affector.Add(GameManager.Instance.globalEvent.HandleTimeSlow);
        this.affectorStruct.Affector.Add(Camera.main.GetComponent<CameraEffect>().HandleZoomIn);
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
    }

    public override EntityAffector Init(Entity _owner, Entity _target){
        EntityAffector EAInstance = base.Init(_owner, _target);
        ExecutionState Instance = new ExecutionState(EAInstance);
        Instance.DurationTime   = this.DurationTime;
        Instance.skin           = this.skin;
        Instance.vfx            = this.vfx;
        Instance.isInitialized  = true;
        return Instance;
    }

//  public virtual void Modifiy(){
//      if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
//      targetEntity.AffectHandler(affectorStruct);
//  }

    public void Execution (){
        targetEntity.GetDamaged((int)1<<16);
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetFinalData().Tenacity;
        float visualDurateTime = DurationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorStruct.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorStruct.affectorType);
    }
}