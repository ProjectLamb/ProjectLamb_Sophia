using System;
using UnityEngine;
using DG.Tweening;
using System.Threading;

namespace Sophia.DataSystem
{
    namespace Modifiers
    {
        namespace NewConcreteAffector
        {
            using NewAffector;
            using Sophia.DataSystem.Functional;

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
                }

                public override void Invoke(Entitys.Entity entity) {
                    DamageFunction.Invoke(entity);
                    MaterialChangeFunction.Invoke(entity);
                    VisualFXFunction.Invoke(entity);
                }

                public override void Run(Entitys.Entity entity)
                {
                    DamageFunction.Run(entity);
                }

                public override void Revert(Entitys.Entity entity) {
                    MaterialChangeFunction.Revert(entity);
                    VisualFXFunction.Revert(entity);
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

                public override void Invoke(Entitys.Entity entity) {
                    EntityStatModifyFunction.Invoke(entity);
                    MaterialChangeFunction.Invoke(entity);
                    VisualFXFunction.Invoke(entity);
                }

                public override void Run(Entitys.Entity entity) { return; }

                public override void Revert(Entitys.Entity entity) {
                    EntityStatModifyFunction.Invoke(entity);
                    MaterialChangeFunction.Revert(entity);
                    VisualFXFunction.Revert(entity);
                }
            }
            
        }
    }
}