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
                    materialVFX.InvokeEntityFunctionalActMaterial(_functionalActType);
                    break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    materialVFX.InvokeAffectMaterial(_affectType);
                    break;
                }
            }
        }

        [ContextMenu("Revert")]
        private void RevertMaterial() {
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    materialVFX.RevertEntityFunctionalActMaterial(_functionalActType);
                    break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    materialVFX.RevertAffectMaterial(_affectType);
                    break;
                }
            }
        }
        
        private void Update() {
            switch(materialType) {
                case E_MATERIAL_TYPE.FunctionalAct : {
                    materialVFX.ChangeAmountEntityFunctionalActMaterial(_functionalActType, _amount);
                    break;
                }
                case E_MATERIAL_TYPE.Affect : {
                    materialVFX.ChangeAmountAffectMaterial(_affectType, _amount);
                    break;
                }
            }
        }
    }   
}