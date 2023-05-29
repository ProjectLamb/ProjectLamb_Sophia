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

    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.affectorStruct.affectorType = E_StateType.Execution;
        this.affectorStruct.Affector.Add(Execution);
        this.affectorStruct.Affector.Add(GameManager.Instance.globalEvent.HandleTimeSlow);
        this.affectorStruct.Affector.Add(Camera.main.GetComponent<CameraEffect>().HandleZoomIn);
        this.affectorStruct.AsyncAffectorCoroutine.Add(VisualActivate());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AffectHandler(affectorStruct);
    }

    public void Execution (){
        this.targetEntity.GetDamaged((int)1<<16);
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetEntityData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin);
        this.targetEntity.visualModulator.InteractByVFX(vfx);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.Revert(this.affectorStruct.affectorType);
    }
}