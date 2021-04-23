using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTPastScenario : BTNode
{
    public override BTResult Execute()
    {
        List < Scenario > scenarios = CoachController.scenarios;
        foreach (Scenario scenario in scenarios)
        {
            if (scenario.teamWithBall == context.rb.tag || scenario.teamWithBall == "None") 
            {
                bool allConditionFit = true;
                //check if agent's position matches
                if ((Mathf.Abs(context.navAgent.transform.position.x - scenario.agentPosition.x) < BlackBoard2.agentR && Mathf.Abs(context.navAgent.transform.position.z - scenario.agentPosition.z) < BlackBoard2.agentR) ||!BlackBoard2.agentPosition)
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
                            bool sameScene = false;
                            foreach (Scenario past in context.pastScenarios)
                            {
                                if (past == scenario)
                                {
                                    sameScene = true;
                                }

                            }
                            if (sameScene == false)
                            {
                                if (context.pastScenarios.Count==0)
                                {
                                    CoachController.agentsUsingPastScenario.Add(context.contextOwner);
                                }
                                context.pastScenarios.Add(scenario);
                                
                            }
                        }
                    }
                }
            }
        }
        return BTResult.SUCCESS;
    }
}
