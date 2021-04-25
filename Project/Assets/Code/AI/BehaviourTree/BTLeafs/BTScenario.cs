using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTScenario : BTNode
{
    public override BTResult Execute()
    {
        if (context.scenarioQueue[0].action == "Move")
        {
            var destination = context.scenarioQueue[0].actionParameter;
            //Debug.Log("Current destination: " + destination.ToString());
            context.navAgent.SetDestination(destination);
            context.navAgent.speed = 10;

            if ((context.navAgent.GetComponent<Transform>().position.x < destination.x + 2 && context.navAgent.GetComponent<Transform>().position.x > destination.x - 2) &&
                (context.navAgent.GetComponent<Transform>().position.z < destination.z + 2 && context.navAgent.GetComponent<Transform>().position.z > destination.z - 2))
            {
                // Action completed; remove marker, actions, and log scenario.
                //Debug.Log("Test: agent has reached destination."); //
                Array.Clear(context.scenarioQueue, 0, 1);

                CoachController.agentsUsingPastScenario.Remove(context.contextOwner);
            }
        }
        else if (context.scenarioQueue[0].action == "GoToBall")
        {
            if (context.ball.GetComponent<SoccerBallController>().owner)
            {
                if (context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name))
                {
                    Array.Clear(context.scenarioQueue, 0, 1);

                    CoachController.agentsUsingPastScenario.Remove(context.contextOwner);
                }
                else if (context.ball.GetComponent<SoccerBallController>().owner.tag.Equals(context.rb.tag))
                {
                    Array.Clear(context.scenarioQueue, 0, 1);
                    CoachController.agentsUsingPastScenario.Remove(context.contextOwner);
                }
                else // Opponent has ball.  Action executing.
                {
                    context.navAgent.SetDestination(context.ball.position);
                    context.navAgent.speed = 10;
                }
            }
            else // Action executing.
            {
                context.navAgent.SetDestination(context.ball.position);
                context.navAgent.speed = 10;
            }
        }
        else if (context.scenarioQueue[0].action == "Kick")
        {
            var target = context.scenarioQueue[0].actionParameter;
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

            Array.Clear(context.scenarioQueue, 0, 1);
            CoachController.agentsUsingPastScenario.Remove(context.contextOwner);
        }
        else
        {
            Debug.Log("WARNING: Unknown user action!");
        }
        return BTResult.SUCCESS;
    }
}
