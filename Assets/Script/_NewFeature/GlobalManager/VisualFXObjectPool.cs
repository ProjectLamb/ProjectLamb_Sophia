using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Sophia
{    
    using Instantiates;
    
    public class VisualFXObjectPool : MonoBehaviour {
        
        private static VisualFXObjectPool _instance;
        
        public static VisualFXObjectPool Instance {
            get {
                if(_instance == null) {
                    _instance = FindObjectOfType(typeof(VisualFXObjectPool)) as VisualFXObjectPool;
                    if(_instance == null) Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set{}
        }

        [SerializeField] private List<VisualFXObject> _creatableVisualFXObjects; 
        public Dictionary<string, IObjectPool<VisualFXObject>> VFXPool {get; private set;}
        public int MaxPoolSize;

        private void Awake() {
            VFXPool = new Dictionary<string, IObjectPool<VisualFXObject>>();
        }
        private void Start() {
            _creatableVisualFXObjects.ForEach((E) => {
                VFXPool.Add(E.gameObject.name, null);

                VFXPool[E.gameObject.name] = new ObjectPool<VisualFXObject>(
                    createFunc: () => {
                        VisualFXObject concrete = Instantiate(E);
                        concrete.SetPool(this.VFXPool[E.gameObject.name]);
                        return concrete;
                    },
                    actionOnGet: OnGetObject,
                    actionOnRelease: OnReleaseObject,
                    actionOnDestroy: OnDestroyObject,
                    maxSize: MaxPoolSize
                );
            });
        }

#region ObjectPool

        private void OnGetObject(VisualFXObject vfxObject) {
            vfxObject.transform.parent = null;
            vfxObject.Get();
        }

        private void OnReleaseObject(VisualFXObject vfxObject) {
            vfxObject.Release();
            vfxObject.transform.SetParent(transform);
        }

        private void OnDestroyObject(VisualFXObject vfxObject) {
            Destroy(vfxObject.gameObject);
        }

#endregion

    }
}