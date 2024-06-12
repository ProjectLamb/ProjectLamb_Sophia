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
        public bool IsSkill = false;
        public bool IsSkill1 = false;
        public LayerMask GroundMask;
        // Start is called before the first frame update
        void Start()
        {
            RbIndicator = GetComponent<Rigidbody>();
            GroundMask = LayerMask.GetMask("Wall", "Map");
        }

        // Update is called once per frame
        void Update()
        {
            // 각 스킬 bool 변수를 통해 update 함수에서 실행하도록 하자
            if(IsSkill1)
                turning();
        }

        public void Indicate(string skillName)
        {
            skillName = skillName.Trim();
            Debug.Log("현재 스킬 이름ㅇ란 : "+skillName);
            if(skillName == null)
            {
                Debug.Log("현재 스킬이 없습니다!");
                return;
            }
            else if(skillName == "머리가 어질어질")
            {
                IsSkill1 = true;
            }
            else  
                IsSkill1 = true;
        }
        private void turning()
        {
            this.transform.position = playerRef.transform.position;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                recentPointer = groundHit.point - playerRef.transform.position;
                recentPointer.y = 0f;
                RbIndicator.DORotate(Quaternion.LookRotation(recentPointer).eulerAngles, 0.1f);
            }
        }
        // 이런식으로 case별로 경우를 다르게 줘서 스킬마다 인디케이터의 방식을 다르게 주도록 하자.
        // private void skill1()
        // {
        //     this.transform.position = playerRef.transform.position;
        //     Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
        //     {
        //         recentPointer = groundHit.point - playerRef.transform.position;
        //         recentPointer.y = 0f;
        //         RbIndicator.DORotate(Quaternion.LookRotation(recentPointer).eulerAngles, 0.1f);
        //     }
        // }
    }
}