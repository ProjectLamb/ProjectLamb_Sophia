
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
        private FieldOfView FOV;
        public RecognizeEntityComposite(GameObject gameObject, in SerialFieldOfViewData serialFieldOfViewData) {
            FOV = gameObject.AddComponent<FieldOfView>();
            FOV.viewAngle       =  serialFieldOfViewData.viewAngle;
            FOV.viewRadius      =  serialFieldOfViewData.viewRadius;
            FOV.targetMask      =  serialFieldOfViewData.targetMask;
            FOV.obstacleMask    =  serialFieldOfViewData.obstacleMask;
            FOV.enabled = true;
            FOV.RecogType = E_RECOG_TYPE.None;

            CurrentViewRadius = serialFieldOfViewData.viewRadius;
            CurrentViewAngle = serialFieldOfViewData.viewAngle;
        }

        private float mCurrentViewRadius;
        public float CurrentViewRadius {
            get {
                return mCurrentViewRadius;
            }
            set {
                mCurrentViewRadius = value;
                FOV.viewRadius = mCurrentViewRadius;
            }
        } 

        private float mCurrentViewAngle;
        public float CurrentViewAngle {
            get {
                return mCurrentViewAngle;
            }
            set {
                if(0 > value || value > 360f) { value %= 360f; }
                mCurrentViewAngle = value;
                FOV.viewAngle = mCurrentViewAngle;
                return;
            }
        } 

        public E_RECOG_TYPE GetCurrentRecogState() => FOV.RecogType;

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