using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Sophia.DataSystem.Atomics
{
    using Sophia.Composite.RenderModels;
    using Sophia.Entitys;
    using Sophia.Instantiates;

    public class MaterialChangeAtomics {
        public CancellationTokenSource cancellationTokenSource;
        [Obsolete] public Material material;
        public E_MATERIAL_TYPE materialType;
        public E_FUNCTIONAL_EXTRAS_TYPE entityFunctionalActType;
        public E_AFFECT_TYPE affectType;

        public MaterialChangeAtomics(in SerialSkinData skinAffectData) {
            // material = skinAffectData._materialRef;
            materialType = skinAffectData._materialType;
            entityFunctionalActType = skinAffectData._entityFunctionActType;
            affectType = skinAffectData._affectType;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async void Invoke(IVisualAccessible visualAccessible) {
            // await visualAccessible.GetModelManager().ChangeSkin(cancellationTokenSource.Token, material);
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    await visualAccessible.GetModelManager().InvokeChangeMaterial(cancellationTokenSource.Token, entityFunctionalActType); break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    await visualAccessible.GetModelManager().InvokeChangeMaterial(cancellationTokenSource.Token, affectType); break;
                }
            }
        }

        public async void Revert(IVisualAccessible visualAccessible) {
            cancellationTokenSource.Cancel();
            // await visualAccessible.GetModelManager().RevertSkin();
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    await visualAccessible.GetModelManager().RevertChangeMaterial(entityFunctionalActType); break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    await visualAccessible.GetModelManager().RevertChangeMaterial(affectType); break;
                }
            }
        }
    }
    public class ProjectileVisualChangeAtomics {
        public ProjectileVisualData projectileVisualData;
        public ProjectileVisualChangeAtomics(in SerialProjectileVisualDatas serialProjectileVisualDatas) {
            projectileVisualData.ShaderUnderbarColor        = serialProjectileVisualDatas._shaderUnderbarColor;
            projectileVisualData.ShaderUnderbarColorPower   = serialProjectileVisualDatas._shaderUnderbarColorPower;
            projectileVisualData.DestroyEffect              = serialProjectileVisualDatas._destroyEffect;
            projectileVisualData.HitEffect                  = serialProjectileVisualDatas._hitEffect;
        }

        public void Invoke(ProjectileObject projectile) {
            projectile.SetProjectileVisual(projectileVisualData);
        }
    }
}