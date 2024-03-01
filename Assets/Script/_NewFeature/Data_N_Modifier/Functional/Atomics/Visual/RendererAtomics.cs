using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Sophia.DataSystem.Atomics
{
    using Sophia.Composite.RenderModels;
    using Sophia.Entitys;

    public class MaterialChangeAtomics {
        public CancellationTokenSource cancellationTokenSource;
        public Material material;
        public MaterialChangeAtomics(in SerialSkinData skinAffectData) {
            material = skinAffectData._materialRef;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async void Invoke(IVisualAccessible visualAccessible) {
            await visualAccessible.GetModelManger().ChangeSkin(cancellationTokenSource.Token, material);
        }
        public async void Revert(IVisualAccessible visualAccessible) {
            cancellationTokenSource.Cancel();
            await visualAccessible.GetModelManger().RevertSkin();
        }
    }   
}