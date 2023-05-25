using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillRankInfo
{
    public int[] numericArray = new int[3];
    public float[] skillDelay = new float[3];
    public float[] durateTime = new float[3];

    public SkillRankInfo(){
        this.numericArray[(int)E_SkillRank.Normal] = 0;
        this.skillDelay[(int)E_SkillRank.Rare] = 0;
        this.durateTime[(int)E_SkillRank.Epic] = 0;
    }
    public void Clear() {
        this.numericArray[(int)E_SkillRank.Normal] = 0;
        this.skillDelay[(int)E_SkillRank.Rare] = 0;
        this.durateTime[(int)E_SkillRank.Epic] = 0;
    }
}
public enum E_SkillType {
    Neutral, Weapon
}
[SerializeField]
public enum E_SkillKey {
    Q, E, R
}

[SerializeField]
public enum E_SkillRank {
    Normal, Rare, Epic
}
