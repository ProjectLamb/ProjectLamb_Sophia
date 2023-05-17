using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VFXBucket : MonoBehaviour {
    public void VFXInstantiator(GameObject obj){
        GameObject HitEffect = Instantiate(obj, transform);
        HitEffect.transform.localScale *= transform.localScale.z;
    }
}