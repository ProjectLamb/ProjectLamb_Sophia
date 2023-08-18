using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sophia_Carriers;

    public class ItemEquipment : Carrier
    {
        //      public      VFXObject       DestroyEffect       = null;
        //      public      CARRIER_TYPE    CarrierType;
        //      public      BUCKET_POSITION BucketPosition;
        //      public      bool            IsInitialized       = false;
        //      public      bool            IsActivated         = false;
        //      public      bool            IsCloned         = false;
        //      protected   Collider        carrierCollider     = null;
        //      protected   Rigidbody       carrierRigidBody    = null;
        public AbstractEquipment equipment;
        public override Carrier Clone()
        {
            if (this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Carrier res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }

        protected override void Awake()
        {
            base.Awake();
            this.CarrierType = CARRIER_TYPE.ITEM;
        }

        public override void Init(Entity _ownerEntity)
        {
            if (IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            if (_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
            IsInitialized = true;
        }
        public override void InitByObject(Entity _ownerEntity, object[] _objects)
        {
            Init(_ownerEntity);
        }
        // public override void EnableSelf() {gameObject.SetActive(true); this.IsActivated = true; }
        // public override void DisableSelf() {gameObject.SetActive(false); this.IsActivated = false;}
        // public override void DestroySelf()  {
        //     if(DestroyEffect != null){
        //         Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        //     }
        //     Destroy(gameObject);
        // }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Player>(out Player player)) { return; }
            if(transform.TryGetComponent(out PurchaseComponent pc)) {
                if(!pc.Purchase()) {return;}
            }
            player.equipmentManager.Equip(this.equipment);
            Debug.Log($"장비 장착! : {equipment.equipmentName}");
            DestroySelf();
        }
    }