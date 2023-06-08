using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillData {
    [field : SerializeField]public E_SkillType SkillType;
    [field : SerializeField]public E_SkillKey CurrentSkillKey;
    [field : SerializeField]public string SkillName;
    [field : SerializeField]public SkillRankInfo[] SkillRankInfos; // Normal, Rare, Epic
    [field : SerializeField]public string SkillDescription;
    [field : SerializeField]public List<Projectile> SkillProjectile;
    
    //Index는 E_SkillKey
    [field : SerializeField]public UnityAction SkillUseState = () => {};
    [field : SerializeField]public UnityAction SkillLevelUpState = () => {};
    [field : SerializeField]public UnityAction SkillChangeState = () => {};
    
    public SkillData() {
        SkillProjectile = new List<Projectile>();
        
        SkillRankInfos = new SkillRankInfo[3];
        
        SkillRankInfos[(int)E_SkillKey.Q] = new SkillRankInfo();
        SkillRankInfos[(int)E_SkillKey.E] = new SkillRankInfo();
        SkillRankInfos[(int)E_SkillKey.R] = new SkillRankInfo();
        
    }

    public SkillData Clone(){
        SkillData res = new SkillData();

        res.SkillType   = this.SkillType;
        res.CurrentSkillKey = this.CurrentSkillKey;
        res.SkillName   = this.SkillName;
        res.SkillDescription    = this.SkillDescription;
        res.SkillProjectile = this.SkillProjectile;

        res.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal] = 
            this.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare]     = 
            this.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic]     = 
            this.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic];

        res.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal] = 
            this.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare]     = 
            this.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic]     = 
            this.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic];

        res.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal] = 
            this.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare]     = 
            this.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic]     = 
            this.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic];

        res.SkillUseState = this.SkillUseState;
        res.SkillLevelUpState = this.SkillLevelUpState;
        res.SkillChangeState = this.SkillChangeState;
        res.SkillProjectile = this.SkillProjectile;

        return res;
    }

    public static SkillData operator +(SkillData x, SkillData y){
        SkillData res = new SkillData();

        res.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal] = 
            x.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal] + 
            y.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare]     = 
            x.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare] + 
            y.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic]     = 
            x.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic] + 
            y.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic];

        res.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal] = 
            x.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal] + 
            y.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare]     = 
            x.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare] + 
            y.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic]     = 
            x.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic] + 
            y.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic];

        res.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal] = 
            x.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal] + 
            y.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare]     = 
            x.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare] + 
            y.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic]     = 
            x.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic] + 
            y.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic];

        res.SkillUseState = x.SkillUseState + y.SkillUseState;
        res.SkillLevelUpState = x.SkillLevelUpState + y.SkillLevelUpState;
        res.SkillChangeState = x.SkillChangeState + y.SkillChangeState;

        return res;
    }

    public static SkillData operator -(SkillData x, SkillData y){
        SkillData res = new SkillData();

        res.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal] = 
            x.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal] - 
            y.SkillRankInfos[(int)E_SkillKey.Q].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare]     = 
            x.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare] - 
            y.SkillRankInfos[(int)E_SkillKey.Q].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic]     = 
            x.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic] - 
            y.SkillRankInfos[(int)E_SkillKey.Q].durateTime[(int)E_SkillRank.Epic];

        res.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal] = 
            x.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal] - 
            y.SkillRankInfos[(int)E_SkillKey.E].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare]     = 
            x.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare] - 
            y.SkillRankInfos[(int)E_SkillKey.E].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic]     = 
            x.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic] - 
            y.SkillRankInfos[(int)E_SkillKey.E].durateTime[(int)E_SkillRank.Epic];

        res.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal] = 
            x.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal] - 
            y.SkillRankInfos[(int)E_SkillKey.R].numericArray[(int)E_SkillRank.Normal];
              
        res.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare]     = 
            x.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare] - 
            y.SkillRankInfos[(int)E_SkillKey.R].skillDelay[(int)E_SkillRank.Rare];

        res.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic]     = 
            x.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic] - 
            y.SkillRankInfos[(int)E_SkillKey.R].durateTime[(int)E_SkillRank.Epic];

        res.SkillUseState = x.SkillUseState - y.SkillUseState;
        res.SkillChangeState = x.SkillChangeState - y.SkillChangeState;
        res.SkillLevelUpState = x.SkillLevelUpState - y.SkillLevelUpState;
        return res;
    }
}