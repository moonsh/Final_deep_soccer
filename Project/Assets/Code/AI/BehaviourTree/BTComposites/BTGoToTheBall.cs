using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTGoToTheBall : BTNode
{
    public override BTResult Execute()
    {
        context.navAgent.SetDestination(context.ball.position);
        context.navAgent.speed = 10;
        return BTResult.SUCCESS;
    }
}
