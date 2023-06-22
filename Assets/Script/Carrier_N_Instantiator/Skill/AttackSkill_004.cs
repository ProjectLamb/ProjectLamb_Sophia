using UnityEngine;

public class AttackSkill_004 : AbstractSkill {
    //  public Player                                   player;
    //  public string                                   skillName;
    //  public SKILL_RANK                               skillRank;
    //  public SKILL_TYPE                               skillType;
    //  public string                                   description;
    //  public bool                                     IsReady = true;
    //  public float                                    PassedTime = 0f;
    //  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();
    //  protected SkillManager                          skillManager;
    public override void Init(Player _player)
    {
        base.Init(_player);
        skillType = SKILL_TYPE.ATTACK;
    }

    protected override void UseQ() {
        
    }
    protected override void UseE() {
        
    }
    protected override void UseR() {
        
    }
}