[BTSmartTerrainPoint(typeof(BTWaitAtSTPPathPoint))]
public class BTWaitAtSTPPathPoint : BTNode, IBTWaitingNode
{
    public override BTResult Execute()
    {
        return BTResult.SUCCESS;
    }

    float IBTWaitingNode.GetWaitTime()
    {
        return context.activeSmartTerrainPoint.waitAtPathPointTime;
    }
}
