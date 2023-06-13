using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AYellowpaper.SerializedCollections
{
    public class CustomSerializedDictionary : MonoBehaviour
    {
        [SerializedDictionary("Input Key", "Skill")]
        public SerializedDictionary<string, ConcreteSkill> DictionarySkill;


        [SerializedDictionary("Skill Rank", "Float")]
        public SerializedDictionary<E_SkillRank, float> DictionaryFloatByRank;

        [SerializedDictionary("Skill Rank", "Numeric")]
        public SerializedDictionary<E_SkillRank, SkillNumeric> DictionaryNumericsByRank;
        
        [SerializedDictionary("Skill Rank", "Projectiles")]
        public SerializedDictionary<E_SkillRank, List<Projectile>> DictionaryProjectileByRank;
        

        [SerializedDictionary("Skill Key", "SkillElement")]
        public SerializedDictionary<E_SkillKey, SkillElement> DictionarySkillElementByKey;


        [SerializedDictionary("Input Key", "GameObject")]
        public SerializedDictionary<string, GameObject> DictionaryObject;
    }
}