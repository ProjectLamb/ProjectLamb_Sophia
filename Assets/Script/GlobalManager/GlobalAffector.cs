using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class GlobalAffector : MonoBehaviour {
    public Material        bleedMaterial; 
    public ParticleSystem  bleedParticleSystem;
    public Material        boundedMaterial; 
    public ParticleSystem  boundedParticleSystem;
    public Material        burnMaterial; 
    public ParticleSystem  burnParticleSystem;
    public Material        confuseedMaterial; 
    public ParticleSystem  confuseedParticleSystem;
    public Material        contractedMaterial; 
    public ParticleSystem  contractedParticleSystem;
    public Material        fearingMaterial; 
    public ParticleSystem  fearingParticleSystem;
    public Material        poisonedMaterial; 
    public ParticleSystem  poisonedParticleSystem;
    public Material        freezeMaterial; 
    public ParticleSystem  freezeParticleSystem;
    public ParticleSystem  sturnParticleSystem;
    public Material        sturnMaterial; 

    [ContextMenu("Poisoned")]
    void EAD_PoisonState() {
        new EAD_PoisonState(
            GameManager.Instance.playerGameObject, 
            GameManager.Instance.playerGameObject, 
            new object[] {3f, 5, poisonedMaterial, poisonedParticleSystem}
        ).Affect();
    }

    [ContextMenu("Freeze")]
    void EAD_FreezeState() {
        new EAD_FreezeState(
            GameManager.Instance.playerGameObject, 
            GameManager.Instance.playerGameObject, 
            new object[] {3f, 0.2f, freezeMaterial}
        ).Affect();
    }
}