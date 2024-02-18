using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Sophia.DataSystem.Atomics
{
    using Sophia.Composite.RenderModels;
    using Sophia.Entitys;

    public class MaterialChangeAtomics {
        public CancellationToken cancellationToken;
        public Material material;
        public MaterialChangeAtomics(in SerialSkinData skinAffectData) {
            material = skinAffectData._materialRef;
            cancellationToken = new CancellationToken();
        }

        public async void Invoke(IVisualAccessible visualAccessible) {
            await visualAccessible.GetModelManger().ChangeSkin(cancellationToken, material);
        }
        public async void Revert(IVisualAccessible visualAccessible) {
            await visualAccessible.GetModelManger().RevertSkin(cancellationToken);
        }
    }   
}