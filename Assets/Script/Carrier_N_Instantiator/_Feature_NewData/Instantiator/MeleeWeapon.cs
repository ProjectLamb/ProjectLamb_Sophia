
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Feature_NewData
{
    /*
    is read에 대한것은 Statemachine으로 관리하기
    */
    public class MeleeWeapon : MonoBehaviour, IWeaponStatAccessable{
        private List<IObjectPool<Projectile>> ProjectilePool;
        private List<Projectile> creatableProjectile;
        private int createIndex = 0;
        private int currentProjectileIndex = 0;
        public int MaxPoolSize;
        public int bucketSize;

        private Stat MeleeRatio;
        private Stat AttackSpeed;

        private Player playerRef;

        public Stat GetWeaponRatioDamage() { return MeleeRatio;  }
        public Stat GetAttackSpeed() { return AttackSpeed;  }        

        private void Awake() {
            MeleeRatio = new Stat(1, 
                E_NUMERIC_STAT_TYPE.MeleeRatio,
                E_STAT_USE_TYPE.Ratio
            );
            AttackSpeed = new Stat(-1, 
                E_NUMERIC_STAT_TYPE.AttackSpeed,
                E_STAT_USE_TYPE.Ratio
            );     // ?? 어떤 수치를 가지고 공식은 어떻게 되야 하는걸까?
            ProjectilePool = new List<IObjectPool<Projectile>>(creatableProjectile.Count);
        }

        private void OnEnable() {
            while(createIndex < creatableProjectile.Count) {
                ProjectilePool[createIndex] = new ObjectPool<Projectile>(CreateProjectile, GetProjectile ,ReleaseProjectile, DestroyProjectile ,maxSize:MaxPoolSize);
                createIndex++;
            }
        }

        public void SetPlayer(Player player) {
            playerRef = player;
        }

        public void SetProjectiles(List<Projectile> projectiles) {
            creatableProjectile = new List<Projectile>(projectiles);
        }
        
        public void Use(){
            var projectile = ProjectilePool[currentProjectileIndex++ % createIndex].Get();
        }

        private Projectile CreateProjectile() {
            Projectile concreteProjectile = Instantiate(creatableProjectile[createIndex]);
            concreteProjectile.SetPool(ProjectilePool[createIndex]);
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
            return playerRef.GetStat(E_NUMERIC_STAT_TYPE.Power) * MeleeRatio;
        }
    }
} 