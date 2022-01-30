using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowScript : StateMachineBehaviour
{
    PlayerScript p;
    EnemyScript e;
    public enum Type { player, enemy};
    public Type type;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        p = animator.GetComponentInParent<PlayerScript>();
        e = animator.GetComponent<EnemyScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (type == Type.player)
        {
            p.accelerate = false;
            p.decelerate = true;
        }
        else if(type == Type.enemy)
        {
            e.accelerate = false;
            e.decelerate = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
