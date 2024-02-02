using UnityEngine;

    using System;
    using System.Collections.Generic;
    using Sophia;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Numerics;
    using UnityEngine.UI;
    using Sophia.Instantiates;
using Sophia.DataSystem.Functional;

public class Equipment : Carrier , IUserInterfaceAccessible{
        [SerializeField] public string _equipmentName;
        [SerializeField] public Sprite   _icon;
        [SerializeField] public string  _description;
        [SerializeField] SerialCalculateDatas _calculateDatas;
        readonly Dictionary<E_NUMERIC_STAT_TYPE, StatModifier> Modifiers = new();

        private void Awake() {
            Debug.Log(_equipmentName);
            foreach(E_NUMERIC_STAT_TYPE statType in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE))) {
                SerialModifireDatas statValue = GetValueByNumericType(statType);
                if(statValue.calType == E_STAT_CALC_TYPE.None) {continue;}
                Debug.Log($"{statType.ToString()} : {statValue.amount}");
                Modifiers.Add(statType, new StatModifier( statValue.amount, statValue.calType, statType));
            }
        }

        public void Equip() {

        }

        public void Drop() {

        }


        protected override void OnTriggerLogic(Collider entity)
        {
            if(entity.TryGetComponent<Sophia.Entitys.Player>(out Sophia.Entitys.Player player)) {
                foreach(var Modifiers in Modifiers) {
                    Stat stetRef = player.GetStat(Modifiers.Key);
                    stetRef.AddModifier(Modifiers.Value);
                    stetRef.RecalculateStat();
                    Debug.Log($"{stetRef.NumericType.ToString()} : {stetRef.GetValueForce()}");
                }
                Destroy(this.gameObject);
            }
        }

        public SerialModifireDatas GetValueByNumericType(E_NUMERIC_STAT_TYPE numericType)
        {
            SerialModifireDatas res = new SerialModifireDatas {amount = -1f, calType = E_STAT_CALC_TYPE.None};
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

    public string GetName()
    {
        throw new NotImplementedException();
    }

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public Sprite GetSprite()
    {
        throw new NotImplementedException();
    }
}