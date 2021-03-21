﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : StateMachineBehaviour
{
    public static float walkSpeed = 2;
    NavMeshAgent navAgent;
    float sqrTolerance = 2;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navAgent = animator.GetComponent<NavMeshAgent>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        float velocitySqrMagnitude = navAgent.velocity.sqrMagnitude;
        if (velocitySqrMagnitude >= 0.5f)
        {
            if (velocitySqrMagnitude > (walkSpeed * walkSpeed) + sqrTolerance)
            {
                animator.SetInteger(BTDefs.MOVEMENT_STATE, (int)MovementState.RUN);
            }
        }
        else animator.SetInteger(BTDefs.MOVEMENT_STATE, (int)MovementState.IDLE);
    }
}
