using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Sophia.Composite.RenderModels
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
        [SerializeField] private GameObject _model;
        [SerializeField] private ModelHands _modelHands;
        [SerializeField] private Animator _modelAnimator;
        
        [Tooltip("SkinMaterial 스킨을 입힐 대상들이다.")]
        [SerializeField]private List<Renderer> _renderers = new List<Renderer>();
        [SerializeField] private List<Material> _materials = new List<Material>();
        public Material TransMaterial;

        private void Awake() {
            _model = this.gameObject;
            
            _model.TryGetComponent<Animator>(out _modelAnimator);
            foreach(Transform skinTransform in transform.Find("skins")) {
                _renderers.Add(skinTransform.GetComponent<Renderer>());
            }
            if(_materials.Count == 0) throw new System.Exception("Skin리스트가 없어서 설정하고싶은 스킨이 없음");
        }

        public async UniTask ChangeSkin(Material skin) {
            _materials[1] = skin;
            await UniTask.WaitForEndOfFrame(this);
            foreach (Renderer renderer in _renderers) {
                renderer.sharedMaterials = _materials.ToArray();
            }
        }

        public async UniTask RevertSkin() {
            _materials[1] = TransMaterial;
            await UniTask.WaitForEndOfFrame(this);
            foreach (Renderer renderer in _renderers) {
                renderer.sharedMaterials = _materials.ToArray();
            }
        }

        public void HoldObject(GameObject go, E_MODEL_HAND handPos) => _modelHands.HoldObject(go, handPos);
        public void DropObject(E_MODEL_HAND handPos) => _modelHands.DropObject(handPos);

        IEnumerator DoAndRenderModel(UnityAction action){
            action.Invoke(); yield return new WaitForEndOfFrame();
        }
        
        public Animator GetAnimator() {return this._modelAnimator;}
    }
}