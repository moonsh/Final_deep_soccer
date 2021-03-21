[BTAgent(typeof(BTSetMoveState))]
public class BTSetMoveState : BTNode
{
    public MovementState desiredMoveState;
    public override BTResult Execute()
    {
        context.animatorController.SetInteger(BTDefs.MOVEMENT_STATE, (int)desiredMoveState);
        return BTResult.SUCCESS;
    }
}
