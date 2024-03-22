using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorAnimatorBoolEnd : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("IsMelee"))
            animator.transform.parent.parent.GetComponent<Raptor>().DoDamage();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (animator.GetBool("IsHowl"))
            {
                animator.SetBool("IsHowl", false);
            }
            else if (animator.GetBool("IsMelee"))
            {
                animator.SetBool("IsMelee", false);
                //animator.gameObject.transform.parent.parent.parent.GetComponent<RaptorFlocks>().AttackCount++;
            }
            else if (animator.GetBool("IsRush"))
            {
                animator.SetBool("IsRush", false);
            }
            else if (animator.GetBool("IsMelee"))
            {
                animator.SetBool("IsMelee", false);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

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
