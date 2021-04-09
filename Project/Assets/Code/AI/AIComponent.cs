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
    public Scenario pendingScenario;
    public GameObject[] opponents; //forward opponents //??
    public GameObject[] teammates;
    public List<string> userActions = new List<string>();
    public List<GameObject> userActionMarkers = new List<GameObject>();
    public List<Vector3> userActionMoves = new List<Vector3>();
    //public CoachController coachController;
    //public RefereeController refereeController;

    internal AIState currentState = AIState.IDLE;
    internal IEventSource currentTarget;

    Animator animatorController;
    NavMeshAgent navAgent;
    BTContext aiContext;
    LineRenderer userActionPath;
    bool ballMarker;
    //BTContext aiCoachContext;
    //BTContext aiRefereeContext;

    public void AddAction(string userAction, GameObject marker=null)
    {
        userActions.Add(userAction);

        if (marker != null)
        {
            userActionMarkers.Add(marker);

            if (userAction.Equals("GoToBall"))
            {
                Instantiate(marker, ball.position, Quaternion.identity);
            }
        }
    }

    public void AddActionMove(Vector3 destination, GameObject waypoint)
    {
        Vector3 markerOffset = new Vector3(destination.x, 1, destination.z);
        userActionMoves.Add(destination);
        var newWaypoint = Instantiate(waypoint, markerOffset, Quaternion.identity);
        userActionMarkers.Add(newWaypoint);
    }

    public void DestroyMarker()
    {
        Destroy(userActionMarkers[0]);
        userActionMarkers.RemoveAt(0);
    }

    public void ClearAllActions()
    {
        userActions.Clear();
        userActionMoves.Clear();

        foreach (GameObject marker in userActionMarkers)
        {
            Destroy(marker);
        }

        userActionMarkers.Clear();
        userActionPath.positionCount = 0;
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aiContext = new BTContext(this, goal, ball, pendingScenario, animatorController, navAgent,
            rb, opponents, teammates, userActions, userActionMarkers, userActionMoves);

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
        ballMarker = false;
        userActionPath = gameObject.AddComponent<LineRenderer>();

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

        if (userActionMarkers.Count > 0)
        {
            int lengthOfLineRenderer = userActionMoves.Count + 1;
            LineRenderer userActionPath = GetComponent<LineRenderer>();
            userActionPath.startWidth = 0.1f;
            userActionPath.endWidth = 0.1f;
            userActionPath.positionCount = lengthOfLineRenderer;
            userActionPath.useWorldSpace = true;
            var points = new Vector3[lengthOfLineRenderer];
            points[0] = this.transform.position;

            for (int i = 0; i < lengthOfLineRenderer - 1; i++)
            {
                points[i + 1] = new Vector3 (userActionMoves[i].x, 1, userActionMoves[i].z);
            }

            userActionPath.SetPositions(points);
        }

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
