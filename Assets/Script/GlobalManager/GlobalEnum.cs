using UnityEngine;

public enum STATE_TYPE{
    //0 ~ 99 // 디버프
    MOVE        = 0, DASH, ATTACK, SKILL, GET_DAMAGE, DIE, TRIGGER,
    //100 ~ 199 //디버프
    BURN        = 100     ,POISONED ,BLEED ,CONTRACTED ,FREEZE ,CONFUSED ,FEAR ,STERN  ,BOUNDED, EXECUTION, KNOCKBACK,
    //200 ~ 299
    MOVE_SPEED_UP = 200   , TENACITY_UP, POWER_UP, ATTACK_SPEED_UP , BARRIER, INVISIBLE, INVINCIBLE, ON_HIT, DEFENCE, PROJECTILE_GENERATOR
}


public enum ANIME_STATE {
    IDLE, ATTACK, JUMP, DIE
}

public enum CARRIER_TYPE {
    PORTAL = 0, ROULETTE, ATTACK, NEUTRAL, ITEM
}

public enum BUCKET_POSITION {
    INNER, OUTER
}

public enum WEAPON_TYPE
{
    MELEE, RANGER, MAGE
}


public enum SKILL_TYPE {
    NEUTRAL, ATTACK
}
[SerializeField]
public enum SKILL_KEY {
    Q, E, R
}

[SerializeField]
public enum SKILL_RANK {
    NORMAL, RARE, EPIC
}

public enum UNITY_TAGS {
    Mesh,
    Wall,
    Enemy,
    Portal,
    PlayerProjectile,
    EnemyProjectile,
    Equipment,
    DebugUI,
}