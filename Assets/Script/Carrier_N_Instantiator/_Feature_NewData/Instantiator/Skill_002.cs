using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Feature_NewData
{
    public class Skill_002 : MonoBehaviour, ISkillStatAccessable {
        private Player playerRef;
        public int MaxPoolSize;
        public int bucketSize;
        public SKILL_RANK skillRank;


        private IObjectPool<Projectile> ProjectilePoolQ;
        private IObjectPool<Projectile> ProjectilePoolE;
        private IObjectPool<Projectile> ProjectilePoolR;
        private Projectile creatableProjectile;
        
        
        public readonly List<float> DamageRatioQByRank = new List<float> {0.8f, 1f, 1.5f};
        public readonly List<float> DamageRatioEByRank = new List<float> {0.8f, 1f, 1.5f};
        public readonly List<float> DamageRatioRByRank = new List<float> {1.2f, 1.5f, 2f};

        private Stat SkillEffectMultiplyer;
        public Stat GetSkillEffectMultiplyer() { return this.SkillEffectMultiplyer;}

        private void Awake() {
            SkillEffectMultiplyer = new Stat(1, E_STAT_USE_TYPE.Ratio);
        }

        private void OnEnable() {
            ProjectilePoolQ = new ObjectPool<Projectile>(
                () => {
                    Projectile concreteProjectile = Instantiate(creatableProjectile);
                    concreteProjectile.SetPool(ProjectilePoolQ);
                    return concreteProjectile;
                },
                (Projectile projectile) => {
                    projectile.SetProjectileSize(bucketSize);
                    projectile.SetForwardingAngle(this.transform.rotation);
                    projectile.SetDamage(CalculateDamage(SKILL_KEY.Q, skillRank));
                    projectile.gameObject.SetActive(true); 
                },
                ReleaseProjectile,
                DestroyProjectile,
                maxSize:MaxPoolSize
            );
            ProjectilePoolE = new ObjectPool<Projectile>(
                () => {
                    Projectile concreteProjectile = Instantiate(creatableProjectile);
                    concreteProjectile.SetPool(ProjectilePoolE);
                    return concreteProjectile;
                },
                (Projectile projectile) => {
                    projectile.SetProjectileSize(bucketSize);
                    projectile.SetForwardingAngle(this.transform.rotation);
                    projectile.SetDamage(CalculateDamage(SKILL_KEY.E, skillRank));
                    projectile.gameObject.SetActive(true); 
                },
                ReleaseProjectile,
                DestroyProjectile,
                maxSize:MaxPoolSize
            );
            
            ProjectilePoolR = new ObjectPool<Projectile>(
                () => {
                    Projectile concreteProjectile = Instantiate(creatableProjectile);
                    concreteProjectile.SetPool(ProjectilePoolR);
                    return concreteProjectile;
                },
                (Projectile projectile) => {
                    projectile.SetProjectileSize(bucketSize);
                    projectile.SetForwardingAngle(this.transform.rotation);
                    projectile.SetDamage(CalculateDamage(SKILL_KEY.R, skillRank));
                    projectile.gameObject.SetActive(true); 
                },
                ReleaseProjectile,
                DestroyProjectile,
                maxSize:MaxPoolSize
            );
        }

        public void UseQ(){
            var projectile = ProjectilePoolQ.Get();
        }
        public void UseE(){
            var projectile = ProjectilePoolE.Get();
        }
        public void UseR(){
            var projectile = ProjectilePoolR.Get();
        }


        private int CalculateDamage(SKILL_KEY key, SKILL_RANK rank)
        {
            float resByKey = -1;
            switch (key)
            {
                case SKILL_KEY.Q: {resByKey = DamageRatioQByRank[(int)rank]; break;}
                case SKILL_KEY.E: {resByKey = DamageRatioEByRank[(int)rank]; break;}
                case SKILL_KEY.R: {resByKey = DamageRatioRByRank[(int)rank]; break;}
            }
            resByKey *= playerRef.GetPower() * playerRef.GetWeaponRatioDamage();
            return (int)resByKey;
        }

        private void ReleaseProjectile(Projectile projectile) {
            projectile.gameObject.SetActive(false);   
        }

        private void DestroyProjectile(Projectile projectile) {
            Destroy(projectile.gameObject);
        }
    }
}