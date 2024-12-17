using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.DB
{
    
    [System.Serializable]
    public struct SerialSkillOverlapData
    {
        [SerializeField] public int skillIndex;
        [SerializeField] public KeyCode assignedKey;
    }
    
    [System.Serializable]
    public class PlayerOverlapData : IPlayerDataPastable
    {
        #region Members

        public int PlayerRevivedCount { get; private set;}
        public int PlayerOverlapWealth { get; private set;}
        public int PlayerOverlapHP { get; private set; }

        private readonly List<int> equipmentOverlapDatas = new List<int>();
        public IReadOnlyList<int> EquipmentOverlapDatas => equipmentOverlapDatas;

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