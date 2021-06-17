// Anthony Tiongson (ast119)

using System.Collections.Generic;
using UnityEngine;

public class BTUserActions : BTNode
{
    private Scenario CreateScenario(string action, Vector3 actionParameter, Vector3? actionParameterSecondary = null)
    {
        Vector3 agentPosition = context.navAgent.transform.position;
        Vector3 ballPosition = context.ball.position;
        GameObject[] teammates = context.teammates;
        GameObject[] opponents = context.opponents;
        HashSet<Vector3> teammatePositions = new HashSet<Vector3>();
        HashSet<Vector3> opponentPositions = new HashSet<Vector3>();
        bool ballPossessed = context.ball.GetComponent<SoccerBallController>().owner;
        string teamWithBall;

        if (ballPossessed)
        {
            teamWithBall = context.ball.GetComponent<SoccerBallController>().owner.tag;
        }
        else
        {
            teamWithBall = "None";
        }

        foreach (var teammate in teammates)
        {
            teammatePositions.Add(teammate.GetComponent<Transform>().position);
        }

        foreach (var opponent in opponents)
        {
            opponentPositions.Add(opponent.GetComponent<Transform>().position);
        }

        return new Scenario(action, actionParameter, agentPosition, ballPosition,
            teammatePositions, opponentPositions, ballPossessed, teamWithBall, 0d);
    }

    private void CreateAndLogScenario(string label, string action, Vector3 actionParameter, Vector3? actionParameterSecondary = null)
    {
        var newLabel = label;
        var labelSuffix = 2;

        while (CoachController.scenarios.ContainsKey(newLabel))
        {
            newLabel = label + labelSuffix.ToString();
            labelSuffix++;
        }

        CoachController.actionSequence.Add(newLabel);
        Scenario scenario = CreateScenario(action, actionParameter, actionParameterSecondary);
        CoachController.scenarios.Add(newLabel, scenario);
    }

    private void CreatePendingScenario(string label, string action, Vector3 actionParameter, Vector3? actionParameterSecondary = null)
    {
        Scenario scenario = CreateScenario(action, actionParameter, actionParameterSecondary);
        context.pendingScenarios.Add((label, scenario));
    }

    private void LogPendingScenario()
    {
        if (context.pendingScenarios.Count > 0)
        {
            var pendingLabel = context.pendingScenarios[0].Item1;
            var labelSuffix = 2;
            string label = pendingLabel;

            while (CoachController.scenarios.ContainsKey(label))
            {
                label = pendingLabel + labelSuffix.ToString();
                labelSuffix++;
            }

            CoachController.actionSequence.Add(label);
            CoachController.scenarios.Add(label, context.pendingScenarios[0].Item2);
        }
        else
        {
            Debug.Log("Warning: no pending scenarios. This is an ERROR.");
        }

        context.contextOwner.RemoveAction();
    }

