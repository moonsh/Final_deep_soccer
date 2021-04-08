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
                context.userActions.RemoveAt(0);
                context.userActionMoves.RemoveAt(0);
                CoachController.scenarios.Add(context.navAgent.GetComponent<AIComponent>().pendingScenario);

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
                    context.userActions.RemoveAt(0);
                    CoachController.scenarios.Add(context.navAgent.GetComponent<AIComponent>().pendingScenario);
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
        else
        {
            return BTResult.FAILURE;
        }
    }
}
