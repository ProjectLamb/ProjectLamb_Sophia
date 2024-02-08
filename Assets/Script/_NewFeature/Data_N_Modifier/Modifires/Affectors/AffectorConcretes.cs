using System.Threading;

namespace Sophia.DataSystem
{
    using Sophia.Composite.NewTimer;
    namespace Modifiers
    {
        namespace NewConcreteAffector
        {

            public class PoisonedAffect : Affector
            {
                public DamageAtomics DamageFunction;
                public MaterialChangeAtomics MaterialChangeFunction;
                public VisualFXAtomics VisualFXFunction;
                public PoisonedAffect(SerialAffectorData affectData) : base(affectData)
                {
                    DamageFunction          = new DamageAtomics(affectData._tickDamageAffectData);
                    MaterialChangeFunction  = new MaterialChangeAtomics(affectData._skinAffectData);
                    VisualFXFunction        = new VisualFXAtomics(affectData._affectType, affectData._visualAffectData);

                    Timer = new TimerComposite(affectData._baseDurateTime)
                                .SetInterval(DamageFunction.intervalTime);
                }

                public override void Invoke(Entitys.Entity entity) {
                    DamageFunction.Invoke(entity);
                    MaterialChangeFunction.Invoke(entity);
                    VisualFXFunction.Invoke(entity);
                }
                public override void Run(Entitys.Entity entity) {
                    DamageFunction.Run(entity);
                }
                public override void Revert(Entitys.Entity entity) {
                    MaterialChangeFunction.Revert(entity);
                    VisualFXFunction.Revert(entity);
                    this.InvokeOnRevertAffect();
                }
            }
            
            public class ColdAffect : Affector
            {
                public readonly EntityStatModifyAtomics     EntityStatModifyFunction;
                public readonly MaterialChangeAtomics       MaterialChangeFunction;
                public readonly VisualFXAtomics             VisualFXFunction;
                
                public ColdAffect(SerialAffectorData affectData) : base(affectData)
                {
                    EntityStatModifyFunction    = new EntityStatModifyAtomics(affectData._calculateAffectData);
                    MaterialChangeFunction      = new MaterialChangeAtomics(affectData._skinAffectData);
                    VisualFXFunction            = new VisualFXAtomics(affectData._affectType, affectData._visualAffectData);

                }

                public override void Invoke(Entitys.Entity entity){
                    EntityStatModifyFunction.Invoke(entity);
                    MaterialChangeFunction.Invoke(entity);
                    VisualFXFunction.Invoke(entity);
                }

                public override void Run(Entitys.Entity entity) { return; }

                public override void Revert(Entitys.Entity entity) {
                    EntityStatModifyFunction.Revert(entity);
                    MaterialChangeFunction.Revert(entity);
                    VisualFXFunction.Revert(entity);
                }
            }
            
        }
    }
}