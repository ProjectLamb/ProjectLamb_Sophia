using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using DG.Tweening;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource;
using Sophia.DataSystem.Modifiers; 

namespace Sophia.Composite.RenderModels
{
    public class MaterialVFX : MonoBehaviour {
        [SerializeField] private GameObject _skinObject;
        [SerializeField] private DB.ScriptableCommonEntityMaterial _commonEntityMaterialRefer;
        [SerializeField] private DB.ScriptableMaterialChangeData _materialChangeDataRefer;
        [SerializeField] private List<Material> _entityMaterials = new List<Material>();
        /// <summary>
        /// 한 엔티티에 적용되는 마테리얼이 아트팀 모델러가 2개 이상 넘겨줌에 대비하기 위함.  
        /// </summary>
        [SerializedDictionary("MaterialKey", "Renderer")]
        [SerializeField] private SerializedDictionary<Material, List<Renderer>> _originVarientMaterial;
        [SerializeField] Material CommonFunctionalActMaterial;
        [SerializeField] Material CommonAffectMaterial;
        public readonly Dictionary<E_FUNCTIONAL_EXTRAS_TYPE, MaterialChanger> FunctionalMaterialChanger = new Dictionary<E_FUNCTIONAL_EXTRAS_TYPE, MaterialChanger>();
        public readonly Dictionary<E_AFFECT_TYPE, MaterialChanger> AffectorMaterialChanger = new Dictionary<E_AFFECT_TYPE, MaterialChanger>();

        private readonly Dictionary<Material, Material[]> sharedMaterialsByVarientMaterial = new Dictionary<Material, Material[]>();

        [ContextMenu("InitMaterials In EditMode")]
        private void InitInEditMode() {
            CommonFunctionalActMaterial = _commonEntityMaterialRefer.CopyFunctionalActMaterialInstant();
            CommonAffectMaterial        = _commonEntityMaterialRefer.CopyAffectMaterialInstant();
            if(_originVarientMaterial == null || _originVarientMaterial.Count > 0) _originVarientMaterial = new SerializedDictionary<Material, List<Renderer>>();
            foreach(Material mat in _entityMaterials) {
                if(!_originVarientMaterial.ContainsKey(mat)) {
                    _originVarientMaterial.Add(mat, new List<Renderer>());
                }
                sharedMaterialsByVarientMaterial.Add(mat, new Material[3] {mat, CommonFunctionalActMaterial, CommonAffectMaterial});
            }
            foreach(Transform skinTransform in _skinObject.transform) {
                Renderer renderer = skinTransform.GetComponent<Renderer>();
                foreach(Material mat in _entityMaterials) {
                    if(mat == renderer.sharedMaterial) {
                        _originVarientMaterial[mat].Add(renderer);
                        renderer.sharedMaterials = sharedMaterialsByVarientMaterial[mat];
                    }
                }
            }
        }

        public void Init() {
            CommonFunctionalActMaterial = _commonEntityMaterialRefer.CopyFunctionalActMaterialInstant();
            CommonAffectMaterial        = _commonEntityMaterialRefer.CopyAffectMaterialInstant();
            InitializeShaderMaterials();
            SetSharedMaterialInRenderer();
            InitializeMaterialChangers();
        }

        private void InitializeShaderMaterials() {
            if(_originVarientMaterial == null || _originVarientMaterial.Count > 0) _originVarientMaterial = new SerializedDictionary<Material, List<Renderer>>();
            foreach(Material mat in _entityMaterials) {
                if(!_originVarientMaterial.ContainsKey(mat)) { _originVarientMaterial.Add(mat, new List<Renderer>()); }
                sharedMaterialsByVarientMaterial.Add(mat, new Material[3] {mat, CommonFunctionalActMaterial, CommonAffectMaterial});
            }
        }

