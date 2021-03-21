using System;

[BTAgent(typeof(BTHas))]
public class BTHas : BTNode
{
    public HasOp operation;
    public float destinationTolerance = 1;

    public override BTResult Execute()
    {
        BTResult result = BTResult.FAILURE;
        switch (operation)
        {
            case HasOp.PATH:
                result = context.navAgent.hasPath ? BTResult.SUCCESS : BTResult.FAILURE;
                break;
            case HasOp.PATH_TO_TARGET:
                if (!context.navAgent.enabled)
                {
                    result = BTResult.FAILURE;
                }
                else if (context.navAgent.hasPath && PathIsWithinToleranceToTarget())
                {
                    result = BTResult.SUCCESS;
                }
                else
                {
                    context.navAgent.ResetPath();
                    result = BTResult.FAILURE;
                }
                break;
            case HasOp.TARGET:
                result = context.contextOwner.currentTarget != null ? BTResult.SUCCESS : BTResult.FAILURE;
                break;
            case HasOp.STP:
                result = context.activeSmartTerrainPoint != null ? BTResult.SUCCESS : BTResult.FAILURE;
                break;
        }
        return result;
    }

    private bool PathIsWithinToleranceToTarget()
    {
        return (context.navAgent.pathEndPosition - context.contextOwner.currentTarget.GetPosition()).sqrMagnitude < destinationTolerance * destinationTolerance;
    }
}
