using System.Collections.Generic;
using UnityEngine;
using XNode;

public class RuntimeBehaviourTree : MonoBehaviour
{
    public BehaviourTreeType behaviourTreeType;
    public BTGraphBase runtimeTree;
    BTRoot rootNode;
    bool isValid = false;

    private void Start()
    {
        isValid = ValidateAndSetRootNode();
        if (isValid)
        {
            BehaviourTreeRuntimeData.RegisterBehaviourTree(this);
        }
    }

    public virtual void RunBehaviourTree(BTContextData _currentContextData)
    {
        if (isValid)
        {

            BTNode runningNode;
            if (_currentContextData.HasRunningNodes(out runningNode))
            {
                //Running node will attempt to get context from parent node
                //So we need to override the parents node context event if it's not executed
                BTNode parentNode = runningNode.GetPort("outResult").Connection.node as BTNode;
                if (parentNode != null)
                {
                    parentNode.context = _currentContextData.owningContext; 
                }
                runningNode.GetPort("outResult").GetOutputValue();
            }
            else
            {
                rootNode.context = _currentContextData.owningContext;
                rootNode.GetInputValue("inResult", BTResult.FAILURE);
            }
        }
    }

    private bool ValidateAndSetRootNode()
    {
        if (runtimeTree == null)
        {
            Debug.LogWarning("runtimeTree is null - Behaviour tree will not run - Add a Behaviour Tree to the RuntimeBehaviourTree Script");
            return false;
        }

        List<BTRoot> rootNodeList = new List<BTRoot>();
        foreach (Node _node in runtimeTree.nodes)
        {
            BTRoot root = _node as BTRoot;
            if (root != null)
            {
                rootNodeList.Add(root);
            }
        }

        if (rootNodeList.Count != 1)
        {
            Debug.LogWarning("There is no root node or more than 1 root node in this behaviourTree - BehaviourTree will not run - Make sure there is exactly 1 BTRoot node in your graph");
            return false;
        }
        else rootNode = rootNodeList[0];

        return true;
    }
}
