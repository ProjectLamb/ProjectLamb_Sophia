using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

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
    
    public class ModelManager : MonoBehaviour {
#region SerializeMember

        [SerializeField] private GameObject _model;
        // [SerializeField] private GameObject _skins;
        [SerializeField] private MotionTrail _motionTrail;
        [SerializeField] private ModelHands _modelHands;
        [SerializeField] private Animator _modelAnimator;
        [SerializeField] private MaterialVFX _materialVFX;
        

        [Tooltip("SkinMaterial 스킨을 입힐 대상들이다.")]
        // [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
        // [SerializeField] private List<Material> _materials = new List<Material>();

#endregion

#region Member
        
        public Material TransMaterial;

#endregion

        private void Awake() {
            _model = this.gameObject;
            
            // foreach(Transform skinTransform in _skins.transform) {
            //     _renderers.Add(skinTransform.GetComponent<Renderer>());
            // }
            // if(_materials.Count == 0) throw new System.Exception("Skin리스트가 없어서 설정하고싶은 스킨이 없음");
        }

#region Skin
        public MaterialVFX GetMaterialVFX() => _materialVFX;
        
        [Obsolete]
        public async UniTask ChangeSkin(CancellationToken cancellationToken, Material skin) {
            // _materials[1] = skin;
            // foreach (Renderer renderer in _renderers) {
            //     renderer.sharedMaterials = _materials.ToArray();
            // }
            // await UniTask.WaitForEndOfFrame(this, cancellationToken);
            // cancellationToken.ThrowIfCancellationRequested();
        }

        [Obsolete]
        public async UniTask RevertSkin() {
            // _materials[1] = TransMaterial;
            // foreach (Renderer renderer in _renderers) {
            //     renderer.sharedMaterials = _materials.ToArray();
            // }
            // await UniTask.WaitForEndOfFrame(this);
        }

        public async UniTask InvokeChangeMaterial(CancellationToken cancellationToken, E_AFFECT_TYPE affectType) {
            _materialVFX.AffectorMaterialChanger[affectType].InvokeAffectMaterial();
            await UniTask.WaitForEndOfFrame(this, cancellationToken);
        }
        public async UniTask InvokeChangeMaterial(CancellationToken cancellationToken, E_FUNCTIONAL_EXTRAS_TYPE entityFunctionalActType) {
            _materialVFX.FunctionalMaterialChanger[entityFunctionalActType].InvokeEntityFunctionalActMaterial();
            await UniTask.WaitForEndOfFrame(this, cancellationToken);
        }

        public async UniTask RevertChangeMaterial(E_AFFECT_TYPE affectType) {
            _materialVFX.AffectorMaterialChanger[affectType].RevertAffectMaterial();
            await UniTask.WaitForEndOfFrame(this);
        }
        public async UniTask RevertChangeMaterial(E_FUNCTIONAL_EXTRAS_TYPE entityFunctionalActType) {
            _materialVFX.FunctionalMaterialChanger[entityFunctionalActType].RevertEntityFunctionalActMaterial();
            await UniTask.WaitForEndOfFrame(this);
        }


#endregion

#region Motion Trail
        public void EnableTrail() {
            _motionTrail.gameObject.SetActive(true);
            _motionTrail.TargetSkinMesh.SetActive(true);
        }
        public void DisableTrail() {
            _motionTrail.gameObject.SetActive(false);
            _motionTrail.TargetSkinMesh.SetActive(false);
        }
#endregion

#region 3D Model
        public GameObject GetModelObject() => _model;
        public void HoldObject(GameObject go, E_MODEL_HAND handPos) => _modelHands.HoldObject(go, handPos);
        public void DropObject(E_MODEL_HAND handPos) => _modelHands.DropObject(handPos);

        IEnumerator DoAndRenderModel(UnityAction action){
            action.Invoke(); yield return new WaitForEndOfFrame();
        }

#endregion

#region Animation

        public Animator GetAnimator() {
            if(this._modelAnimator == null) {
                _model.TryGetComponent<Animator>(out _modelAnimator);
            }
            return this._modelAnimator;
        }

#endregion
    }
}