using UnityEngine;

/////////////////////////////////////////////////////////////////////////////////

#region Affector State Enums
    /// <summary>
    /// 엔테티의 행동양식, 버프, 디버프의 상태를 모아놓은것이다. State패턴과는 관련이 없다,
    /// </summary>
    public enum STATE_TYPE
    {
        //0 ~ 99 // 디버프
        MOVE = 0, DASH, ATTACK, SKILL, GET_DAMAGE, DIE, TRIGGER,
        
        //100 ~ 199 //디버프
        BURN = 100, POISONED, BLEED, CONTRACTED, FREEZE, CONFUSED, FEAR, Stun, BOUNDED, EXECUTION, KNOCKBACK, BLACK_HOLE,
        
        //200 ~ 299
        MOVE_SPEED_UP = 200, TENACITY_UP, POWER_UP, ATTACK_SPEED_UP, BARRIER, INVISIBLE, INVINCIBLE, ON_HIT, DEFENCE, PROJECTILE_GENERATOR
    }
#endregion

/////////////////////////////////////////////////////////////////////////////////

#region Carrier & VFXObject Enums
    public enum CARRIER_TYPE
    {
        PORTAL = 0, ROULETTE, ATTACK, NEUTRAL, ITEM
    }
    
    /// <summary>
    /// `INNER` 라면 버켓의 자식으로 생성됨을 나타냄 <br/>
    /// `OUTER` 라면 버켓의 transform.position 에서 생성됨을 나타냄 <br/>
    /// </summary>
    public enum BUCKET_POSITION
    {
        INNER, OUTER
    }

    /// <summary>
    /// `SKIN` 라면 SkinnedMeshRenderer 컴포넌트의 Material에 접근하는 모드 <br/>
    /// `MESH` 라면 MeshRenderor 컴포넌트의 Material에 접근하는 모드 <br/>
    /// </summary>
    public enum RENDERER_MODE 
    { 
        SKIN, MESH 
    } 
    /// <summary>
    /// `NONE_STACK` 리면 단 하나만 생성되고 끝날때 까지는 생성할 수 없음을 나타냄 <br/>
    /// `STACK`     리면 계속 생성가능 <br/>
    /// </summary>
    public enum BUCKET_STACKING_TYPE
    {
        NONE_STACK, STACK
    }

    public enum GEAR_TYPE
    {
        SILVER, GOLD
    }
    
    public enum EQUIPMENT_GENERATE_POOL_TYPE {
        DEFAULT, NORMAL, BOSS, SHOP, HIDDEN
    }
#endregion

/////////////////////////////////////////////////////////////////////////////////

#region Player Enums
    public enum WEAPON_TYPE
    {
        MELEE, RANGER, MAGE
    }
    public enum WEAPON_STATE
    {
        NORMAL,
        ON_HIT  // 
    }
    
    public enum SKILL_TYPE
    {
        NEUTRAL, ATTACK
    }
    [SerializeField]
    public enum SKILL_KEY
    {
        Q, E, R
    }
    
    [SerializeField]
    public enum SKILL_RANK
    {
        NORMAL, RARE, EPIC
    }
#endregion

/////////////////////////////////////////////////////////////////////////////////

#region Tag Enums
    public enum UNITY_TAGS
    {
        Mesh, Wall, Enemy, Portal, PlayerProjectile, EnemyProjectile, Equipment, DebugUI,
    }
#endregion

#region Enemy Enums
    public enum Enemy_TYPE
    {
        Enemy_Template, Raptor,
    }
#endregion

/////////////////////////////////////////////////////////////////////////////////