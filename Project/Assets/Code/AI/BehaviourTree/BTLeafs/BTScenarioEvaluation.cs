
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTScenarioEvaluation : BTNode
{
    Vector3 diff;
    Vector3 expectedBallPos;
    HashSet<Vector3> expectedTeamPositions;
    HashSet<Vector3> expectedOppoPositions;
    private bool checkStrategy(string strategy)
    {
        String blueStrategy;
        String purpleStrategy;
        if (int.Parse(SoccerEnvController.blueScore1.text) == int.Parse(SoccerEnvController.purpleScore1.text))
        {
            blueStrategy= "NoStrategy";
            purpleStrategy = "NoStrategy";
        }
        else if (int.Parse(SoccerEnvController.blueScore1.text) > int.Parse(SoccerEnvController.purpleScore1.text))
        {
            
            blueStrategy = "DeActiveStrategy";
            purpleStrategy = "ActiveStrategy";
        }
        else
        {
            blueStrategy = "ActiveStrategy";
            purpleStrategy = "DeActiveStrategy";
        }
        if(context.navAgent.name[0]=='B')
        {
            if (strategy == blueStrategy)
            {
                return true;
            }
            return false;
        }
        
        else
        {
            if (strategy == purpleStrategy)
            {
                return true;
            }
            return false;
        }
    }
    public override BTResult Execute()
    {
        
        if (context.userActions.Count == 0)
        {
            foreach (var entry in CoachController.scenarios)
            {
                if(checkStrategy(entry.Value.strategy))
                {
                    string label = entry.Key;
                    Scenario scenario = entry.Value;
                    bool allConditionFit = true;
                    //the difference between agent's current position and agent's position in the scene
                    diff = context.navAgent.transform.position - scenario.agentPosition;
                    //the expected position of ball based on diff
                    expectedBallPos = context.ball.position - diff;
                    //the expected position of teammates based on diff
                    expectedTeamPositions = new HashSet<Vector3>();
                    //the expected position of opponents based on diff
                    expectedOppoPositions = new HashSet<Vector3>();
                    foreach (GameObject teammate in context.teammates)
                    {
                        expectedTeamPositions.Add(teammate.transform.position - diff);
                    }

                    foreach (GameObject opponent in context.opponents)
                    {
                        expectedOppoPositions.Add(opponent.transform.position - diff);
                    }

                    Filter(scenario);

                    //Debug.Log("BTScenarioEvaluation (" + label + "): checking to see if current game state matches...");               
                    //check if ball's position matches
                    if(!scenario.ballPossessed || (scenario.ballPossessed&& (context.navAgent.transform.name == SoccerBallController.ownerName)) )
                    {
                        if ((Mathf.Abs(expectedBallPos.x - scenario.ballPosition.x) < BlackBoard2.soccerR && Mathf.Abs(expectedBallPos.z - scenario.ballPosition.z) < BlackBoard2.soccerR) || !BlackBoard2.soccerPosition)
                        {
                            //Debug.Log("BTScenarioEvaluation: ball's position matches");
                            //check if all teammate positions matches
                            if (BlackBoard2.teamPosition)
                            {
                                foreach (Vector3 expectedTeammPos in expectedTeamPositions)
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
                                    if (context.pastScenario.Item2 == null)
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
                
                
            }
        }

        return BTResult.SUCCESS;
    }

    public void Filter(Scenario scenario)
    {
        Vector3 target = scenario.actionParameter;
        if (scenario.action == "Kick")
        {
            Vector3 changedTarget = scenario.actionParameter + diff;
            var targetDistanceToGoal = Mathf.Sqrt((target.x - context.goal.position.x) * (target.x - context.goal.position.x)+ (target.z - context.goal.position.z) * (target.z - context.goal.position.z));
            var newTargetDistanceToGoal = Mathf.Sqrt((changedTarget.x - context.goal.position.x) * (changedTarget.x - context.goal.position.x) + (changedTarget.z - context.goal.position.z) * (changedTarget.z - context.goal.position.z));
            if (targetDistanceToGoal < 3) //shooting:Ignore teammates and opponents not near the player
            {
                HashSet<Vector3> tempSet = new HashSet<Vector3>();
                foreach (Vector3 team in expectedTeamPositions)
                {
                    var distanceToTeam = ((context.navAgent.transform.position.x - team.x) * (context.navAgent.transform.position.x - team.x) + (context.navAgent.transform.position.z - team.z) * (context.navAgent.transform.position.z - team.z));
                    if (distanceToTeam > BlackBoard2.teamR)
                    {
                        tempSet.Add(team);
                    }
                }
                foreach(var temp in tempSet)
                {
                    expectedOppoPositions.Remove(temp);
                }
                HashSet<Vector3> tempSet2 = new HashSet<Vector3>();
                foreach (Vector3 oppo in expectedOppoPositions)
                {
                    var distanceToOppo = ((context.navAgent.transform.position.x - oppo.x) * (context.navAgent.transform.position.x - oppo.x) + (context.navAgent.transform.position.z - oppo.z) * (context.navAgent.transform.position.z - oppo.z));
                    if (distanceToOppo > BlackBoard2.oppoR)
                    {
                        tempSet2.Add(oppo);                        
                    }
                }
                foreach(var temp in tempSet2)
                {
                    expectedOppoPositions.Remove(temp);
                }

                if (newTargetDistanceToGoal > 3)
                {
                    expectedOppoPositions.Add(new Vector3(100, 100, 100));
                }
                
            }
            else //Passing:If opponent team players not near, ignore these players positions
            {
                HashSet<Vector3> tempSet = new HashSet<Vector3>();
                foreach (Vector3 oppo in expectedOppoPositions)
                {
                    var distanceToOppo = ((target.x - oppo.x) * (target.x - oppo.x) + (target.z - oppo.z) * (target.z - oppo.z));
                    if (distanceToOppo > BlackBoard2.oppoR)
                    {
                        tempSet.Add(oppo);          
                    }
                }
                foreach (var temp in tempSet)
                {
                    expectedOppoPositions.Remove(temp);
                }
                if (newTargetDistanceToGoal < 3)
                {
                    expectedOppoPositions.Add(new Vector3(100, 100, 100));
                }      
            }
        }
        else if(scenario.action == "Move")
        {
            if(scenario.ballPossessed) //move with a ball: Teammates position ^and  near opponents
            {
                HashSet<Vector3> tempSet = new HashSet<Vector3>();
                foreach (Vector3 oppo in expectedOppoPositions)
                {
                    var distanceToOppo = ((context.navAgent.transform.position.x - oppo.x) * (context.navAgent.transform.position.x - oppo.x) + (context.navAgent.transform.position.z - oppo.z) * (context.navAgent.transform.position.z - oppo.z));
                    if (distanceToOppo > BlackBoard2.oppoR)
                    {
                        tempSet.Add(oppo);
                    }
                }
                foreach(var temp in tempSet)
                {
                    expectedOppoPositions.Remove(temp);
                }
            }
            else //Attack move without ball: When user add a move action to opponent area, don’t consider opponents positions
            {
                if ((context.contextOwner.name[0] == 'P' && target.z > 0)
                    || (context.contextOwner.name[0] == 'B' && target.z < 0))
                {
                    expectedOppoPositions = new HashSet<Vector3>();
                }                
            }
            
        }
    }
}
