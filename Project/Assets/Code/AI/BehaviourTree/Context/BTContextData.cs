using System.Collections.Generic;

public class BTContextData
{
    public BTContextData(BTContext _owningContext)
    {
        owningContext = _owningContext;
    }

    public BTContext owningContext;
    List<BTNode> runningNodes = new List<BTNode>();

    public void AddRunningNode(BTNode _node)
    {
        if (!runningNodes.Contains(_node))
        {
            runningNodes.Insert(0, _node);
        }
    }

    public void RemoveRunningNode(BTNode _node)
    {
        runningNodes.Remove(_node);
    }

    public bool HasRunningNodes(out BTNode _runningNode)
    {
        _runningNode = null;

        if (runningNodes.Count != 0)
        {
            _runningNode = runningNodes[0];
            return true;
        }
        else return false;
    }
}
