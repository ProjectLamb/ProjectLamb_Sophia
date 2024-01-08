using System.Collections.Generic;
using UnityEngine;
using UnityEditor.ShaderGraph;
using UnityEngine.Events;

namespace Sophia.DataSystem.Functional
{
    using Sophia.Entitys;

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