        private void SetSharedMaterialInRenderer() {
            foreach(Transform skinTransform in _skinObject.transform) {
                Renderer renderer = skinTransform.GetComponent<Renderer>();
                foreach(Material mat in _entityMaterials) {
                    if(mat == renderer.sharedMaterial) {
                        _originVarientMaterial[mat].Add(renderer);
                        renderer.sharedMaterials = sharedMaterialsByVarientMaterial[mat];
                    }
                }
            }
        }

        private void InitializeMaterialChangers() {
            foreach(var data in _materialChangeDataRefer.FunctionalMaterialChangerData) {
                FunctionalMaterialChanger.Add(data.Key, new MaterialChanger(data.Key, data.Value, CommonFunctionalActMaterial));
            }
            foreach(var data in _materialChangeDataRefer.AffectorMaterialChangerData) {
                AffectorMaterialChanger.Add(data.Key, new MaterialChanger(data.Key, data.Value, CommonAffectMaterial));
            }
        }
    }

    public class MaterialChanger {
        public static readonly string SHADER_PREPOSITIONS = "_";
        public static readonly string BOOLEAN_PREPOSITIONS = "Is";
        public static readonly string AMOUNT_POSTPOSITIONS = "Amount";

        public E_FUNCTIONAL_EXTRAS_TYPE functionalActType = E_FUNCTIONAL_EXTRAS_TYPE.None;
        public E_AFFECT_TYPE affectType = E_AFFECT_TYPE.None;

        public float speed;
        public AnimationCurve curve;
        public bool zeroStartPoint;
        public Material materialRef;
        public bool IsVFXActivated = false;
        public float StartPoint;
        public float EndPoint;
        public int AccelerateSign;

        public MaterialChanger(E_FUNCTIONAL_EXTRAS_TYPE fat, in DB.SerialMaterialChange serialMaterialData, Material material) {
            functionalActType = fat;
            speed = (serialMaterialData._speed > 0 ) ? serialMaterialData._speed : 0.01f;
            curve = serialMaterialData._curve;
            zeroStartPoint = serialMaterialData._zeroStartPoint;
            materialRef = material;

            StartPoint = zeroStartPoint ? 0 : 1;
            AccelerateSign = zeroStartPoint ? 1 : -1;
            EndPoint = StartPoint + AccelerateSign;
        }

        public MaterialChanger(E_AFFECT_TYPE at,in DB.SerialMaterialChange serialMaterialData, Material material) {
            affectType = at;
            speed = (serialMaterialData._speed > 0 ) ? serialMaterialData._speed : 0.01f;
            curve = serialMaterialData._curve;
            zeroStartPoint = serialMaterialData._zeroStartPoint;
            materialRef = material;

            StartPoint = zeroStartPoint ? 0 : 1;
            AccelerateSign = zeroStartPoint ? 1 : -1;
            EndPoint = StartPoint + AccelerateSign;
        }

        private Sequence currentSequnce;

        public void InvokeEntityFunctionalActMaterial() {
            if(IsVFXActivated == true) return;
            if((int)functionalActType % 10 == 0) {throw new System.Exception("올바르지 않은 엔티티 헹동 타입");}
            materialRef.SetInt(SHADER_PREPOSITIONS + BOOLEAN_PREPOSITIONS + functionalActType.ToString(), 1);
            IsVFXActivated = true;
        }

        public void ChangeAmountEntityFunctionalActMaterial(float amount) {
            if(IsVFXActivated == false) return;
            if((int)functionalActType % 10 == 0) {throw new System.Exception("올바르지 않은 엔티티 헹동 타입");}
            materialRef.SetFloat(SHADER_PREPOSITIONS + functionalActType.ToString() + AMOUNT_POSTPOSITIONS, amount);
        }

        public void RevertEntityFunctionalActMaterial() {
            if(IsVFXActivated == false) return;
            if((int)functionalActType % 10 == 0) {throw new System.Exception("올바르지 않은 엔티티 헹동 타입");}
            materialRef.SetInt(SHADER_PREPOSITIONS + BOOLEAN_PREPOSITIONS + functionalActType.ToString(), 0);
            materialRef.SetFloat(SHADER_PREPOSITIONS + functionalActType.ToString() + AMOUNT_POSTPOSITIONS, StartPoint);
            IsVFXActivated = false;
        }

