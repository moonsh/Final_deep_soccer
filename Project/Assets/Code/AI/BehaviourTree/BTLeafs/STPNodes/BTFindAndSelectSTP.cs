using UnityEngine.AI;

[BTSmartTerrainPoint(typeof(BTFindAndSelectSTP))]
public class BTFindAndSelectSTP : BTNode
{
    public override BTResult Execute()
    {
        BTResult result = BTResult.FAILURE;
        SmartTerrainPoint terrainPoint = SmartTerrainPointManager.SelectNearestReachableTerrainPoint(context.contextOwner, out NavMeshPath _path);

        if (terrainPoint != null)
        {
            context.activeSmartTerrainPoint = terrainPoint;
            context.navAgent.SetPath(_path);
            result = BTResult.SUCCESS;
        }

        return result;
    }
}
