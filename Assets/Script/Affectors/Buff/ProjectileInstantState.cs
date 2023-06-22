using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

[System.Serializable]
public class ProjectileGenState : EntityAffector {
    public float DurationTime;
    public float RepeatTimeInterval;
    public Projectile projectile;

    public ProjectileGenState(EntityAffector _eaData){
        this.affectorPackage = _eaData.affectorPackage;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
        this.affectorPackage.affectorType = STATE_TYPE.PROJECTILE_GENERATOR;
        this.affectorPackage.AsyncAffectorCoroutine.Add(Generator());
    }

    public override EntityAffector Init(Entity _owner, Entity _target)
    {
        EntityAffector EAInstance = base.Init(_owner, _target);
        ProjectileGenState Instance = new ProjectileGenState(EAInstance);
        Instance.DurationTime = this.DurationTime;
        Instance.RepeatTimeInterval = this.RepeatTimeInterval;
        Instance.projectile = this.projectile;

        Instance.isInitialized  = true;
        return Instance;
    }

    public override void SetValue(List<float> objects)
    {
        DurationTime            = objects[0];
        RepeatTimeInterval      = objects[1];
    }
    IEnumerator Generator(){
        float passedTime = 0;
        while(DurationTime > passedTime){
            passedTime += RepeatTimeInterval;
            ownerEntity.carrierBucket.CarrierInstantiator(ownerEntity, projectile);
            yield return YieldInstructionCache.WaitForSeconds(RepeatTimeInterval);
        }
    }
}