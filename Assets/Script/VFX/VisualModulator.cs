using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualModulator : MonoBehaviour, IVisuallyInteractable
{
    public SkinModulator    skinModulator;
    public VFXBucket    vfxModulator;

    public void Interact(DebuffData debuffData){
        skinModulator.SetSkinSets(1, debuffData.entitySkin);
        vfxModulator.VFXInstantiatorWithTime(debuffData.particles, debuffData.durationTime);
    }
    
    public void Interact(ParticleSystem particleSystem){
        vfxModulator.VFXInstantiator(particleSystem);
    }

    public void Interact(GameObject obj) {
        vfxModulator.VFXInstantiator(obj);
    }

    public void Revert(){
        skinModulator.SetSkinSets(1);
        //vfxModulator.VFXDestroyer();
    }
}
