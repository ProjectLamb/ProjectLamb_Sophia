```cs
public enum BUCKET_STACKING_TYPE { NONE_STACK, STACK }
public enum BUCKET_POSITION { INNER, OUTER }

public class VFXBucket : MonoBehaviour {
    public Dictionary<STATE_TYPE, VFXObject>    VisualStacks = new Dictionary<STATE_TYPE, VFXObject>();
    public UnityAction<STATE_TYPE>              OnDestroyHandler;
    public float BucketScale = 1;
    private void Awake() {
        InitializeVisualStacksByEnums(Enum.GetValues(typeof(STATE_TYPE)));
        OnDestroyHandler = (STATE_TYPE type) => {RemoveStateByType(type);};
    }

    private void InitializeVisualStacksByEnums(Array _stateTypes){
        foreach(STATE_TYPE E in _stateTypes){ VisualStacks.Add(E, null); }
    }

    private void RemoveStateByType(STATE_TYPE _type){ VisualStacks[_type] = null; }

    public void VFXInstantiator(VFXObject _vfx){
        switch(_vfx.BucketStaking){
            case BUCKET_STACKING_TYPE.NONE_STACK : 
            {
                STATE_TYPE stateType = _vfx.AffectorType;
                if(VisualStacks.TryGetValue(stateType, out VFXObject value)){
                    //null이 아니라면 더 쌓을 수 없으므로 리턴
                    if(value != null){return;}
                }

                VFXObject vfxObject = Instantiate(_vfx, transform);
                vfxObject.OnDestroyActionByState += OnDestroyHandler;
                vfxObject.SetScale(BucketScale);
                VisualStacks[stateType] = vfxObject;
                VisualStacks[stateType].DestroyVFX();
                break;
            }
            case BUCKET_STACKING_TYPE.STACK : 
            {
                VFXObject vfxObject = Instantiate(_vfx, transform);
                vfxObject.SetScale(BucketScale);
                vfxObject.DestroyVFX();
                break;
            }
        }
    }
    public GameObject VFXGameObjectInstantiator(GameObject _obj){
        //재작성하기
        return null;
    }

    public void RevertVFX(STATE_TYPE _stateType){
        if(VisualStacks.TryGetValue(_stateType, out VFXObject value)){
            if(value == null) {return;}
            VisualStacks[_stateType].DestroyVFXForce();
            VisualStacks[_stateType] = null;
        }
    }
    public Quaternion GetForwardingAngle(Transform _useCarrier){
        return Quaternion.Euler(transform.eulerAngles + _useCarrier.transform.eulerAngles);
    }
}
```

```cs
public class CarrierBucket : MonoBehaviour {
    public float BucketScale = 1f;
    public void CarrierTransformPositionning(GameObject _owner, Carrier _carrier){
        Vector3     offset       = _carrier.transform.position;
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_carrier.transform);
        _carrier.transform.position = position;
        _carrier.transform.rotation = forwardAngle;
        switch (_carrier.BucketPosition)
        {
            case BUCKET_POSITION.INNER   :
            {
                _carrier.transform.SetParent(transform);
                _carrier.SetScale(BucketScale);
                _carrier.transform.position += offset * _owner.transform.localScale.z;
                _carrier.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  :
            {
                _carrier.SetScale(BucketScale);
                _carrier.transform.position += offset * _owner.transform.localScale.z;
                _carrier.EnableSelf();
                break;
            }
        }
    }

    public void CarrierTransformPositionning(Entity _owner, Carrier _carrier){
        Vector3     offset       = _carrier.transform.position;
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_carrier.transform);
        _carrier.transform.position = position;
        _carrier.transform.rotation = forwardAngle;
        switch (_carrier.BucketPosition)
        {
            case BUCKET_POSITION.INNER   :
            {
                _carrier.transform.SetParent(transform);
                _carrier.SetScale(BucketScale);
                _carrier.transform.position += offset * _owner.transform.localScale.z;
                _carrier.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  :
            {
                _carrier.SetScale(BucketScale);
                _carrier.transform.position += offset * _owner.transform.localScale.z;
                _carrier.EnableSelf();
                break;
            }
        }
    }

    public GameObject GameObjectInstantiator(Entity _owner, GameObject _go){
        //재작성하기
        return null;
    }

    public Carrier CarrierInstantiator(Entity _owner, Carrier _carrier)
    {
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_carrier.transform);
        Carrier carrierInstant = _carrier.Clone();
        carrierInstant.Init(_owner);
        carrierInstant.transform.position = position;
        carrierInstant.transform.rotation = forwardAngle;
        switch (_carrier.BucketPosition)
        {
            case BUCKET_POSITION.INNER   :
            {
                carrierInstant.transform.SetParent(transform);
                carrierInstant.SetScale(BucketScale);
                carrierInstant.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  : 
            {
                carrierInstant.SetScale(BucketScale);
                carrierInstant.EnableSelf();
                break;
            }
        }
        return carrierInstant;
    }

    public Carrier CarrierInstantiatorByObjects(Entity _owner, Carrier _carrier, object[] _objects){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_carrier.transform);
        Carrier carrierInstant = _carrier.Clone();
        carrierInstant.InitByObject(_owner, _objects);
        carrierInstant.transform.position = position;
        carrierInstant.transform.rotation = forwardAngle;
        switch (_carrier.BucketPosition)
        {
            case BUCKET_POSITION.INNER   :
            {
                carrierInstant.transform.SetParent(transform);
                carrierInstant.SetScale(BucketScale);
                carrierInstant.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  :
            {
                carrierInstant.SetScale(BucketScale);
                carrierInstant.EnableSelf();
                break;
            }
        }
        return carrierInstant;
    }    
    public Quaternion GetForwardingAngle(Transform _useCarrier){
        Debug.Log(Quaternion.Euler(transform.eulerAngles + _useCarrier.transform.eulerAngles).eulerAngles);
        return Quaternion.Euler(transform.eulerAngles + _useCarrier.transform.eulerAngles);
    }
    
}
```

