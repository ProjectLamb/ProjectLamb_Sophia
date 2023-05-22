using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VFXBucket : MonoBehaviour {
    public List<GameObject> VFXLists;
    public void VFXInstantiatorWithTime(GameObject obj,float lifeTime){
        GameObject vFX = Instantiate(obj, transform);
        vFX.transform.localScale *= transform.localScale.z;
        Destroy(vFX, lifeTime);
    }

    public void VFXInstantiatorWithTime(ParticleSystem particleSystem, float lifeTime){
        if(particleSystem == null) return;
        ParticleSystem particle = Instantiate(particleSystem, transform);
        particle.transform.localScale *= transform.localScale.z;
        if(particleSystem.transform.rotation.y < 0){
            particle.transform.position += transform.position.y * Vector3.down;
        }
    }    
    public void VFXInstantiator(GameObject obj){
        GameObject vFX = Instantiate(obj, transform);
        vFX.transform.localScale *= transform.localScale.z;
    }

    public void VFXInstantiator(ParticleSystem particleSystem){
        ParticleSystem particle = Instantiate(particleSystem, transform);
        particle.transform.localScale *= transform.localScale.z;
    }

    public void VFXDestroyer(){
        foreach(GameObject E in transform){
            Destroy(E);
        }
        //VFXLists.ForEach((E) => {Destroy(E);});
        //VFXLists.Clear();
    }
}