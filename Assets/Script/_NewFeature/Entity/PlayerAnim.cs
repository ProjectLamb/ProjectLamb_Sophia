using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

namespace Sophia
{
    public class PlayerAnim : MonoBehaviour
    {
        //public UnityEvent[] animCallback;

        public Entitys.Player player;
        public Instantiates.Weapon weapon;
        public Animator animator;

        static public bool IsAttack = false;
        static public bool IsExitAttack = false; // 공격 모션 도중 탈출 여부
        static public bool attackProTime = false;
        static public bool IsThirdAttack = false;

        private void OnEnable()
        {
            weapon = (Instantiates.Weapon)player.GetWeaponManager().GetCurrentWeapon();
            animator = player.GetModelManager().GetAnimator();
            IdleStart();
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

        void AttackStart()
        {
            IsAttack = true;
        }
        void AttackEnd() // 공격 종료 시점
        {
            IsThirdAttack = false;
            IsAttack = false;
        }

        void ExitAttack() // 공격 애니메이션 중 이동 입력 시 탈출
        {
            IsExitAttack = true;
        }

         void IdleStart() // idle 상태 돌입 시 변수 초기화
        {
            IsExitAttack = false;
            IsAttack = false;
            IsThirdAttack = false;
        }

        void AttackPro()
        {
            attackProTime = true;
        }

        void ThirdAttack()
        {
            IsThirdAttack = true;
            IsExitAttack = true;
        }
        
        public void CheckAttack()
        {
            // 공격중이라면
            if(IsAttack){
                animator.SetBool("IsAttack",true);
            }
            else{
                animator.SetBool("IsAttack",false);
            }

            //공격 중 이동이 감지되었다면
            if(IsExitAttack && player.MoveInput.magnitude >= 0.01f)
            {
                animator.SetBool("IsExitAttack",true);
            }
            else if(!IsExitAttack)
            {
                animator.SetBool("IsExitAttack",false);
            }

            if(IsThirdAttack) // 세번째 공격 종료시점에 선입력되어있는 DoAttack 트리거 reset
                animator.ResetTrigger("DoAttack");
        }

        private void Update() {
            CheckAttack();
        }
    }

}