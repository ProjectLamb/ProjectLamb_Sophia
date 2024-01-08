using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Feature_State;
using Sophia.Entitys;

namespace Sophia.Instantiates {

    public enum E_WEAPONE_USE_STATE {
        None, Normal, OnHit, Charge
    }

    public class WeaponUseNormal : IState<Weapon>
    {
        private static WeaponUseNormal _instnace = new WeaponUseNormal();
        public static WeaponUseNormal Instance => _instnace;
        public void Enter(Weapon weapon)
        {
            throw new NotImplementedException();
        }

        public void Execute(Weapon weapon)
        {
            
        }

        public void Exit(Weapon weapon)
        {
            throw new NotImplementedException();
        }
    }

    public class WeaponUseOnHit : IState<Weapon>
    {
        private static WeaponUseOnHit _instnace = new WeaponUseOnHit();
        public static WeaponUseOnHit Instance => _instnace;

        public Queue<Projectile>  OnHitProjectiles = new Queue<Projectile>();

        public void Enter(Weapon weapon)
        {
            throw new NotImplementedException();
        }

        public void Execute(Weapon weapon)
        {
            throw new NotImplementedException();
        }

        public void Exit(Weapon weapon)
        {
            throw new NotImplementedException();
        }
    }
    /*변하는 녀석*/

    public class Weapon 
    {
        public readonly List<Projectile> projectiles = new();
        public readonly ProjectileBucket InstantiatorRef;
        public readonly Entity OwnerRef;
        public readonly List<Animation> performAnimation = new ();

        private const int _basePoolSize = 3;
        private const float _baseRatioAttackSpeed = 1f;
        private const float _baseRatioDamage = 1f;

        private int mCurrentPoolSize;
        public int CurrentPoolSize {
            get {return mCurrentPoolSize;}
            private set {
                if(value <= 0) {
                    mCurrentPoolSize = 0;
                }
                mCurrentPoolSize = value;
            }
        }
        private float mCurrentRatioAttackSpeed;
        public float CurrentRatioAttackSpeed {
            get {return mCurrentRatioAttackSpeed;}
            private set {
                if(value <= 0.01f) {
                    mCurrentRatioAttackSpeed = 0;
                }
                mCurrentRatioAttackSpeed = value;
            }
        }
        private float mCurrentRatioDamage;
        public float CurrentRatioDamage {
            get {return mCurrentRatioDamage;}
            private set {
                if(value < 1) {
                    mCurrentRatioDamage = 1;
                }
                mCurrentRatioDamage = value;
            }
        }

        public Weapon(Entity owner, ProjectileBucket bucket) {
            OwnerRef = owner;
            InstantiatorRef = bucket;
            StateType = E_WEAPONE_USE_STATE.Normal;
        }

#region State
        
        internal E_WEAPONE_USE_STATE StateType = E_WEAPONE_USE_STATE.None;
        public WeaponUseNormal NormalState = WeaponUseNormal.Instance;
        public WeaponUseOnHit  OnHitState = WeaponUseOnHit.Instance;

        public WeaponUseOnHit ChangeStateToOnHit() {
            StateType = E_WEAPONE_USE_STATE.OnHit;
            return this.OnHitState;
        }
        public WeaponUseNormal ChangeStateToNormal() {
            StateType = E_WEAPONE_USE_STATE.Normal;
            return this.NormalState;
        }

#endregion
        /*UI에 데이터가 내장되어 있다. 그걸 받아서 사용하는식으로*/
        public void InitByWeaponUI() {}

#region Getter

#endregion

#region Setter

        private void ResetSettings() {
            CurrentPoolSize = _basePoolSize;
            CurrentRatioAttackSpeed = _baseRatioAttackSpeed;
            CurrentRatioDamage = _baseRatioDamage;
        }

#endregion

        public void Use(int _amount) {
            switch (StateType)
            {
                case E_WEAPONE_USE_STATE.Normal :
                {

                    break;
                }
                case E_WEAPONE_USE_STATE.OnHit :
                {

                    break;
                }
            }
        }

    }
}