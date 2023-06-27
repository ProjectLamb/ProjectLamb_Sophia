using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sophia_Carriers;

namespace AYellowpaper.SerializedCollections
{
    public class CustomSerializedDictionary : MonoBehaviour
    {
        [SerializedDictionary("Skill Rank", "Float")]
        public SerializedDictionary<SKILL_RANK, float> DictionaryFloatByRank;
        
        [SerializedDictionary("Skill Rank", "Projectiles")]
        public SerializedDictionary<SKILL_RANK, List<Projectile>> DictionaryProjectileByRank;


        [SerializedDictionary("Input Key", "GameObject")]
        public SerializedDictionary<string, GameObject> DictionaryObject;
    }
}