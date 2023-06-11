using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillElement : MonoBehaviour {
    public float[]      nemericArray    = new float[Enum.GetValues(typeof(E_SkillRank)).Length];
    public float[]      skillDelay      = new float[Enum.GetValues(typeof(E_SkillRank)).Length];
    public float[]      durateTime      = new float[Enum.GetValues(typeof(E_SkillRank)).Length];
    public Carrier[]    skillCarrier    = new Carrier[Enum.GetValues(typeof(E_SkillRank)).Length];

    public Carrier GetCarrierRank(E_SkillRank _rank){
        return skillCarrier[(int)_rank];
    }
    public float GetNumericByRank(E_SkillRank _rank){
        return nemericArray[(int)_rank];
    }
    public float GetSkillDelayByRank(E_SkillRank _rank){
        return skillDelay[(int)_rank];
    }
    public float GetDurateTimeByRank(E_SkillRank _rank){
        return durateTime[(int)_rank];
    }
}