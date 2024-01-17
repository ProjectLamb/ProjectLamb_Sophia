using UnityEngine;

    using System;
    using System.Collections.Generic;
    using Sophia;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifires;
    using Sophia.DataSystem.Numerics;
    using UnityEngine.UI;

    public class Equipment : MonoBehaviour {
        [SerializeField] public string _equipmentName;
        [SerializeField] public Sprite   _icon;
        [SerializeField] public string  _description;
        [SerializeField] SerialCalculateDatas _calculateDatas;
        readonly Dictionary<E_NUMERIC_STAT_TYPE, StatCalculator> calculators = new();

        private void Awake() {
            Debug.Log(_equipmentName);
            foreach(E_NUMERIC_STAT_TYPE statType in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE))) {
                SerialModifireDats statValue = GetValueByNumericType(statType);
                if(statValue.calType == E_STAT_CALC_TYPE.None) {continue;}
                Debug.Log($"{statType.ToString()} : {statValue.amount}");
                calculators.Add(statType, new StatCalculator( statValue.amount, statValue.calType, statType));
            }
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Sophia.Entitys.Player>(out Sophia.Entitys.Player player)) {
                foreach(var modifires in calculators) {
                    Stat stetRef = player.GetStat(modifires.Key);
                    stetRef.AddCalculator(modifires.Value);
                    stetRef.RecalculateStat();
                    Debug.Log($"{stetRef.NumericType.ToString()} : {stetRef.GetValueForce()}");
                }
                Destroy(this.gameObject);
            }
        }

        public SerialModifireDats GetValueByNumericType(E_NUMERIC_STAT_TYPE numericType)
        {
            SerialModifireDats res = new SerialModifireDats {amount = -1f, calType = E_STAT_CALC_TYPE.None};
            switch (numericType)
            {
                case E_NUMERIC_STAT_TYPE.MaxHp                                      : {res = _calculateDatas.MaxHp; break;}
                case E_NUMERIC_STAT_TYPE.Defence                                    : {res = _calculateDatas.Defence; break;}
                case E_NUMERIC_STAT_TYPE.MoveSpeed                                  : {res = _calculateDatas.MoveSpeed; break;}
                case E_NUMERIC_STAT_TYPE.Accecerate                                 : {res = _calculateDatas.Accecerate; break;}
                case E_NUMERIC_STAT_TYPE.Tenacity                                   : {res = _calculateDatas.Tenacity; break;}
                case E_NUMERIC_STAT_TYPE.MaxStamina                                 : {res = _calculateDatas.MaxStamina; break;}
                case E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed                        : {res = _calculateDatas.StaminaRestoreSpeed; break;}
                case E_NUMERIC_STAT_TYPE.Power                                      : {res = _calculateDatas.Power; break;}
                case E_NUMERIC_STAT_TYPE.Luck                                       : {res = _calculateDatas.Luck; break;}
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio    : {res = _calculateDatas.InstantiableDurateLifeTimeMultiplyRatio; break;}
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio              : {res = _calculateDatas.InstantiableSizeMultiplyRatio; break;}
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio   : {res = _calculateDatas.InstantiableForwardingSpeedMultiplyRatio; break;}
                case E_NUMERIC_STAT_TYPE.PoolSize                                   : {res = _calculateDatas.PoolSize; break;}
                case E_NUMERIC_STAT_TYPE.AttackSpeed                                : {res = _calculateDatas.AttackSpeed; break;}
                case E_NUMERIC_STAT_TYPE.MeleeRatio                                 : {res = _calculateDatas.MeleeRatio; break;}
                case E_NUMERIC_STAT_TYPE.RangerRatio                                : {res = _calculateDatas.RangerRatio; break;}
                case E_NUMERIC_STAT_TYPE.TechRatio                                  : {res = _calculateDatas.TechRatio; break;}
                case E_NUMERIC_STAT_TYPE.EfficienceMultiplyer                       : {res = _calculateDatas.EfficienceMultiplyer; break;}
                case E_NUMERIC_STAT_TYPE.CoolDownSpeed                              : {res = _calculateDatas.CoolDownSpeed; break;}
            }
            return res;
        }
    }