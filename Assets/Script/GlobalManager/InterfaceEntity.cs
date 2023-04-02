public interface IDieAble
{
    public void Die();
}

public interface IInteractable {
    public void Interact();
}

public interface IDamagable
{
   public void GetDamaged(int _amount);
}