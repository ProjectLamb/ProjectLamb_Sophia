using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sophia_Carriers{
    /// <summary>
    /// 엔티티와 엔티티간의 데이터 교환은 무조건 Carrier를 컴포넌트로 가지는 오브젝트만 가능하다.</br>
    /// 특히 OnTrigger을 통해서 데이터 교환을 하게 된다.
    /// </summary>
    public abstract class Carrier : MonoBehaviour {
        public      VFXObject       DestroyEffect       = null;
        public      CARRIER_TYPE    CarrierType;
        public      BUCKET_POSITION BucketPosition;
        public      bool            IsInitialized       = false;
        public      bool            IsActivated         = false;
        public      bool            IsCloned         = false;
        protected   Collider        carrierCollider     = null;
        public      Entity          ownerEntity;
        
        public abstract Carrier Clone();
        
        protected virtual void Awake() {
            TryGetComponent<Collider>(out carrierCollider);
        }

        public abstract void Init(Entity _ownerEntity);
        public abstract void InitByObject(Entity _ownerEntity, object[] _objects);

        //public virtual void Initialize(Entity _genOwner){
        //    if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
        //    this.isInitialized = true;
        //}
        public virtual void  SetScale(float _sizeRatio){transform.localScale *= _sizeRatio;}
        public virtual void EnableSelf() {gameObject.SetActive(true); this.IsActivated = true; }
        public virtual void DisableSelf() {gameObject.SetActive(false); this.IsActivated = false;}
        public virtual void DestroySelf()  {
            if(DestroyEffect != null){
                Instantiate(DestroyEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        public virtual Entity GetOwner(){return this.ownerEntity;}
    }
}