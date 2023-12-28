using UnityEngine;

namespace Feature_NewData
{    

    /*********************************************************************************
    * ModelManger 
    * 모델 매니저는 모델의 
        * 애니메이터와 
        * SkinnedMesh에
    의존하는 클래스다.
    * VFX는 아니지만
    * UI와 같이 게임 로직에 영향은 없지만. 비주얼로 영향을 주는 녀석

    * 스킨의 변경. - Affector와 연관이 있음.
    * 애니메이션 과 로직 함수와 바인딩
    *********************************************************************************/
    
    public class ModelManger : MonoBehaviour {
        [SerializeField] private GameObject Model;
        
        [Tooltip("SkinMaterial 스킨을 입힐 대상들이다.")]
        private SkinnedMeshRenderer[] skinnedMeshRenderers;
        private Animator modelAnimator;

        private void Awake() {
            
        }

        public void ChangeSkin(Material skin) {}
        public void RevertSkin() {}
        public Animator GetAnimator() {return this.modelAnimator;}
    }
}