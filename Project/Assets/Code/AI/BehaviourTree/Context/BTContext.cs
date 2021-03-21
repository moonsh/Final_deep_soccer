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

    public SmartTerrainPoint activeSmartTerrainPoint;

#if UNITY_EDITOR
    public List<string> behaviourHistory = new List<string>();
#endif //UNITY EDITOR

    public BTContext(AIComponent _owner, Animator _animatorController, NavMeshAgent _navAgent)
    {
        contextOwner = _owner;
        animatorController = _animatorController;
        navAgent = _navAgent;
    }
}
