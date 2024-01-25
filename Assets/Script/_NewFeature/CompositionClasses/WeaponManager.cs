using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Composite 
{   
    using Sophia.Instantiates;
    using Sophia.DataSystem;
    using Sophia.Entitys;
    using Sophia.DataSystem.Numerics;
    
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private SerialBaseWeaponData _baseWeaponData;
        [SerializeField] private Entity _ownerRef;
        [SerializeField] private ProjectileBucket _projectileBucket;

        [SerializeField] private Weapon _currentWeapon;

        /******************************
        * Common
        ******************************/

        public Stat PoolSize        {get; private set;}
        public Stat AttackSpeed     {get; private set;}
        public Stat MeleeRatio      {get; private set;}

        private void Awake() {   
            PoolSize = new Stat(_baseWeaponData.PoolSize,
                E_NUMERIC_STAT_TYPE.PoolSize,
                E_STAT_USE_TYPE.Natural,
                OnPoolSizeUpdated
            );

            AttackSpeed = new Stat(_baseWeaponData.AttackSpeed,
                E_NUMERIC_STAT_TYPE.AttackSpeed,
                E_STAT_USE_TYPE.Ratio,
                OnAttackSpeedUpdated
            );

            MeleeRatio = new Stat(_baseWeaponData.MeleeRatio,
                E_NUMERIC_STAT_TYPE.MeleeRatio,
                E_STAT_USE_TYPE.Ratio,
                OnMeleeRatioUpdated
            );
        }

        private void Start() {
            ChangeWeapon(_currentWeapon);
        }
    
#region Getter

        public Weapon GetCurrentWeapon() => this._currentWeapon;

#endregion

#region Setter
        
#endregion

#region Event

        private void OnPowerUpdated(){}
        private void OnPoolSizeUpdated(){}
        private void OnAttackSpeedUpdated(){}
        private void OnMeleeRatioUpdated(){}

        public event UnityAction OnWeaponChanged = ()=>{};

        public void ClearEvents() {
            OnWeaponChanged = null;
            OnWeaponChanged ??= ()=>{};
        }

#endregion

        public void ChangeWeapon(Weapon newWeapon) {
            _currentWeapon.WeaponDistructor();
            _currentWeapon = newWeapon;
            newWeapon.WeaponConstructor(this._projectileBucket, PoolSize.GetValueByNature(), AttackSpeed.GetValueByRatio(), MeleeRatio.GetValueByRatio() );
            OnWeaponChanged?.Invoke();
        }
    }
}