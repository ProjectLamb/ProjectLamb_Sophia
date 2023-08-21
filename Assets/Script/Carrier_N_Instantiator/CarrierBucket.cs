using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

public class CarrierBucket : MonoBehaviour {    
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
                _carrier.SetScale(_owner.transform.localScale.z);
                _carrier.transform.position += offset * _owner.transform.localScale.z;
                _carrier.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  :
            {
                _carrier.SetScale(_owner.transform.localScale.z);
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
                _carrier.SetScale(_owner.transform.localScale.z);
                _carrier.transform.position += offset * _owner.transform.localScale.z;
                _carrier.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  :
            {
                _carrier.SetScale(_owner.transform.localScale.z);
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
                carrierInstant.SetScale(_owner.transform.localScale.z);
                carrierInstant.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  : 
            {
                carrierInstant.SetScale(_owner.transform.localScale.z);
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
                carrierInstant.SetScale(_owner.transform.localScale.z);
                carrierInstant.EnableSelf();
                break;
            }
            case BUCKET_POSITION.OUTER  :
            {
                carrierInstant.SetScale(_owner.transform.localScale.z);
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