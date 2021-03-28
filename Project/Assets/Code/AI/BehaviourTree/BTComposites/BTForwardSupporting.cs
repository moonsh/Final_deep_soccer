using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BTForwardSupporting : BTNode
{
    public override BTResult Execute()
    {
        context.navAgent.SetDestination(context.goal.position);
        context.navAgent.speed = 10;
        return BTResult.SUCCESS;
    }
}
