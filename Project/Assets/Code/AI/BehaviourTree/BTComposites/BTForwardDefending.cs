using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTForwardDefending : BTNode
{
    public override BTResult Execute()
    {
        context.navAgent.speed = 10;
        if (context.navAgent.name.Contains("Forward"))
        {
            bool isClosest = true;
            Vector3 closestTeammatePos = context.navAgent.transform.position;
            float distance1 = Mathf.Sqrt(((context.navAgent.transform.position.z - context.ball.transform.position.z) * (context.navAgent.transform.position.z - context.ball.transform.position.z))
               + ((context.navAgent.transform.position.x - context.ball.transform.position.x) * (context.navAgent.transform.position.x - context.ball.transform.position.x)));
            foreach (GameObject teammate in context.teammates)
            {
                float distance2 = Mathf.Sqrt(((teammate.transform.position.z - context.ball.transform.position.z) * (teammate.transform.position.z - context.ball.transform.position.z))
                + ((teammate.transform.position.x - context.ball.transform.position.x) * (teammate.transform.position.x - context.ball.transform.position.x)));
                if (distance1 > distance2)
                {
                    isClosest = false;
                    distance1 = distance2;
                    closestTeammatePos = teammate.transform.position;
                }
            }
            if (isClosest)
            {
                if (context.navAgent.tag == "blueAgent")
                {
                    Vector3 destination = new Vector3(context.ball.position.x, context.ball.position.y, context.ball.position.z + 5);
                    context.navAgent.SetDestination(destination);
                }
                else
                {
                    Vector3 destination = new Vector3(context.ball.position.x, context.ball.position.y, context.ball.position.z - 5);
                    context.navAgent.SetDestination(destination);
                }
                if (distance1 < 10)
                {
                    if (context.navAgent.tag == "blueAgent" && (context.navAgent.transform.position.z > context.ball.position.z))
                    {
                        context.navAgent.SetDestination(context.ball.position);
                    }
                    else if (context.navAgent.tag == "purpleAgent" && (context.navAgent.transform.position.z < context.ball.position.z))
                    {
                        context.navAgent.SetDestination(context.ball.position);
                    }
                }
            }
            else
            {
                Vector3 dest = new Vector3(0, 0, 0);
                if (context.navAgent.transform.position.x > closestTeammatePos.x)
                {
                    int objectBetween = 1;
                    foreach (GameObject teammate in context.teammates)
                    {
                        if (teammate.transform.position.x > closestTeammatePos.x)
                        {
                            if (teammate.transform.position.x < context.navAgent.transform.position.x)
                            {
                                objectBetween++;
                            }
                        }
                    }
                    if (context.navAgent.tag == "purpleAgent")
                    {
                        dest = new Vector3(closestTeammatePos.x + 7 * objectBetween, 0, closestTeammatePos.z + 7 * objectBetween);
                    }
                    else
                    {  
                        dest = new Vector3(closestTeammatePos.x + 5 * objectBetween, 0, closestTeammatePos.z - 5 * objectBetween);
                    }
                }
                else
                {
                    int objectBetween = 1;
                    foreach (GameObject teammate in context.teammates)
                    {
                        if (teammate.transform.position.x < closestTeammatePos.x)
                        {
                            if (teammate.transform.position.x > context.navAgent.transform.position.x)
                            {
                                objectBetween++;
                            }
                        }
                    }
                    if (context.navAgent.tag == "purpleAgent")
                    {
                        dest = new Vector3(closestTeammatePos.x - 7 * objectBetween, 0, closestTeammatePos.z + 7 * objectBetween);
                    }
                    else
                    {
                        dest = new Vector3(closestTeammatePos.x - 5 * objectBetween, 0, closestTeammatePos.z - 5 * objectBetween);
                    }

                }
                context.navAgent.SetDestination(dest);
                context.navAgent.speed = 10;
            }
        }
        if (context.navAgent.name.Contains("Back"))
        {
            context.navAgent.speed = 10;
            float distance = Mathf.Sqrt(((context.goal.position.z - context.ball.transform.position.z) * (context.goal.position.z - context.ball.transform.position.z))
                + ((context.goal.position.x - context.ball.transform.position.x) * (context.goal.position.x - context.ball.transform.position.x)));
            if (distance < 20)
            {
                if(context.navAgent.tag == "blueAgent")
                {
                    Vector3 destination = new Vector3(context.ball.position.x, context.ball.position.y, context.ball.position.z + 5);
                    context.navAgent.SetDestination(destination);
                }
                else
                {
                    Vector3 destination = new Vector3(context.ball.position.x, context.ball.position.y, context.ball.position.z - 5);
                    context.navAgent.SetDestination(destination);
                }
                if(distance <10)
                {
                    if(context.navAgent.tag == "blueAgent"&& (context.navAgent.transform.position.z > context.ball.position.z))
                    {
                        context.navAgent.SetDestination(context.ball.position);
                    }
                    else if (context.navAgent.tag == "purpleAgent" && (context.navAgent.transform.position.z < context.ball.position.z))
                    {
                        context.navAgent.SetDestination(context.ball.position);
                    }
                }
                
            }
            else
            {
                context.navAgent.SetDestination(context.goal.position);
            }
        }
        return BTResult.SUCCESS;
    }
}
