using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

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
            animator.ResetTrigger("DoAttack");
            animator.SetFloat("Move", 0);

            canExitAttack = false;
            animator.SetBool("canExitAttack", false);
            canNextAttack = false;
            animator.SetBool("canNextAttack", false);
            isAttack = true;
            animator.SetBool("isAttack", true);
        }

        void AttackEnd() // 공격 종료 시점
        {
            isAttack = false;
            animator.SetBool("isAttack", false);
        }

        void NextAttack()
        {
            canExitAttack = false;
            animator.SetBool("canExitAttack", false);
            canNextAttack = true;
            animator.SetBool("canNextAttack", true);
        }

        void ExitAttack() // 공격 애니메이션 중 이동 입력 시 탈출
        {
            animator.ResetTrigger("DoAttack");

            canExitAttack = true;
            animator.SetBool("canExitAttack", true);
        }

        void ResetIdle() // idle 상태 돌입 시 변수 초기화
        {
            animator.ResetTrigger("DoAttack");
            canNextAttack = true;
            animator.SetBool("canNextAttack", true);
            canExitAttack = true;
            animator.SetBool("canExitAttack", true);
            isAttack = false;
            animator.SetBool("isAttack", false);
            attackProTime = false;
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
            animator.SetBool("isAttack", false);
        }
    }

}