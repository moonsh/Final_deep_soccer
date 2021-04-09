using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario
{
    public string action;
    public Vector3 actionParameter;
    public string teamTag;
    public Vector3 agentPosition;
    public Vector3 ballPosition;
    public LinkedList<Vector3> teammatePositions = new LinkedList<Vector3>();
    public LinkedList<Vector3> opponentPositions = new LinkedList<Vector3>();

    public Scenario(string _action, Vector3 _actionParameter, string _teamTag,
        Vector3 _agentPosition, Vector3 _ballPosition,
        LinkedList<Vector3> _teammatePositions, LinkedList<Vector3> _opponentPositions)
    {
        action = _action;
        actionParameter = _actionParameter;
        teamTag = _teamTag;
        agentPosition = _agentPosition;
        ballPosition = _ballPosition;
        teammatePositions = _teammatePositions;
        opponentPositions = _opponentPositions;
    }
}
