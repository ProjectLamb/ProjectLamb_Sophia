using System;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    public Player player;
    public Skill[] skills = new Skill[Enum.GetValues(typeof(E_SkillKey)).Length];
    public ProjectileBucket projectileBucket;

    private void Start(){
        for(int i = 0; i < 3; i++){
            if(skills[i] == null) continue;
            E_SkillKey keyByIndex = (E_SkillKey)Enum.GetValues(typeof(E_SkillKey)).GetValue(i);
            skills[i].Initialisze(player, projectileBucket, keyByIndex);
        }
    }

    public void AssignSkill(Skill _skill, E_SkillKey _key){
        skills[(int)_key] = _skill;
        skills[(int)_key].Initialisze(player, projectileBucket, _key);
    }
    public void SwapSkill(E_SkillKey _insert, E_SkillKey _target){
        (skills[(int)_insert], skills[(int)_target]) = (skills[(int)_target], skills[(int)_insert]);
        skills[(int)_insert].Initialisze(player, projectileBucket, _insert);
        skills[(int)_target].Initialisze(player, projectileBucket, _target);
    }
    public void DumpSkill(E_SkillKey _key){
        skills[(int)_key] = null;
    }
}