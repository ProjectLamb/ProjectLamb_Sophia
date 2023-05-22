using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileBucket : MonoBehaviour {
    public Sandbag sandbag;
    
    private void Awake() {
        sandbag = GetComponentInParent<Sandbag>();
    }
    
    public void ProjectileInstantiator(Projectile projectile, E_ProjectileType type){
        Destroy(projectile.InstanciateProjectile(sandbag.GetEntityData(), sandbag.GetAddingData(), type, transform), 0.6f);
    }
}