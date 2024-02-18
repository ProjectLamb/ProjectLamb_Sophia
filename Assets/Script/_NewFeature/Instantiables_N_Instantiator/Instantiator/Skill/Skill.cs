using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sophia.Instantiates {
    using Sophia.Entitys;
    using Sophia.State;
    using Sophia.Composite.RenderModels;
    public enum E_SKILL_TYPE {
        None = 0, Neutral, Melee
    }

    public enum E_SKILL_KEY {
        None = 0, QKey, EKey, RKey
    }

    public enum E_SKILL_RANK {
        None = 0, Normal, Rare, Epic
    }

    [System.Serializable]
    public struct SerialSkillData
    {   
        [SerializeField] public int             _skillID;
        [SerializeField] public string          _skillName;
        [SerializeField] public E_SKILL_RANK    _skillRank;
        [SerializeField] public E_SKILL_TYPE    _skillType;
        [SerializeField] public SerialSkillRankElementData  _skillNormalRankData;
        [SerializeField] public SerialSkillRankElementData  _skillRareRankData;
        [SerializeField] public SerialSkillRankElementData  _skillEpicRankData;
    }

    public struct SerialSkillRankElementData {
        [SerializeField] public bool    _isRankExist;
        [SerializeField] public string  _description;
        [SerializeField] public Sprite  _icon;
        [SerializeField] public float   _coolTime;
        [SerializeField] public SerialStatCalculateDatas    _statCalculateDatas;
        [SerializeField] public SerialExtrasCalculateDatas  _extrasCalculateDatas;
    }
    
    /// <summary>
    /// Q, E, R 각각마다 들어갈 수 있는 스킬이고, Rank에 대한 데이터가 존재합니다.
    /// </summary>
    public abstract class Skill : IUserInterfaceAccessible {
        public readonly string Name;
        public readonly E_SKILL_TYPE SkillType;
        public readonly E_SKILL_RANK SkillRank;
        public readonly bool[]    IsRankExist = new bool[3];
        public readonly string[]  Description = new string[3];
        public readonly Sprite[]  Icon = new Sprite[3];
        public readonly float[]   CoolTime = new float[3];
        // 어펙트 데이터가 들어가 있어야 한다.

        // public Skill(SerialSkillData skillData)
        public Skill() {

        }

        public abstract void Use();

        public string GetName() => this.Name;
        public string GetDescription() => this.Description[(int)SkillRank];
        public Sprite GetSprite() => this.Icon[(int)SkillRank];
    }

    public class SkillCard : IUserInterfaceAccessible
    {

        public readonly Skill QSkill;
        public readonly Skill ESkill;
        public readonly Skill RSkill;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public Sprite GetSprite()
        {
            throw new NotImplementedException();
        }
    }
}