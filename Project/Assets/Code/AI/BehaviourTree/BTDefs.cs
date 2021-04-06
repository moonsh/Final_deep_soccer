// Anthony Tiongson (ast119)

using System;
using System.Reflection;
using static XNode.Node;

public static class BTDefs
{
    //public const string stpMenuName = "SmartTerrainPoint";
    public const string MOVEMENT_STATE = "MoveState";
    // Consider mondioring the current game state
    public const string GAME_STATE = "GameState";
}

// BehaviourTreeType PLAYER
public enum MovementState
{
    IDLE,
    WALK,
    RUN,
}

// BehaviourTreeType REFEREE
public enum GameState
{
    COACHMODE,
    KICKOFF,
    BALLOUTOFPLAY,
    BALLINPLAY,
    GAMEOVER
}

public enum BTResult
{
    SUCCESS,
    FAILURE,
    XRUNNING_DO_NOT_USE // LEAF NODES SHOULD NEVER RETURN RUNNING! BEHAVIOUR TREE IS NOT MULTITHREADED
}

//Add new types to the bottom of this enum, before COUNT
//Failing to do so will reorder the enum and mess up the values you set in the scene!!
// Consider 4 BehaviorTreeTypes: PLAYER, TEAM, COACH, REFEREE
public enum BehaviourTreeType
{
    //GUARD,
    //Forward, // Consider PLAYER agent instead of forward, middle, back
    //Middle,
    //Back,
    PLAYER,
    TEAM,
    COACH, // Agent to monitor user input for interactive strategy
    REFEREE, // Agent to officiate the game
    //Add stuff here
    COUNT,
}

// Consider for -
//  Player agent: NEARBY_BALL
//  Player/Team agent: POSSESSION
public enum HasOp
{
    PATH,
    PATH_TO_TARGET,
    TARGET,
    STP, // Consider removing
}

// Consider for -
//  Player agent: ATTACKING, DEFENDING
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

/* Consider an enumeration of available soccer strategis
// Team agent decides
public enum OffensiveStrategy
{
    BASIC_ATTACK,
    POSSESSION,
    COUNTER_ATTACK,
}

public enum DefensiveStrategy
{
    BASIC_DEFEND,
    CLOSING_DOWN,
    OFFSIDE_TRAP,
}*/


/* Consider an enumeration of available soccer formations
// Team agent decides
public enum Formation
{
    FORM442,
    FORM443
}*/

public class BTCompositeAttribute : CreateNodeMenuAttribute
{
    public BTCompositeAttribute(Type _type)
    {
        menuName = "Composites/" + _type.ToString();
    }
}

// Consider 4 kinds of agents: Player, Team, Coach, Referee
public class BTAgentAttribute : CreateNodeMenuAttribute
{
    public BTAgentAttribute(Type _type)
    {
        menuName = "Player/" + _type.ToString(); // Previously Agent
    }
}

public class BTTeamAttribute : CreateNodeMenuAttribute
{
    public BTTeamAttribute(Type _type)
    {
        menuName = "Team/" + _type.ToString();
    }
}

public class BTCoachAttribute : CreateNodeMenuAttribute
{
    public BTCoachAttribute(Type _type)
    {
        menuName = "Coach/" + _type.ToString();
    }
}

public class BTRefereeAttribute : CreateNodeMenuAttribute
{
    public BTRefereeAttribute(Type _type)
    {
        menuName = "Referee/" + _type.ToString();
    }
}

// Consider removing
public class BTSmartTerrainPointAttribute : CreateNodeMenuAttribute
{
    public BTSmartTerrainPointAttribute(Type _type)
    {
        menuName = "SmartTerrainPoint/" + _type.ToString();
    }
}
