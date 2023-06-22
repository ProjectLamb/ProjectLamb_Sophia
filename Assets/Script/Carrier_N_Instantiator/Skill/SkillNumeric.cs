using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class SkillNumeric {
    [SerializeField] public Dictionary<SKILL_RANK, float> AffectorNumeric;
    [SerializeField] public float Duration;
}

/*
[System.Serializable]
public class SkillNumeric
{
    [SerializeField] public Dictionary<SKILL_RANK, float> AffectorNumeric;
    [SerializeField] public float Duration;
}

[SerializeField] public Dictionary<STATE_TYPE, SkillNumeric> NumericQ = new Dictionary<STATE_TYPE, SkillNumeric>();

NumericR.Add(STATE_TYPE.MOVE_SPEED_UP, new SkillNumeric {
    AffectorNumeric = new Dictionary<SKILL_RANK, float> {
        {SKILL_RANK.NORMAL   , 0.1f},
        {SKILL_RANK.RARE     , 0.2f},
        {SKILL_RANK.EPIC     , 0.3f}
    },
    Duration = 10f
});
*/