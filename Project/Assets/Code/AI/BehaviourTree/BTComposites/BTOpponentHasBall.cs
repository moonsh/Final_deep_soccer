using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTOpponentHasBall : BTNode
{
    [Input] public List<BTResult> inResults;
    public override BTResult Execute()
    {
        if (context.ball.GetComponent<SoccerBallController>().owner)
        {
            if (context.ball.GetComponent<SoccerBallController>().owner.tag != context.rb.tag)
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
        else return BTResult.FAILURE;
    }
}
