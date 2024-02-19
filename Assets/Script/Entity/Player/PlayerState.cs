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
            Debug.Log("Idle state 진입");
        }

        public override void Update(Player player)
        {

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
            Debug.Log("Move state 진입");
        }

        public override void Update(Player player)
        {

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
            Debug.Log("Attack state 진입");
        }

        public override void Update(Player player)
        {

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
            Debug.Log("GetDamaged state 진입");
        }

        public override void Update(Player player)
        {

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
            Debug.Log("Die state 진입");
        }

        public override void Update(Player player)
        {

        }

        public override void Exit(Player player)
        {
            
        }
    }

}