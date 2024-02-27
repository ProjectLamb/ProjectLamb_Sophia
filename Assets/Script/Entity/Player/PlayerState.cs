using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    // 플레이어 기본 상태(키 입력 X일때)
    public class Idle : State
    {
        public override void Enter(Player player)
        {
        
        }

        public override void Update(Player player)
        {
            Debug.Log("현재 상태 : Idle");
        }

        public override void Exit(Player player)
        {

        }
    }

    // 플레이어 이동 상태
    public class Move : State
    {
        public override void Enter(Player player)
        {
            
        }

        public override void Update(Player player)
        {
            Debug.Log("현재 상태 : Move");
        }

        public override void Exit(Player player)
        {

        }
    }

    // 플레이어 공격 상태
    public class Attack : State
    {
        public override void Enter(Player player)
        {
            
        }

        public override void Update(Player player)
        {
            if(player.DoAttackDash)
                player.AttackDash();
        }

        public override void Exit(Player player)
        {
   
        }
    }

    // 플레이어 피격(공격받음) 상태
    public class GetDamaged : State
    {
        public override void Enter(Player player)
        {
            
        }

        public override void Update(Player player)
        {
            Debug.Log("현재 상태 : GetDamaged");
        }

        public override void Exit(Player player)
        {
            
        }
    }

    // 플레이어 사망 상태
    public class Die : State
    {
        public override void Enter(Player player)
        {
            
        }

        public override void Update(Player player)
        {
            Debug.Log("현재 상태 : Die");
        }

        public override void Exit(Player player)
        {
            
        }
    }

}