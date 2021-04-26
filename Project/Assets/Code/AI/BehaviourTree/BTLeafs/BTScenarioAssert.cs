using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTScenarioAssert : BTNode
{
    public override BTResult Execute()
    {
        if (context.pastScenario != null)
        {
            if (context.pastScenario.Item2 != null)
            {
                //Debug.Log("BTScenarioAssert: existing scenario detected for agent (" + context.contextOwner.name + ").");
                return BTResult.SUCCESS;
            }
        }
        else
        {
            Debug.Log("BTScenarioAssert: context.pastScenario.Item2 is null.");
        }

        //Debug.Log("BTScenarioAssert: game state was not detected in evaluation.");
        return BTResult.FAILURE;
    }
}
