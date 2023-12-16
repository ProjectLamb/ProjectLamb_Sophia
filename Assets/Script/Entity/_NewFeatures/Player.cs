using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using System.Runtime.CompilerServices;

namespace Feature_NewData {
    public class Player : MonoBehaviour{
        private static PlayerData playerData;
        private int CurrentStamina;
        private int CurrentHealth;
        private int MoveSpeed;
        public static Wealths Wealth;

        private void Awake() {
            CurrentStamina = playerData.GetNumeric(E_NUMERIC_STAT_MEMBERS.FixedMaxStamina);
            CurrentHealth = playerData.GetNumeric(E_NUMERIC_STAT_MEMBERS.FixedMaxHp);
        }

        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType) {
            return playerData.GetNumeric(numericMemberType);
        }

        public  void GetDamaged(int _amount) {

        }

        public  void GetDamaged(int _amount, VFXObject _vfx) {
            if(CurrentHealth <= 0) {Die();}

        }

        public void Die() {

        }

        public void Move() {
            // ...
            playerData.GetNumeric(E_NUMERIC_STAT_MEMBERS.)
        }
    }
}