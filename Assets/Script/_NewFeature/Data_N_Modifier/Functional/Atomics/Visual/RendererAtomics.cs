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
        public Material material;
        public MaterialChangeAtomics(in SerialSkinData skinAffectData) {
            material = skinAffectData._materialRef;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async void Invoke(IVisualAccessible visualAccessible) {
            await visualAccessible.GetModelManager().ChangeSkin(cancellationTokenSource.Token, material);
        }

        public async void Revert(IVisualAccessible visualAccessible) {
            cancellationTokenSource.Cancel();
            await visualAccessible.GetModelManager().RevertSkin();
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