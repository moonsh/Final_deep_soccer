using UnityEngine;

public class BTScenario : BTNode
{
    public override BTResult Execute()
    {
        var label = context.pastScenario.Item1;
        var scenario = context.pastScenario.Item2;
        var action = scenario.action;
        var actionParameter = scenario.actionParameter;

        

        switch (action)
        {
            // Movement actions use pending scenarios because they are completed once an agent reaches a destination.
            // "Move", "GoToBall", and "PursuePlayer" (not implemented) are movement actions.
            // The pending scenario data is taken the initial moment the agent receives the movement action.
            case "Move":
                var destination = actionParameter;
                //Debug.Log("Current destination: " + destination.ToString());

                if ((context.navAgent.GetComponent<Transform>().position.x < destination.x + 2 && context.navAgent.GetComponent<Transform>().position.x > destination.x - 2) &&
                    (context.navAgent.GetComponent<Transform>().position.z < destination.z + 2 && context.navAgent.GetComponent<Transform>().position.z > destination.z - 2))
                {
                    // Action completed; remove marker and past scenario.
                    //Debug.Log("Test: agent has reached destination.");
                    context.contextOwner.RemoveAgentScenarioIndicator();
                    context.pastScenario = null;

                    return BTResult.SUCCESS;
                }
                else // Action executing.
                {
                    //Debug.Log("Test: agent not at destination.");
                    //Debug.Log("Current agent location: " + context.navAgent.GetComponent<Transform>().position.ToString());
                    context.navAgent.SetDestination(destination);
                    context.navAgent.speed = 10;

                    return BTResult.SUCCESS;
                }
            case "GoToBall": // Pursue ball for ownership.
                if (context.ball.GetComponent<SoccerBallController>().owner) // Ball is possessed.
                {
                    if (context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name))
                    {
                        // Action completed; remove marker and past scenario.
                        context.contextOwner.RemoveAgentScenarioIndicator();
                        context.pastScenario = null;

                        return BTResult.SUCCESS;
                    }
                    else if (context.ball.GetComponent<SoccerBallController>().owner.tag.Equals(context.rb.tag))
                    {
                        // Teammate has ball.  Action incomplete, remove marker and past scenario.
                        context.contextOwner.RemoveAgentScenarioIndicator();
                        context.pastScenario = null;

                        return BTResult.SUCCESS;
                    }
                    else // Opponent has ball.  Action executing.
                    {
                        context.navAgent.SetDestination(context.ball.position);
                        context.navAgent.speed = 10;

                        return BTResult.SUCCESS;
                    }
                }
                else // Ball is not possessed.  Action executing.
                {
                    context.navAgent.SetDestination(context.ball.position);
                    context.navAgent.speed = 10;

                    return BTResult.SUCCESS;
                }
            /*case "PursuePlayer": // Follow an opposing agent.
                break;*/
            case "Kick":
                var target = actionParameter;
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
                    float distance = Mathf.Sqrt(((target.z - agentPosition.z) * (target.z - agentPosition.z))
                        + ((target.x - agentPosition.x) * (target.x - agentPosition.x)));
                    context.navAgent.GetComponent<AgentSoccer>().Kick(direction, 200f * distance);
                }

                context.contextOwner.RemoveAgentScenarioIndicator();
                context.pastScenario = null;

                return BTResult.SUCCESS;
            case "Pass":
                Transform targetTeammate = null;

                foreach (GameObject teammate in context.teammates)
                {
                    if (Mathf.Abs(teammate.transform.position.x - actionParameter.x) < 3f && Mathf.Abs(teammate.transform.position.z - actionParameter.z) < 3f)
                    {
                        targetTeammate = teammate.transform;
                        break;
                    }
                }

                if (targetTeammate)
                {
                    target = targetTeammate.position;
                    agentPosition = context.rb.transform.position;
                    direction = (target - agentPosition) / Vector3.Distance(agentPosition, target);

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
                        float distance = Mathf.Sqrt(((target.z - agentPosition.z) * (target.z - agentPosition.z))
                            + ((target.x - agentPosition.x) * (target.x - agentPosition.x)));
                        context.navAgent.GetComponent<AgentSoccer>().Kick(direction, 200f * distance);
                    }
                }

                context.contextOwner.RemoveAgentScenarioIndicator();
                context.pastScenario = null;

                return BTResult.SUCCESS;
            default:
                Debug.Log("BTScenario: WARNING, unknown user action!");
                return BTResult.SUCCESS;
        }
    }
}
