using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TEST_AttributeData : MonoBehaviour
{
    public class TempAttributeData
    {
        public Dictionary<E_DebuffState, bool> mDebuffState;        
        public Dictionary<E_BuffState, bool> mBuffState;
        public Dictionary<E_SynergyState, bool> mSynergyState;

        public TempAttributeData()
        {
            mDebuffState = new Dictionary<E_DebuffState, bool>();
            mBuffState = new Dictionary<E_BuffState, bool>();
            mSynergyState = new Dictionary<E_SynergyState, bool>();
        }
    }

    public TempAttributeData tempAttributeData;
    private void Awake()
    {
        tempAttributeData = new TempAttributeData();
    }

    [ContextMenu("Print This Attribute")]
    private void PrintAttributeAll()
    {
        string json = JsonConvert.SerializeObject(tempAttributeData.mDebuffState, Formatting.Indented);
        Debug.Log(json);
        json = JsonConvert.SerializeObject(tempAttributeData.mBuffState, Formatting.Indented);
        Debug.Log(json);
        json = JsonConvert.SerializeObject(tempAttributeData.mSynergyState, Formatting.Indented);
        Debug.Log(json);
    }
}