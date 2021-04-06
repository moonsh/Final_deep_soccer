// Anthony Tiongson (ast119)

using UnityEngine;

public class BTDebugTest : BTNode
{
    public override BTResult Execute()
    {
        if (context.userActions.Count == 0)
        {
            Debug.Log("Testing.");
            return BTResult.SUCCESS;
        }
        else
        {
            return BTResult.FAILURE;
        }
    }
}
