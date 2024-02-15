using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia_Carriers
{
    public class BarrierProjectile : Projectile { //, IDamagable {
//      public      VFXObject       DestroyEffect       = null;
//      public      CARRIER_TYPE    CarrierType;
//      public      bool            IsInitialized       = false;
//      public      bool            IsActivated         = false;
//      public      bool            IsCloned         = false;
//      protected   Collider        carrierCollider     = null;
//      protected   Rigidbody       carrierRigidBody    = null;
//      [SerializeField] public VFXObject       HitEffect = null;
//      [SerializeField] public ParticleSystem  ProjectileParticle = null;
        public ParticleSystem.MainModule        particleModule;
//      [SerializeField] public int             ProjecttileDamage;
//      [SerializeField] public float           DestroyTime = 0.5f;
//      [SerializeField] public float           ColliderTime = 0.5f;
//      [SerializeField] private bool           isDestroyBySelf;
//      public Entity                           ownerEntity;
//      public List<EntityAffector>             projectileAffector;

        public int BarrierHealth;
        private UnityAction HitState;

        public override Carrier Clone()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Carrier res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }

        public override Projectile CloneProjectile()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Projectile res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }

        protected override void Awake()
        {
            base.Awake();
            particleModule = ProjectileParticle.main;
        }
        
        public override void Init(Entity _ownerEntity)
        {
            if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            if(_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
            ownerEntity = _ownerEntity;
            particleModule.startLifetime    = DestroyTime;
            particleModule.duration         = DestroyTime;
            IsInitialized = true;
        }

        /// <summary>
        /// 베리어의 체력을 설정한다.
        /// <param name="_ownerEntity"></param>
        /// <param name="_objects">0 : 보호막체력, 1 : 유지시간</param>
        /// </summary>
        public override void InitByObject(Entity _ownerEntity, object[] _objects)
        {
            if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            if(_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
            ownerEntity = _ownerEntity;
            this.DestroyTime = (float)_objects[0];
            this.BarrierHealth = (int)_objects[1];
            particleModule.startLifetime    = this.DestroyTime;
            particleModule.duration         = this.DestroyTime;
            IsInitialized = true;
        }

        private void OnEnable() {
            if(!IsInitialized) return;
            ProjectileParticle.Play();
            HitState = () => {WatchBarrierAmount();};
            GameManager.Instance.PlayerGameObject.GetComponent<Player>().Life.BarrierCoverd(BarrierHealth);
            PlayerDataManager.GetEntityData().HitState += HitState;
        }

//      public override void EnableSelf() {gameObject.SetActive(true); this.IsActivated = true; }
//      public override void DisableSelf() {gameObject.SetActive(false); this.IsActivated = false;}
//      public override void DestroySelf()  {
//          if(DestroyEffect != null){
//              Instantiate(DestroyEffect, transform.position, Quaternion.identity);
//          }
//          Destroy(gameObject);
//      }

//      public void GetDamaged(int _amount){
//          BarrierHealth -= _amount;
//          if (BarrierHealth <= 0) {this.DisableSelf();}
//      }
//      public void GetDamaged(int _amount, VFXObject _vfx){
//          GetDamaged(_amount);
//      }
        protected override void OnTriggerEnter(Collider _other) {return;}

        private void SetPlayerHitIgnore(ref int _amount){ Debug.Log("탱탱 막아버리기"); _amount = 0; }
        private void WatchBarrierAmount(){
            if(GameManager.Instance.PlayerGameObject.GetComponent<Player>().Life.CurrentBarrier <= 0){
                DisableSelf();
            }
        }

        private void OnDisable() {
            if(!IsInitialized) return;
            Debug.Log("베리어 비 활성화");
            PlayerDataManager.GetEntityData().HitState -= HitState;
            GameManager.Instance.PlayerGameObject.GetComponent<Player>().Life.SetBarrier(0);
        }
    }
}