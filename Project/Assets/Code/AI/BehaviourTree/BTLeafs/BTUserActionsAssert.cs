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
            context.pastScenario = null; // Override any past Scenario.
            context.contextOwner.RemoveAgentScenarioIndicator();
            return BTResult.SUCCESS;
        }
    }
}
