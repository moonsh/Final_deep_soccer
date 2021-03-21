[BTAgent(typeof(BTIs))]
public class BTIs : BTNode
{
    public IsOp isOperation;

    public override BTResult Execute()
    {
        switch (isOperation)
        {
            case IsOp.IDLE:
                return context.contextOwner.currentState == AIState.IDLE ? BTResult.SUCCESS : BTResult.FAILURE;
            case IsOp.HOSTILE:
                return context.contextOwner.currentState == AIState.HOSTILE ? BTResult.SUCCESS : BTResult.FAILURE;
            default:
                break;
        }
        return BTResult.FAILURE;
    }
}
