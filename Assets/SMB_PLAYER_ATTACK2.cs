using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_PLAYER_ATTACK2 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("canExitAttack", false);
        animator.SetBool("canNextAttack", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4f)
        {
            animator.SetBool("canNextAttack", true);
            animator.SetBool("canExitAttack", false);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            animator.SetBool("canExitAttack", true);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            animator.ResetTrigger("DoAttack");
            animator.SetBool("isAttack", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("DoAttack");
        animator.SetBool("canExitAttack", true);
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
