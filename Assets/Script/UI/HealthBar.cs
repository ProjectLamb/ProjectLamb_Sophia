using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EntityData entityData;
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    private void Awake() {
        fill.color = gradient.Evaluate(1f);
        entityData = GetComponentInParent<IEntityAddressable>().GetEntityData();
        if(entityData != null) {Debug.Log($"{entityData}");}
    }

    private void Start(){
        entityData.HitState += SetSlider;
        Debug.Log("Tagged");
    }

    public void SetSlider(){
        slider.value = (((float)entityData.CurHP / (float)entityData.MaxHP) * slider.maxValue);
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //slider.value = ((float)sandbag.sandbagData.CurHP / sandbag.sandbagData.MaxHP) * slider.maxValue;
    }
}
