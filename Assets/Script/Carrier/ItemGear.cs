using UnityEngine;

public class ItemGear : Carrier {
    // public E_CarrierType carrierType;
    // public List<EntityAffector> carrierEntityAffector;
    // public VFXObject destroyEffect = null;
    // protected Collider carrierCollider = null;
    // protected Rigidbody carrierRigidBody = null;
    // protected bool    isInitialized = false;
    
    public int num = 1;
    public float flowSpeed = 5;

    bool mIsTracking = false;
    void ActivateTracking(){mIsTracking = true;}
    
    protected override void Awake() {
        base.Awake();
        this.carrierType = E_CarrierType.Item;
    }
    private void Start(){
        Invoke("ActivateTracking", 1f);
    }

    // public virtual void Initialize(Entity _genOwner){
    //     if(_genOwner == null) {throw new System.Exception("투사체 생성 엔티티가 NULL임");}
    //     transform.localScale *= _genOwner.transform.localScale.x;
    //     this.isInitialized = true;
    // }

    private void OnTriggerEnter(Collider other) {
        if(!other.TryGetComponent<Player>(out Player player)){return;}
        PlayerDataManager.GetPlayerData().Gear += num;
        DestroySelf();
    }

    private void FixedUpdate() {
        if(mIsTracking) { FlowLerp(); }
    }
    private void FlowLerp(){
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x,GameManager.Instance.playerGameObject.transform.position.x, Time.deltaTime * flowSpeed),
            Mathf.Lerp(transform.position.y,GameManager.Instance.playerGameObject.transform.position.y, Time.deltaTime * flowSpeed),
            Mathf.Lerp(transform.position.z,GameManager.Instance.playerGameObject.transform.position.z, Time.deltaTime * flowSpeed)
        );
    }
}