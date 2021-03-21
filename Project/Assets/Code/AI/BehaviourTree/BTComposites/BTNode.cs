using System;
using System.Xml;
using UnityEngine;
using XNode;

[Serializable]
public abstract class BTNode : Node
{
    [Output] public BTResult outResult;
    public string nodeDescription = "";

    public abstract BTResult Execute(); 

    internal BTContext context;

    public virtual void OnStart() { }

    public override object GetValue(NodePort port)
    {
        if (!Application.isPlaying || port == null || port.Connection == null) { return BTResult.FAILURE; }

        BTNode parentNode = port.Connection.node as BTNode;

        if ( parentNode != null)
        {
            context = parentNode.context;
            //Debug.Log("From " + parentNode.GetType() + " To " + GetType());
        }

        if (context.contextOwner == null)
        {
            return BTResult.FAILURE;
        }
        else
        {
#if UNITY_EDITOR
            context.behaviourHistory.Add(nodeDescription == "" ? GetType().ToString() : nodeDescription);
#endif //UNITY_EDITOR
            return Execute();
        }
    }
}
