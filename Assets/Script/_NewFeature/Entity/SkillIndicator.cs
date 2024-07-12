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
        public Vector3 currentPosition;
        public int skillNumber = 0;
        public string currentSkillName = null;
        public bool IsIndicate;
        public Canvas currentIndicator;
        private RaycastHit hit;
        private Ray ray;

        [Header("Indicators")]
        
        [SerializeField] public Canvas arrowIndicator;
        [SerializeField] public Canvas circleIndicator;

        // Start is called before the first frame update
        void Start()
        {
            currentIndicator = arrowIndicator;
            currentIndicator.enabled = false; 
        }

        // Update is called once per frame
        void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!IsIndicate){
                currentIndicator.enabled = false;
            }
            else if(IsIndicate){
                currentIndicator.enabled = true;
                turning();
            }
        }

        public void changeIndicate(string skillName)
        {
            if(skillName == "")
            {
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
                currentIndicator = circleIndicator;
            }
            else if(skillName == "바람의 상처")
            {
                currentIndicator = arrowIndicator;
            }
            else if(skillName == "바닥은 용암이야")
            {
                currentIndicator = circleIndicator;
            }
        }
        private void turning()
        {   
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // 공격 도중에는 방향 전환 금지
            {
                currentPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion skillCanvas = Quaternion.LookRotation(currentPosition - transform.position);
            skillCanvas.eulerAngles = new Vector3(0,skillCanvas.eulerAngles.y,skillCanvas.eulerAngles.z);
            currentIndicator.transform.rotation = Quaternion.Lerp(skillCanvas, currentIndicator.transform.rotation,0);
        }
    }
}