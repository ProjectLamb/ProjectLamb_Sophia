using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Composite
{
    using Sophia.DataSystem.Modifiers;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    using Sophia.UserInterface;
    using UnityEngine.InputSystem;

    public class SkillManager : MonoBehaviour
    {

        #region SerializeMember
        [SerializeField] private Entitys.Player _ownerPlayer;
        [SerializeField] private string _currentQSkills;
        [SerializeField] private string _currentESkills;
        [SerializeField] private string _currentRSkills;

        #endregion

        #region Member

        public Dictionary<KeyCode, Skill> collectedSkill = new Dictionary<KeyCode, Skill>();
        public Dictionary<KeyCode, IUserInterfaceAccessible> collectedSkillInfo = new Dictionary<KeyCode, IUserInterfaceAccessible>();
        public float QcoolTime = 10;
        public float EcoolTime = 10;
        public float RcoolTime = 20;

        #endregion

        #region Getter
        public Skill GetSkillByKey(KeyCode key)
        {
            Skill res;
            if (!collectedSkill.ContainsKey(key))
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if (collectedSkill.TryGetValue(key, out res))
            {
                return res;
            }
            return null;
        }

        public IUserInterfaceAccessible GetSkillInfoByKey(KeyCode key)
        {
            IUserInterfaceAccessible res;
            if (!collectedSkillInfo.ContainsKey(key))
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if (collectedSkillInfo.TryGetValue(key, out res))
            {
                return res;
            }
            return null;
        }

        [ContextMenu("Print Q")]
        public void TEST_DebugQSkillInfo()
        {
            Debug.Log($"스킬 이름 {collectedSkillInfo[KeyCode.Q].GetName()}: / 스킬 설명 {collectedSkillInfo[KeyCode.Q].GetDescription()}");
        }
        [ContextMenu("Print E")]
        public void TEST_DebugESkillInfo()
        {
            Debug.Log($"스킬 이름 {collectedSkillInfo[KeyCode.E].GetName()}: / 스킬 설명 {collectedSkillInfo[KeyCode.E].GetDescription()}");
        }
        [ContextMenu("Print R")]
        public void TEST_DebugRSkillInfo()
        {
            Debug.Log($"스킬 이름 {collectedSkillInfo[KeyCode.R].GetName()}: / 스킬 설명 {collectedSkillInfo[KeyCode.R].GetDescription()}");
        }


        [ContextMenu("Drop Q")]
        public void TEST_DropQSkill() => Drop(KeyCode.Q);
        [ContextMenu("Drop E")]
        public void TEST_DropESkill() => Drop(KeyCode.E);
        [ContextMenu("Drop R")]
        public void TEST_DropRSkill() => Drop(KeyCode.R);

        #endregion

        private void Awake()
        {
            collectedSkill.Add(KeyCode.Q, null);
            collectedSkill.Add(KeyCode.E, null);
            collectedSkill.Add(KeyCode.R, null);

            collectedSkillInfo.Add(KeyCode.Q, EmptySkill.Instance);
            collectedSkillInfo.Add(KeyCode.E, EmptySkill.Instance);
            collectedSkillInfo.Add(KeyCode.R, EmptySkill.Instance);
        }

        public bool Collect(Skill skill, KeyCode key)
        {
            if (!collectedSkillInfo.ContainsKey(key))
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if (collectedSkillInfo[key] != EmptySkill.Instance) { Drop(key); }
            collectedSkill[key] = skill;
            collectedSkill[key].GetCoolTimeComposite().AddOnUseEvent(() =>
            {
                if (skill.GetIsSkillIndicate())
                    _ownerPlayer.GetModelManager().GetAnimator().Play("SkillAttack");
                else
                    _ownerPlayer.GetModelManager().GetAnimator().Play("SkillNormal");
            });
            collectedSkillInfo[key] = skill;
            
            switch (key)
            {
                case KeyCode.Q: { InGameScreenUI.Instance._playerSkillCoolUIElement[0].SetSkill(collectedSkill[KeyCode.Q]); break; }
                case KeyCode.E: { InGameScreenUI.Instance._playerSkillCoolUIElement[1].SetSkill(collectedSkill[KeyCode.E]); break; }
                case KeyCode.R: { InGameScreenUI.Instance._playerSkillCoolUIElement[2].SetSkill(collectedSkill[KeyCode.R]); break; }
            }

            return true;
        }

        public bool Drop(KeyCode key)
        {
            if (!collectedSkill.ContainsKey(key))
                throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
            if (collectedSkillInfo[key] != EmptySkill.Instance)
            {
                collectedSkill[key] = null;
                collectedSkillInfo[key] = EmptySkill.Instance;
                switch (key)
                {
                    case KeyCode.Q: { InGameScreenUI.Instance._playerSkillCoolUIElement[0].RemoveSkill(); break; }
                    case KeyCode.E: { InGameScreenUI.Instance._playerSkillCoolUIElement[1].RemoveSkill(); break; }
                    case KeyCode.R: { InGameScreenUI.Instance._playerSkillCoolUIElement[2].RemoveSkill(); break; }
                }
                return true;
            }
            else { return false; }
        }

        private void OnDestroy() {
            InGameScreenUI.Instance._playerSkillCoolUIElement[0].RemoveSkill();
            InGameScreenUI.Instance._playerSkillCoolUIElement[1].RemoveSkill();
            InGameScreenUI.Instance._playerSkillCoolUIElement[2].RemoveSkill();
            collectedSkillInfo[KeyCode.Q] = EmptySkill.Instance;
            collectedSkillInfo[KeyCode.E] = EmptySkill.Instance;
            collectedSkillInfo[KeyCode.R] = EmptySkill.Instance;
            collectedSkill[KeyCode.Q] = null;
            collectedSkill[KeyCode.E] = null;
            collectedSkill[KeyCode.R] = null;
        }

        public bool SwapSkill(KeyCode keyA, KeyCode keyB)
        {
            int KeyAUiIndex = -1;
            int KeyBUiIndex = -1;
            switch (keyA)
            {
                case KeyCode.Q: { KeyAUiIndex = 0; break; }
                case KeyCode.E: { KeyAUiIndex = 1; break; }
                case KeyCode.R: { KeyAUiIndex = 2; break; }
            }
            switch (keyB)
            {
                case KeyCode.Q: { KeyBUiIndex = 0; break; }
                case KeyCode.E: { KeyBUiIndex = 1; break; }
                case KeyCode.R: { KeyBUiIndex = 2; break; }
            }
            
            if (KeyAUiIndex == -1 || KeyBUiIndex == -1) throw new System.Exception("Key Index 코드가 잘 안됨");

            if (collectedSkill.ContainsKey(keyA) && collectedSkill.ContainsKey(keyB))
            {
                (collectedSkill[keyA], collectedSkill[keyB]) = (collectedSkill[keyB], collectedSkill[keyA]);
                if (collectedSkill[keyA] != null)
                    Collect(collectedSkill[keyA], keyA);
                else
                    Drop(keyA);

                if (collectedSkill[keyB] != null)
                    Collect(collectedSkill[keyB], keyB);
                else
                    Drop(keyB);

                /* Collect(), Drop()과 기능이 겹쳐 주석처리함 */
                // collectedSkillInfo[keyA] = collectedSkill[keyA];
                // collectedSkillInfo[keyB] = collectedSkill[keyB];
                
                // InGameScreenUI.Instance._playerSkillCoolUIElement[KeyAUiIndex].RemoveSkill();
                // InGameScreenUI.Instance._playerSkillCoolUIElement[KeyAUiIndex].SetSkill(collectedSkill[keyA]);
                InGameScreenUI.Instance._playerSkillCoolUIElement[KeyAUiIndex].DrawForce();

                // InGameScreenUI.Instance._playerSkillCoolUIElement[KeyBUiIndex].RemoveSkill();
                // InGameScreenUI.Instance._playerSkillCoolUIElement[KeyBUiIndex].SetSkill(collectedSkill[keyB]);
                InGameScreenUI.Instance._playerSkillCoolUIElement[KeyBUiIndex].DrawForce();
                return true;
            }
            throw new System.Exception("올바른 스킬 키보드 접근이 아님 QER 중 하나로..");
        }
    }
}