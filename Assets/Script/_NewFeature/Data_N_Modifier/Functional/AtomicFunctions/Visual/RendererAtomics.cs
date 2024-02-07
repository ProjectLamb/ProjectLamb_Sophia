using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Sophia.DataSystem.Functional
{
    using Sophia.Composite.RenderModels;
    using Sophia.Entitys;

    public class MaterialChangeAtomics {
        public CancellationToken cts;
        public Material materialRef;
        public Entity entityRef;
        public MaterialChangeAtomics(Material material, Entity entity) {
            material = materialRef;
            entityRef = entity;
        }

        public async void Invoke() {
            await entityRef.GetModelManger().ChangeSkin(cts, materialRef);
        }
        public async void Revert() {
            await entityRef.GetModelManger().RevertSkin(cts);
        }
    }   
}