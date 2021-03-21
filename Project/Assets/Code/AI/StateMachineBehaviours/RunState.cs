using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunState : StateMachineBehaviour
{
    NavMeshAgent navAgent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navAgent = animator.GetComponent<NavMeshAgent>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        float velocitySqrMagnitude = navAgent.velocity.sqrMagnitude;
        if (velocitySqrMagnitude >= 0.5f)
        {
            if (velocitySqrMagnitude <= WalkState.walkSpeed * WalkState.walkSpeed)
            {
                animator.SetInteger(BTDefs.MOVEMENT_STATE, (int)MovementState.WALK);
            }
        }
        else animator.SetInteger(BTDefs.MOVEMENT_STATE, (int)MovementState.IDLE);
    }
}