```cs

/*
인스턴시에이터의 역할은 그저 트랜스폼 지정된 위치에서 Carrier나 VFX 가 생기게 하는 역할이다.

1. Inner과 Outer의 방식으로 생성 가능한데.
    Inner은 Bucket의 Transform의 자식으로 들어가게끔
    Outer은 Bucket의 Transform.position으로 생성되게끔 하는것이다.
        대표적으로 그브 궁에 맞았을때 
        Entity 피격위치 바로 정 반대편에 VFX가 생기지만, 
        그, Entity가 Rotate할시 Inner 같은경우 VFX도 큼지막하게 회전을 하면 안될것이다.
    
            public void VFXInstantiator(VFXObject _vfx){} 
            public Carrier CarrierInstantiator(Entity _owner, Carrier _carrier) {}
            public Carrier CarrierInstantiatorByObjects(Entity _owner, Carrier _carrier, object[] _objects){}    

            public Quaternion GetForwardingAngle(Transform _useCarrier) => 
        Quaternion.Euler(transform.eulerAngles + Carriers.transform.eulerAngles);
            public Quaternion GetForwardingAngle(Transform _useCarrier) =>
        Quaternion.Euler(transform.eulerAngles + Carriers.transform.eulerAngles)


2. 다만, VFX같은 경우, 이펙트의 중복이 되면 안되는 경우도 있기 때문에, 
기본은 Nonstackable이겠지만, Stacking 한 경우를 대비하게끔 해야한다.
따라서 생성한 
    "Nonstacking & Stacking 방식의 인스턴시에이트"
        private void InitializeVisualStacksByEnums(Array _stateTypes){ }

    "Carrier과 VFX의 Ref를 참조할 수는 있어야 한다.."
    "그리고 중복이 되는지 안되는지는 Ref를 참조한다"
            public Dictionary<STATE_TYPE, VFXObject> VisualStacks = new Dictionary<STATE_TYPE, VFXObject>();
            public UnityAction<STATE_TYPE>           OnDestroyHandler;

    "그에 따라서 생성과, 파괴의 역할을 수행할 수 있다는것"
            private void RemoveStateByType(STATE_TYPE _type){}
            public void RevertVFX(STATE_TYPE _stateType){}

    Carrier 같은 경우 : Spline을 따라서 복귀가 가능할 수 있겠고,
    탈론 W와 같이 돌아올 수도 있음.

정리하면
    생성 : Inner & Outer, NonStacking & Stacking 
    파괴 : Ref를 참조해서 파괴 가능
        Async & Sync 방식으로.

3.  버켓은 스텟을 가져야 한다.
    어떤, Weapon이던, 어떤 Skill 이던지간에 항상 Native한, Bucket이 되야 한다.
            public float BucketScale = 1;
            public float BucketScale = 1f;
*/

public enum BUCKET_STACKING_TYPE { NONE_STACK, STACK }
public enum BUCKET_POSITION { INNER, OUTER }

public class VFXBucket : MonoBehaviour {
    public Dictionary<STATE_TYPE, VFXObject>    VisualStacks = new Dictionary<STATE_TYPE, VFXObject>();
    public UnityAction<STATE_TYPE>              OnDestroyHandler;
    
    public float BucketScale = 1;
    public float BucketScale = 1f;
    
    private void Awake() {throw new System.NotImplementedException();}
    
    private void RemoveStateByType(STATE_TYPE _type){}
    public void RevertVFX(STATE_TYPE _stateType){}

    private void InitializeVisualStacksByEnums(Array _stateTypes){ }
 
    public void CarrierTransformPositionning(GameObject _owner, Carrier _carrier){}
    public void CarrierTransformPositionning(Entity _owner, Carrier _carrier){}
 
    public GameObject GameObjectInstantiator(Entity _owner, GameObject _go){}
    public GameObject VFXGameObjectInstantiator(GameObject _obj) {}

    public void VFXInstantiator(VFXObject _vfx){} 
    public Carrier CarrierInstantiator(Entity _owner, Carrier _carrier) {}
    public Carrier CarrierInstantiatorByObjects(Entity _owner, Carrier _carrier, object[] _objects){}    
    public Quaternion GetForwardingAngle(Transform _useCarrier){}
    public Quaternion GetForwardingAngle(Transform _useCarrier){}
}

```

