// Anthony Tiongson (ast119)

using UnityEngine;

public class BTUserActions : BTNode
{
    public override BTResult Execute()
    {
        //Debug.Log("context.userActions.Count: " + context.userActions.Count);
        string userAction = context.userActions[0];

        switch (userAction)
        {
            case "Move":
                var destination = context.userActionMarkers[0].transform.position;
                //Debug.Log("Current destination: " + destination.ToString());

                if ((context.navAgent.GetComponent<Transform>().position.x < destination.x + 2 && context.navAgent.GetComponent<Transform>().position.x > destination.x - 2) &&
                    (context.navAgent.GetComponent<Transform>().position.z < destination.z + 2 && context.navAgent.GetComponent<Transform>().position.z > destination.z - 2))
                {
                    // Action completed; remove marker, actions, and log scenario.
                    //Debug.Log("Test: agent has reached destination."); //
                    context.navAgent.GetComponent<AIComponent>().DestroyMarker(true);

                    if (context.userActions.Count == 0)
                    {
                        CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                    }

                    return BTResult.SUCCESS;
                }
                else // Action executing.
                {
                    //Debug.Log("Test: agent not at destination.");
                    //Debug.Log("Current agent location: " + context.navAgent.GetComponent<Transform>().position.ToString());
                    context.navAgent.SetDestination(destination);
                    context.navAgent.speed = 10;
                    return BTResult.FAILURE;
                }
            case "GoToBall":
                if (context.ball.GetComponent<SoccerBallController>().owner)
                {
                    if (context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name))
                    {
                        // Action completed.
                        context.navAgent.GetComponent<AIComponent>().DestroyMarker(true);

                        if (context.userActions.Count == 0)
                        {
                            CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                        }

                        return BTResult.SUCCESS;
                    }
                    else if (context.ball.GetComponent<SoccerBallController>().owner.tag.Equals(context.rb.tag))
                    {
                        // Teammate has ball.  Action incomplete, do not save scenario.
                        context.navAgent.GetComponent<AIComponent>().DestroyMarker(false);

                        if (context.userActions.Count == 0)
                        {
                            CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                        }

                        return BTResult.SUCCESS;
                    }
                    else // Opponent has ball.  Action executing.
                    {
                        context.navAgent.SetDestination(context.ball.position);
                        context.navAgent.speed = 10;

                        return BTResult.FAILURE;
                    }
                }
                else // Action executing.
                {
                    context.navAgent.SetDestination(context.ball.position);
                    context.navAgent.speed = 10;

                    return BTResult.FAILURE;
                }
            case "Kick":
                var target = context.userActionMarkers[0].transform.position;
                var agentPosition = context.rb.transform.position;
                var direction = (target - agentPosition) / Vector3.Distance(agentPosition, target);
                bool possession;

                if (context.ball.GetComponent<SoccerBallController>().owner)
                {
                    possession = context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name);
                }
                else
                {
                    possession = false;
                }

                if (possession)
                {
                    context.navAgent.GetComponent<AgentSoccer>().Kick(direction, 5000f);
                }

                context.navAgent.GetComponent<AIComponent>().DestroyMarker(possession);

                if (context.userActions.Count == 0)
                {
                    CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                }

                return BTResult.SUCCESS;
            default:
                Debug.Log("WARNING: Unknown user action!");
                return BTResult.SUCCESS;
        }
    }
}
