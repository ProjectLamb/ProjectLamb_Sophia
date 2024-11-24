using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/Player", order = int.MaxValue)]
public class ScriptableObjPlayerData : ScriptableObjEntityData {
    public int MaxStamina;
    public float StaminaRestoreRatio;
    public int Luck;
    public int Gear;
    public int Frag;
    public UnityAction SkillState = () => {};
    public UnityAction InteractState = () => {};
    public UnityAction UpdateState = () => {};
}