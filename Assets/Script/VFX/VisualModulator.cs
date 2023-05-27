using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualModulator : MonoBehaviour, IVisuallyInteractable
{
    public SkinModulator    skinModulator;
    public VFXBucket    vfxModulator;

    public void InteractByMaterial(Material skin, float durationTime){
        skinModulator.SetSkinSets(1, skin);
    }
    
    public void InteractByParticle(ParticleSystem particle, float durationTime){
        vfxModulator.VFXInstantiator(particle);
    }

    public void Interact(GameObject obj) {
        vfxModulator.VFXInstantiator(obj);
    }

    public void Revert(){
        skinModulator.SetSkinSets(1);
        //vfxModulator.VFXDestroyer();
    }
}
