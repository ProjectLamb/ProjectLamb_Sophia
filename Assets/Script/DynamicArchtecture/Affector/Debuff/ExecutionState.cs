using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Excute", menuName = "ScriptableObject/EntityAffector/Owner/Excute", order = int.MaxValue)]
public class ExecutionState : EntityAffector{
    /*아래 3줄은 EntityAffector 상속받아서 이미 있음*/
    //protected List<IEnumerator> AsyncAffectorCoroutine;
    //protected List<UnityAction> Affector;
    //protected Entity targetEntity //protected Entity ownerEntity;

    public float durationTime;    
    public Material skin;
    public ParticleSystem particles;

    public override void Init(Entity _owner, Entity _target){
        base.Init(_owner, _target);
        this.Affector.Add(Execution);
        this.Affector.Add(GameManager.Instance.globalEvent.HandleTimeSlow);
        this.Affector.Add(Camera.main.GetComponent<CameraEffect>().HandleZoomIn);
        this.AsyncAffectorCoroutine.Add(VisualActivate());
    }

    public override void Modifiy(IAffectable affectableEntity) {
        if(this.isInitialized == false) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        affectableEntity.AffectHandler(this.Affector);
        affectableEntity.AsyncAffectHandler(this.affectorType,this.AsyncAffectorCoroutine);
    }

    public void Execution (){
        this.targetEntity.GetDamaged((int)1<<16);
    }
    
    IEnumerator VisualActivate(){
        float tenacity = this.targetEntity.GetEntityData().Tenacity;
        float visualDurateTime = durationTime * (1 - tenacity);

        this.targetEntity.visualModulator.InteractByMaterial(skin, visualDurateTime);
        this.targetEntity.visualModulator.InteractByParticle(particles, visualDurateTime);
        yield return YieldInstructionCache.WaitForSeconds(visualDurateTime);
        this.targetEntity.visualModulator.Revert();
    }
}