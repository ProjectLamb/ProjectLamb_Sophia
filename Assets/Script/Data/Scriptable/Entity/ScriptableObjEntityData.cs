using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EntityData", menuName = "ScriptableObject/Entity", order = int.MaxValue)]
public class ScriptableObjEntityData : ScriptableObject {
    public string EntityTag;
    public int MaxHP;
    public int Power;
    public float MoveSpeed;
    public float Defence;
    public float Tenacity;
    public float AttackSpeed;
    public UnityAction MoveState = () => { };
    public UnityAction AttackState = () => { };
    public UnityActionRef<float> AttackStateRef = (ref float i) => { };
    public UnityAction HitState = () => { };
    public UnityActionRef<float> HitStateRef = (ref float i) => { };
    public UnityAction<Entity, Entity> ProjectileShootState = (owner, target) => { };
    public UnityAction PhysicTriggerState = () => { };
    public UnityAction DieState = () => { };
    public UnityAction UIAffectState = () => { };
}