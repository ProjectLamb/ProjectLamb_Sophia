using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Sophia
{    
    using Entitys;
    using Instantiates;

    public class VisualFXObjectPool : MonoBehaviour {
        
        private static VisualFXObjectPool _instance;
        
        public static VisualFXObjectPool Instance {
            get {
                if(_instance == null) {
                    _instance = FindFirstObjectByType(typeof(VisualFXObjectPool)) as VisualFXObjectPool;
                    if(_instance == null) Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set{}
        }

        public static VisualFXObject GetObject(VisualFXObject visualFXReference) {
            return Instance.VFXPool[visualFXReference.gameObject.name].Get();
        }

        [SerializeField] private List<VisualFXObject> _creatableVisualFXObjects; 
        public Dictionary<string, IObjectPool<VisualFXObject>> VFXPool {get; private set;}
        public int MaxPoolSize;

        private void Awake() {
            VFXPool = new Dictionary<string, IObjectPool<VisualFXObject>>();
        }
        
        /*⚠️ Instantiate(E)라면.. 같이 달린 컴포넌트도 같이 생성되나? 이거 버그 조심할것*/

        private void Start() {
            _creatableVisualFXObjects.ForEach((E) => {
                VFXPool.Add(E.gameObject.name, null);

                VFXPool[E.gameObject.name] = new ObjectPool<VisualFXObject>(
                    createFunc: () => {
                        if(E.DEBUG) {Debug.Log("인스턴시에이트 실행");}
                        VisualFXObject concrete = Instantiate<VisualFXObject>(E) as VisualFXObject;
                        concrete.SetByPool(this.VFXPool[E.gameObject.name]);
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
            vfxObject.GetByPool();
        }

        private void OnReleaseObject(VisualFXObject vfxObject) {
            vfxObject.ReleaseByPool();
            vfxObject.transform.SetParent(transform);
        }

        private void OnDestroyObject(VisualFXObject vfxObject) {
            Destroy(vfxObject.gameObject);
        }

#endregion

    }
}