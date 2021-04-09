// Anthony Tiongson (ast119)

using UnityEngine;

public class BTUserActions : BTNode
{
    public override BTResult Execute()
    {
        //Debug.Log("context.userActions.Count: " + context.userActions.Count);
        string userAction = context.userActions[0];


        if (userAction.Equals("Move"))
        {
            Vector3 destination = context.userActionMoves[0];
            //Debug.Log("Current destination: " + destination.ToString());

            if (!(context.navAgent.GetComponent<Transform>().position.x <= destination.x + 2 && context.navAgent.GetComponent<Transform>().position.x >= destination.x - 2) ||
                !(context.navAgent.GetComponent<Transform>().position.z <= destination.z + 2 && context.navAgent.GetComponent<Transform>().position.z >= destination.z - 2))
            {
                //Debug.Log("Test: agent not at destination.");
                //Debug.Log("Current agent location: " + context.navAgent.GetComponent<Transform>().position.ToString());
                context.navAgent.SetDestination(destination);
                context.navAgent.speed = 10;
                return BTResult.FAILURE;
            }
            else // Action completed; remove marker, actions, and log scenario.
            {
                //Debug.Log("Test: agent has reached destination."); //
                context.navAgent.GetComponent<AIComponent>().DestroyMarker();
                context.userActions.Clear();
                context.userActionMoves.Clear();
                

                if (context.userActions.Count == 0)
                {
                    CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                }

                return BTResult.SUCCESS;
            }
        }
        else if (userAction.Equals("GoToBall"))
        {
            if (context.ball.GetComponent<SoccerBallController>().owner)
            {
                if (context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name)) // ||
                    //(context.ball.GetComponent<SoccerBallController>().owner. = context.rb.team)) need a way to check if an agent's team has ownership
                {
                    context.userActions.Clear();
                    
                    if (context.userActions.Count == 0)
                    {
                        CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                    }

                    return BTResult.SUCCESS;
                }
                else
                {
                    context.navAgent.SetDestination(context.ball.position);
                    context.navAgent.speed = 10;

                    return BTResult.FAILURE;
                }
            }
            else
            {
                context.navAgent.SetDestination(context.ball.position);
                context.navAgent.speed = 10;

                return BTResult.FAILURE;
            }
        }
        else if (userAction.Equals("Kick"))
        {
            //move to target's position
            context.navAgent.SetDestination(context.userActionMoves[0]);
            // if the agent is facing the target position kick the ball and remove the action
            if (Vector3.Angle(context.navAgent.transform.forward, context.goal.position - context.navAgent.transform.position) < 15)
            {
                float distanceToTarget = Mathf.Sqrt(((context.userActionMoves[0].z - context.navAgent.transform.position.z) * (context.userActionMoves[0].z - context.navAgent.transform.position.z))
                    + ((context.userActionMoves[0].x - context.navAgent.transform.position.x) * (context.userActionMoves[0].x - context.navAgent.transform.position.x)));
                context.navAgent.GetComponent<AgentSoccer>().Kick(context.userActionMoves[0] - context.navAgent.transform.position, 200f * distanceToTarget);

                context.userActions.Clear();
                context.userActionMoves.Clear();

                //context.navAgent.GetComponent<AIComponent>().DestroyMarker();
                
                if (context.userActions.Count == 0)
                {
                    CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());

                }
                return BTResult.SUCCESS;
            }
            return BTResult.FAILURE;
        }
        else
        {
            return BTResult.FAILURE;
        }
    }
}
