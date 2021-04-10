using UnityEngine;

public class BTForwardDribbling : BTNode
{
    public override BTResult Execute()
    {
        if (context.navAgent.name.Contains("Forward"))
        {
            //if opponent close, pass the ball
            foreach (GameObject opponent in context.opponents)
            {
                if (context.navAgent.tag == "blueAgent")
                {
                    if (context.navAgent.transform.position.z > opponent.transform.position.z)
                    {
                        float distance1 = Mathf.Sqrt(((opponent.transform.position.z - context.navAgent.transform.position.z) * (opponent.transform.position.z - context.navAgent.transform.position.z))
                            + ((opponent.transform.position.x - context.navAgent.transform.position.x) * (opponent.transform.position.x - context.navAgent.transform.position.x)));
                        if (distance1 < 10)
                        {
                            return BTResult.FAILURE;
                        }
                    }
                }
                else
                {
                    if (context.navAgent.transform.position.z < opponent.transform.position.z)
                    {
                        float distance1 = Mathf.Sqrt(((opponent.transform.position.z - context.navAgent.transform.position.z) * (opponent.transform.position.z - context.navAgent.transform.position.z))
                            + ((opponent.transform.position.x - context.navAgent.transform.position.x) * (opponent.transform.position.x - context.navAgent.transform.position.x)));
                        if (distance1 < 10)
                        {
                            return BTResult.FAILURE;
                        }
                    }
                }


            }
            //Dribbling 
            context.navAgent.SetDestination(context.goal.position);
            context.navAgent.speed = 8;
            if (Vector3.Angle(context.navAgent.transform.forward, context.goal.position - context.navAgent.transform.position) < 15)
            {
                float distanceToGoal = Mathf.Sqrt(((context.goal.position.z - context.navAgent.transform.position.z) * (context.goal.position.z - context.navAgent.transform.position.z))
                + ((context.goal.position.x - context.navAgent.transform.position.x) * (context.goal.position.x - context.navAgent.transform.position.x)));
                if (distanceToGoal < 20f)
                {
                    context.navAgent.GetComponent<AgentSoccer>().Kick(context.goal.position - context.navAgent.transform.position , 200f * distanceToGoal);
                }
                else
                {
                    context.navAgent.GetComponent<AgentSoccer>().Kick(context.goal.position - context.navAgent.transform.position, 50f);
                }
            }
        }
        else
        {
            return BTResult.FAILURE;
        }

        
        return BTResult.SUCCESS;
    }
}
