using UnityEngine;

public class Barrier : Carrier {
//  public E_CarrierType carrierType;
//  public    VFXObject destroyEffect = null;
//  protected Collider  carrierCollider = null;
//  protected Rigidbody carrierRigidBody = null;
//  protected bool      isInitialized = false;
    public int barrierHP;
    private void Awake() {
        base.Awake();
    }

//  protected virtual void Awake() {
//      TryGetComponent<Collider>(out carrierCollider);
//      TryGetComponent<Rigidbody>(out carrierRigidBody);
//  }

//  public virtual void Initialize(Entity _genOwner){
//      if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
//      transform.localScale *= _genOwner.transform.localScale.x;
//      this.isInitialized = true;
//  }

//  public void DestroySelf(){
//      if(destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity).Initialize();
//      Destroy(gameObject);
//  }
}