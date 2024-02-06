using TMPro;
using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    public class DodgeCommand : IFunctionalCommand<float>
    {
        private Entitys.Entity OwnerRef;
        private Material materialRef;

        public DodgeCommand(Entitys.Entity owner, Material material) {
            OwnerRef = owner;
            materialRef = material;
        }
        
        public string GetDescription() => "들어온 데미지를 보정합니다.";

        public string GetName() => "데미지 보정";

        public Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }
        public void Invoke(ref float referer)
        {
            if(10 < UnityEngine.Random.Range(0,100)) return;
            referer = 0;
            OwnerRef.GetComponentInChildren<DamageTextInstantiator>().Generate("Dodge!");
        }
    }
}