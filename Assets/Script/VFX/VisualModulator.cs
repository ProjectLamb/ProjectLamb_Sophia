using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VisualModulator : MonoBehaviour
{
    public SkinModulator    skinModulator;
    public VFXBucket    vfxModulator;

    public void InteractByMaterial(Material skin){
        skinModulator.ChangeSkin(skin);
    }
    
    public void InteractByVFX(VFXObject vfx){
        vfxModulator.VFXInstantiator(vfx);
    }

    public void InteractByGameObject(GameObject obj){
        vfxModulator.GameObjectInstantiator(obj);
    }

    public void RevertByMaterial(E_StateType _type){
        skinModulator.RevertSkin();
    }
    public void RevertByVFX(E_StateType _type){
        vfxModulator.VFXDestroyForce(_type);
    }
}
