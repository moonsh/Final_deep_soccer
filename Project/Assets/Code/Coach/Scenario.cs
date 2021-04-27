using System.Collections.Generic;
using UnityEngine;

public class Scenario
{
    public string action;
    public Vector3 actionParameter;
    public Vector3 agentPosition;
    public Vector3 ballPosition;
    public HashSet<Vector3> teammatePositions = new HashSet<Vector3>();
    public HashSet<Vector3> opponentPositions = new HashSet<Vector3>();
    public bool ballPossessed;
    public string teamWithBall;
    public double reward;

    public Scenario(string _action, Vector3 _actionParameter,
        Vector3 _agentPosition, Vector3 _ballPosition,
        HashSet<Vector3> _teammatePositions, HashSet<Vector3> _opponentPositions,
        bool _ballPossessed, string _teamWithBall, double _reward)
    {
        action = _action;
        actionParameter = _actionParameter;
        teamWithBall = _teamWithBall;
        agentPosition = _agentPosition;
        ballPosition = _ballPosition;
        teammatePositions = _teammatePositions;
        opponentPositions = _opponentPositions;
        ballPossessed = _ballPossessed;
        reward = _reward;
    }
}
