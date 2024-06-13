using System.Collections.Generic;
using System.Collections.ObjectModel;
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Sophia.DB
{
    
    [CreateAssetMenu(fileName = "MaterialChangeData", menuName = "ScriptableObject/Entity/MaterialChangeData", order = int.MaxValue)]
    public class ScriptableMaterialChangeData : ScriptableObject {
        [SerializedDictionary("EntityFunctionalActType", "SerialMaterialChangerData")]
        [SerializeField] SerializedDictionary<E_FUNCTIONAL_EXTRAS_TYPE, DB.SerialMaterialChange> _functionalMaterialChangerData;
        [SerializedDictionary("AffectType", "SerialMaterialChangerData")]
        [SerializeField] SerializedDictionary<E_AFFECT_TYPE, DB.SerialMaterialChange> _affectorMaterialChangerData;

        public IReadOnlyDictionary<E_FUNCTIONAL_EXTRAS_TYPE, DB.SerialMaterialChange> FunctionalMaterialChangerData => _functionalMaterialChangerData;
        public IReadOnlyDictionary<E_AFFECT_TYPE, DB.SerialMaterialChange> AffectorMaterialChangerData => _affectorMaterialChangerData;

    }
}