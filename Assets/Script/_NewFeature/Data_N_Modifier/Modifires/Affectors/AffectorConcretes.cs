using System.Threading;

namespace Sophia.DataSystem
{
    using System.Numerics;
    using Sophia.Composite.NewTimer;
    using Sophia.Entitys;
    using UnityEngine;

    namespace Modifiers.ConcreteAffector
    {
        public static class FactoryConcreteAffect {
            public static Affector GetAffectByID(in SerialAffectorData affectData, Entitys.Entity entity) {
                switch(affectData._affectType)
                {
                    case E_AFFECT_TYPE.Burn         :   {return new BurnAffect(in affectData);}
                    case E_AFFECT_TYPE.Poisoned     :   {return new PoisonedAffect(in affectData);}
                    case E_AFFECT_TYPE.Bleed        :   {return new BleedAffect(in affectData);}
                    case E_AFFECT_TYPE.Cold         :   {return new ColdAffect(in affectData);}
                    case E_AFFECT_TYPE.Stern        :   {return new SternAffect(in affectData);}
                    case E_AFFECT_TYPE.Bounded      :   {return new BoundedAffect(in affectData);}
                    case E_AFFECT_TYPE.Knockback    :   {return new KnockbackAffect(in affectData, entity.GetGameObject().transform);}
                    case E_AFFECT_TYPE.BlackHole    :   {return new BlackHoleAffect(in affectData, entity.GetGameObject().transform);}
                    case E_AFFECT_TYPE.Airborne    :    {return new AirborneAffect(in affectData);}
                    default : {return null;}
                }
            }
        }
        
        public class BurnAffect : Affector, IUserInterfaceAccessible
        {
            private DamageAtomics DamageAffector;

            public BurnAffect(in SerialAffectorData affectData) : base(in affectData)
            {
            }

            public override void Enter(Entity entity)
            {
                DamageAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                this.InvokeOnClearAffect(this);
            }

