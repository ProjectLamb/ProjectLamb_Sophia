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
        //public Rigidbody RbIndicator;
        public const float CamRayLength = 500f;
        public int skillNumber = 0;
        public string currentSkillName = null;
        public LayerMask GroundMask;
        public Canvas currentIndicator;
        private RaycastHit hit;
        private Ray ray;

        [Header("Indicators")]
        
        [SerializeField] public Canvas arrowIndicator;

        // Start is called before the first frame update
        void Start()
        {
            currentIndicator = arrowIndicator;
            //RbIndicator = currentIndicator.GetComponent<Rigidbody>();
            GroundMask = LayerMask.GetMask("Wall", "Map");
            currentIndicator.enabled = true;     
        }

        // Update is called once per frame
        void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            turning();

            // if(currentSkillName == ""){
            //     currentIndicator.enabled = false;
            //     return;
            // }
            // else{
            //     currentIndicator.enabled = true;
            //     Debug.Log("current"+currentSkillName);
            //     turning();
            // }
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
                currentIndicator = arrowIndicator;
            }
            else if(skillName == "바람의 상처")
            {
                currentIndicator = arrowIndicator;
            }
            else if(skillName == "바닥은 용암이야")
            {
                currentIndicator = arrowIndicator;
            }

        }
        private void turning()
        {
            //currentIndicator.transform.position = this.transform.InverseTransformPoint(Camera.main.WorldToScreenPoint(playerRef.transform.position));
            
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