
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Feature_NewData
{
    public class MeleeWeapon : MonoBehaviour{
        private List<IObjectPool<Projectile>> ProjectilePool;
        private List<Projectile> creatableProjectile;
        private int creteIndex = 0;
        public int MaxPoolSize;
        public int bucketSize;

        private Stat RatioMeleeDamage;
        private Stat AttackSpeed;

        private Player playerRef;
        

        private void Awake() {
            RatioMeleeDamage = new Stat(1, E_STAT_USE_TYPE.Ratio);
            AttackSpeed = new Stat(-1, E_STAT_USE_TYPE.Ratio);     // ?? 어떤 수치를 가지고 공식은 어떻게 되야 하는걸까?
        }

        private void OnEnable() {
            ProjectilePool = new List<IObjectPool<Projectile>>(creatableProjectile.Count);
            while(creteIndex < creatableProjectile.Count) {
                ProjectilePool.Add(new ObjectPool<Projectile>(CreateProjectile, GetProjectile ,ReleaseProjectile, DestroyProjectile ,maxSize:MaxPoolSize));
                creteIndex++;
            }
            creteIndex = 0;
        }

        public void SetPlayer(Player player) {
            playerRef = player;
        }

        public void SetProjectiles(List<Projectile> projectiles) {
            creatableProjectile = new List<Projectile>(projectiles);
        }
        
        public void Attack(){
            var projectile = ProjectilePool[(creteIndex++ % creatableProjectile.Count)].Get();
            
        }

        private Projectile CreateProjectile() {
            Projectile concreteProjectile = Instantiate(creatableProjectile[creteIndex]);
            concreteProjectile.SetPool(ProjectilePool[creteIndex]);
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

        private int CalculateDamage() {
            return playerRef.GetPower() * RatioMeleeDamage;
        }
    }
} 