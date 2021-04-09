using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BTForwardSupporting : BTNode
{
    public override BTResult Execute()
    {
        Vector3 dest = new Vector3(0, 0, 0);
        Vector3 closestTeammatePos = context.navAgent.transform.position;
        float distance1 = Mathf.Sqrt(((context.navAgent.transform.position.z - context.ball.transform.position.z) * (context.navAgent.transform.position.z - context.ball.transform.position.z))
           + ((context.navAgent.transform.position.x - context.ball.transform.position.x) * (context.navAgent.transform.position.x - context.ball.transform.position.x)));
        foreach (GameObject teammate in context.teammates)
        {
            float distance2 = Mathf.Sqrt(((teammate.transform.position.z - context.ball.transform.position.z) * (teammate.transform.position.z - context.ball.transform.position.z))
            + ((teammate.transform.position.x - context.ball.transform.position.x) * (teammate.transform.position.x - context.ball.transform.position.x)));
            if (distance1 > distance2)
            {
                distance1 = distance2;
                closestTeammatePos = teammate.transform.position;
            }
        }
        if (context.navAgent.name.Contains("Forward"))
        {
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

                    if (context.navAgent.transform.position.z > 20f)
                    {
                        dest = new Vector3(1.2f + 3 * objectBetween, 0, 38.7f + 2 * objectBetween);
                    }
                }
                else
                {
                    dest = new Vector3(closestTeammatePos.x + 7 * objectBetween, 0, closestTeammatePos.z - 7 * objectBetween);
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
                    if (context.navAgent.transform.position.z > 20f)
                    {
                        dest = new Vector3(-1.2f - 3 * objectBetween, 0, 38.7f + 2 * objectBetween);
                    }
                }
                else
                {
                    dest = new Vector3(closestTeammatePos.x - 7 * objectBetween, 0, closestTeammatePos.z - 7 * objectBetween);
                    if (context.navAgent.transform.position.z < -20f)
                    {
                        dest = new Vector3(-1.2f - 3 * objectBetween, 0, -38.7f - 2 * objectBetween);
                    }
                }

            }
        }
        else if(context.navAgent.name.Contains("Back"))
        {
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
                    dest = new Vector3(closestTeammatePos.x + 7 * objectBetween, 0, closestTeammatePos.z - 15 * objectBetween);
                }
                else
                {
                    dest = new Vector3(closestTeammatePos.x + 7 * objectBetween, 0, closestTeammatePos.z + 15 * objectBetween);
                    
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
                    dest = new Vector3(closestTeammatePos.x - 7 * objectBetween, 0, closestTeammatePos.z - 15 * objectBetween);  
                }
                else
                {
                    dest = new Vector3(closestTeammatePos.x - 7 * objectBetween, 0, closestTeammatePos.z + 15 * objectBetween);
                }

            }
        }
        Debug.DrawLine(context.navAgent.transform.position, dest, Color.blue, 0.01f);
        context.navAgent.SetDestination(dest);
        context.navAgent.speed = 10;
        return BTResult.SUCCESS;
    }
}
