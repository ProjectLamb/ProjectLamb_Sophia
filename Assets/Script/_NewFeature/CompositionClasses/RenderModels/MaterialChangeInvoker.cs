// using System.Collections.Generic;
// using AYellowpaper.SerializedCollections;
// using DG.Tweening;
// using UnityEngine;

// namespace Sophia.Composite.RenderModels
// {
//     public interface IMaterialChangeable<T> where T : System.Enum {
//         public void PlayOnShot(T EnumType);
//     }

//     /// <summary>
//     /// 마테리얼 인보커는 Functional인지, Affector인지 등등 마테리얼 종류에 따라 구분
//     /// </summary>
//     public class MaterialChangeInvoker<T> : MonoBehaviour, IMaterialChangeable<T> where T : System.Enum  {
//         [SerializedDictionary("EntityFunctionalAct", "SerialMaterialChangerData")]
//         [SerializeField] SerializedDictionary<T, SerialMaterialChange> _materialChangerData;
//         public readonly Dictionary<T, MaterialChanger> mMaterialChanger = new Dictionary<T, MaterialChanger>();
//         [SerializeField] MaterialVFX materialVFXRef;

//         public void Init() {
//             if(materialVFXRef == null) throw new Exception("");
//             foreach(var serialData in _materialChangerData) {
//                 mMaterialChanger.Add(serialData.Key, new MaterialChanger(serialData.Value, materialVFXRef));
//             }
//         }

//         public void PlayOnShot(T functionalActType) {
//             if(mMaterialChanger.ContainsKey(functionalActType)) {
//                 mMaterialChanger[functionalActType].PlayOnShot();
//             }
//             else throw new System.Exception("엔티티 동작 타입이 없음");
//         }
//     }

//     [System.Serializable]
//     public struct SerialMaterialChange {
//         [SerializeField] public E_MATERIAL_TYPE _materialType;
//         [SerializeField] public E_FUNCTIONAL_EXTRAS_TYPE _functionalActType;
//         [SerializeField] public E_AFFECT_TYPE _affectType;
//         [SerializeField] public float _speed;
//         [SerializeField] public AnimationCurve _curve;
//         [SerializeField] public bool _zeroStartPoint;
//     }

//     public class MaterialChanger {
//         public E_MATERIAL_TYPE materialType;
//         public E_FUNCTIONAL_EXTRAS_TYPE functionalActType = E_FUNCTIONAL_EXTRAS_TYPE.None;
//         public E_AFFECT_TYPE affectType = E_AFFECT_TYPE.None;
//         public float speed;
//         public AnimationCurve curve;
//         public bool zeroStartPoint;
//         public MaterialVFX materialVFXRef;

//         public float Amount;
//         public int AccelerateSign;

//         public MaterialChanger(in SerialMaterialChange serialMaterialData, MaterialVFX materialVFX) {
//             materialType = serialMaterialData._materialType;
//             functionalActType = serialMaterialData._functionalActType;
//             affectType = serialMaterialData._affectType;
//             speed = serialMaterialData._speed;
//             curve = serialMaterialData._curve;
//             zeroStartPoint = serialMaterialData._zeroStartPoint;
//             materialVFXRef = materialVFX;

//             Amount = zeroStartPoint ? 0 : 1;
//             AccelerateSign = zeroStartPoint ? 1 : -1;
//         }

//         private Sequence currentSequnce;

//         public void PlayOnShot(T ) {
//             if (currentSequnce != null && currentSequnce.IsPlaying())
//             {
//                 currentSequnce.Kill();
//                 currentSequnce = null;
//             }
//             currentSequnce = DOTween.Sequence();
//             currentSequnce
//                 .OnStart(materialVFXRef.M)
//                 .Append(DOVirtual.Float(Amount, ))
//         }

//         public void Play() {

//         }
//     }
// }