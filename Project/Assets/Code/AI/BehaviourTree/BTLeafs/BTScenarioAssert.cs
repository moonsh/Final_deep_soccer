// Anthony Tiongson (ast119)

using UnityEngine;

public class BTScenarioAssert : BTNode
{
    public override BTResult Execute()
    {
        if (context.pastScenario != null)
        {
            if (context.pastScenario.Item2 != null)
            {
                Debug.Log("BTScenarioAssert: existing scenario detected for agent (" + context.contextOwner.name + ").");
                return BTResult.SUCCESS;
            }

            Debug.Log("BTScenarioAssert: context.pastScenario exists but somehow context.pastScenario.Item2 is null ****ERROR****");
        }
        

        //Debug.Log("BTScenarioAssert: game state was not detected in evaluation.");
        return BTResult.FAILURE;
    }
}
