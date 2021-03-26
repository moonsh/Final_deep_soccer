using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTGoToTheBall : BTNode
{
    public override BTResult Execute()
    {
        context.navAgent.SetDestination(context.ball.position);
        return BTResult.SUCCESS;
    }
}
