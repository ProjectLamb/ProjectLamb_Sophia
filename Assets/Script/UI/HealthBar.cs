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

    IEntityAddressable entityAddressable;
    private void Awake() {
        entityAddressable = GetComponentInParent<IEntityAddressable>();
        fill.color = gradient.Evaluate(1f);
    }

    private void Start(){
        entityData = entityAddressable.GetEntityData();
        //addingData = entityAddressable.GetAddingData();
        entityData.UIAffectState += SetSlider;        
    }

    private void Update() {
        SetSlider();
    }

    public void SetSlider(){
        slider.value = (((float)entityData.CurHP / (float)entityData.MaxHP) * slider.maxValue);
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //slider.value = ((float)sandbag.sandbagData.CurHP / sandbag.sandbagData.MaxHP) * slider.maxValue;
    }
}
