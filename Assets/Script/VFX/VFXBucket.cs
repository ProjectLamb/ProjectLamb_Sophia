using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VFXBucket : MonoBehaviour {
    public Dictionary<E_StateType, VFXObject> visualStacks;
    private void Awake() {
        visualStacks = new Dictionary<E_StateType, VFXObject>();   
    }
    public void VFXInstantiator(VFXObject vfx){
        VFXObject vfxObject = Instantiate(vfx, transform);
        vfxObject.transform.localScale *= transform.localScale.z;
        //HandleNoneStacking(vfxObject);
        vfxObject.Initialize();
    }

    public void GameObjectInstantiator (GameObject obj){
        GameObject effectObject = Instantiate(obj, transform);
        effectObject.transform.localScale *= transform.localScale.z;
    }

    public void HandleStackingVFX(VFXObject vfxObject){
        
    }

    public void HandleNoneStacking(VFXObject vfxObject){
        if(visualStacks.ContainsKey(vfxObject.affectorType).Equals(false)) {
            visualStacks.Add(vfxObject.affectorType, vfxObject);
        }
        visualStacks[vfxObject.affectorType] = vfxObject;
    }
    public void VFXDestroyForce(E_StateType type){ 
        if(!visualStacks[type].Equals(null)) Destroy(visualStacks[type].gameObject);
    }
}