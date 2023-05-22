using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EntityData entityData;
    public AddingData addingData;
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    IPipelineAddressable pipelineAddressable;
    private void Awake() {
        pipelineAddressable = GetComponentInParent<IPipelineAddressable>();
        fill.color = gradient.Evaluate(1f);
    }

    private void Start(){
        entityData = pipelineAddressable.GetEntityData();
        addingData = pipelineAddressable.GetAddingData();
        entityData.UIAffectState += SetSlider;        
    }

    private void Update() {
        SetSlider();
    }

    public void SetSlider(){
        AddingData calPipeline = addingData + entityData;
        slider.value = (((float)calPipeline.CurHP / (float)calPipeline.MaxHP) * slider.maxValue);
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //slider.value = ((float)sandbag.sandbagData.CurHP / sandbag.sandbagData.MaxHP) * slider.maxValue;
    }
}
