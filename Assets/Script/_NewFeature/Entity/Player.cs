using System.Runtime.InteropServices;
using UnityEngine;

namespace Feature_NewData
{
    public class Player 
        : MonoBehaviour, ILifeAccessable, 
        IStatAccessable, IModelAccessable {
        
        public Numerics.PlayerStatReferer playerStat {get; private set;}
        public Numerics.Wealths PlayerWealth;
        public LifeComposite Life {get; private set;}
        public DashSkill DashSkillAbility {get; private set;}
      
        public ModelManger modelManger;
        

        public MeleeWeapon meleeWeapon;
        

        public void ChangeSkin(Material skin) { modelManger.ChangeSkin(skin); }
        public void RevertSkin() { modelManger.RevertSkin(); }
        public Animator GetAnimator() { return modelManger.GetAnimator(); }

        private void Awake() {}
        public void Die() {}
        public void GetDamaged(int damage) {}
        public void GetDamaged(int damage, VisualFXObject _obj) {}
        public void Dash() {}
        public void Attack() {}
        public void Skill() {}
        public void Turning() {}

        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType)
        {
            return playerStat.GetStat(numericType);
        }
        
        public LifeComposite GetLifeComposite()
        {
            return this.Life;
        }

        public string GetStatsInfo()
        {
            throw new System.NotImplementedException();
        }
    }    
}