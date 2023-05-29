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

    public void Revert(E_StateType _type){
        skinModulator.RevertSkin();
        vfxModulator.VFXDestroyForce(_type);
    }
}
