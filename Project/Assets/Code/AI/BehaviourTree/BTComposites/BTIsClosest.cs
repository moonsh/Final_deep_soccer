using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsClosest : BTNode
{
    [Input] public List<BTResult> inResults;
    public override BTResult Execute()
    {
        bool isClosest = true;
        float distance1 = Mathf.Sqrt(((context.navAgent.transform.position.z - context.ball.transform.position.z) * (context.navAgent.transform.position.z - context.ball.transform.position.z))
           + ((context.navAgent.transform.position.x - context.ball.transform.position.x) * (context.navAgent.transform.position.x - context.ball.transform.position.x)));
        foreach (GameObject teammate in context.teammates)
        {
            float distance2 = Mathf.Sqrt(((teammate.transform.position.z - context.ball.transform.position.z) * (teammate.transform.position.z - context.ball.transform.position.z))
            + ((teammate.transform.position.x - context.ball.transform.position.x) * (teammate.transform.position.x - context.ball.transform.position.x)));
            if (distance1 > distance2)
            {
                isClosest = false;
            }
        }
        if (isClosest)
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
