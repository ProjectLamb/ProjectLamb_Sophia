using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEST_ControllerMAnager : MonoBehaviour
{
    [SerializeField] TEST_GroundCastController groundCasterComponent;
    [SerializeField] TEST_InstantiatorController instantiatorComponent;

    private void Start() {
        groundCasterComponent.HitGroundHander.AddListener(instantiatorComponent.InstantiatePrefebByPosition);
    }

    private void OnDisable() {
        groundCasterComponent.HitGroundHander.RemoveListener(instantiatorComponent.InstantiatePrefebByPosition);
    }
}