using UnityEngine;
using UnityEngine.Events;
using System.Collections;
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
        #region public
        public Player playerRef;
        public Vector3 currentPosition;
        public Vector3 arrowPosition;

        public int skillNumber = 0;
        public float rotationSpeed = 5f;
        public string currentSkillName = null;
        public bool IsIndicate;
        public Canvas currentIndicator;
        public Canvas playerArrow;
        public LayerMask indicatorMask;
        public const float CamRayLength = 300f;
        #endregion

        #region private
        private RaycastHit hit;
        private Ray ray;
        #endregion

        [Header("Indicators")]

        [SerializeField] public Canvas arrowIndicator;
        [SerializeField] public Canvas circleIndicator;

        void Awake()
        {
            indicatorMask = LayerMask.GetMask("Wall", "Map");
        }

        // Start is called before the first frame update
        void Start()
        {
            currentIndicator = arrowIndicator;
            currentIndicator.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            MouseArrow();
            ray = GameManager.Instance.PlayerGameObject.GetComponent<PlayerController>().ray;
            if (!IsIndicate)
            {
                playerArrow.enabled = true;
                currentIndicator.enabled = false;
            }
            else if (IsIndicate)
            {
                playerArrow.enabled = false;
                currentIndicator.enabled = true;
                if (currentIndicator == arrowIndicator)
                {
                    Turning();
                }
            }
        }

        public void changeIndicate(string skillName)
        {
            if (skillName == "")
            {
                Debug.Log("현재 스킬이 없습니다!");
                return;
            }
            else if (skillName == "갈아버리기")
            {
                currentIndicator = circleIndicator;
            }
            else if (skillName == "바닥은 용암이야")
            {
                currentIndicator = circleIndicator;
            }
            else if (skillName == "바람처럼 칼날")
            {
                currentIndicator = arrowIndicator;
            }
            else if (skillName == "거침없는 질주")
            {
                currentIndicator = arrowIndicator;
            }
            else if (skillName == "바람처럼 돌진")
            {
                currentIndicator = arrowIndicator;
            }
            else if (skillName == "바람의 상처")
            {
                currentIndicator = arrowIndicator;
            }
            else
            {
                IsIndicate = false;
                return;
            }
        }
        private void Turning()
        {
            if (Physics.Raycast(ray, out hit, CamRayLength, indicatorMask)) // 공격 도중에는 방향 전환 금지
            {
                currentPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                if (currentIndicator == arrowIndicator)
                {
                    Quaternion skillCanvas = Quaternion.LookRotation(currentPosition - transform.position);
                    skillCanvas.eulerAngles = new Vector3(0, skillCanvas.eulerAngles.y, 0);
                    currentIndicator.transform.rotation = Quaternion.Lerp(currentIndicator.transform.rotation, skillCanvas, Time.deltaTime * rotationSpeed);
                }
            }
        }

        private void MouseArrow()
        {
            if (Physics.Raycast(ray, out hit, CamRayLength)) // 공격 도중에는 방향 전환 금지
            {
                arrowPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                Quaternion arrowCanvas = Quaternion.LookRotation(arrowPosition - transform.position);
                arrowCanvas.eulerAngles = new Vector3(0, arrowCanvas.eulerAngles.y, 0);
                playerArrow.transform.rotation = Quaternion.Lerp(playerArrow.transform.rotation, arrowCanvas, Time.deltaTime * rotationSpeed);
            }
        }
    }

}