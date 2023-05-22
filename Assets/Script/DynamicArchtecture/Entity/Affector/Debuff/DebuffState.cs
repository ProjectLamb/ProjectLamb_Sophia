using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DebuffState {
    public List<IEnumerator> AsyncAffectorCoroutine;
    public List<UnityAction> Affector;
    public AddingData addingData;
    public EntityData entityData;
}