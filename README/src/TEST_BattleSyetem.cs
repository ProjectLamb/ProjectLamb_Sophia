using UnityEngine;
namespace TEST
{   
    enum BATTLE_STATE {BEGINNING, PLAYER_TURN, ENEMY_TURN}
    public class BattleSyetem : StateMachine {
        BATTLE_STATE curBattleState;
        private void Start(){
        }
        public void OnAttackButton() {
            StartCoroutine(this.state.Attack());
        }
        public void OnHealButton() {
            if(this.state)
            StartCoroutine(this.state.Heal());
        }
    }
}