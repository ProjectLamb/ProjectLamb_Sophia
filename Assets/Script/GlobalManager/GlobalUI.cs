using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class GlobalUI : MonoBehaviour {
    public Material        PoisionMaterial;
    public ParticleSystem  PoisionParticleSystem;
    public Material        FreezeMaterial;
    
    [ContextMenu("Poisoned")]
    void EAD_PoisonState() {
        new EAD_PoisonState(
            GameManager.Instance.playerGameObject, 
            GameManager.Instance.playerGameObject, 
            new object[] {3f, 5, PoisionMaterial, PoisionParticleSystem}
        ).Affect();
    }


    [ContextMenu("Freeze")]
    void EAD_FreezeState() {
        new EAD_FreezeState(
            GameManager.Instance.playerGameObject, 
            GameManager.Instance.playerGameObject, 
            new object[] {3f, 0.2f, FreezeMaterial}
        ).Affect();
    }
}