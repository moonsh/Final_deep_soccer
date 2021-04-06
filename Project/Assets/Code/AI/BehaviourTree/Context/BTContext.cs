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
    public Transform soccer;
    public GameObject[] opponents;
    public GameObject[] teammates;
    public GameObject threatOpponent; // What is this used for?
    public List<string> userActions;
    public List<Vector3> userActionMoves;

    /*public CoachController coachController;
    public RefereeController refereeController;*/
    public SmartTerrainPoint activeSmartTerrainPoint; // Consider removing.

#if UNITY_EDITOR
    public List<string> behaviourHistory = new List<string>();
#endif //UNITY EDITOR

    public BTContext(AIComponent _owner, Transform _goal, Transform _ball,
        Animator _animatorController, NavMeshAgent _navAgent,
        Rigidbody _rb, GameObject[] _opponents, GameObject[] _teammates,
        List<string> _userActions, List<Vector3> _userActionMoves)
    {
        contextOwner = _owner;
        goal = _goal;
        ball = _ball;
        animatorController = _animatorController;
        navAgent = _navAgent;
        rb = _rb;
        opponents = _opponents;
        teammates = _teammates;
        userActions = _userActions;
        userActionMoves = _userActionMoves;
    }
}
