using UnityEngine;
using System;
using System.Collections.Generic;
using FMODPlus;

namespace Sophia.Instantiates {
    using Sophia.Entitys;
    using Sophia.State;
    using Sophia.Composite.RenderModels;
    using Sophia.Instantiates.Skills;
    using Sophia.Composite;

    public abstract class Skill : IUserInterfaceAccessible, IUpdatorBindable
    {
        public abstract void    AddToUpdator();
        public abstract void    FrameTick();
        public abstract string  GetDescription();
        public abstract string  GetName();
        public abstract Sprite  GetSprite();
        public abstract bool    GetUpdatorBind();
        public abstract void    LateTick();
        public abstract void    PhysicsTick();
        public abstract void    RemoveFromUpdator();
        public abstract void    Use();
    }
    public class EmptySkill : IUserInterfaceAccessible
    {
        private static EmptySkill _instance = new EmptySkill();
        public static EmptySkill Instance => _instance;
        public string GetDescription() => "현재 스킬이 비어있습니다.";
        public string GetName() => "비어있음";
        public Sprite GetSprite() => null;

    }
}