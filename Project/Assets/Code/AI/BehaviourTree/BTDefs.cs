using System;
using System.Reflection;
using static XNode.Node;

public static class BTDefs
{
    public const string stpMenuName = "SmartTerrainPoint";
    public const string MOVEMENT_STATE = "MoveState";

}


public enum MovementState
{
    IDLE,
    WALK,
    RUN,
}

public enum BTResult
{
    SUCCESS,
    FAILURE,
    XRUNNING_DO_NOT_USE // LEAF NODES SHOULD NEVER RETURN RUNNING! BEHAVIOUR TREE IS NOT MULTITHREADED
}

//Add new types to the bottom of this enum, before COUNT
//Failing to do so will reorder the enum and mess up the values you set in the scene!!
public enum BehaviourTreeType
{
    GUARD,
    LUMBERJACK,
    ANIMAL_IDLE,
    //Add stuff here
    COUNT,
}

public enum HasOp
{
    PATH,
    PATH_TO_TARGET,
    TARGET,
    STP,
}

public enum IsOp
{
    IDLE,
    HOSTILE,
}

public enum PathType
{
    TARGET,
    RANDOM
}

public class BTCompositeAttribute : CreateNodeMenuAttribute
{
    public BTCompositeAttribute(Type _type)
    {
        menuName = "Composites/" + _type.ToString();
    }
}

public class BTAgentAttribute : CreateNodeMenuAttribute
{
    public BTAgentAttribute(Type _type)
    {
        menuName = "Agent/" + _type.ToString();
    }
}

public class BTSmartTerrainPointAttribute : CreateNodeMenuAttribute
{
    public BTSmartTerrainPointAttribute(Type _type)
    {
        menuName = "SmartTerrainPoint/" + _type.ToString();
    }
}
