using System;
using UnityEngine;
using Sophia_Carriers;

namespace Sophia_Carriers {
    public class ItemGear : Carrier {
//      public      VFXObject       DestroyEffect       = null;
//      public      CARRIER_TYPE    CarrierType;
//      public      BUCKET_POSITION BucketPosition;
//      public      bool            IsInitialized       = false;
//      public      bool            IsActivated         = false;
//      public      bool            IsCloned         = false;
//      protected   Collider        carrierCollider     = null;
//      protected   Rigidbody       carrierRigidBody    = null;
        
        [SerializeField] bool IsTracking = false;
        [SerializeField] private float flowSpeed = 5;
        public int num = 1;
        
        public override Carrier Clone()
        {
            if(this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Carrier res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }
    
        protected override void Awake() {
            base.Awake();
            this.CarrierType = CARRIER_TYPE.ITEM;
        }
    
        public override void Init(Entity _ownerEntity)
        {
            if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            IsInitialized = true;
        }
        
        public override void InitByObject(Entity _ownerEntity, object[] _objects)
        {
            if(IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            num = (int)_objects[0];
        }
    
        private void OnEnable() { Invoke("ActivateTracking", 1f); }
        
        // public override void EnableSelf() {gameObject.SetActive(true); this.IsActivated = true; }
        // public override void DisableSelf() {gameObject.SetActive(false); this.IsActivated = false;}
        // public override void DestroySelf()  {
        //     if(DestroyEffect != null){
        //         Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        //     }
        //     Destroy(gameObject);
        // }
    
        private void OnTriggerEnter(Collider other) {
            if(!other.TryGetComponent<Player>(out Player player)){return;}
            PlayerDataManager.GetPlayerData().Gear += num;
            DestroySelf();
        }
    
        private void FixedUpdate() {
            if(IsTracking) { FlowLerp(); }
        }
        private void ActivateTracking(){IsTracking = true;}
    
        private void FlowLerp(){
            transform.position = Vector3.Lerp(transform.position, GameManager.Instance.PlayerGameObject.transform.position, Time.deltaTime * flowSpeed);
        }
    }
}