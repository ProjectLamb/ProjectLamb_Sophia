using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
[System.Serializable]
public class SkillData
{
    [field: SerializeField] public E_SkillType SkillType;
    [field: SerializeField] public SKILL_KEY CurrentSkillKey;
    [field: SerializeField] public string SkillName;
    [field: SerializeField] public SkillRankInfo[] SkillRankInfos; // Normal, Rare, Epic
    [field: SerializeField] public string SkillDescription;
    [field: SerializeField] public List<Projectile> SkillProjectile;

    //Index는 SKILL_KEY
    [field: SerializeField] public UnityAction SkillUseState = () => { };
    [field: SerializeField] public UnityAction SkillLevelUpState = () => { };
    [field: SerializeField] public UnityAction SkillChangeState = () => { };

    public SkillData()
    {
        SkillProjectile = new List<Projectile>();

        SkillRankInfos = new SkillRankInfo[3];

        SkillRankInfos[(int)SKILL_KEY.Q] = new SkillRankInfo();
        SkillRankInfos[(int)SKILL_KEY.E] = new SkillRankInfo();
        SkillRankInfos[(int)SKILL_KEY.R] = new SkillRankInfo();

    }

    public SkillData Clone()
    {
        SkillData res = new SkillData();

        res.SkillType = this.SkillType;
        res.CurrentSkillKey = this.CurrentSkillKey;
        res.SkillName = this.SkillName;
        res.SkillDescription = this.SkillDescription;
        res.SkillProjectile = this.SkillProjectile;

        res.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL] =
            this.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE] =
            this.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC] =
            this.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL] =
            this.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE] =
            this.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC] =
            this.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL] =
            this.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE] =
            this.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC] =
            this.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillUseState = this.SkillUseState;
        res.SkillLevelUpState = this.SkillLevelUpState;
        res.SkillChangeState = this.SkillChangeState;
        res.SkillProjectile = this.SkillProjectile;

        return res;
    }

    public static SkillData operator +(SkillData x, SkillData y)
    {
        SkillData res = new SkillData();

        res.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL] =
            x.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL] +
            y.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE] =
            x.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE] +
            y.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC] =
            x.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC] +
            y.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL] =
            x.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL] +
            y.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE] =
            x.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE] +
            y.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC] =
            x.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC] +
            y.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL] =
            x.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL] +
            y.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE] =
            x.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE] +
            y.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC] =
            x.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC] +
            y.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillUseState = x.SkillUseState + y.SkillUseState;
        res.SkillLevelUpState = x.SkillLevelUpState + y.SkillLevelUpState;
        res.SkillChangeState = x.SkillChangeState + y.SkillChangeState;

        return res;
    }

    public static SkillData operator -(SkillData x, SkillData y)
    {
        SkillData res = new SkillData();

        res.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL] =
            x.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL] -
            y.SkillRankInfos[(int)SKILL_KEY.Q].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE] =
            x.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE] -
            y.SkillRankInfos[(int)SKILL_KEY.Q].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC] =
            x.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC] -
            y.SkillRankInfos[(int)SKILL_KEY.Q].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL] =
            x.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL] -
            y.SkillRankInfos[(int)SKILL_KEY.E].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE] =
            x.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE] -
            y.SkillRankInfos[(int)SKILL_KEY.E].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC] =
            x.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC] -
            y.SkillRankInfos[(int)SKILL_KEY.E].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL] =
            x.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL] -
            y.SkillRankInfos[(int)SKILL_KEY.R].numericArray[(int)SKILL_RANK.NORMAL];

        res.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE] =
            x.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE] -
            y.SkillRankInfos[(int)SKILL_KEY.R].skillDelay[(int)SKILL_RANK.RARE];

        res.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC] =
            x.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC] -
            y.SkillRankInfos[(int)SKILL_KEY.R].durateTime[(int)SKILL_RANK.EPIC];

        res.SkillUseState = x.SkillUseState - y.SkillUseState;
        res.SkillChangeState = x.SkillChangeState - y.SkillChangeState;
        res.SkillLevelUpState = x.SkillLevelUpState - y.SkillLevelUpState;
        return res;
    }
}
*/