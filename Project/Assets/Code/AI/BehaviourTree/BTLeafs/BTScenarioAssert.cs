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
                var label = context.pastScenario.Item1;
                var strategy_name = context.pastScenario.Item2.strategy;

                //Debug.Log("BTScenarioAssert: existing scenario detected for agent (" + context.contextOwner.name + ").");
                if (!context.contextOwner.IsAgentScenarioIndicatorVisible())
                {
                    context.contextOwner.CreateAgentScenarioIndicator(label, strategy_name);
                }
                else
                {
                    if (context.contextOwner.GetAgentScenarioIndicatorValue() != label)
                    {
                        context.contextOwner.RemoveAgentScenarioIndicator();
                        context.contextOwner.CreateAgentScenarioIndicator(label, strategy_name);
                    }
                }

                return BTResult.SUCCESS;
            }

            //Debug.Log("BTScenarioAssert: context.pastScenario exists but somehow context.pastScenario.Item2 is null ****ERROR****");
        }
        /*else
        {
            Debug.Log("BTScenarioAssert: context.pastScenario is null.");
        }*/

        //Debug.Log("BTScenarioAssert: game state was not detected in evaluation.");
        return BTResult.FAILURE;
    }
}
