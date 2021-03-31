using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BTForwardSupporting : BTNode
{
    public override BTResult Execute()
    {
        Vector3 dest = new Vector3(0,0,0);
        if(context.navAgent.transform.position.x > context.ball.position.x)
        {
            int objectBetween = 1;
            foreach(GameObject teammate in context.teammates)
            {
                if(teammate.transform.position.x > context.ball.position.x)
                {
                    if(teammate.transform.position.x < context.navAgent.transform.position.x)
                    {
                        objectBetween++;
                    }
                }
            }
            if(context.navAgent.tag == "purpleAgent")
            {
                dest = new Vector3(context.ball.position.x + 7 * objectBetween, 0, context.ball.position.z + 7 * objectBetween);
                
                if (context.navAgent.transform.position.z > 20f)
                {
                    dest = new Vector3(1.2f + 3 * objectBetween, 0, 38.7f + 2 * objectBetween);
                }
            }
            else
            {
                dest = new Vector3(context.ball.position.x + 7 * objectBetween, 0, context.ball.position.z - 7 * objectBetween);
                if (context.navAgent.transform.position.z < -20f)
                {
                    dest = new Vector3(1.2f + 3 * objectBetween, 0, -38.7f - 2 * objectBetween);
                }
            }
        }
        else
        {
            int objectBetween = 1;
            foreach (GameObject teammate in context.teammates)
            {
                if (teammate.transform.position.x < context.ball.position.x)
                {
                    if (teammate.transform.position.x > context.navAgent.transform.position.x)
                    {
                        objectBetween++;
                    }
                }
            }
            if (context.navAgent.tag == "purpleAgent")
            {
                dest = new Vector3(context.ball.position.x - 7 * objectBetween, 0, context.ball.position.z + 7 * objectBetween);
                if (context.navAgent.transform.position.z > 20f)
                {
                    dest = new Vector3(-1.2f - 3 * objectBetween, 0, 38.7f + 2 * objectBetween);
                }
            }
            else
            {
                dest = new Vector3(context.ball.position.x - 7 * objectBetween, 0, context.ball.position.z - 7 * objectBetween);
                if (context.navAgent.transform.position.z < -20f)
                {
                    dest = new Vector3(-1.2f - 3 * objectBetween, 0, -38.7f - 2 * objectBetween);
                }
            }

        }
        Debug.DrawLine(context.navAgent.transform.position, dest, Color.blue, 0.01f);
        context.navAgent.SetDestination(dest);
        context.navAgent.speed = 10;
        return BTResult.SUCCESS;
    }
}
