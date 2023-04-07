using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    public class CustomSerializedDictionary : MonoBehaviour
    {
        [SerializedDictionary("Input Key", "Skill")]
        public SerializedDictionary<string, Skill> ElementDescriptions;
    }
}