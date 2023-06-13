using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileBucket : MonoBehaviour {
    public void TransformPositionning(Entity _owner, Transform _obj, E_BucketPosition _type){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_obj);
        switch (_type)
        {
            case E_BucketPosition.Inner   :
                _obj.parent = transform;
                _obj.rotation = forwardAngle;
                break;
            case E_BucketPosition.Outer  :
                _obj.position = transform.position;
                _obj.rotation = forwardAngle;
                break;
        }
    }
    public void GameObjectInstantiator(Entity _owner, GameObject _go, E_BucketPosition _type){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_go.transform);
        switch (_type)
        {
            case E_BucketPosition.Inner   :
                Instantiate(_go, transform);
                break;
            case E_BucketPosition.Outer  :
                Instantiate(_go, position, forwardAngle);
                break;
        }
    }

    public void ProjectileInstantiator(Entity _owner, Projectile _projectile, E_BucketPosition _type){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_projectile.transform);
        switch (_type)
        {
            case E_BucketPosition.Inner   :
                Instantiate(_projectile, transform).Initialize(_owner);
                break;
            case E_BucketPosition.Outer  :
                Instantiate(_projectile, position, forwardAngle).Initialize(_owner);
                break;
        }
    }
    public void ProjectileInstantiatorByDamage(Entity _owner, Projectile _projectile, E_BucketPosition _type, int _amount){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(_projectile.transform);
        switch (_type)
        {
            case E_BucketPosition.Inner   :
                Instantiate(_projectile, transform).InitializeByDamage(_owner, _amount);
                break;
            case E_BucketPosition.Outer  :
                Instantiate(_projectile, position, forwardAngle).InitializeByDamage(_owner, _amount);
                break;
        }
    }
    public Quaternion GetForwardingAngle(Transform _useCarrier){
        return Quaternion.Euler(transform.eulerAngles + _useCarrier.transform.eulerAngles);
    }
}