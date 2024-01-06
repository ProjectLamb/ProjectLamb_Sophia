using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Feature_NewData
{   
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

        [SerializeField] private List<Projectile> _creatableProjectiles;
        public Dictionary<string, IObjectPool<Projectile>> ProPool {get; private set;}
        public int MaxPoolSize;

        private void Awake() {
            ProPool = new Dictionary<string, IObjectPool<Projectile>>();
        }
        private void Start() {
            _creatableProjectiles.ForEach((E) => {
                ProPool.Add(E.gameObject.name, null);

                ProPool[E.gameObject.name] = new ObjectPool<Projectile>(
                    createFunc: () => {
                        Projectile concrete = Instantiate(E);
                        concrete.SetPool(this.ProPool[E.gameObject.name]);
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

        private void OnGetObject(Projectile projectile) {
            projectile.transform.parent = null;
            projectile.Get();
        }

        private void OnReleaseObject(Projectile projectile) {
            projectile.Release();
            projectile.transform.SetParent(transform);
        }

        private void OnDestroyObject(Projectile projectile) {
            Destroy(projectile.gameObject);
        }

#endregion

    }
}