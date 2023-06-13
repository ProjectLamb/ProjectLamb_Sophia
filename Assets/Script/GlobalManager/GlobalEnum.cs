using System;
using UnityEngine;

public enum E_StateType{
    //0 ~ 99 // 디버프
    Move        = 0, Dash, Attack, Skill, GetDamaged, Die, Trigger,
    //100 ~ 199 //디버프
    Burn        = 100     ,Poisend ,Bleed ,Contracted ,Freeze ,Confused ,Fearing ,Stern ,Bounded, Execution, KnockBack,
    //200 ~ 299
    MoveSpeedUp = 200   , TenacityUp, PowerUp, AttackSpeedUp, Barrier, Invisible, Invincible, CriticalAttack, Defence
}


public enum E_AnimState {
    Idle, Attack, Jump, Die
}

public enum E_BucketPosition {
    Inner, Outer
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
