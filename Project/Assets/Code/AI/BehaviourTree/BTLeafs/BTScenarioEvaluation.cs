using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTScenarioEvaluation : BTNode
{
    public override BTResult Execute()
    {
        Debug.Log("Evaluating for existing scenario for agent " + context.rb.name + "...");

        if (context.userActions.Count == 0)
        {
            Dictionary<string, Scenario> scenarios = CoachController.scenarios;

            foreach (var entry in scenarios)
            {
                Scenario scenario = entry.Value; 

                if (scenario.teamWithBall == context.rb.tag || scenario.teamWithBall == "None")
                {
                    bool allConditionFit = true;
                    //check if agent's position matches
                    if ((Mathf.Abs(context.navAgent.transform.position.x - scenario.agentPosition.x) < BlackBoard2.agentR && Mathf.Abs(context.navAgent.transform.position.z - scenario.agentPosition.z) < BlackBoard2.agentR) || !BlackBoard2.agentPosition)
                    {
                        //check if ball's position matches
                        if ((Mathf.Abs(context.ball.position.x - scenario.ballPosition.x) < BlackBoard2.soccerR && Mathf.Abs(context.ball.position.z - scenario.ballPosition.z) < BlackBoard2.soccerR) || !BlackBoard2.soccerPosition)
                        {
                            //check if all teammate positions matches
                            if (BlackBoard2.teamPosition)
                            {
                                foreach (GameObject teammate in context.teammates)
                                {
                                    bool teammateMatch = false;

                                    foreach (Vector3 teammatePosition in scenario.teammatePositions)
                                    {
                                        if (Mathf.Abs(teammate.transform.position.x - teammatePosition.x) < BlackBoard2.teamR && Mathf.Abs(teammate.transform.position.z - teammatePosition.z) < BlackBoard2.teamR)
                                        {
                                            teammateMatch = true;
                                        }
                                    }

                                    if (teammateMatch == false)
                                    {
                                        allConditionFit = false;
                                        break;
                                    }
                                }
                            }

                            if (BlackBoard2.oppoPosition)
                            {
                                //check if all opponent position matches
                                foreach (GameObject opponent in context.opponents)
                                {
                                    bool opponentMatch = false;

                                    foreach (Vector3 opponentPosition in scenario.opponentPositions)
                                    {
                                        if (Mathf.Abs(opponent.transform.position.x - opponentPosition.x) < BlackBoard2.oppoR && Mathf.Abs(opponent.transform.position.z - opponentPosition.z) < BlackBoard2.oppoR)
                                        {
                                            opponentMatch = true;
                                        }
                                    }

                                    if (opponentMatch == false)
                                    {
                                        allConditionFit = false;
                                        break;
                                    }
                                }
                            }

                            if (allConditionFit)
                            {
                                if (context.scenarioQueue[0] == null)
                                {
                                    CoachController.agentsUsingPastScenario.Add(context.contextOwner);
                                }

                                context.scenarioQueue[0] = scenario;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return BTResult.SUCCESS;
    }
}
