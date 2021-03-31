using System.Xml;
using XNode;
using UnityEngine;

[BTComposite(typeof(BTRoot))]
public class BTRoot : BTNode
{
    [Input] public BTResult inResult;

    public override object GetValue(NodePort port)
    {
        return Execute();
    }

    public override BTResult Execute()
    {
        return GetInputValue("inResult", BTResult.FAILURE);
    }
}
