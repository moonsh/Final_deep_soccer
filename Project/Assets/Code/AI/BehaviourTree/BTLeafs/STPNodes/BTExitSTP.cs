[BTSmartTerrainPoint(typeof(BTExitSTP))]
public class BTExitSTP : BTNode
{
    public override BTResult Execute()
    {
        SmartTerrainPointManager.OnSmartTerrainPointExit(context.activeSmartTerrainPoint);
        context.activeSmartTerrainPoint.OnExit();
        context.activeSmartTerrainPoint = null;
        context.navAgent.ResetPath();
        return BTResult.SUCCESS;
    }
}
