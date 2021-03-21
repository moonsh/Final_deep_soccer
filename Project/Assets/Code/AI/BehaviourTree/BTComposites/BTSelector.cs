using System.Collections.Generic;
using System.Xml;
using XNode;

[BTComposite(typeof(BTSelector))]
public class BTSelector : BTNode
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
                if (result == BTResult.SUCCESS) { return BTResult.SUCCESS; }
                if (result == BTResult.XRUNNING_DO_NOT_USE) { return BTResult.XRUNNING_DO_NOT_USE; }
            }
            return BTResult.FAILURE;
        }
        else return BTResult.FAILURE;
    }
}
