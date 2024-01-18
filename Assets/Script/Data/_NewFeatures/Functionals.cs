using UnityEngine;

namespace Feature_NewData
{
    public enum E_FUNCTIONAL_ACTION_MEMBERS
    {
        Move = 0, Damaged, Attack, Affected, Dead, Standing, PhyiscTriggered, Skill,

        Created = 10, Triggerd, Released, Forwarding,
        WeaponUse = 20, ProjectileRestore,
        SkillUse = 30, SkillRefilled
    }

    // Move UnityAction
    // Damaged UnityActionRef(ref T)
    // Attack UnityAction
    // Affected ?
    // Dead UnityAction
    // Standing UnityAcion
    // PhyiscTriggered UnityActionRef(ref T)
    // Skill UnityAction
    // Triggerd : UnityActionRef(ref T)

    /*
    평행 State머신을 사용해야하나?
    */

    public interface IFunctionalAccessable
    {

    }

    public abstract class Functionals : IFunctionalAccessable
    {
        protected readonly ExtraAction<bool>        moveExtra               = new();
        protected readonly ExtraAction<int>         damagedExtra            = new();
        protected readonly ExtraAction<bool>        attackExtra             = new();
        protected readonly ExtraAction<bool>        deadExtra               = new();
        protected readonly ExtraAction<bool>        standingExtra           = new();
        protected readonly ExtraAction<Collision>   phyiscTriggeredExtra    = new();
        protected readonly ExtraAction<bool>        skillExtra              = new();

        protected readonly ExtraAction<Entity>      createdExtra            = new();
        protected readonly ExtraAction<Collider>    triggerdExtra           = new();
        protected readonly ExtraAction<bool>        releasedExtra           = new();
        protected readonly ExtraAction<bool>        forwardingExtra         = new();

        protected readonly ExtraAction<int>         weaponUseExtra          = new();
        protected readonly ExtraAction<bool>        projectileRestoreExtra  = new();

        protected readonly ExtraAction<int>         skillUseExtra           = new();
        protected readonly ExtraAction<bool>        skillRefilledExtra      = new();
    }
}