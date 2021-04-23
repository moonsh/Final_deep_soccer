using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTHavePastScenarios : BTNode
{
    public override BTResult Execute()
    {
        if (context.pastScenarios.Count > 0)
        {
            return BTResult.SUCCESS;
        }
        return BTResult.FAILURE;
    }
}
