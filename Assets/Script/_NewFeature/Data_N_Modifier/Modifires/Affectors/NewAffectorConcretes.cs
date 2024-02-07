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
                    
                }
            }
            /*
            public class PoisonedAffect : Affector
            {
                #region Members
                //          public readonly E_AFFECT_TYPE AffectType;
                //          public readonly Entity TargetRef;
                //          public readonly Entity ownerEntity;
                //          public float BaseDurateTime              {get; private set;}
                //          public TimerComposite Timer              {get; private set;}

                private TickDamageCommand FunctionalTickDamageCommand;
                private SkinCommand FunctionalSkinCommand;
                private VisualFXCommand FunctionalVisualFXCommand;

                public PoisonedAffect(Entity ownerReceivers, Entity targetReceivers, float durateTime)
                : base(E_AFFECT_TYPE.Poisoned, ownerReceivers, targetReceivers, durateTime)
                {
                    targetReceivers.GetLifeComposite().OnExitDie += () => cts.Cancel();
                    Timer.OnFinished += Revert;
                }

                #endregion

                #region Setter

                public PoisonedAffect SetMaterial(SerialSkinAffectData serialSkinAffectData)
                {
                    if (FunctionalSkinCommand == null)
                    {

                        FunctionalSkinCommand = new SkinCommand(serialSkinAffectData, this.cts);
                        void MeshOn() => FunctionalSkinCommand.Invoke(ref TargetRef);
                        void MeshOff() => FunctionalSkinCommand.Revert(ref TargetRef);

                        Timer.OnStart += MeshOn;
                        Timer.OnFinished += MeshOff;
                    }
                    return this;
                }

                public PoisonedAffect SetVisualFXObject(SerialVisualAffectData serialVisualAffectData)
                {
                    if (FunctionalVisualFXCommand == null)
                    {

                        FunctionalVisualFXCommand = new VisualFXCommand(E_AFFECT_TYPE.Poisoned, serialVisualAffectData);
                        void VFXOn() => FunctionalVisualFXCommand.Invoke(ref TargetRef);
                        void VFXOff() => FunctionalVisualFXCommand.Revert(ref TargetRef);

                        Timer.OnStart += VFXOn;
                        Timer.OnFinished += VFXOff;
                    }
                    return this;
                }

                public PoisonedAffect SetTickDamage(SerialTickDamageAffectData serialTickDamageAffectData)
                {
                    if (FunctionalTickDamageCommand == null)
                    {

                        FunctionalTickDamageCommand = new TickDamageCommand(serialTickDamageAffectData);
                        void TickDamage() => FunctionalTickDamageCommand.Invoke(ref TargetRef);

                        Timer.SetIntervalTime(CalcDurateTime(serialTickDamageAffectData._intervalTime));
                        Timer.OnInterval += TickDamage;
                    }
                    return this;
                }

                public PoisonedAffect SetRewindCondition(Func<bool> func)
                {
                    Timer.SetRewindCondition(func);
                    return this;
                }

                #endregion
            }
            */
        }
    }
}