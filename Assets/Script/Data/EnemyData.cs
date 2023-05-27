
[System.Serializable]
public class EnemyData : EntityData {
    public EnemyData() : base(){}

    public EnemyData Clone(){
        EnemyData res = new EnemyData();
        res.EntityTag = this.EntityTag;
        res.MaxHP = this.MaxHP;
        res.CurHP = this.CurHP; 
        res.MoveSpeed = this.MoveSpeed;
        res.Defence = this.Defence;
        res.Tenacity = this.Tenacity;
        res.Power = this.Power;
        res.AttackSpeed = this.AttackSpeed;
        res.MoveState       = this.MoveState;
        res.AttackState     = this.AttackState;
        res.AttackStateRef  = this.AttackStateRef;
        res.HitState        = this.HitState;
        res.HitStateRef     = this.HitStateRef;
        res.ProjectileShootState = this.ProjectileShootState;
        res.PhyiscTriggerState = this.PhyiscTriggerState;
        res.DieState        = this.DieState;
        res.UIAffectState   = this.UIAffectState;
        return res;
    }
    public static EnemyData operator +(EnemyData x, MasterData y){
        EnemyData res = new EnemyData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;        
        res.Power = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;
        
        res.MoveState       = x.MoveState + y.MoveState;
        res.AttackState     = x.AttackState + y.AttackState;
        res.AttackStateRef  = x.AttackStateRef + y.AttackStateRef;
        res.HitState        = x.HitState + y.HitState;
        res.HitStateRef     = x.HitStateRef + y.HitStateRef;
        res.ProjectileShootState = x.ProjectileShootState + y.ProjectileShootState;
        res.PhyiscTriggerState = x.PhyiscTriggerState + y.PhyiscTriggerState;
        res.DieState        = x.DieState + y.DieState;
        res.UIAffectState   = x.UIAffectState + y.UIAffectState;
        return res;
    }
    public static EnemyData operator -(EnemyData x, MasterData y){
        EnemyData res = new EnemyData();
        res.MaxHP = x.MaxHP -y.MaxHP;
        res.CurHP = x.CurHP -y.CurHP;
        res.MoveSpeed = x.MoveSpeed -y.MoveSpeed;
        res.Defence = x.Defence -y.Defence;
        res.Tenacity = x.Tenacity -y.Tenacity;        
        res.Power = x.Power - y.Power;
        res.AttackSpeed = x.AttackSpeed - y.AttackSpeed;

        res.MoveState       = x.MoveState - y.MoveState;
        res.AttackState     = x.AttackState - y.AttackState;
        res.AttackStateRef  = x.AttackStateRef - y.AttackStateRef;
        res.HitState        = x.HitState - y.HitState;
        res.HitStateRef     = x.HitStateRef - y.HitStateRef;
        res.ProjectileShootState = x.ProjectileShootState - y.ProjectileShootState;
        res.PhyiscTriggerState = x.PhyiscTriggerState - y.PhyiscTriggerState;
        res.DieState        = x.DieState - y.DieState;
        res.UIAffectState   = x.UIAffectState - y.UIAffectState;
        return res;
    }
}

//** Base를 저장들을 하자.