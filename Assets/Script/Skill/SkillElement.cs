using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class SkillNumeric{
    [SerializeField] public float affectorDurationTime;
    [SerializeField] public float amountRatio;
    public SkillNumeric skillNumeric;
    public List<float> ToList(){
        List<float> res = new List<float> {
            this.skillNumeric.affectorDurationTime, 
            this.skillNumeric.amountRatio
        };
        return res;
    }
}

public class SkillElement : MonoBehaviour {
    public E_SkillKey skillKey;
    [SerializedDictionary("Skill Rank", "Numeric")]

    public SerializedDictionary<E_SkillRank, SkillNumeric> numericsArray = new SerializedDictionary<E_SkillRank, SkillNumeric>();
    
    [SerializedDictionary("Skill Rank", "Float")]
    public SerializedDictionary<E_SkillRank, float> coolTime  = new SerializedDictionary<E_SkillRank, float>();
    public SerializedDictionary<E_SkillRank, List<Projectile>> skillCarrier    = new SerializedDictionary<E_SkillRank, List<Projectile>>();

}