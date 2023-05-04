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
    
    [ContextMenu("DotDamage")]
    void Activate_DotDam_State() {
        new EADA_DotDamageState(
            GameManager.Instance.playerGameObject, 
            GameManager.Instance.playerGameObject, 
            new object[] {3f, 5}
        ).Affect();
    }
    [ContextMenu("Slow")]
    void Activate_Slow_State() {
        new EADA_SlowState(
            GameManager.Instance.playerGameObject, 
            GameManager.Instance.playerGameObject, 
            new object[] {3f, 0.2f}
        ).Affect();
    }

    [ContextMenu("Uncontrollable")]
    void EADA_Uncontrollable() {
        new EADA_Inattackable(
            GameManager.Instance.playerGameObject, 
            GameManager.Instance.playerGameObject, 
            new object[] {3f}
        ).Affect();
    }


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