using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ExecutionState : EntityAffector{
/*  아래 3줄은 EntityAffector 상속받아서 이미 있음  */
//  public AffectorPackage affectorPackage;
//  public Entity targetEntity;
//  public Entity ownerEntity;
//  public bool  isInitialized;
    public float DurationTime;    
    public Material skin;
    public VFXObject vfx;

    //자가 복제를 하는놈이다.
    public ExecutionState(EntityAffector _eaData) {
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.EXECUTION;
        this.affectorPackage.Affector.Add(Execution);
        //this.affectorPackage.Affector.Add(GameManager.Instance.GlobalEvent.HandleTimeSlow);
        //this.affectorPackage.Affector.Add(Camera.main.GetComponent<CameraEffect>().HandleZoomIn);
        this.affectorPackage.AsyncAffectorCoroutine.Add(VisualActivate());
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
//      targetEntity.AffectHandler(affectorPackage);
//  }

    public void Execution (){
        targetEntity.GetDamaged((int)1<<16);
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetFinalData().Tenacity;
        float visualDurateTime = DurationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX( vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorPackage.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorPackage.affectorType);
    }
}