using UnityEngine;
/// <summary>
/// 죽는것에 대한 Action
/// </summary>
public interface IDieAble
{
    public void Die();
}

/// <summary>
/// 상호작용에 대한 인터페이스
/// </summary>
public interface IInteractable {
    public void Interact();
}

/// <summary>
/// 맞았을떄 대한 인터페이스
/// </summary>
public interface IDamagable
{
   public void GetDamaged(int _amount);
   public void GetDamaged(int _amount, GameObject obj);
}

public interface IVisuallyInteractable {
    public void Interact(DebuffData debuffData);
    public void Interact(ParticleSystem particleSystem);
    public void Revert();
}

public interface ColliderHandeler{
    public void HandleCollider(){}
}

public interface IEntityAddressable : IDamagable, IDieAble, IAffectable {
    public EntityData GetEntityData();
}
