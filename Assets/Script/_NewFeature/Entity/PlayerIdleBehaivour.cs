using System.Collections;
using System.Collections.Generic;
using FMODPlus;
using UnityEngine;

namespace Sophia.Entitys
{
    public class PlayerIdleBehaivour : StateMachineBehaviour
    {
        FMODAudioSource playerRestAudio;
        bool IsDataInitialized = false;

        public void InitByData(Player playerRef) {
            playerRestAudio = playerRef.IdleRestAudio;
            playerRestAudio.Play();
            IsDataInitialized = true;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!IsDataInitialized) return;
            if(!playerRestAudio.isPlaying) 
                playerRestAudio.Play();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!IsDataInitialized) return;
            if(playerRestAudio.isPlaying) 
                playerRestAudio.Stop();
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
    
}