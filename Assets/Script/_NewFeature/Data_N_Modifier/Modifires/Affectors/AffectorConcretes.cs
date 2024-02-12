using System.Threading;

namespace Sophia.DataSystem
{
    using System.Numerics;
    using Sophia.Composite.NewTimer;
    using Sophia.Entitys;
    using UnityEngine;

    namespace Modifiers.ConcreteAffector
    {
        public class BurnAffect : Affector
        {
            public DamageAtomics DamageAffector { get; private set; }
            public BurnAffect(SerialAffectorData affectData) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Burn;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                DamageAffector = new DamageAtomics(affectData._tickDamageAffectData);
                Timer = new TimerComposite(affectData._baseDurateTime)
                            .SetInterval(DamageAffector.intervalTime);
                CurrentState = AffectorReadyState.Instance;
            }
        }

        public class PoisonedAffect : Affector
        {
            public PoisonedAffect(SerialAffectorData affectData) : base(affectData)
            {
            }

            public DamageAtomics DamageAffector { get; private set; }
            public MaterialChangeAtomics MaterialChangeAffector { get; private set; }
            public VisualFXAtomics VisualFXAffector { get; private set; }

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

            protected override void Init(SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Poisoned;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                DamageAffector = new DamageAtomics(affectData._tickDamageAffectData);
                MaterialChangeAffector = new MaterialChangeAtomics(affectData._skinAffectData);
                VisualFXAffector = new VisualFXAtomics(affectData._affectType, affectData._visualAffectData);

                Timer = new TimerComposite(affectData._baseDurateTime)
                            .SetInterval(DamageAffector.intervalTime);
                CurrentState = AffectorReadyState.Instance;
            }
        }

        public class BleedAffect : Affector
        {
            public DamageAtomics DamageAffector { get; private set; }
            public BleedAffect(SerialAffectorData affectData) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Bleed;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                DamageAffector = new DamageAtomics(affectData._tickDamageAffectData);
                Timer = new TimerComposite(affectData._baseDurateTime)
                            .SetInterval(DamageAffector.intervalTime);
                CurrentState = AffectorReadyState.Instance;
            }
        }

        public class ColdAffect : Affector
        {
            public EntityStatModifyAtomics EntityStatModifyAffector { get; private set; }
            public MaterialChangeAtomics MaterialChangeAffector { get; private set; }
            public VisualFXAtomics VisualFXAffector { get; private set; }

            public ColdAffect(SerialAffectorData affectData) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Cold;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
                EntityStatModifyAffector = new EntityStatModifyAtomics(affectData._calculateAffectData);
                MaterialChangeAffector = new MaterialChangeAtomics(affectData._skinAffectData);
                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }
        }

        public class SternAffect : Affector
        {
            public HoldAtomics HoldAffector { get; private set; }
            public MaterialChangeAtomics MaterialChangeAffector { get; private set; }
            public VisualFXAtomics VisualFXAffector { get; private set; }

            public SternAffect(SerialAffectorData affectData) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
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
        }

        public class BoundedAffect : Affector
        {
            public HoldAtomics HoldAffector { get; private set; }

            public BoundedAffect(SerialAffectorData affectData) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
            {
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;
            }
        }

        public class KnockbackAffect : Affector
        {
            public RigidImpulseAtomics RigidImpulseAffector { get; private set; }
            public HoldAtomics HoldAffector { get; private set; }

            public KnockbackAffect(SerialAffectorData affectData, Transform ownerTransform) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Knockback;
                Name = affectData._equipmentName;
                HoldAffector = new HoldAtomics();

                Timer = new TimerComposite(0.5f);
                CurrentState = AffectorReadyState.Instance;
                Description = affectData._description;
                Icon = affectData._icon;
            }
        }

        public class BlackHoleAffect : Affector
        {
            public RigidGradualAtomics RigidGradualAffector { get; private set; }
            public HoldAtomics HoldAffector { get; private set; }
            public BlackHoleAffect(SerialAffectorData affectData,Transform ownerTransform) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.BlackHole;
                Name = affectData._equipmentName;
                Description = affectData._description;
                Icon = affectData._icon;

                HoldAffector = new HoldAtomics();

                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }
        }

        public class AirborneAffect : Affector
        {
            public TweenJumpTransformAtomics TweenJumpAffector { get; private set; }
            public HoldAtomics HoldAffector { get; private set; }
            public AirborneAffect(SerialAffectorData affectData) : base(affectData)
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

            protected override void Init(SerialAffectorData affectData)
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
        }
    }
}