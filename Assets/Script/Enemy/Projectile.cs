using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class Projectile : MonoBehaviour {
    public GameObject hitEffect;
    IEnumerator mCoEnableOff;
    Collider hitBox;
    private void Awake() {
        TryGetComponent<Collider>(out hitBox);
    }
}