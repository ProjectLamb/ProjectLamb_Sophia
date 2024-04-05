using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모델의 비주얼적인 요소를 다루는 컴포넌트 <br/>
/// > Method : Interact ↔️ Revert <br/>
/// > Parameter : Material, VFXObject 
/// </summary>
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

    public void RevertByMaterial(STATE_TYPE _type){
        skinModulator.RevertSkin();
    }
    public void RevertByVFX(STATE_TYPE _type){
        vfxModulator.RevertVFX(_type);
    }
}
