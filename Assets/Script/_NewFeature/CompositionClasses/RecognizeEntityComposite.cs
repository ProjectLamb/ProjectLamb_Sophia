using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;


namespace Sophia.Composite
{
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Instantiates;
    using Sophia.Entitys;
    using System.Diagnostics.Tracing;

    public enum E_RECOG_TYPE {
        None = 0, FirstRecog, Lose, ReRecog
    }

    [System.Serializable]
    public struct SerialFieldOfViewData {
        [SerializeField] [Range(0, 999)] public float viewRadius;
        [SerializeField] [Range(0, 360)] public float viewAngle;
        [SerializeField] public LayerMask targetMask, obstacleMask;
    }

    public interface IRecogStateAccessible {
        public RecognizeEntityComposite GetRecognizeComposite();
    }

    public class RecognizeEntityComposite {
        private E_RECOG_TYPE RecogType;
        private FieldOfView FOV;
        private bool RecogOnce;
        public RecognizeEntityComposite(GameObject gameObject, in SerialFieldOfViewData serialFieldOfViewData) {
            FOV = gameObject.AddComponent<FieldOfView>();
            FOV.viewAngle       =  serialFieldOfViewData.viewAngle;
            FOV.viewRadius      =  serialFieldOfViewData.viewRadius;
            FOV.targetMask      =  serialFieldOfViewData.targetMask;
            FOV.obstacleMask    =  serialFieldOfViewData.obstacleMask;
            FOV.enabled = true;
            RecogType = E_RECOG_TYPE.None;
        }

        public float GetViewRadius() => FOV.viewRadius;
        public float GetViewAngle() => FOV.viewAngle;
        public E_RECOG_TYPE GetCurrentRecogState() {
            if(FOV.IsRecog)
                if(!RecogOnce){
                    RecogOnce = true;
                    return RecogType = E_RECOG_TYPE.FirstRecog;
                }
                else 
                    return RecogType = E_RECOG_TYPE.ReRecog;
            else 
                return RecogType = E_RECOG_TYPE.Lose;
        }

        public List<Entity> GetEntities() {
            List<Entity> entities = new List<Entity>();
            FOV.visibleTargets.ForEach(E => entities.Add(E.GetComponent<Entity>()));
            return entities;
        }

        public List<ProjectileObject> GetProjectileObjects() {
            List<ProjectileObject> projectiles = new List<ProjectileObject>();
            FOV.visibleTargets.ForEach(E => projectiles.Add(E.GetComponent<ProjectileObject>()));
            return projectiles;
        }
    }
}