    public override BTResult Execute()
    {
        //Debug.Log("context.userActions.Count: " + context.userActions.Count);
        var label = context.userActions[0].Item1;
        var action = context.userActions[0].Item2;

        switch (action)
        {
            // Movement actions use pending scenarios because they are completed once an agent reaches a destination.
            // "Move", "GoToBall", and "PursuePlayer" (not implemented) are movement actions.
            // The pending scenario data is taken the initial moment the agent receives the movement action.
            case "Move":
                var destination = context.userActions[0].Item3.transform.position;
                //Debug.Log("Current destination: " + destination.ToString());

                if ((context.navAgent.GetComponent<Transform>().position.x < destination.x + 2 && context.navAgent.GetComponent<Transform>().position.x > destination.x - 2) &&
                    (context.navAgent.GetComponent<Transform>().position.z < destination.z + 2 && context.navAgent.GetComponent<Transform>().position.z > destination.z - 2))
                {
                    // Action completed; log pending scenario then remove marker.
                    //Debug.Log("Test: agent has reached destination."); //
                    LogPendingScenario();

                    // Check to see if the subsequent action is another movement, and if so create a pending scenario.
                    if (context.userActions.Count > 0)
                    {
                        var pendingLabel = context.userActions[0].Item1;
                        var pendingAction = context.userActions[0].Item2;

                        if (pendingAction.Equals("Move") ||
                        pendingAction.Equals("GoToBall"))/* ||
                        pendingAction.Equals("PursuePlayer"))*/
                        {
                            var pendingActionParameter = context.userActions[0].Item3.transform.position;
                            CreatePendingScenario(pendingLabel, pendingAction, pendingActionParameter);
                        }
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
            case "GoToBall": // Pursue ball for ownership.
                if (context.ball.GetComponent<SoccerBallController>().owner) // Ball is possessed.
                {
                    if (context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name))
                    {
                        // Action completed; log pending scenario then remove marker.
                        LogPendingScenario();

                        // Check to see if the subsequent action is another movement, and if so create a pending scenario.
                        if (context.userActions.Count > 0)
                        {
                            var pendingLabel = context.userActions[0].Item1;
                            var pendingAction = context.userActions[0].Item2;

                            if (pendingAction.Equals("Move") ||
                                pendingAction.Equals("GoToBall"))/* ||
                                pendingAction.Equals("PursuePlayer"))*/
                            {
                                var pendingActionParameter = context.userActions[0].Item3.transform.position;
                                CreatePendingScenario(pendingLabel, pendingAction, pendingActionParameter);
                            }
                        }

                        return BTResult.SUCCESS;
                    }
                    else if (context.ball.GetComponent<SoccerBallController>().owner.tag.Equals(context.rb.tag))
                    {
                        // Teammate has ball.  Action incomplete, remove marker and pending scenario.
                        context.navAgent.GetComponent<AIComponent>().RemoveAction();

                        // Check to see if the subsequent action is another movement, and if so create a pending scenario.
                        if (context.userActions.Count > 0)
                        {
                            var pendingLabel = context.userActions[0].Item1;
                            var pendingAction = context.userActions[0].Item2;

                            if (pendingAction.Equals("Move") ||
                            pendingAction.Equals("GoToBall"))/* ||
                        pendingAction.Equals("PursuePlayer"))*/
                            {
                                var pendingActionParameter = context.userActions[0].Item3.transform.position;
                                CreatePendingScenario(pendingLabel, pendingAction, pendingActionParameter);
                            }
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
                else // Ball is not possessed.  Action executing.
                {
                    context.navAgent.SetDestination(context.ball.position);
                    context.navAgent.speed = 10;

                    return BTResult.FAILURE;
                }
            /*case "PursuePlayer": // Follow an opposing agent.
                break;*/
            case "Kick":
                var target = context.userActions[0].Item3.transform.position;
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
                    CreateAndLogScenario(label, action, target);
                }

                context.navAgent.GetComponent<AIComponent>().RemoveAction();

                // Check to see if the subsequent action is another movement, and if so create a pending scenario.
                if (context.userActions.Count > 1)
                {
                    var pendingLabel = context.userActions[0].Item1;
                    var pendingAction = context.userActions[0].Item2;

                    if (pendingAction.Equals("Move") ||
                    pendingAction.Equals("GoToBall"))/* ||
                        pendingAction.Equals("PursuePlayer"))*/
                    {
                        var pendingActionParameter = context.userActions[0].Item3.transform.position;
                        CreatePendingScenario(pendingLabel, pendingAction, pendingActionParameter);
                    }
                }

                return BTResult.SUCCESS;
            case "Pass":
                var targetTeammate = context.contextOwner.GetCurrentPassTarget();
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
                    CreateAndLogScenario(label, action, target);
                }

                context.navAgent.GetComponent<AIComponent>().RemoveAction();

                // Check to see if the subsequent action is another movement, and if so create a pending scenario.
                if (context.userActions.Count > 1)
                {
                    var pendingLabel = context.userActions[0].Item1;
                    var pendingAction = context.userActions[0].Item2;

                    if (pendingAction.Equals("Move") ||
                    pendingAction.Equals("GoToBall"))/* ||
                        pendingAction.Equals("PursuePlayer"))*/
                    {
                        var pendingActionParameter = context.userActions[0].Item3.transform.position;
                        CreatePendingScenario(pendingLabel, pendingAction, pendingActionParameter);
                    }
                }

                return BTResult.SUCCESS;
            default:
                Debug.Log("BTUserActions: WARNING, unknown user action!");
                return BTResult.SUCCESS;
        }
    }
}