            public override void Run(Entity entity)
            {
                DamageAffector.Run(entity);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Burn;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                DamageAffector = new DamageAtomics(in affectData._tickDamageAffectData);
                Timer = new TimerComposite(affectData._baseDurateTime)
                            .SetInterval(DamageAffector.intervalTime);
                CurrentState = AffectorReadyState.Instance;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class PoisonedAffect : Affector, IUserInterfaceAccessible
        {
            public PoisonedAffect(in SerialAffectorData affectData) : base(in affectData)
            {
            }

            private DamageAtomics DamageAffector;
            private MaterialChangeAtomics MaterialChangeAffector;
            private VisualFXAtomics VisualFXAffector;

            public override void Enter(Entitys.Entity entity)
            {
                DamageAffector.Invoke(entity);
                MaterialChangeAffector.Invoke(entity);
                VisualFXAffector.Invoke(entity);
            }
            public override void Run(Entitys.Entity entity)
            {
                DamageAffector.Run(entity);
            }
            public override void Exit(Entitys.Entity entity)
            {
                MaterialChangeAffector.Revert(entity);
                VisualFXAffector.Revert(entity);
                this.InvokeOnClearAffect(this);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Poisoned;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                DamageAffector = new DamageAtomics(in affectData._tickDamageAffectData);
                MaterialChangeAffector = new MaterialChangeAtomics(affectData._skinAffectData);
                VisualFXAffector = new VisualFXAtomics(affectData._affectType, affectData._visualAffectData);

                Timer = new TimerComposite(affectData._baseDurateTime)
                            .SetInterval(DamageAffector.intervalTime);
                CurrentState = AffectorReadyState.Instance;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class BleedAffect : Affector, IUserInterfaceAccessible
        {
            private DamageAtomics DamageAffector;
            public BleedAffect(in SerialAffectorData affectData) : base(in affectData)
            {

            }
            public override void Enter(Entity entity)
            {
                DamageAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                this.InvokeOnClearAffect(this);
            }

            public override void Run(Entity entity)
            {
                DamageAffector.Run(entity);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Bleed;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                DamageAffector = new DamageAtomics(in affectData._tickDamageAffectData);
                Timer = new TimerComposite(affectData._baseDurateTime)
                            .SetInterval(DamageAffector.intervalTime);
                CurrentState = AffectorReadyState.Instance;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class ColdAffect : Affector, IUserInterfaceAccessible
        {
            private EntityStatModifyAtomics EntityStatModifyAffector;
            private MaterialChangeAtomics MaterialChangeAffector;
            private VisualFXAtomics VisualFXAffector;

            public ColdAffect(in SerialAffectorData affectData) : base(in affectData)
            {
            }

            public override void Enter(Entitys.Entity entity)
            {
                EntityStatModifyAffector.Invoke(entity);
                MaterialChangeAffector.Invoke(entity);
            }

            public override void Run(Entitys.Entity entity) { return; }

            public override void Exit(Entitys.Entity entity)
            {
                EntityStatModifyAffector.Revert(entity);
                MaterialChangeAffector.Revert(entity);
                this.InvokeOnClearAffect(this);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Cold;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                EntityStatModifyAffector = new EntityStatModifyAtomics(in affectData._calculateAffectData);
                MaterialChangeAffector = new MaterialChangeAtomics(affectData._skinAffectData);
                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class SternAffect : Affector, IUserInterfaceAccessible
        {
            private HoldAtomics HoldAffector;
            private MaterialChangeAtomics MaterialChangeAffector;
            private VisualFXAtomics VisualFXAffector;

            public SternAffect(in SerialAffectorData affectData) : base(in affectData)
            {

            }

            public override void Enter(Entity entity)
            {
                HoldAffector.Invoke(entity as IMovable);
                MaterialChangeAffector.Invoke(entity);
                VisualFXAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                HoldAffector.Revert(entity as IMovable);
                MaterialChangeAffector.Revert(entity);
                VisualFXAffector.Revert(entity);
                this.InvokeOnClearAffect(this);
            }

            public override void Run(Entity entity) { }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Stern;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                HoldAffector = new HoldAtomics();
                MaterialChangeAffector = new MaterialChangeAtomics(affectData._skinAffectData);
                VisualFXAffector = new VisualFXAtomics(AffectType, affectData._visualAffectData);

                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class BoundedAffect : Affector, IUserInterfaceAccessible
        {
            private HoldAtomics HoldAffector;

            public BoundedAffect(in SerialAffectorData affectData) : base(in affectData)
            {
                AffectType = E_AFFECT_TYPE.Bounded;
                HoldAffector = new HoldAtomics();

                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }

            public override void Enter(Entity entity)
            {
                HoldAffector.Invoke(entity as IMovable);
            }

            public override void Exit(Entity entity)
            {
                HoldAffector.Revert(entity as IMovable);
                this.InvokeOnClearAffect(this);
            }

            public override void Run(Entity entity)
            {
                throw new System.NotImplementedException();
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class KnockbackAffect : Affector, IUserInterfaceAccessible
        {
            private RigidImpulseAtomics RigidImpulseAffector;
            private HoldAtomics HoldAffector;

            public KnockbackAffect(in SerialAffectorData affectData, Transform ownerTransform) : base(in affectData)
            {
                RigidImpulseAffector = new RigidImpulseAtomics(ownerTransform, affectData._physicsAffectData._physicsForce);
            }

            public override void Enter(Entity entity)
            {
                RigidImpulseAffector.Invoke(entity);
                HoldAffector.Invoke(entity as IMovable);
            }

            public override void Exit(Entity entity)
            {
                HoldAffector.Revert(entity as IMovable);
                this.InvokeOnClearAffect(this);
            }

            public override void Run(Entity entity)
            {
                throw new System.NotImplementedException();
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Knockback;
                Name = affectData._equipmentName;
                HoldAffector = new HoldAtomics();

                Timer = new TimerComposite(0.5f);
                CurrentState = AffectorReadyState.Instance;
                Description = affectData._description;
                Icon = affectData._icon;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class BlackHoleAffect : Affector, IUserInterfaceAccessible
        {
            private RigidGradualAtomics RigidGradualAffector;
            private HoldAtomics HoldAffector;
            public BlackHoleAffect(in SerialAffectorData affectData, Transform ownerTransform) : base(in affectData)
            {
                RigidGradualAffector = new RigidGradualAtomics(
                    ownerTransform,
                    affectData._physicsAffectData._physicsForce,
                    affectData._physicsAffectData._intervalTime
                );
            }

            public override void Enter(Entity entity)
            {
                RigidGradualAffector.Invoke(entity);
                HoldAffector.Invoke(entity as IMovable);
            }


            public override void Run(Entity entity)
            {
                RigidGradualAffector.Run(entity);
            }
            public override void Exit(Entity entity)
            {
                RigidGradualAffector.Revert(entity);
                HoldAffector.Revert(entity as IMovable);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.BlackHole;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;

                HoldAffector = new HoldAtomics();

                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }

        public class AirborneAffect : Affector, IUserInterfaceAccessible
        {
            private TweenJumpTransformAtomics TweenJumpAffector;
            private HoldAtomics HoldAffector;
            public AirborneAffect(in SerialAffectorData affectData) : base(in affectData)
            {
            }

            public override void Enter(Entity entity)
            {
                TweenJumpAffector.Invoke(entity);
                HoldAffector.Invoke(entity as IMovable);
            }

            public override void Run(Entity entity) { return; }
            public override void Exit(Entity entity)
            {
                HoldAffector.Revert(entity as IMovable);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.BlackHole;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                TweenJumpAffector = new TweenJumpTransformAtomics(
                    affectData._physicsAffectData._physicsForce,
                    affectData._baseDurateTime
                );
                HoldAffector = new HoldAtomics();

                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }
#region User Interface
            public string GetDescription()
            {
                throw new System.NotImplementedException();
            }

            public string GetName()
            {
                throw new System.NotImplementedException();
            }

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
#endregion
        }
    }
}