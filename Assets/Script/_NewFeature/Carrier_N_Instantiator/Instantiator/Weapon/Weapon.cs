using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Feature_State;
using Sophia.Entitys;

namespace Sophia.Instantiates {
    using Sophia.Entitys;
    using Sophia.Composite.RenderModels;

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

        public Queue<ProjectileObject>  OnHitProjectiles = new Queue<ProjectileObject>();

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

    public class Weapon : MonoBehaviour
    {

#region Serialize Member

        [SerializeField] protected ModelManger  _modelManger;
        [SerializeField] protected List<VisualFXBucket>  _visualFXBucket;

        [SerializeField] private List<ProjectileObject> _projectiles;
        [SerializeField] private List<Animation> _performAnimation;
        [SerializeField] private int    _basePoolSize = 3;
        [SerializeField] private float  _baseRatioAttackSpeed = 1f;
        [SerializeField] private float  _baseRatioDamage = 1f;
        [SerializeField] private Vector3 _instantiateOffsetPosition;

#endregion

#region Member

        private ProjectileBucket        mInstantiatorRef;
        private Queue<ProjectileObject> NormalQueue = new Queue<ProjectileObject>();
        private Queue<ProjectileObject> OnHitQueue = new Queue<ProjectileObject>();

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
        
#endregion

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
        public void InitByWeaponUI() {throw new System.NotImplementedException();}

#region Getter

#endregion

#region Setter

        public bool IsInitialized = false;
        public void WeaponConstructor(ProjectileBucket bucket, int poolSize, float ratioAttackSpeed, float ratioDamage) {
            if(IsInitialized) throw new System.Exception("이미 생성자로 초기화 됨");
            mInstantiatorRef = bucket;
            StateType = E_WEAPONE_USE_STATE.Normal;
            _projectiles.ForEach(E => NormalQueue.Enqueue(E));

            CurrentPoolSize         = _basePoolSize;
            CurrentRatioAttackSpeed = ratioAttackSpeed * _baseRatioAttackSpeed;
            CurrentRatioDamage      = ratioDamage * _baseRatioDamage;

            IsInitialized = true;
        }

        public void ResetSettings() {
            CurrentPoolSize = _basePoolSize;
            CurrentRatioAttackSpeed = _baseRatioAttackSpeed;
            CurrentRatioDamage = _baseRatioDamage;
            NormalQueue.Clear();
            _projectiles.ForEach(E => NormalQueue.Enqueue(E));
        }

        public void WeaponDistructor() {
            mInstantiatorRef = null;
            StateType = E_WEAPONE_USE_STATE.None;
            CurrentPoolSize = _basePoolSize;
            CurrentRatioAttackSpeed = _baseRatioAttackSpeed;
            CurrentRatioDamage = _baseRatioDamage;
            NormalQueue.Clear();
            IsInitialized = false;
        }

#endregion

        public void Use(Player player) {
            if(NormalQueue.Count == 0) {
                _projectiles.ForEach(E => NormalQueue.Enqueue(E));
            }
            ProjectileObject useProjectile = mInstantiatorRef.ActivateInstantable(NormalQueue.Dequeue());
            int useDamage = CalculateDamage(player);
            useProjectile.SetProjectileDamage(useDamage)?.Activate();
        }

        private int CalculateDamage(Player player) {
            int res = player.GetStat(E_NUMERIC_STAT_TYPE.Power);
            res = (int)(res * CurrentRatioDamage);
            return res;
        }
    }
}