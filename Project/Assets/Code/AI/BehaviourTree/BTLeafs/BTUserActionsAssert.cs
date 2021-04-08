// Anthony Tiongson (ast119)

using UnityEngine;

public class BTUserActionsAssert : BTNode
{
    public override BTResult Execute()
    {
        if (context.userActions.Count == 0)
        {
            return BTResult.FAILURE;
        }
        else
        {
            if (context.userActions[0].Equals("GoToBall"))
            {
                //move to ball's position
                context.navAgent.SetDestination(context.ball.transform.position);
                //if the agent is ball's owner remove the action
                if(context.ball.GetComponent<SoccerBallController>().owner.name.Equals(context.rb.name))
                {
                    context.userActions.RemoveAt(0);

                    //context.navAgent.GetComponent<AIComponent>().DestroyMarker();
                    CoachController.scenarios.Add(context.navAgent.GetComponent<AIComponent>().pendingScenario);
                    if (context.userActions.Count == 0)
                    {
                        CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                    }
                }
            }
            if(context.userActions[0].Equals("Move"))
            {
                //move to the position that user inputs.
                context.navAgent.SetDestination(context.userActionMoves[0]);
                //if close to the position remove the action
                if (Mathf.Abs(context.navAgent.transform.position.x -  context.userActionMoves[0].x)<1 &&
                    Mathf.Abs(context.navAgent.transform.position.z - context.userActionMoves[0].z)< 1)
                {

                    context.userActions.RemoveAt(0);
                    context.userActionMoves.RemoveAt(0);

                    context.navAgent.GetComponent<AIComponent>().DestroyMarker();
                    CoachController.scenarios.Add(context.navAgent.GetComponent<AIComponent>().pendingScenario);
                    if (context.userActions.Count == 0)
                    {
                        CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                    }
                }
            }
            if(context.userActions[0].Equals("Kick"))
            {
                //move to target's position
                context.navAgent.SetDestination(context.userActionMoves[0]);
                // if the agent is facing the target position kick the ball and remove the action
                if (Vector3.Angle(context.navAgent.transform.forward, context.goal.position - context.navAgent.transform.position) < 15)
                {
                    float distanceToTarget = Mathf.Sqrt(((context.userActionMoves[0].z - context.navAgent.transform.position.z) * (context.userActionMoves[0].z - context.navAgent.transform.position.z))
                        + ((context.userActionMoves[0].x - context.navAgent.transform.position.x) * (context.userActionMoves[0].x - context.navAgent.transform.position.x)));
                    context.navAgent.GetComponent<AgentSoccer>().Kick(context.userActionMoves[0] - context.navAgent.transform.position, 200f * distanceToTarget);

                    context.userActions.RemoveAt(0);
                    context.userActionMoves.RemoveAt(0);

                    //context.navAgent.GetComponent<AIComponent>().DestroyMarker();
                    CoachController.scenarios.Add(context.navAgent.GetComponent<AIComponent>().pendingScenario);
                    if (context.userActions.Count == 0)
                    {
                        CoachController.agentsWithUserActions.Remove(context.navAgent.GetComponent<AIComponent>());
                    }
                }
            }
                return BTResult.SUCCESS;
        }
    }
}
