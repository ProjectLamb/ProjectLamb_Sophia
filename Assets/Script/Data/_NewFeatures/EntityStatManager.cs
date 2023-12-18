namespace Feature_NewData
{
    public class EntityStatManager : IStatAccessable {
        private readonly Stat MaxUp;
        private readonly Stat Defence;
        private readonly Stat Power;
        private readonly Stat AttackSpeed;
        private readonly Stat MoveSpeed;
        private readonly Stat Tenacity;
        
        public Stat GetMaxHP() { return MaxUp; }
        public Stat GetDefence() { return Defence; }
        public Stat GetPower() { return Power; }
        public Stat GetAttackSpeed() { return AttackSpeed; }
        public Stat GetMoveSpeed() { return MoveSpeed; }
        public Stat GetTenacity() { return Tenacity; }
    }
}