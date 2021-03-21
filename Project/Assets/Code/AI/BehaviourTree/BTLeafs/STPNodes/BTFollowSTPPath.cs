using UnityEngine;

[BTSmartTerrainPoint(typeof(BTFollowSTPPath))]
public class BTFollowSTPPath : BTNode
{
    public override BTResult Execute()
    {
        BTResult result = BTResult.SUCCESS;

        SmartTerrainPoint currentTerrainPoint = context.activeSmartTerrainPoint;
        Vector3 terrainPointPosition = currentTerrainPoint.GetNextPathPoint();
        terrainPointPosition.y = 0;

        Vector3 currentDestination = context.navAgent.pathEndPosition;
        currentDestination.y = 0;

        if ((terrainPointPosition - currentDestination).sqrMagnitude > 0.1f) 
        {
            context.navAgent.SetDestination(terrainPointPosition);
        }

        Vector3 agentPosition = context.contextOwner.transform.position;
        agentPosition.y = 0;
        if (currentTerrainPoint.pathFollowType == SmartTerrainPoint.STPPathFollowType.EXIT_ON_PATH_END &&
            currentTerrainPoint.IsOnLastPoint() && 
            (agentPosition - terrainPointPosition).sqrMagnitude < 0.1f) 
        {
            result = BTResult.FAILURE;
        }
        return result;
    }
}
