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
            turning();
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
    }
}