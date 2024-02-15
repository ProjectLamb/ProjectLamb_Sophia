using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Sophia
{
    using Entitys;
    using Instantiates;
    
    public class ProjectilePool : MonoBehaviour {
        private static ProjectilePool _instance;

        public static ProjectilePool Instance {
            get {
                if(_instance == null) {
                    _instance = FindObjectOfType(typeof(ProjectilePool)) as ProjectilePool;
                    if(_instance == null) Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set {}
        }

        public static ProjectileObject GetObject(ProjectileObject projectileReferernce) {
            return Instance.ProPool[projectileReferernce.gameObject.name].Get();
        }

        [SerializeField] private List<ProjectileObject> _creatableProjectiles;
        public Dictionary<string, IObjectPool<ProjectileObject>> ProPool {get; private set;}
        public int MaxPoolSize;

        private void Awake() {
            ProPool = new Dictionary<string, IObjectPool<ProjectileObject>>();
        }
        
        /*⚠️ Instantiate(E)라면.. 같이 달린 컴포넌트도 같이 생성되나? 이거 버그 조심할것*/

        private void Start() {
            _creatableProjectiles.ForEach((E) => {
                ProPool.Add(E.gameObject.name, null);

                ProPool[E.gameObject.name] = new ObjectPool<ProjectileObject>(
                    createFunc: () => {
                        ProjectileObject concrete = Instantiate(E);
                        concrete.SetByPool(this.ProPool[E.gameObject.name]);
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

        private void OnGetObject(ProjectileObject projectile) {
            projectile.transform.parent = null;
            projectile.GetByPool();
        }

        private void OnReleaseObject(ProjectileObject projectile) {
            projectile.ReleaseByPool();
            projectile.transform.SetParent(transform);
        }

        private void OnDestroyObject(ProjectileObject projectile) {
            Destroy(projectile.gameObject);
        }

#endregion

    }
}