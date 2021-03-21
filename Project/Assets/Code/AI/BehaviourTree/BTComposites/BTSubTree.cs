using System.Collections.Generic;
using UnityEngine;
using XNode;

[BTComposite(typeof(BTSubTree))]
public class BTSubTree : BTNode
{
    public BTGraphBase subTree;
    BTRoot subTreeRoot;

    bool isValid = false;
    public override void OnStart()
    {
        isValid = ValidateAndSetRootNode();
    }

    public override BTResult Execute()
    {
        if (!isValid)
        {
            return BTResult.FAILURE;
        }
        else
        {
            subTreeRoot.context = context;
            return subTreeRoot.GetInputValue("inResult", BTResult.FAILURE);
        }
    }

    private bool ValidateAndSetRootNode()
    {
        if (subTree == null)
        {
            Debug.LogWarning("subTree is null - tree will not run");
            return false;
        }

        List<BTRoot> rootNodeList = new List<BTRoot>();
        foreach (Node _node in subTree.nodes)
        {
            BTRoot root = _node as BTRoot;
            if (root != null)
            {
                rootNodeList.Add(root);
            }
        }

        if (rootNodeList.Count != 1)
        {
            Debug.LogWarning("There is no root node or more than 1 root node in this subTree - subtree will not run - Make sure there is exactly 1BTRoot node in your graph");
            return false;
        }
        else subTreeRoot = rootNodeList[0];

        return true;
    }
}
