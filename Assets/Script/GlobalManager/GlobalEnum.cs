public enum E_AffectorType{
    //0 ~ 99 // 디버프
    Burn        = 0     ,Poisend ,Bleed ,Contracted ,Freeze ,Confused ,Fearing ,Stern ,Bounded, Execution, KnockBack,
    //100 ~ 199 //버프
    MoveSpeedUp = 100   , TenacityUp, PowerUp, AttackSpeedUp, Barrier, Invisible, Invinvible, CriticalAttack
}

//EntityState.cs 에 또 정의함

public enum Affector_PlayerState{
    Move, Dash, Attack, Skill, GetDamaged, Die, Trigger
}
