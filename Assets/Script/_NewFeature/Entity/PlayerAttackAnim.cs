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

        static public bool isAttack = false; // 공격 가능 여부
        static public bool resetAtkTrigger = false; // doattack 트리거 reset 여부(true일때 reset)
        static public bool canExitAttack = false; // 공격 중 탈출 가능 여부
        static public bool attackProTime = false;

        private void OnEnable()
        {
            weapon = (Instantiates.Weapon)player.GetWeaponManager().GetCurrentWeapon();
            animator = player.GetModelManger().GetAnimator();
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
            isAttack = true;
        }

        void AttackEnd() // 공격 종료 시점
        {
            isAttack = false;
        }

        void ExitAttack() // 공격 애니메이션 중 이동 입력 시 탈출
        {
            canExitAttack = true;
        }

        void ResetIdle() // idle 상태 돌입 시 변수 초기화
        {
            canExitAttack = false;
            isAttack = false;
            attackProTime = false;
            resetAtkTrigger = false;
        }

        void AttackPro()
        {
            attackProTime = true;
        }

        void thrAttackEnd()
        {
            isAttack = false;
            resetAtkTrigger = true;
        }

        void thrAttackExit()
        {
            canExitAttack = true;
            resetAtkTrigger = true;
        }
        
        public void CheckAttack()
        {
            // 공격중이라면
            if(isAttack){
                animator.SetBool("isAttack",true);
            }
            else{
                animator.SetBool("isAttack",false);
            }

            //공격 중 이동이 감지되었다면
            if(canExitAttack && player.GetMovementComposite().GetForwardingVector().magnitude <= 5f)
            {
                animator.SetBool("canExitAttack",true);
            }
            else if(!canExitAttack)
            {
                animator.SetBool("canExitAttack",false);
            }

            if(resetAtkTrigger) // 세번째 공격 종료시점에 선입력되어있는 DoAttack 트리거 reset
                animator.ResetTrigger("DoAttack");
        }

        private void Update() {
            CheckAttack();
        }
    }

}