using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.DB
{
    using EquipmentIndex = System.Int32;
    using SkillIndex = System.Int32;
    
    [System.Serializable]
    public struct SerialSkillOverlapData
    {
        [SerializeField] public SkillIndex skillIndex;
        [SerializeField] public KeyCode assignedKey;
    }
    
    [System.Serializable]
    public class PlayerOverlapData : IPlayerDataPastable
    {
        #region Members

        public int PlayerRevivedCount { get; private set;}
        public int PlayerOverlapWealth { get; private set;}
        public int PlayerOverlapHP { get; private set; }

        private readonly List<EquipmentIndex> equipmentOverlapDatas = new List<EquipmentIndex>();
        public IReadOnlyList<EquipmentIndex> EquipmentOverlapDatas => equipmentOverlapDatas;

        private readonly List<SerialSkillOverlapData> skillOverlapDatas = new List<SerialSkillOverlapData>();
        public IReadOnlyList<SerialSkillOverlapData> SkillOverlapDatas => skillOverlapDatas;

        #endregion

        #region Equipment

        public void AddToEquipmentOverlapData(int equipmentIndex)
        {
            equipmentOverlapDatas.Add(equipmentIndex);
        }

        public void RemoveFromEquipmentOverlapData(int equipmentIndex)
        {
            equipmentOverlapDatas.Remove(equipmentIndex);
        }

        public void ClearEquipmentOverlapData()
        {
            equipmentOverlapDatas.Clear();
        }

        #endregion

        #region Skill

        public void AddToSkillOverlapData(KeyCode key, int skillIndex)
        {
            skillOverlapDatas.Add(new SerialSkillOverlapData
            {
                skillIndex = skillIndex, assignedKey = key
            });
        }

        public void RemoveFromSkillOverlapData(int skillIndex)
        {
            var removeTarget = skillOverlapDatas.Find(e => e.skillIndex == skillIndex);
            skillOverlapDatas.Remove(removeTarget);
        }

        public void ClearSkillOverlapData()
        {
            skillOverlapDatas.Clear();
        }

        #endregion

        public void SetOverlapHP(int hp)
        {
            PlayerOverlapHP = hp;
        }
    }
}