using System.Threading;

namespace Sophia.DataSystem
{
    using Atomics;
    using Sophia.Composite.NewTimer;
    using Sophia.Entitys;
    using UnityEngine;

    namespace Modifiers.ConcreteAffector
    {
        public class PowerUpAffect : Affector, IUserInterfaceAccessible {
            private EntityStatModifyAtomics EntityStatModifyAffector;
            private VisualFXAtomics VisualFXAffector;
            private MaterialChangeAtomics MaterialChange;

            public PowerUpAffect(in SerialAffectorData affectData) : base (affectData) {}

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.PowerUp;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;

                EntityStatModifyAffector    = new EntityStatModifyAtomics(in affectData._calculateData);
                VisualFXAffector            = new VisualFXAtomics(AffectType, in affectData._visualData);
                MaterialChange              = new MaterialChangeAtomics(in affectData._skinData);
                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }

            public override void Enter(Entity entity)
            {
                EntityStatModifyAffector.Invoke(entity);
                VisualFXAffector.Invoke(entity);
                MaterialChange.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                EntityStatModifyAffector.Revert(entity);
                VisualFXAffector.Revert(entity);
                MaterialChange.Revert(entity);
                base.Exit(entity);
            }

            public override void Run(Entity entity)
            {
                return;
            }
            #region User Interface
            public string GetName() => Name;
            public string GetDescription() => Description;
            public Sprite GetSprite() => Icon;
            #endregion
        }
        
        public class MoveFasterAffect : Affector, IUserInterfaceAccessible
        {
            private EntityStatModifyAtomics EntityStatModifyAffector;
            private VisualFXAtomics VisualFXAffector;

            public MoveFasterAffect(in SerialAffectorData affectData) : base(affectData)
            {
            }

            protected override void Init(in SerialAffectorData affectData)
            {
                AffectType = E_AFFECT_TYPE.MoveSpeedUp;
                Name = affectData._uiData._name;
                Description = affectData._uiData._description;
                Icon = affectData._uiData._icon;

                EntityStatModifyAffector = new EntityStatModifyAtomics(in affectData._calculateData);
                VisualFXAffector = new VisualFXAtomics(AffectType, affectData._visualData);
                Timer = new TimerComposite(affectData._baseDurateTime);
                CurrentState = AffectorReadyState.Instance;
            }

            public override void Enter(Entity entity)
            {
                EntityStatModifyAffector.Invoke(entity);
                VisualFXAffector.Invoke(entity);
            }

            public override void Exit(Entity entity)
            {
                EntityStatModifyAffector.Revert(entity);
                VisualFXAffector.Revert(entity);
                base.Exit(entity);
            }

            public override void Run(Entity entity)
            {
                return;
            }

            #region User Interface
            public string GetName() => Name;
            public string GetDescription() => Description;
            public Sprite GetSprite() => Icon;
            #endregion
        }
    }
}