// Anthony Tiongson (ast119)

using System;

public class BTUserActionsAssert : BTNode
{
    public override BTResult Execute()
    {
        if (context.userActions.Count == 0)
        {
            return BTResult.FAILURE;
        }
        else
        {
            Array.Clear(context.scenarioQueue, 0, 1); // Override any pending Scenario.
            return BTResult.SUCCESS;
        }
    }
}