```cs
Projectile onHitProjectile = _projectile.CloneProjectile();
Projectile onHitProjectile = _projectile.CloneProjectile();
useProjectile = E.CloneProjectile();
useProjectile = ProjectileE.CloneProjectile();
useProjectile = ProjectileE.CloneProjectile();
useProjectile = circleProjectile.CloneProjectile();
useProjectile = moveProjectileShort.CloneProjectile();
TriggerExplosion useTrigger = TriggerE.CloneTriggerExplosion();
TriggerExplosion useTrigger = TriggerQ.CloneTriggerExplosion();
TriggerExplosion useTrigger = TriggerE.CloneTriggerExplosion();
Projectile useProjectile = AttackProjectiles[currentProjectileIndex++].CloneProjectile();
Projectile useProjectile = AttackProjectiles[0].CloneProjectile();
carrier = carrier.Clone();

Projectile useProjectile = OnHitProjectiles.Dequeue();

useTrigger.Init(player);
useProjectile.Init(player);
useProjectile.Init(player);
useProjectile.Init(player);
useProjectile.Init(player);
useProjectile.Init(player);
useTrigger.Init(player);
useTrigger.Init(player);
useProjectile.Init(ownerEntity);
useProjectile.Init(ownerEntity);
useProjectile.Init(ownerEntity);
carrier.InitByObject(null, new object[] { 30 });

onHitProjectile.ProjecttileDamage = Ratio;
onHitProjectile.ProjecttileDamage = Ratio;
useProjectile.ProjecttileDamage = skillDamage;
useProjectile.ProjecttileDamage = skillDamage;
useProjectile.ProjecttileDamage = skillDamage;
useProjectile.ProjecttileDamage = skillDamage;
useProjectile.ProjecttileDamage = skillDamage;
useProjectile.ProjecttileDamage = _amount * PlayerDataManager.GetWeaonData().DamageRatio;
useProjectile.ProjecttileDamage = _amount * useProjectile.ProjecttileDamage * PlayerDataManager.GetWeaonData().DamageRatio;
useProjectile.ProjecttileDamage = _amount * PlayerDataManager.GetWeaonData().DamageRatio;

useTrigger.triggerAffectors.Add(pullState);
useTrigger.triggerAffectors.Add(pullState);
useTrigger.triggerAffectors.Add(pushState);
useTrigger.triggerAffectors.Add(pushState);

pushState.DurationTime = DurationE[(int)skillRank];
pullState.DurationTime = DurationQ[(int)skillRank];
pushState.DurationTime = DurationE[(int)skillRank];

pushState.Direction = 0.5f;
pullState.Direction = -0.5f;
pushState.Direction = 0.5f;

player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
player.carrierBucket.CarrierTransformPositionning(player, useTrigger);
projectileBucket.CarrierTransformPositionning(ownerEntity, useProjectile);
projectileBucket.CarrierTransformPositionning(ownerEntity, useProjectile);
projectileBucket.CarrierTransformPositionning(ownerEntity, useProjectile);
carrierBucket.CarrierTransformPositionning(gameObject, carrier);
```

```cs
/*
캐리어 파이프라인
Cloner와 Instantiator
*/

Projectile.Clone();
Projectile.Init();

Projectile.Setter;
Projectile.AffecterSetter

Projectile.CarrierTransformPositionning();
``````