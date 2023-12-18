using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;
using Sophia_Carriers;
using Microsoft.SqlServer.Server;

namespace Feature_NewData
{
    public class Projectile : MonoBehaviour{
        private Stat DurateLifeTime;
        private Stat RatioSize;
        private Stat ForwardingSpeed;

        public int Damage;

        public void SetDamage(int damage) { this.Damage = damage; }

        public void SetProjectileSize(int instantiatorBucketSize) {
            this.transform.localScale *= (RatioSize * instantiatorBucketSize);
        }

        public void SetForwardingAngle(Quaternion instantiatorAngle) {
            this.transform.rotation = Quaternion.Euler(instantiatorAngle.eulerAngles + this.transform.eulerAngles);
        }
        public void OnDisable() {
            OnBecameInvisible();
        }

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<IDamagable>(out IDamagable damagables)) {
                damagables.GetDamaged(Damage);
            }
        }

        private void Update(){
            transform.Translate(Vector3.forward * ForwardingSpeed * Time.deltaTime);
        }


        private IObjectPool<Projectile> poolRefer {get; set;}
        public void SetPool(IObjectPool<Projectile> pool) {
            poolRefer = pool;
        }

        private void OnBecameInvisible() {
            poolRefer.Release(this);
            this.transform.localScale = Vector3.one;
            this.transform.rotation = Quaternion.identity;
            this.Damage = 0;
        }
    }
}