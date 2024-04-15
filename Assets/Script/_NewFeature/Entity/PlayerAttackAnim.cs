using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;
using System.Runtime.CompilerServices;

namespace Sophia
{
    public class PlayerAttackAnim : MonoBehaviour
    {
        //public UnityEvent[] animCallback;

        public Entitys.Player player;
        public Instantiates.Weapon weapon;
        public Animator animator;

        static public bool isAttack = false; // 공격 애니메이션 진행 여부
        static public bool canNextAttack = true;    //공격 중 다음 공격 가능 여부 (콤보)
        static public bool canExitAttack = true; // 공격 중 탈출 가능 여부
        static public bool attackProTime = false;
        public float attackSpeedWeight = 0f;    //공격 애니메이션 속도 가중치
        float attackAnimSpeed;  // 공격 애니메이션 속도

        private void OnEnable()
        {
            weapon = (Instantiates.Weapon)player.GetWeaponManager().GetCurrentWeapon();
            animator = player.GetModelManager().GetAnimator();
            ResetIdle();
        }
        void AttackProjectile1()
        {
            weapon.CurrentProjectileIndex = 0;
            weapon.UseByIndex(player);
        }
        void AttackProjectile2()
        {
            weapon.CurrentProjectileIndex = 1;
            weapon.UseByIndex(player);
        }
        void AttackProjectile3()
        {
            weapon.CurrentProjectileIndex = 2;
            weapon.UseByIndex(player);
        }

        // Start is called before the first frame update

        void AttackStart() // 공격 시작 시점
        {
            // //DoAttack 선입력 해제
            // if (animator.GetFloat("attackAnimSpeed") <= 1.5f)
            // {
            //     animator.ResetTrigger("DoAttack");
            // }
            isAttack = true;
            canExitAttack = false;
            canNextAttack = false;
        }

        void AttackEnd() // 공격 종료 시점
        {
            isAttack = false;
        }

        void NextAttack()
        {
            canNextAttack = true;
            canExitAttack = false;
        }

        void ExitAttack() // 공격 애니메이션 중 이동 입력 시 탈출
        {
            animator.ResetTrigger("DoAttack");
            canExitAttack = true;
            animator.SetBool("canExitAttack", true);
        }

        void ResetIdle() // idle 상태 돌입 시 변수 초기화
        {
            animator.SetFloat("attackAnimSpeed", attackAnimSpeed);
            animator.ResetTrigger("DoAttack");
            canNextAttack = true;
            canExitAttack = true;
            animator.SetBool("canExitAttack", true);
            isAttack = false;
            attackProTime = false;
        }

        void ResetMove()
        {
            animator.ResetTrigger("DoAttack");
            canNextAttack = true;
            isAttack = false;
        }

        void AttackPro()
        {
            attackProTime = true;
        }

        void thrAttackExit()
        {
            animator.ResetTrigger("DoAttack");
            canExitAttack = true;
            animator.SetBool("canExitAttack", true);
        }

        void thrAttackEnd()
        {
            animator.ResetTrigger("DoAttack");
            isAttack = false;
        }

        public void CheckAttack()
        {
            // 공격중이라면
            if (isAttack)
            {
                animator.SetBool("isAttack", true);
            }
            else
            {
                animator.SetBool("isAttack", false);
            }

            //공격 중 이동이 감지되었다면
            if (canExitAttack && player.MoveInput.magnitude >= 0.01f)
            {
                animator.SetBool("canExitAttack", true);
            }
            else if (!canExitAttack)
            {
                animator.SetBool("canExitAttack", false);
            }

            if (canNextAttack)
            {
                animator.SetBool("canNextAttack", true);
            }
            else
            {
                animator.SetBool("canNextAttack", false);
            }
        }

        private void Update()
        {
            CheckAttack();
            attackAnimSpeed = weapon.CurrentRatioAttackSpeed + attackSpeedWeight;
        }
    }
}