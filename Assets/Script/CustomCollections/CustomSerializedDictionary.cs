using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AYellowpaper.SerializedCollections
{
    public class CustomSerializedDictionary : MonoBehaviour
    {
        [SerializedDictionary("Input Key", "Skill")]
        public SerializedDictionary<string, Skill> DictionarySkill;

        [SerializedDictionary("Input Key", "GameObject")]
        public SerializedDictionary<string, GameObject> DictionaryObject;
    }
}