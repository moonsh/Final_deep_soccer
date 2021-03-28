using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTeammateCloseToBall : BTNode
{
    [Input] public List<BTResult> inResults;
    public override BTResult Execute()
    {
        float closestTeammate = Mathf.Sqrt(((context.navAgent.transform.position.z - context.ball.transform.position.z) * (context.navAgent.transform.position.z - context.ball.transform.position.z))
            + ((context.navAgent.transform.position.x - context.ball.transform.position.x) * (context.navAgent.transform.position.x - context.ball.transform.position.x)));
        foreach (GameObject teammate in context.teammates)
        {
            float distance2 = Mathf.Sqrt(((teammate.transform.position.z - context.ball.transform.position.z) * (teammate.transform.position.z - context.ball.transform.position.z))
            + ((teammate.transform.position.x - context.ball.transform.position.x) * (teammate.transform.position.x - context.ball.transform.position.x)));

            if (closestTeammate > distance2)
            {
                closestTeammate = distance2;
            }
        }
        float closestOpponent = Mathf.Sqrt(((context.opponents[0].transform.position.z - context.ball.transform.position.z) * (context.opponents[0].transform.position.z - context.ball.transform.position.z))
            + ((context.opponents[0].transform.position.x - context.ball.transform.position.x) * (context.opponents[0].transform.position.x - context.ball.transform.position.x)));
        foreach (GameObject opponent in context.opponents)
        {
            float distance2 = Mathf.Sqrt(((opponent.transform.position.z - context.ball.transform.position.z) * (opponent.transform.position.z - context.ball.transform.position.z))
            + ((opponent.transform.position.x - context.ball.transform.position.x) * (opponent.transform.position.x - context.ball.transform.position.x)));

            if (closestOpponent > distance2)
            {
                closestOpponent = distance2;
            }
        }

        if (closestTeammate < closestOpponent)
        {
            
            XNode.NodePort inPort = GetPort("inResults");
            if (inPort != null)
            {
                List<XNode.NodePort> connections = inPort.GetConnections();

                foreach (XNode.NodePort _port in connections)
                {
                    BTResult result = (BTResult)_port.GetOutputValue();
                    if (result == BTResult.SUCCESS) { return BTResult.SUCCESS; }
                    if (result == BTResult.XRUNNING_DO_NOT_USE) { return BTResult.XRUNNING_DO_NOT_USE; }
                }
                return BTResult.FAILURE;
            }
            else return BTResult.FAILURE;
        }
        else return BTResult.FAILURE;
    }
}
