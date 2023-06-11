using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileBucket : MonoBehaviour {
    public void CarrierInstantiator(Entity _owner, Carrier projectile){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(projectile);
        Instantiate(projectile, position, forwardAngle).Initialize(_owner);
    }
    public void ProjectileInstantiator(Entity _owner, Projectile projectile){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(projectile);
        Instantiate(projectile, position, forwardAngle).Initialize(_owner);
    }
    public void ProjectileInstantiatorByDamage(Entity _owner, Projectile projectile, int _amount){
        Vector3     position     = transform.position;
        Quaternion  forwardAngle = GetForwardingAngle(projectile);
        Instantiate(projectile, position, forwardAngle).InitializeByDamage(_amount, _owner);
    }
    public Quaternion GetForwardingAngle(Carrier _useCarrier){
        return Quaternion.Euler(transform.eulerAngles + _useCarrier.transform.eulerAngles);
    }
}