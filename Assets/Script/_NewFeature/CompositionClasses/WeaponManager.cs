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
    using Sophia.DataSystem.Referer;

    public class WeaponManager : MonoBehaviour, IDataSettable
    {
#region SerializeMember

        [SerializeField] private SerialBaseWeaponData _baseWeaponData;
        [SerializeField] private Player _playerRef;
        [SerializeField] private ProjectileBucket _projectileBucket;
        [SerializeField] private Weapon _currentWeapon;

#endregion


#region Member

        public Stat                 PoolSize                {get; private set;}
        public Stat                 AttackSpeed             {get; private set;}
        public Stat                 MeleeRatio              {get; private set;}
        public Extras<DamageInfo>   WeaponUseExtras         {get; private set;}
        public Extras<object>       ProjectileRestoreExtras {get; private set;}

        public void Init(in SerialBaseWeaponData baseWeaponData) {
            PoolSize = new Stat(baseWeaponData.PoolSize,
                E_NUMERIC_STAT_TYPE.PoolSize,
                E_STAT_USE_TYPE.Natural,
                OnPoolSizeUpdated
            );

            AttackSpeed = new Stat(baseWeaponData.AttackSpeed,
                E_NUMERIC_STAT_TYPE.AttackSpeed,
                E_STAT_USE_TYPE.Ratio,
                OnAttackSpeedUpdated
            );

            MeleeRatio = new Stat(baseWeaponData.MeleeRatio,
                E_NUMERIC_STAT_TYPE.MeleeRatio,
                E_STAT_USE_TYPE.Ratio,
                OnMeleeRatioUpdated
            );

            WeaponUseExtras         = new Extras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse,OnWeaponUseExtrasUpdated);
            ProjectileRestoreExtras = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore, OnProjectileRestoreExtrasUpdated);
        }

#endregion

        private void Awake() {   
            Init(in _baseWeaponData);
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

        private void OnPoolSizeUpdated()                    => Debug.Log("최대 공격횟수 변경됨!");
        private void OnAttackSpeedUpdated()                 => Debug.Log("공격속도 변경됨!");
        private void OnMeleeRatioUpdated()                  => Debug.Log("근접공격 배율 변경됨!");
        private void OnWeaponUseExtrasUpdated()             => Debug.Log("무기 사용 추가 동작 변경됨!");
        private void OnProjectileRestoreExtrasUpdated()     => Debug.Log("장전 사용 추가 동작 변경됨!");

        public event UnityAction OnWeaponChanged = ()=>{};

        public void ClearEvents() {
            OnWeaponChanged = null;
            OnWeaponChanged ??= ()=>{};
        }

#endregion

#region Data Referer

        public void SetStatDataToReferer(EntityStatReferer statReferer)
        {
            statReferer.SetRefStat(PoolSize);
            statReferer.SetRefStat(AttackSpeed);
            statReferer.SetRefStat(MeleeRatio);
        }

        public void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer)
        {
            extrasReferer.SetRefExtras<DamageInfo>(WeaponUseExtras);
            extrasReferer.SetRefExtras<object>(ProjectileRestoreExtras);
        }

#endregion

        public void ChangeWeapon(Weapon newWeapon) {
            _currentWeapon.WeaponDistructor();
            _currentWeapon = newWeapon;
            newWeapon.WeaponConstructor(this._projectileBucket, PoolSize, AttackSpeed.GetValueByRatio(), MeleeRatio.GetValueByRatio() ,_playerRef);
            OnWeaponChanged?.Invoke();
        }

        public void Use() => _currentWeapon.Use(_playerRef);
    }
}