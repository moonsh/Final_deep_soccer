using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTScenarioAssert : BTNode
{
    public override BTResult Execute()
    {
        if (context.pastScenario != null)
        {
            Debug.Log("BTScenarioAssert: existing scenario detected.");
            return BTResult.SUCCESS;
        }

        Debug.Log("BTScenarioAssert: game state not detected in scenarios.");
        return BTResult.FAILURE;
    }
}