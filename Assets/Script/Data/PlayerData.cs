using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct PlayerData
{
    //public int CurStamina; 이 EntityData를 포함하는 컴포넌트그 스코프 내에서 따로 정의한다. 오직 이 데이터
    //int CurHP; 이 EntityData를 포함하는 컴포넌트그 스코프 내에서 따로 정의한다. 오직 이 데이터
    public EntityData EntityDatas;
    public int MaxStamina;
    public float StaminaRestoreRatio;
    public int Luck;
    public int Gear;
    public int Frag;

    public UnityAction SkillState;
    public UnityAction InteractState;
    public UnityAction UpdateState;

    public PlayerData(ScriptableObjPlayerData _scriptable){
        EntityDatas = new EntityData(_scriptable);
        MaxStamina = _scriptable.MaxStamina;
        StaminaRestoreRatio = _scriptable.StaminaRestoreRatio;
        Luck = _scriptable.Luck;
        Gear = _scriptable.Gear;
        Frag = _scriptable.Frag;

        SkillState  = _scriptable.SkillState;
        InteractState   = _scriptable.InteractState;
        UpdateState = _scriptable.UpdateState;
    }

    public static PlayerData operator +(PlayerData x, PlayerData y) {
        PlayerData res = new PlayerData();
        res = x;
        res.EntityDatas += y.EntityDatas;
        res.MaxStamina += y.MaxStamina;
        res.StaminaRestoreRatio += y.StaminaRestoreRatio;
        res.Luck += y.Luck;
        res.Gear += y.Gear;
        res.Frag += y.Frag;
        res.SkillState += y.SkillState;
        res.InteractState += y.InteractState;
        res.UpdateState += y.UpdateState;
        return res;
    }
    public static PlayerData operator -(PlayerData x, PlayerData y)
    {
        PlayerData res = new PlayerData();
        res = x;
        res.EntityDatas -= y.EntityDatas;
        res.MaxStamina -= y.MaxStamina;
        res.StaminaRestoreRatio -= y.StaminaRestoreRatio;
        res.Luck -= y.Luck;
        res.Gear -= y.Gear;
        res.Frag -= y.Frag;
        res.SkillState -= y.SkillState;
        res.InteractState -= y.InteractState;
        res.UpdateState -= y.UpdateState;
        return res;
    }
    public readonly override string ToString() => $"PlayerEntity {EntityDatas.ToString()} MaxStamina : {MaxStamina}, StaminaRestoreRatio : {StaminaRestoreRatio}, Luck : {Luck}, Gear : {Gear}, Frag : {Frag}";
}