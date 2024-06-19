using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sophia.Entitys;
using Sophia.Instantiates;
using Sophia.UserInterface;

namespace Sophia.Entitys
{
    using Sophia.Instantiates;
    using Sophia.UserInterface;
    public class SkillIndicator : MonoBehaviour
    {
        public Player playerRef;
        public Vector3 recentPointer;
        public Rigidbody RbIndicator;
        public const float CamRayLength = 500f;
        public int skillNumber = 0;
        public string currentSkillName = null;
        public LayerMask GroundMask;
        public GameObject currentIndicator;

        [Header("Indicators")]
        
        [SerializeField] public GameObject arrowIndicator;
        [SerializeField] public GameObject cycleIndicator;
        
        // Start is called before the first frame update
        void Start()
        {
            currentIndicator = arrowIndicator;
            RbIndicator = currentIndicator.GetComponent<Rigidbody>();
            GroundMask = LayerMask.GetMask("Wall", "Map");
            currentIndicator.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(currentSkillName == ""){
                currentIndicator.SetActive(false);
                Debug.Log("업데이트 아리안ㄹ");
                return;
            }
            else{
                currentIndicator.SetActive(true);
                Debug.Log("업데이트 아리안ㄹㄴㅇㄹㅇㄴㄴㄹㄴㄹ");
                Debug.Log("current"+currentSkillName);
                turning();
            }
        }

        public void Indicate(string skillName)
        {
            if(skillName == null)
            {
                currentSkillName = "";
                Debug.Log("현재 스킬이 없습니다!");
                return;
            }
            else if(skillName == "바람처럼 칼날")
            {
                currentSkillName = "바람처럼 칼날";
            }
            else if(skillName == "거침없는 질주")
            {
                currentSkillName = "거침없는 질주";
            }
            else if(skillName == "바람처럼 돌진")
            {
                currentSkillName = "바람처럼 돌진";
            }
            else if(skillName == "갈아버리기")
            {
                currentSkillName = "갈아버리기";
            }
            else if(skillName == "바람의 상처")
            {
                currentSkillName = "바람의 상처";
            }
            else if(skillName == "빠르게 탈출하기")
            {
                currentSkillName = "빠르게 탈출하기";
            }
            else if(skillName == "바닥은 용암이야")
            {
                currentSkillName = "바닥은 용암이야";
            }
            else if(skillName == "모두 발사!")
            {
                currentSkillName = "모두 발사!";
            }
            else
                currentSkillName = "";

        }

        public void changeIndicate(string skillName)
        {
            if(skillName == null)
            {
                currentSkillName = null;
                Debug.Log("현재 스킬이 없습니다!");
                return;
            }
            else if(skillName == "바람처럼 칼날")
            {
                currentIndicator = arrowIndicator;
            }
            else if(skillName == "거침없는 질주")
            {
                currentIndicator = arrowIndicator;
            }
            else if(skillName == "바람처럼 돌진")
            {
                currentIndicator = arrowIndicator;
            }
            else if(skillName == "갈아버리기")
            {
                currentIndicator = cycleIndicator;
            }
            else if(skillName == "바람의 상처")
            {
                currentIndicator = arrowIndicator;
            }
            else if(skillName == "바닥은 용암이야")
            {
                currentIndicator = cycleIndicator;
            }

        }
        private void turning()
        {
            currentIndicator.transform.position = playerRef.transform.position;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                recentPointer = groundHit.point - playerRef.transform.position;
                recentPointer.y = 0f;
                RbIndicator.DORotate(Quaternion.LookRotation(recentPointer).eulerAngles, 0.1f);
            }
        }
    }
}