using System;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    public Player player;
    public AbstractSkill[] skills = new AbstractSkill[Enum.GetValues(typeof(SKILL_KEY)).Length];
    private void Start() {
        for(int i = 0; i < 3; i++){
            if(skills[i] == null) continue;
            skills[i].Init(player);
        }
    }
    public void AssignSkill(AbstractSkill _skill, SKILL_KEY _key){
        skills[(int)_key] = _skill;
        skills[(int)_key].Init(player);
    }
    public void SwapSkill(SKILL_KEY _insert, SKILL_KEY _target){
        (skills[(int)_insert], skills[(int)_target]) = (skills[(int)_target], skills[(int)_insert]);
        skills[(int)_insert].Init(player);
        skills[(int)_target].Init(player);
    }
    public void DumpSkill(SKILL_KEY _key){
        skills[(int)_key] = null;
    }
    
    public void RankUp(){
        for(int i = 0; i < 3; i++){
            if(skills[i] == null) continue;
            skills[i].skillRank++;
        }
    }
}