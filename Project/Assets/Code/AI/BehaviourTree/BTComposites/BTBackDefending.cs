using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTBackDefending : BTNode
{
    public override BTResult Execute()
    {
        context.navAgent.speed = 10;
        float distance =  Mathf.Sqrt(((context.goal.position.z - context.ball.transform.position.z) * (context.goal.position.z - context.ball.transform.position.z))
            + ((context.goal.position.x - context.ball.transform.position.x) * (context.goal.position.x - context.ball.transform.position.x)));
        if(distance < 12)
        {
            context.navAgent.SetDestination(context.ball.position);
        }
        else
        {
            context.navAgent.SetDestination(context.goal.position);
        }
        return BTResult.SUCCESS;
    }
}
