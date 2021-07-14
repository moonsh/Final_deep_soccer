// Anthony Tiongson (ast119)

using System;
using System.Collections.Generic;
using UnityEngine;

public class BTScenarioEvaluation : BTNode
{
    public override BTResult Execute()
    {
        
        if (context.userActions.Count == 0)
        {
            foreach (var entry in CoachController.scenarios)
            {
                string label = entry.Key;
                Scenario scenario = entry.Value;
                bool allConditionFit = true;
                //the difference between agent's current position and agent's position in the scene
                Vector3 diff = context.navAgent.transform.position - scenario.agentPosition;
                //the expected position of ball based on diff
                Vector3 expectedBallPos = context.ball.position - diff;
                //the expected position of teammates based on diff
                HashSet<Vector3> expectedTeamPositions = new HashSet<Vector3>();
                //the expected position of opponents based on diff
                HashSet<Vector3> expectedOppoPositions = new HashSet<Vector3>();
                foreach (GameObject teammate in context.teammates)
                {
<<<<<<< HEAD
                    //Debug.Log("BTScenarioEvaluation:" + context.rb.name + "agent's position matches");

                    //check if ball's position matches
                    if ((Mathf.Abs(context.ball.position.x - scenario.ballPosition.x) < BlackBoard2.soccerR && Mathf.Abs(context.ball.position.z - scenario.ballPosition.z) < BlackBoard2.soccerR) || !BlackBoard2.soccerPosition)
                    {
                        //Debug.Log("BTScenarioEvaluation: ball's position matches");
                        //check if all teammate positions matches
                        if (BlackBoard2.teamPosition)
=======
                    expectedTeamPositions.Add(teammate.transform.position - diff);
                }
                foreach (GameObject opponent in context.opponents)
                {
                    expectedOppoPositions.Add(opponent.transform.position - diff);
                }
                
                
                //Debug.Log("BTScenarioEvaluation (" + label + "): checking to see if current game state matches...");               
                //check if ball's position matches
                if ((Mathf.Abs(expectedBallPos.x - scenario.ballPosition.x) < BlackBoard2.soccerR && Mathf.Abs(expectedBallPos.z - scenario.ballPosition.z) < BlackBoard2.soccerR) || !BlackBoard2.soccerPosition)
                {
                    //Debug.Log("BTScenarioEvaluation: ball's position matches");
                    //check if all teammate positions matches
                    if (BlackBoard2.teamPosition)
                    {
                        foreach (Vector3 expectedTeammPos in expectedTeamPositions)
>>>>>>> Jiazhao
                        {
                            bool teammateMatch = false;

                            foreach (Vector3 teammatePosition in scenario.teammatePositions)
                            {
                                if (Mathf.Abs(expectedTeammPos.x - teammatePosition.x) < BlackBoard2.teamR && Mathf.Abs(expectedTeammPos.z - teammatePosition.z) < BlackBoard2.teamR)
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
                        foreach (Vector3 expectedOpponentPos in expectedOppoPositions)
                        {
                            bool opponentMatch = false;

                            foreach (Vector3 opponentPosition in scenario.opponentPositions)
                            {
                                if (Mathf.Abs(expectedOpponentPos.x - opponentPosition.x) < BlackBoard2.oppoR && Mathf.Abs(expectedOpponentPos.z - opponentPosition.z) < BlackBoard2.oppoR)
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
                        Tuple<string, Scenario> pastScenario = context.pastScenario;
                        //Debug.Log("BTScenarioEvaluation (" + label + "): all conditions fit.");
                        //Debug.Log("diff:" + diff);
                        //Debug.Log("ballPos:" + context.ball.position);
                        //Debug.Log("expectedBallPos:" + expectedBallPos);
                        //Debug.Log("sceneBallPos:" + scenario.ballPosition);
                        if (context.pastScenario != null)
                        {
<<<<<<< HEAD
                            //Debug.Log("BTScenarioEvaluation (" + label + "): all conditions fit.");

                            if (context.pastScenario != null)
=======
                            if (context.pastScenario.Item2 == null)
>>>>>>> Jiazhao
                            {
                                CoachController.agentsUsingPastScenario.Add(context.contextOwner);
                            }
                        }
                        if (Mathf.Abs(scenario.relativeTarget.z) < 55f || Mathf.Abs(scenario.relativeTarget.x) < 37.5f)
                        {
                            context.pastScenario = new Tuple<string, Scenario>(label, scenario);
                        }
                        if (pastScenario != context.pastScenario)
                        {
                            scenario.relativeTarget = scenario.actionParameter + diff;
                            
                        }
                        
                        
                        break;
                    }
                }
                
            }
        }

        return BTResult.SUCCESS;
    }
}
