using UnityEngine;

[BTSmartTerrainPoint(typeof(BTWalkToSTP))]

public class BTWalkToSTP : BTNode
{
    public override BTResult Execute()
    {
        BTResult result = BTResult.SUCCESS;

        SmartTerrainPoint currentTerrainPoint = context.activeSmartTerrainPoint;

        Vector3 agentPosition = context.contextOwner.transform.position;
        Vector3 terrainPointPosition = currentTerrainPoint.GetNextPathPoint();

        agentPosition.y = 0;
        terrainPointPosition.y = 0;

        if (!currentTerrainPoint.stpStarted)
        {
            if ((agentPosition - terrainPointPosition).sqrMagnitude < 0.1f)
            {
                currentTerrainPoint.stpStarted = true;
                currentTerrainPoint.OnPathPointReached();
                context.navAgent.SetDestination(currentTerrainPoint.GetNextPathPoint());
            }
        }
        else result = BTResult.FAILURE;

        return result;
    }
}
