using UnityEngine;
namespace Sophia.DB
{
    
    [CreateAssetMenu(fileName = "CommonEntityMaterial", menuName = "ScriptableObject/Entity/CommonMaterial", order = int.MaxValue)]
    public class ScriptableCommonEntityMaterial : ScriptableObject {
        [SerializeField] private Material _commonFunctionalActMaterialPrefeb;
        [SerializeField] private Material _commonAffectMaterialPrefeb;

        public Material CopyFunctionalActMaterialInstant() {
            return Instantiate(_commonFunctionalActMaterialPrefeb);
        }
        public Material CopyAffectMaterialInstant() {
            return Instantiate(_commonAffectMaterialPrefeb);
        }
    }
}