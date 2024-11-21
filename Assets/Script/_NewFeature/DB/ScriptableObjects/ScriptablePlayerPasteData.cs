using System.Collections.Generic;
using System.Collections.ObjectModel;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Sophia.DB
{
    interface IPlayerDataPastable
    {
        public int PlayerRevivedCount { get; }
        public int PlayerOverlapWealth { get; }
        public int PlayerOverlapHP { get; }
        public IReadOnlyList<int> EquipmentOverlapDatas { get; }
        public IReadOnlyList<SerialSkillOverlapData> SkillOverlapDatas { get; }
    }
    
    [CreateAssetMenu(fileName = "PlayerPasteData", menuName = "ScriptableObject/DB/PlayerPasteData", order = int.MaxValue)]
    public class ScriptablePlayerPasteData : ScriptableObject, IPlayerDataPastable
    {
        [SerializeField] private int _playerRevivedCount;
        [SerializeField] private int _playerOverlapHP;
        [SerializeField] private int _playerOverlapWealth;
        [SerializeField] private List<int> _equipmentOverlapDatas;
        [SerializeField] private List<SerialSkillOverlapData> _skillOverlapDatas;

        public int PlayerRevivedCount { get; }
        public int PlayerOverlapWealth { get; }
        public int PlayerOverlapHP { get; }
        public IReadOnlyList<int> EquipmentOverlapDatas { get; }
        public IReadOnlyList<SerialSkillOverlapData> SkillOverlapDatas { get; }
    }
}