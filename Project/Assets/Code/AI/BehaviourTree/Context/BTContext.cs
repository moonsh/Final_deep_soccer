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
    public GameObject threatOpponent;

    public SmartTerrainPoint activeSmartTerrainPoint;

#if UNITY_EDITOR
    public List<string> behaviourHistory = new List<string>();
#endif //UNITY EDITOR

    public BTContext(AIComponent _owner, Animator _animatorController, NavMeshAgent _navAgent, Rigidbody _rb,  Transform _goal, Transform _ball,  GameObject[] _opponents,
        GameObject[] _teammates)
    {
        contextOwner = _owner;
        animatorController = _animatorController;
        navAgent = _navAgent;
        rb = _rb;
        goal = _goal;
        ball = _ball;
        opponents = _opponents;
        teammates = _teammates;
    }
}
