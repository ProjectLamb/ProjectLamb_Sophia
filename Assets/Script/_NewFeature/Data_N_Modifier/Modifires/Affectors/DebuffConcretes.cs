using System.Threading;
using System;
using Cysharp.Threading.Tasks;
using System.Numerics;

namespace Sophia.DataSystem
{

    using Sophia.Composite.NewTimer;
    using Sophia.Entitys;
    using UnityEngine;
    using UnityEngine.InputSystem.Utilities;

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
                    case E_AFFECT_TYPE.Stun         :   {return new StunAffect(in affectData);}
                    case E_AFFECT_TYPE.Bounded      :   {return new BoundedAffect(in affectData);}
                    case E_AFFECT_TYPE.Knockback    :   {return new KnockbackAffect(in affectData, entity.GetGameObject().transform);}
                    case E_AFFECT_TYPE.BlackHole    :   {return new BlackHoleAffect(in affectData, entity.GetGameObject().transform);}
                    case E_AFFECT_TYPE.Airborne     :   {return new AirborneAffect(in affectData);}
                    case E_AFFECT_TYPE.Execution    :   {return new BurnAffect(in affectData);}
                    default : {return null;}
                }
            }
        }

        public class ExecutionStrike : Affector, IUserInterfaceAccessible
        {
            private Atomics.AudioAtomics            AudioAffector;
            private Atomics.GetHitAtomics           DamageAffector;
            private Atomics.MaterialChangeAtomics   MaterialChangeAffector;
            private Atomics.VisualFXAtomics         VisualFXAffector;

            public ExecutionStrike(in SerialAffectorData affectData) : base(in affectData)
            {
            }

            public override void Enter(Entity entity)
            {
                AudioAffector.Invoke(entity);
                DamageAffector.Invoke(entity);
                MaterialChangeAffector.material.SetFloat("_Activate", 1);
                MaterialChangeAffector.Invoke(entity);
                VisualFXAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                MaterialChangeAffector.Revert(entity);
                VisualFXAffector.Revert(entity);
                AudioAffector.Revert(entity);
                base.Exit(entity);
            }
            
            public override void Run(Entity entity)
            {
                float ActivateNum = 0;
                if(1 - Timer.GetProgressAmount()*4 > 0) { ActivateNum = 1 - Timer.GetProgressAmount(); }
                Debug.Log("ActivateNum");
                MaterialChangeAffector.material.SetFloat("_Activate", ActivateNum);
                return;
            }
            protected override void Init(in SerialAffectorData affectData)
            {
                AudioAffector = new Atomics.AudioAtomics(in affectData._audioData);

                AffectType = E_AFFECT_TYPE.Execution;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
                DamageAffector          = new Atomics.GetHitAtomics(in affectData._tickDamageData);
                MaterialChangeAffector  = new Atomics.MaterialChangeAtomics(in affectData._skinData);
                VisualFXAffector        = new Atomics.VisualFXAtomics(E_AFFECT_TYPE.None, in affectData._visualData);
                
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

        public class BurnAffect : Affector, IUserInterfaceAccessible
        {
            private Atomics.GetHitAtomics DamageAffector;

            public BurnAffect(in SerialAffectorData affectData) : base(in affectData)
            {
            }

            public override void Enter(Entity entity)
            {
                DamageAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                base.Exit(entity);
            }

            public override void Run(Entity entity)
            {
                DamageAffector.Run(entity);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Burn;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
                DamageAffector = new Atomics.GetHitAtomics(in affectData._tickDamageData);
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
            private Atomics.AudioAtomics            AudioAffector;
            public PoisonedAffect(in SerialAffectorData affectData) : base(in affectData)
            {
            }

            private Atomics.GetHitAtomics DamageAffector;
            private Atomics.MaterialChangeAtomics MaterialChangeAffector;
            private Atomics.VisualFXAtomics VisualFXAffector;

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
                AudioAffector.Revert(entity);
                base.Exit(entity);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AudioAffector = new Atomics.AudioAtomics(in affectData._audioData);
                AffectType = E_AFFECT_TYPE.Poisoned;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
                DamageAffector = new Atomics.GetHitAtomics(in affectData._tickDamageData);
                MaterialChangeAffector = new Atomics.MaterialChangeAtomics(affectData._skinData);
                VisualFXAffector = new Atomics.VisualFXAtomics(affectData._affectType, affectData._visualData);

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
            private Atomics.GetHitAtomics DamageAffector;
            public BleedAffect(in SerialAffectorData affectData) : base(in affectData)
            {

            }
            public override void Enter(Entity entity)
            {
                DamageAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                base.Exit(entity);
            }

            public override void Run(Entity entity)
            {
                DamageAffector.Run(entity);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Bleed;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
                DamageAffector = new Atomics.GetHitAtomics(in affectData._tickDamageData);
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
            private Atomics.AudioAtomics            AudioAffector;
            private Atomics.EntityStatModifyAtomics EntityStatModifyAffector;
            private Atomics.MaterialChangeAtomics MaterialChangeAffector;
            private Atomics.VisualFXAtomics VisualFXAffector;

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
                AudioAffector.Revert(entity);
                base.Exit(entity);
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AudioAffector = new Atomics.AudioAtomics(in affectData._audioData);
                AffectType = E_AFFECT_TYPE.Cold;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
                EntityStatModifyAffector = new Atomics.EntityStatModifyAtomics(in affectData._calculateData);
                MaterialChangeAffector = new Atomics.MaterialChangeAtomics(affectData._skinData);
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

        public class StunAffect : Affector, IUserInterfaceAccessible
        {
            private Atomics.AudioAtomics            AudioAffector;
            private Atomics.HoldAtomics HoldAffector;
            private Atomics.MaterialChangeAtomics MaterialChangeAffector;
            private Atomics.VisualFXAtomics VisualFXAffector;

            public StunAffect(in SerialAffectorData affectData) : base(in affectData)
            {

            }

            public override void Enter(Entity entity)
            {
                AudioAffector.Invoke(entity);
                HoldAffector.Invoke(entity as IMovable);
                MaterialChangeAffector.Invoke(entity);
                VisualFXAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                HoldAffector.Revert(entity as IMovable);
                MaterialChangeAffector.Revert(entity);
                VisualFXAffector.Revert(entity);
                AudioAffector.Revert(entity);
                base.Exit(entity);
            }

            public override void Run(Entity entity) { }

            protected override void Init(in SerialAffectorData affectData)
            {
                AudioAffector = new Atomics.AudioAtomics(in affectData._audioData);
                AffectType = E_AFFECT_TYPE.Stun;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
                HoldAffector = new Atomics.HoldAtomics();
                MaterialChangeAffector = new Atomics.MaterialChangeAtomics(affectData._skinData);
                VisualFXAffector = new Atomics.VisualFXAtomics(AffectType, affectData._visualData);

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
            private Atomics.HoldAtomics HoldAffector;

            public BoundedAffect(in SerialAffectorData affectData) : base(in affectData)
            {
                AffectType = E_AFFECT_TYPE.Bounded;
                HoldAffector = new Atomics.HoldAtomics();

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
                base.Exit(entity);
            }

            public override void Run(Entity entity)
            {
                throw new System.NotImplementedException();
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
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
            private Atomics.RigidImpulseAtomics RigidImpulseAffector;
            private Atomics.HoldAtomics HoldAffector;

            public KnockbackAffect(in SerialAffectorData affectData, Transform ownerTransform) : base(in affectData)
            {
                RigidImpulseAffector = new Atomics.RigidImpulseAtomics(ownerTransform, affectData._physicsData._physicsForce);
            }

            public override void Enter(Entity entity)
            {
                RigidImpulseAffector.Invoke(entity);
                HoldAffector.Invoke(entity as IMovable);
            }

            public override void Exit(Entity entity)
            {
                HoldAffector.Revert(entity as IMovable);
                base.Exit(entity);
            }

            public override void Run(Entity entity)
            {
                throw new System.NotImplementedException();
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.Knockback;
                Name = affectData._uiData._name;
                HoldAffector = new Atomics.HoldAtomics();

                Timer = new TimerComposite(0.5f);
                CurrentState = AffectorReadyState.Instance;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
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
            private Atomics.AudioAtomics            AudioAffector;
            private Atomics.RigidGradualAtomics RigidGradualAffector;
            private Atomics.HoldAtomics HoldAffector;
            public BlackHoleAffect(in SerialAffectorData affectData, Transform ownerTransform) : base(in affectData)
            {
                RigidGradualAffector = new Atomics.RigidGradualAtomics(
                    ownerTransform,
                    affectData._physicsData._physicsForce,
                    affectData._physicsData._intervalTime
                );
            }

            public override void Enter(Entity entity)
            {
                AudioAffector.Invoke(entity);
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
                AudioAffector = new Atomics.AudioAtomics(in affectData._audioData);
                AffectType = E_AFFECT_TYPE.BlackHole;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;

                HoldAffector = new Atomics.HoldAtomics();

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
            private Atomics.TweenJumpTransformAtomics TweenJumpAffector;
            private Atomics.HoldAtomics HoldAffector;
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
                AffectType = E_AFFECT_TYPE.Airborne;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;
                TweenJumpAffector = new Atomics.TweenJumpTransformAtomics(
                    affectData._physicsData._physicsForce,
                    affectData._baseDurateTime
                );
                HoldAffector = new Atomics.HoldAtomics();

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