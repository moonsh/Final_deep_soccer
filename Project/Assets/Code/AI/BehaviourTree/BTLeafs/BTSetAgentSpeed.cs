[BTAgent(typeof(BTSetAgentSpeed))]
public class BTSetAgentSpeed : BTNode
{
    public float desiredSpeed;
    public override BTResult Execute()
    {
        context.navAgent.speed = desiredSpeed;
        return BTResult.SUCCESS;
    }
}
