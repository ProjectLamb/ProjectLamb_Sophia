using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dodge", menuName = "ScriptableObject/EntityAffector/Buff/Dodge", order = int.MaxValue)]
public class DodgeState : EntityAffector {
    public float durationTime;
    public DodgeState(EntityAffector _eaData){
        this.affectorStruct = _eaData.affectorStruct;
        this.targetEntity   = _eaData.targetEntity;
        this.ownerEntity    = _eaData.ownerEntity;
        this.isInitialized  = _eaData.isInitialized;
    }
}
