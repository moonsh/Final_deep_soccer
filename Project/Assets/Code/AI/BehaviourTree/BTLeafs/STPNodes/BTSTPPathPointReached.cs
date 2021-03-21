using UnityEngine;

[BTSmartTerrainPoint(typeof(BTSTPPathPointReached))]
public class BTSTPPathPointReached : BTNode
{
    public override BTResult Execute()
    {
        BTResult result = BTResult.FAILURE;

        SmartTerrainPoint currentTerrainPoint = context.activeSmartTerrainPoint;

        Vector3 agentPosition = context.contextOwner.transform.position;
        Vector3 terrainPointPosition = currentTerrainPoint.GetNextPathPoint();

        agentPosition.y = 0;
        terrainPointPosition.y = 0;

        if (!currentTerrainPoint.endPointReached && (agentPosition - terrainPointPosition).sqrMagnitude <= 0.1f)
        {
            currentTerrainPoint.OnPathPointReached();

            if (currentTerrainPoint.endPointReached && currentTerrainPoint.onPathEndTriggerName != "")
            {
                context.animatorController.SetTrigger(currentTerrainPoint.onPathEndTriggerName);
            }
            else if (!currentTerrainPoint.IsOnFirstPoint() && currentTerrainPoint.onPathPointReachedTriggerName != "")
            {
                context.animatorController.SetTrigger(currentTerrainPoint.onPathPointReachedTriggerName);
            }

            result = BTResult.SUCCESS;
        }

        return result;
    }
}
