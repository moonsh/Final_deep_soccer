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
                                context.pastScenarios.Add(scenario);
                            }
                        }
                    }
                }
            }
        }
        if(context.pastScenarios.Count > 0)
        {
            
            if(context.pastScenarios[0].action=="Move")
            {
                var destination = context.pastScenarios[0].actionParameter;
                //Debug.Log("Current destination: " + destination.ToString());
                context.navAgent.SetDestination(destination);
                context.navAgent.speed = 10;
                if ((context.navAgent.GetComponent<Transform>().position.x < destination.x + 2 && context.navAgent.GetComponent<Transform>().position.x > destination.x - 2) &&
                    (context.navAgent.GetComponent<Transform>().position.z < destination.z + 2 && context.navAgent.GetComponent<Transform>().position.z > destination.z - 2))
                {
                    // Action completed; remove marker, actions, and log scenario.
                    //Debug.Log("Test: agent has reached destination."); //
                    context.pastScenarios.Remove(context.pastScenarios[0]);
                }
                
            }
            else if (context.pastScenarios[0].action == "GoToBall")
            {
                if (context.ball.GetComponent<SoccerBallController>().owner)
                {
                    if (context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name))
                    {

                        context.pastScenarios.Remove(context.pastScenarios[0]);

                    }
                    else if (context.ball.GetComponent<SoccerBallController>().owner.tag.Equals(context.rb.tag))
                    {
                       
                        if (context.userActions.Count == 0)
                        {
                            CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                        }

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
            else if(context.pastScenarios[0].action == "Kick")
            {
                var target = context.pastScenarios[0].actionParameter;
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
                context.pastScenarios.Remove(context.pastScenarios[0]);
            }
            else
            {
                Debug.Log("WARNING: Unknown user action!");
            }

            return BTResult.SUCCESS;
        }
        return BTResult.FAILURE;
    }
}
