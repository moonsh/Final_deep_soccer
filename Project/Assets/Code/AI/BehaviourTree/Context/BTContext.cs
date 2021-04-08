// Anthony Tiongson (ast119)
// Modified from MARL soccer

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class BTContext
{
    public AIComponent contextOwner;
    public Animator animatorController;
    public NavMeshAgent navAgent;
    public Rigidbody rb;
    public Transform goal;
    public Transform ball;
    public Scenario pendingScenario;
    public Transform soccer; // What is this used for?
    public GameObject[] opponents;
    public GameObject[] teammates;
    public GameObject threatOpponent; // What is this used for?
    public List<string> userActions = new List<string>();
    public List<GameObject> userActionMarkers = new List<GameObject>();
    public List<Vector3> userActionMoves = new List<Vector3>();

    /*public CoachController coachController;
    public RefereeController refereeController;*/
    public SmartTerrainPoint activeSmartTerrainPoint; // Consider removing.

#if UNITY_EDITOR
    public List<string> behaviourHistory = new List<string>();
#endif //UNITY EDITOR

    public BTContext(AIComponent _owner, Transform _goal, Transform _ball,
        Scenario _pendingScenario, Animator _animatorController,
        NavMeshAgent _navAgent, Rigidbody _rb, GameObject[] _opponents,
        GameObject[] _teammates, List<string> _userActions,
        List<GameObject> _userActionMarkers, List<Vector3> _userActionMoves)
    {
        contextOwner = _owner;
        goal = _goal;
        ball = _ball;
        pendingScenario = _pendingScenario;
        animatorController = _animatorController;
        navAgent = _navAgent;
        rb = _rb;
        opponents = _opponents;
        teammates = _teammates;
        userActions = _userActions;
        userActionMarkers = _userActionMarkers;
        userActionMoves = _userActionMoves;
    }
}
