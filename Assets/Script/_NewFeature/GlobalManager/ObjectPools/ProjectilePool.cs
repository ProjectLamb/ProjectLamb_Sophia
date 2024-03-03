using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Sophia
{
    using System;
    using Entitys;
    using Instantiates;
    
    public class ProjectilePool : MonoBehaviour {
        private static ProjectilePool _instance;

        public static ProjectilePool Instance {
            get {
                if(_instance == null) {
                    _instance = FindFirstObjectByType(typeof(ProjectilePool)) as ProjectilePool;
                    if(_instance == null) Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set {}
        }

        public static ProjectileObject GetObject(ProjectileObject projectileReferernce) {
            return Instance.ProPool[projectileReferernce.ProjectileID].Get();
        }

        [SerializeField] private List<ProjectileObject> _creatableProjectiles;
        public IObjectPool<ProjectileObject>[] ProPool {get; private set;}
        private List<Func<ProjectileObject>> OnCreateFuncs = new();
        public int MaxPoolSize;

        private void Awake() {
            ProPool = new IObjectPool<ProjectileObject>[_creatableProjectiles.Count];
            for(int i = 0 ; i < _creatableProjectiles.Count; i++) {
                var idCount = i;
                _creatableProjectiles[idCount].SetIndex(idCount);
                OnCreateFuncs.Add(() => {
                    ProjectileObject concrete = Instantiate(_creatableProjectiles[idCount]);
                    concrete.SetByPool(this.ProPool[_creatableProjectiles[idCount].ProjectileID]);
                    return concrete;
                });
            }
            
        }
        
        /*⚠️ Instantiate(E)라면.. 같이 달린 컴포넌트도 같이 생성되나? 이거 버그 조심할것*/

        private void Start() {
            for(int i = 0; i < _creatableProjectiles.Count; i++) {
                ProPool[_creatableProjectiles[i].ProjectileID] = new ObjectPool<ProjectileObject>(
                                                                    createFunc: OnCreateFuncs[i],
                                                                    actionOnGet: OnGetObject,
                                                                    actionOnRelease: OnReleaseObject,
                                                                    actionOnDestroy: OnDestroyObject,
                                                                    maxSize: MaxPoolSize
                                                                );
            }
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