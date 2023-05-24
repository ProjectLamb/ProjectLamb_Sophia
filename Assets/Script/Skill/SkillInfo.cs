using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillInfo
{
    [SerializeField]
    public enum Rank {
        Normal, Rare, Epic
    }

    public int[] numericArray = new int[3];
    public float[] skillDelay = new float[3];
    public float[] durateTime = new float[3];

    public SkillInfo(){
        for(int j = 0; j < 3; j++){
            this.numericArray[j] = 0;
            this.skillDelay[j] = 0;
            this.durateTime[j] = 0;
        }
    }
    public void Clear() {
        for(int j = 0; j < 3; j++){
            this.numericArray[j] = 0;
            this.skillDelay[j] = 0;
            this.durateTime[j] = 0;
        }
    }
}
public enum E_SkillType {
    Neutral, Weapon
}
[SerializeField]
public enum E_SkillKey {
    Q, E, R
}