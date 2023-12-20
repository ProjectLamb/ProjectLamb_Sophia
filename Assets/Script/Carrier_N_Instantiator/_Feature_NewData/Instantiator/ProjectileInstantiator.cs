using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Feature_NewData
{
    public abstract class ProjectileInstantiator : MonoBehaviour
    {
        private IObjectPool<Projectile> ProjectilePool;
        private List<Projectile> creatableProjectile;
        private int createIndex = 0;
        public int MaxPoolSize;
        public int bucketSize;

        private void OnEnable() {
            ProjectilePool = new ObjectPool<Projectile>(
                createFunc: CreateProjectile,
                actionOnGet: GetProjectile,
                actionOnRelease: ReleaseProjectile,
                actionOnDestroy: DestroyProjectile,
                maxSize: MaxPoolSize
            );
        }

        public void SetProjectiles(List<Projectile> projectiles) {
            creatableProjectile = new List<Projectile>(projectiles);
        }

        private Projectile CreateProjectile() {
            Projectile concreteProjectile = Instantiate(creatableProjectile[createIndex++ % creatableProjectile.Count]);
            concreteProjectile.SetPool(ProjectilePool);
            return concreteProjectile;
        }

        private void GetProjectile(Projectile projectile) {
            projectile.SetProjectileSize(bucketSize);
            projectile.SetForwardingAngle(this.transform.rotation);
            projectile.SetDamage(CalculateDamage());
            projectile.gameObject.SetActive(true);   
        }

        private void ReleaseProjectile(Projectile projectile) {
            projectile.gameObject.SetActive(false);   
        }

        private void DestroyProjectile(Projectile projectile) {
            Destroy(projectile.gameObject);
        }

        public abstract int CalculateDamage();
    }
}