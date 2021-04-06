// Anthony Tiongson (ast119)
// Resources: MARL soccer

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(BehaviourTreeType))]
public class AIComponent : MonoBehaviour, IEventSource
{
    public BehaviourTreeType behaviourTreeType;
    public SensorySystem sensorySystem;
    public AIEventHandler eventHandler;
    public Rigidbody rb;
    public Transform goal;
    public Transform ball;
    public GameObject[] opponents; //forward opponents //??
    public GameObject[] teammates;
    public List<string> userActions = new List<string>();
    public List<Vector3> userActionMoves = new List<Vector3>();
    //public CoachController coachController;
    //public RefereeController refereeController;

    internal AIState currentState = AIState.IDLE;
    internal IEventSource currentTarget;

    Animator animatorController;
    NavMeshAgent navAgent;
    BTContext aiContext;
    //BTContext aiCoachContext;
    //BTContext aiRefereeContext;

    public List<string> GetUserActionList()
    {
        return userActions;
    }

    public void AddAction(string userAction)
    {
        userActions.Add(userAction);
    }

    public void AddActionMove(Vector3 destination)
    {
        userActionMoves.Add(destination);
    }

    public void ClearAllActions()
    {
        userActions.Clear();
        userActionMoves.Clear();
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aiContext = new BTContext(this, goal, ball, animatorController, navAgent,
            rb, opponents, teammates, userActions, userActionMoves);

        /*navAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponent<Animator>();

        aiContext = new BTContext(this, animatorController, navAgent, rb, goal, ball, opponents, teammates);*/

        /*if (behaviourTreeType.Equals(BehaviourTreeType.PLAYER))
        {
            navAgent = GetComponent<NavMeshAgent>();
            animatorController = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            aiContext = new BTContext(this, goal, ball, animatorController, navAgent,
                rb, opponents, teammates);
        }

        if (behaviourTreeType.Equals(BehaviourTreeType.COACH))
        {
            coachController = GetComponent<CoachController>();
            aiCoachContext = new BTContext(this, goal, ball, null, null, null, null,
                null, coachController);
        }

        if (behaviourTreeType.Equals(BehaviourTreeType.REFEREE))
        {
            refereeController = GetComponent<RefereeController>();
            aiRefereeContext = new BTContext(this, goal, ball, null, null, null, null,
                null, null, refereeController);
        }*/
    }

    private void Start()
    {
        sensorySystem.Initialize(this, navAgent);
        eventHandler.Initialize(this, animatorController, navAgent);
        BehaviourTreeRuntimeData.RegisterAgentContext(behaviourTreeType, aiContext);

        /*if (behaviourTreeType.Equals(BehaviourTreeType.PLAYER))
        {
            sensorySystem.Initialize(this, navAgent);
            eventHandler.Initialize(this, animatorController, navAgent);
            BehaviourTreeRuntimeData.RegisterAgentContext(behaviourTreeType, aiContext);
        }
        if (behaviourTreeType.Equals(BehaviourTreeType.COACH))
        {
            BehaviourTreeRuntimeData.RegisterAgentContext(behaviourTreeType, aiCoachContext);
        }
        if (behaviourTreeType.Equals(BehaviourTreeType.REFEREE))
        {
            BehaviourTreeRuntimeData.RegisterAgentContext(behaviourTreeType, aiRefereeContext);
        }*/
    }

    void Update()
    {
        sensorySystem.Update();
        eventHandler.Update();

        /*if (behaviourTreeType.Equals(BehaviourTreeType.PLAYER))
        {
            sensorySystem.Update();
            eventHandler.Update();
        }*/
    }

    void OnDestroy()
    {
        eventHandler.OnDestroy();
        BehaviourTreeRuntimeData.UnregisterAgentContext(behaviourTreeType, aiContext);

        /*if (behaviourTreeType.Equals(BehaviourTreeType.PLAYER))
        {
            eventHandler.OnDestroy();
            BehaviourTreeRuntimeData.UnregisterAgentContext(behaviourTreeType, aiContext);
        }
        if (behaviourTreeType.Equals(BehaviourTreeType.COACH))
        {
            BehaviourTreeRuntimeData.UnregisterAgentContext(behaviourTreeType, aiCoachContext);
        }
        if (behaviourTreeType.Equals(BehaviourTreeType.REFEREE))
        {
            BehaviourTreeRuntimeData.UnregisterAgentContext(behaviourTreeType, aiRefereeContext);
        }*/
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
