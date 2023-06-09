using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Excute", menuName = "ScriptableObject/EntityAffector/Debuff/Excute", order = int.MaxValue)]
public class ExecutionState : EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
//  protected AffectorStruct affectorStruct;
    // affectorStruct.affectorType;
    // affectorStruct.AsyncAffectorCoroutine;
    // affectorStruct.Affector;
//  protected Entity targetEntity;
//  protected Entity ownerEntity;
//  protected bool  isInitialized;
    public float durationTime;    
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
        Instance.durationTime   = this.durationTime;
        Instance.skin           = this.skin;
        Instance.vfx            = this.vfx;
        Instance.isInitialized  = true;
        return Instance;
    }

    public void Execution (){
        targetEntity.GetDamaged((int)1<<16);
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetFinalData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.RevertByMaterial(this.affectorStruct.affectorType);
        this.targetEntity.visualModulator.RevertByVFX(this.affectorStruct.affectorType);
    }
}