using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Composite
{
    using Sophia.DataSystem.Modifiers;
    using Sophia.Entitys;
    using Sophia.Instantiates;

    public class SkillManager : MonoBehaviour {

#region SerializeMember
        [SerializeField] private Entitys.Player _ownerPlayer;
        [SerializeField] private string _currentQSkills;
        [SerializeField] private string _currentESkills;
        [SerializeField] private string _currentRSkills;

#endregion

#region Member
        
        public Dictionary<KeyCode, Skill> collectedSkill = new Dictionary<KeyCode, Skill>();
        public Dictionary<KeyCode, IUserInterfaceAccessible> collectedSkillInfo = new Dictionary<KeyCode, IUserInterfaceAccessible>();

#endregion

#region Getter
        public Skill GetSkillByKey(KeyCode key) {
            Skill res;
            if(!collectedSkill.ContainsKey(key)) 
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if(collectedSkill.TryGetValue(key ,out res)) {
                return res;
            }
            return null;
        }

        public IUserInterfaceAccessible SkillGetSkillInfoByKey(KeyCode key) {
            IUserInterfaceAccessible res;
            if(!collectedSkillInfo.ContainsKey(key)) 
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if(collectedSkillInfo.TryGetValue(key ,out res)) {
                return res;
            }
            return null;     
        }

        [ContextMenu("Print Q")]
        public void TEST_DebugQSkillInfo() {
            Debug.Log($"스킬 이름 {collectedSkillInfo[KeyCode.Q].GetName()}: / 스킬 설명 {collectedSkillInfo[KeyCode.Q].GetDescription()}");
        }
        [ContextMenu("Print E")]
        public void TEST_DebugESkillInfo() {
            Debug.Log($"스킬 이름 {collectedSkillInfo[KeyCode.E].GetName()}: / 스킬 설명 {collectedSkillInfo[KeyCode.E].GetDescription()}");
        }
        [ContextMenu("Print R")]
        public void TEST_DebugRSkillInfo() {
            Debug.Log($"스킬 이름 {collectedSkillInfo[KeyCode.R].GetName()}: / 스킬 설명 {collectedSkillInfo[KeyCode.R].GetDescription()}");
        }


        [ContextMenu("Drop Q")]
        public void TEST_DropQSkill() => Drop(KeyCode.Q);
        [ContextMenu("Drop E")]
        public void TEST_DropESkill() => Drop(KeyCode.E);
        [ContextMenu("Drop R")]
        public void TEST_DropRSkill() => Drop(KeyCode.R);

#endregion

        private void Awake() {
            collectedSkill.Add(KeyCode.Q, null);
            collectedSkill.Add(KeyCode.E, null);
            collectedSkill.Add(KeyCode.R, null);

            collectedSkillInfo.Add(KeyCode.Q, EmptySkill.Instance);
            collectedSkillInfo.Add(KeyCode.E, EmptySkill.Instance);
            collectedSkillInfo.Add(KeyCode.R, EmptySkill.Instance);
        }

        public bool Collect(Skill skill, KeyCode key)
        {
            if(!collectedSkillInfo.ContainsKey(key)) 
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if(collectedSkillInfo[key] != EmptySkill.Instance) 
                return false;
            collectedSkill[key] = skill;
            collectedSkillInfo[key] = skill;
            return true;
        }

        public bool Drop(KeyCode key)
        {
            if(!collectedSkill.ContainsKey(key)) 
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if(collectedSkillInfo[key] != EmptySkill.Instance) {
                collectedSkill[key] = null;
                collectedSkillInfo[key] = EmptySkill.Instance;
                return true;
            }
            else {return false;}
        }

        public bool SwapSkill(KeyCode keyA, KeyCode keyB) {
            if(collectedSkill.ContainsKey(keyA) && collectedSkill.ContainsKey(keyB)) {
                Skill temp = collectedSkill[keyA];
                collectedSkill[keyA] = collectedSkill[keyB];
                collectedSkill[keyB] = temp;
                collectedSkillInfo[keyA] = collectedSkill[keyA];
                collectedSkillInfo[keyB] = collectedSkill[keyB];
                return true;
            }
            throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
        }
    }
}