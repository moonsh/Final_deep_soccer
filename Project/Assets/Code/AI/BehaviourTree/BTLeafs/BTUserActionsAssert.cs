// Anthony Tiongson (ast119)

using UnityEngine;

public class BTUserActionsAssert : BTNode
{
    public override BTResult Execute()
    {
        if (context.userActions.Count == 0)
        {
            Debug.Log("NoImmediate action");
            return BTResult.FAILURE;
        }
        else
        {
             Debug.Log("immediate action");
             return BTResult.SUCCESS;
        }
    }
}
