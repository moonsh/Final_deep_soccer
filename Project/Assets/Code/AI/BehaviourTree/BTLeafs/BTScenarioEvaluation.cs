using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTScenarioEvaluation : BTNode
{
    public override BTResult Execute()
    {
        if (context.userActions.Count == 0)
        {
            Dictionary<string, Scenario> scenarios = CoachController.scenarios;

            foreach (var entry in scenarios)
            {
                Scenario scenario = entry.Value;

                Debug.Log("BTScenarioEvaluation (" + scenario.ToString() + "): checking to see if current game state matches...");

                if (scenario.teamWithBall == context.rb.tag || scenario.teamWithBall == "None")
                {
                    
                    bool allConditionFit = true;
                    //check if agent's position matches
                    if ((Mathf.Abs(context.navAgent.transform.position.x - scenario.agentPosition.x) < BlackBoard2.agentR && Mathf.Abs(context.navAgent.transform.position.z - scenario.agentPosition.z) < BlackBoard2.agentR) || !BlackBoard2.agentPosition)
                    {
                        Debug.Log("BTScenarioEvaluation:"+context.rb.name+"agent's position matches");
                        
                        //check if ball's position matches
                        if ((Mathf.Abs(context.ball.position.x - scenario.ballPosition.x) < BlackBoard2.soccerR && Mathf.Abs(context.ball.position.z - scenario.ballPosition.z) < BlackBoard2.soccerR) || !BlackBoard2.soccerPosition)
                        {
                            Debug.Log("BTScenarioEvaluation: ball's position matches");
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
                                Debug.Log("BTScenarioEvaluation (" + scenario.ToString() + "): all condition fit");
                                if (context.pastScenario == null)
                                {
                                    CoachController.agentsUsingPastScenario.Add(context.contextOwner);
                                }

                                context.pastScenario = scenario;
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
