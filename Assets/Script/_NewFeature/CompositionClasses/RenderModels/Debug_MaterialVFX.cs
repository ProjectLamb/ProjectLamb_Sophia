using BehaviorDesigner.Runtime.ObjectDrawers;
using Sophia;
using Sophia.Composite.RenderModels;
using Unity.Cinemachine;
using UnityEngine;

namespace DEBUG
{
    [ExecuteInEditMode]
    public class Debug_MaterialVFX : MonoBehaviour {
        [SerializeField] MaterialVFX materialVFX;
        public E_MATERIAL_TYPE materialType;
        public E_AFFECT_TYPE _affectType;
        public E_FUNCTIONAL_EXTRAS_TYPE _functionalActType;
        
        [Range(0, 1)] public float _amount;

        [ContextMenu("Invoke")]
        private void InvokeMaterial() {
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    materialVFX.FunctionalMaterialChanger[_functionalActType].InvokeEntityFunctionalActMaterial();
                    break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    materialVFX.AffectorMaterialChanger[_affectType].InvokeAffectMaterial();
                    break;
                }
            }
        }

        [ContextMenu("Revert")]
        private void RevertMaterial() {
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    materialVFX.FunctionalMaterialChanger[_functionalActType].RevertEntityFunctionalActMaterial();
                    break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    materialVFX.AffectorMaterialChanger[_affectType].RevertAffectMaterial();
                    break;
                }
            }
        }

        [ContextMenu("OneShot")]
        private void PlayOneShotMaterial() {
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    materialVFX.FunctionalMaterialChanger[_functionalActType].PlayFunctionalActOneShot();
                    break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    materialVFX.AffectorMaterialChanger[_affectType].PlayAffectOneShot();
                    break;
                }
            }
        }
        
        private void Update() {
            if(Input.GetKeyDown(KeyCode.O)) {PlayOneShotMaterial();}
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    materialVFX.FunctionalMaterialChanger[_functionalActType].ChangeAmountEntityFunctionalActMaterial(_amount);
                    break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    materialVFX.AffectorMaterialChanger[_affectType].ChangeAmountAffectMaterial(_amount);
                    break;
                }
            }
        }
    }   
}