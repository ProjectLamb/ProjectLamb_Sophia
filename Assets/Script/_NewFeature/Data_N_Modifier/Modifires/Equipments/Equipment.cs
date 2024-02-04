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

#region SerializeMember

        [SerializeField] public string _equipmentName;
        [SerializeField] public Sprite   _icon;
        [SerializeField] public string  _description;
        [SerializeField] SerialCalculateDatas _calculateDatas;

#endregion
        readonly Dictionary<E_NUMERIC_STAT_TYPE, StatModifier> Modifiers = new();

        private void InitializeStatModifiers(SerialCalculateDatas calculateDatas) {
            foreach(E_NUMERIC_STAT_TYPE statType in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE))) 
            {
                SerialModifireDatas statValue = calculateDatas.GetModifireDatas(statType);
                if(statValue.calType != E_STAT_CALC_TYPE.None) {
                    Modifiers.Add(statType, new StatModifier( statValue.amount, statValue.calType, statType));
                }
            }
        }

        private void Awake() {
            InitializeStatModifiers(_calculateDatas);
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