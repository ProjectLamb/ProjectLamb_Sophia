namespace Feature_NewData
{
    public abstract class Entity : IDamagable, IDieable{
        public readonly Stat MaxHp;
        public float CurrentHelth;

        public abstract void GetDamaged(int _amount);
        public abstract void GetDamaged(int _amount, VFXObject _obj);

        public abstract void Die();
    }    
}