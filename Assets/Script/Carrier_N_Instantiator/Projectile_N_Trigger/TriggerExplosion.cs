using System.Collections.Generic;
using UnityEngine;
namespace Sophia_Carriers {
public class TriggerExplosion : Trigger {
//      public      VFXObject           DestroyEffect       = null;
//      public      CARRIER_TYPE        CarrierType;
//      public      BUCKET_POSITION     BucketPosition;
//      public      bool                IsInitialized       = false;
//      public      bool                IsActivated         = false;
//      public      bool                IsCloned         = false;
//      protected   Collider            carrierCollider     = null;
//      protected   Rigidbody           carrierRigidBody    = null;
//      [SerializeField] public float   DestroyTime = 0.5f;
//      [SerializeField] public float   ColliderTime = 0.5f;
//      [SerializeField] private bool   isDestroyBySelf;
//      public List<EntityAffector>     triggerAffectors;
        public      Vector3             targetScale = new Vector3(300f, 300f,300f);

        public TriggerExplosion CloneTriggerExplosion()
        {
            if(this.IsCloned == true) throw new System.Exception("복제본이 복제본을 만들 수 는 없다.");
            TriggerExplosion res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }
        private void GrowLerp(){
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime);
        }
        private void FixedUpdate() {
            GrowLerp();
        }


        //0.25  초 안에 300정도의 사이즈 만큼 커져야한다.
    
        protected override void OnEnable(){
            if (isDestroyBySelf == true) Invoke("DestroySelf", DestroyTime);
            Invoke("ColliderDisenabled", ColliderTime);
        }
        //protected void ColliderDisenabled() { this.carrierCollider.enabled = false; }
        // public override void DestroySelf()  {
        //     if(DestroyEffect != null){
        //         Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        //     }
        //     Destroy(gameObject);
        // }
        protected override void OnTriggerEnter(Collider _other) {
            CheckException();
            Entity targetEntity = GetComponent<Entity>();
            if(!CheckIsOwnerCollider(_other)) {
                ConveyAffectorToTarget(_other);
            }
        }
    }
}