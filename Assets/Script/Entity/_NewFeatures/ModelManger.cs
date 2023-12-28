using UnityEngine;

namespace Feature_NewData
{    
    public class ModelManger : MonoBehaviour {
        [SerializeField] private GameObject Model;
        
        [Tooltip("SkinMaterial 스킨을 입힐 대상들이다.")]
        private SkinnedMeshRenderer[] skinnedMeshRenderers;
        private Animator modelAnimator;

        public void ChangeSkin(Material skin) {}
        public void RevertSkin() {}
        public Animator GetAnimator() {return this.modelAnimator;}
    }
}