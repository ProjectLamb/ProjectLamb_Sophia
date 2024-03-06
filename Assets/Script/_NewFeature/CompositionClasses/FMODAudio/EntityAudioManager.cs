using UnityEngine;
using System.Collections.Generic;
using FMODPlus;
using FMODUnity;

namespace Sophia.Composite
{
    public class EntityAudioManager : MonoBehaviour 
    {
#region SerializeMember

        [SerializeField] private Entitys.Entity _entity;

#endregion
        private Dictionary<EventReference, FMODAudioSource> AudioSourceDic = new ();
        public void AddSFX(EventReference eventRef) {
            if(!AudioSourceDic.ContainsKey(eventRef)){
                AudioSourceDic.Add(eventRef, gameObject.AddComponent<FMODAudioSource>());
                AudioSourceDic[eventRef].playOnAwake = false;
                AudioSourceDic[eventRef].clip = eventRef;
            }
        }
        
        public void PlaySFX(EventReference eventRef) {
            AudioSourceDic[eventRef].Play();
        }

        public void StopSFX(EventReference eventRef) {
            AudioSourceDic[eventRef].Stop(true);
        }

        public void SetParameterSFX(EventReference eventRef, string paramName, float value, bool ignoreseekspeed = false) {
            AudioSourceDic[eventRef].SetParameter(paramName, value, ignoreseekspeed);
        }
        
        public void ApplyParameterSFX(EventReference eventRef, ParamRef[] paramRefs) {
            if(paramRefs.Length == 0) return;
            AudioSourceDic[eventRef].ApplyParameter(paramRefs);
        }
    }
}