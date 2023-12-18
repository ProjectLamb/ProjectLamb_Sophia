namespace Feature_NewData
{
    public class PlayerStatManager : IPlayerStatAccessable{
        private readonly Stat MaxUp;
        private readonly Stat Defence;
        private readonly Stat Power;
        private readonly Stat AttackSpeed;
        private readonly Stat MoveSpeed;
        private readonly Stat Tenacity;
        // private readonly Stat MaxStamina;
        // private readonly Stat StaminaRestoreSpeed;
        private readonly Stat Luck;
        
        public Stat GetMaxHP() { return MaxUp; }
        public Stat GetDefence() { return Defence; }
        public Stat GetPower() { return Power; }
        public Stat GetAttackSpeed() { return AttackSpeed; }
        public Stat GetMoveSpeed() { return MoveSpeed; }
        public Stat GetTenacity() { return Tenacity; }
        public Stat GetLuck() {return Luck;}

        public Stat GetMaxStamina()
        {
            throw new System.NotImplementedException();
        }

        public Stat GetStaminaRestoreSpeed()
        {
            throw new System.NotImplementedException();
        }
    }
}