using System.Collections.Generic;
using System.Xml;
using XNode;

[BTComposite(typeof(BTSequence))]
public class BTSequence : BTNode
{
    [Input] public List<BTResult> inResults;

    public override BTResult Execute()
    {
        NodePort inPort = GetPort("inResults");
        if (inPort != null)
        {
            List<NodePort> connections = inPort.GetConnections();

            foreach (NodePort _port in connections)
            {
                BTResult result = (BTResult)_port.GetOutputValue();
                if (result == BTResult.FAILURE) { return BTResult.FAILURE; }
                if (result == BTResult.XRUNNING_DO_NOT_USE) { return BTResult.XRUNNING_DO_NOT_USE; }
            }
            return BTResult.SUCCESS;
        }
        else return BTResult.FAILURE;
    }

}