        public void InvokeAffectMaterial() {
            if(IsVFXActivated == true) return;
            if((int)affectType % 100 == 0) {throw new System.Exception("올바르지 않은 어펙터 타입");}
            materialRef.SetInt(SHADER_PREPOSITIONS + BOOLEAN_PREPOSITIONS + affectType.ToString(), 1);
            IsVFXActivated = true;
        }

        public void ChangeAmountAffectMaterial(float amount) {
            if(IsVFXActivated == false) return;
            if((int)affectType % 100 == 0) {throw new System.Exception("올바르지 않은 어펙터 타입");}
            materialRef.SetFloat(SHADER_PREPOSITIONS + affectType.ToString() + AMOUNT_POSTPOSITIONS, amount);
        }

        public void RevertAffectMaterial() {
            if(IsVFXActivated == false) return;
            if((int)affectType % 100 == 0) {throw new System.Exception("올바르지 않은 어펙터 타입");}
            materialRef.SetInt(SHADER_PREPOSITIONS + BOOLEAN_PREPOSITIONS + affectType.ToString(), 0);
            materialRef.SetFloat(SHADER_PREPOSITIONS + affectType.ToString() + AMOUNT_POSTPOSITIONS, StartPoint);
            IsVFXActivated = false;
        }

        public void PlayFunctionalActOneShot() {
            if (currentSequnce != null && currentSequnce.IsPlaying())
            {
                currentSequnce.Kill();
                currentSequnce = null;
            }
            currentSequnce = DOTween.Sequence();
            currentSequnce
                .OnStart(InvokeEntityFunctionalActMaterial)
                .Append(DOVirtual.Float(StartPoint, EndPoint, 1 / speed, ChangeAmountEntityFunctionalActMaterial).SetEase(curve))
                .OnComplete(RevertEntityFunctionalActMaterial);
            currentSequnce.Play();
        }

        public void PlayFunctionalActOneShotWithDuration(float durateTime) {
            if (currentSequnce != null && currentSequnce.IsPlaying())
            {
                currentSequnce.Kill();
                currentSequnce = null;
            }
            currentSequnce = DOTween.Sequence();
            currentSequnce
                .OnStart(InvokeEntityFunctionalActMaterial)
                .Append(DOVirtual.Float(StartPoint, EndPoint, durateTime, ChangeAmountEntityFunctionalActMaterial).SetEase(curve))
                .OnComplete(RevertEntityFunctionalActMaterial);
            currentSequnce.Play();
        }
        public void PlayAffectOneShot() {
            if (currentSequnce != null && currentSequnce.IsPlaying())
            {
                currentSequnce.Kill();
                currentSequnce = null;
            }
            currentSequnce = DOTween.Sequence();
            currentSequnce
                .OnStart(InvokeAffectMaterial)
                .Append(DOVirtual.Float(StartPoint, EndPoint, 1 / speed, ChangeAmountAffectMaterial).SetEase(curve))
                .OnComplete(RevertAffectMaterial);
            currentSequnce.Play();
        }

        public void PlayAffectOneShotWithDuration(float durateTime) {
            if (currentSequnce != null && currentSequnce.IsPlaying())
            {
                currentSequnce.Kill();
                currentSequnce = null;
            }
            currentSequnce = DOTween.Sequence();
            currentSequnce
                .OnStart(InvokeAffectMaterial)
                .Append(DOVirtual.Float(StartPoint, EndPoint, durateTime, ChangeAmountAffectMaterial).SetEase(curve))
                .OnComplete(RevertAffectMaterial);
            currentSequnce.Play();
        }
    }
}

namespace Sophia.DB{

    [System.Serializable]
    public struct SerialMaterialChange {
        [SerializeField] public float _speed;
        [SerializeField] public AnimationCurve _curve;
        [SerializeField] public bool _zeroStartPoint;
    }
}