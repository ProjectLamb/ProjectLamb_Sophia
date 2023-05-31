<<<<<<< HEAD
public enum E_DebuffAtomic {
    Dot = 0, Slow, Uncontrollable
}

public enum E_DebuffState{
    Burn = 0 ,Poisend ,Bleed ,Contracted ,Freeze ,Confused ,Fearing ,Stern ,Bounded, Execution
}

public enum E_AffectorState {
    //Debuff는  100  부터 시작
    Burn = 0 ,Poisend ,Bleed ,Contracted ,Freeze ,Confused ,Fearing ,Stern ,Bounded, Execution, BlackHole
    
    //Buff  는    200  부터 시작
}
=======
public enum E_StateType{
    //0 ~ 99 // 디버프
    Move        = 0, Dash, Attack, Skill, GetDamaged, Die, Trigger,
    //100 ~ 199 //디버프
    Burn        = 100     ,Poisend ,Bleed ,Contracted ,Freeze ,Confused ,Fearing ,Stern ,Bounded, Execution, KnockBack,
    //200 ~ 299
    MoveSpeedUp = 200   , TenacityUp, PowerUp, AttackSpeedUp, Barrier, Invisible, Invinvible, CriticalAttack
}
>>>>>>> TA_Escatrgot_AffectorManager
