using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileBucket : MonoBehaviour {
    public Sandbag sandbag;
    
    private void Awake() {
        sandbag = GetComponentInParent<Sandbag>();
    }
    
    public void ProjectileInstantiator(GameObject obj){
        GameObject projectile = Instantiate(obj, transform);
        Destroy(projectile, 0.6f);
    }
}