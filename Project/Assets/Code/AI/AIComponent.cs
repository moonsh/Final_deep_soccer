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
    public List<GameObject> userActionMarkers = new List<GameObject>();
    public List<Scenario> pendingScenarios = new List<Scenario>();
    public List<Scenario> pastScenarios = new List<Scenario>();
    //public List<Vector3> userActionMoves = new List<Vector3>();
    //public CoachController coachController;
    //public RefereeController refereeController;

    internal AIState currentState = AIState.IDLE;
    internal IEventSource currentTarget;

    Animator animatorController;
    NavMeshAgent navAgent;
    BTContext aiContext;
    LineRenderer userActionPath;
    GameObject ballMarker;
    bool ballMarkerVisible;
    //BTContext aiCoachContext;
    //BTContext aiRefereeContext;

    public void AddAction(string userAction, GameObject marker,
        Vector3? actionParameter = null)
    {
        userActions.Add(userAction);
 
        switch (userAction)
        {
            case "Move":
                var destination = (Vector3)actionParameter;
                var markerOffset = new Vector3(destination.x, 1, destination.z);
                var newWaypoint = Instantiate(marker, markerOffset, Quaternion.identity);
                newWaypoint.layer = LayerMask.NameToLayer("Ignore Raycast");
                userActionMarkers.Add(newWaypoint);
                break;
            case "GoToBall":
                ballMarkerVisible = true;
                markerOffset = new Vector3(ball.position.x, 1, ball.position.y);
                var markerRotation = Quaternion.identity;
                markerRotation.eulerAngles = new Vector3(90, 45, 0);
                ballMarker = Instantiate(marker, markerOffset, markerRotation);
                ballMarker.layer = LayerMask.NameToLayer("Ignore Raycast");
                userActionMarkers.Add(ballMarker);
                break;
            case "Kick":
                var point = (Vector3)actionParameter;
                markerOffset = new Vector3(point.x, 1, point.z);
                markerRotation = Quaternion.identity;
                markerRotation.eulerAngles = new Vector3(90, 0, 0);
                var kickDirection = Instantiate(marker, markerOffset, markerRotation);
                kickDirection.layer = LayerMask.NameToLayer("Ignore Raycast"); ;
                userActionMarkers.Add(kickDirection);
                break;
        }
    }

    /*public void AddActionMove(Vector3 destination, GameObject waypoint)
    {
        Vector3 markerOffset = new Vector3(destination.x, 1, destination.z);
        userActionMoves.Add(destination);
        var newWaypoint = Instantiate(waypoint, markerOffset, Quaternion.identity);
        userActionMarkers.Add(newWaypoint);
    }*/

    public void DestroyMarker()
    {
        Destroy(userActionMarkers[0]);
        userActionMarkers.RemoveAt(0);

        if (userActionMarkers.Count == 0)
        {
            userActionPath.positionCount = 0;
            CoachController.agentsWithUserActions.Remove(this);
        }

        // Special case checks.
        // Check to see if the marker action type creates a pending scenario, then remove it.
        if (userActions[0].Equals("Move") || userActions[0].Equals("GoToBall"))
        {
            pendingScenarios.RemoveAt(0);
        }
        // Check to see if the marker action type is of "GoToBall" to disable ballMarker.
        if (userActions[0].Equals("GoToBall"))
        {
            ballMarkerVisible = false;
        }

        userActions.RemoveAt(0);
    }

    public void ClearAllActions()
    {
        ballMarkerVisible = false;
        userActions.Clear();
        pendingScenarios.Clear();

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
        aiContext = new BTContext(this, goal, ball, animatorController, navAgent,
            rb, opponents, teammates, userActions, userActionMarkers, pendingScenarios, pastScenarios);

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
        ballMarkerVisible = false;
        sensorySystem.Initialize(this, navAgent);
        eventHandler.Initialize(this, animatorController, navAgent);
        BehaviourTreeRuntimeData.RegisterAgentContext(behaviourTreeType, aiContext);
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
        if (ballMarkerVisible)
        {
            var markerOffset = new Vector3(ball.position.x, 1, ball.position.z);
            ballMarker.transform.position = markerOffset;
        }

        if (userActionMarkers.Count > 0)
        {
            int lengthOfLineRenderer = userActionMarkers.Count + 1;
            LineRenderer userActionPath = GetComponent<LineRenderer>();
            userActionPath.startWidth = 0.1f;
            userActionPath.endWidth = 0.1f;
            userActionPath.positionCount = lengthOfLineRenderer;
            userActionPath.useWorldSpace = true;
            var points = new Vector3[lengthOfLineRenderer];
            points[0] = this.transform.position;

            for (int i = 0; i < lengthOfLineRenderer - 1; i++)
            {
                string action = userActions[i];

                switch (action)
                {
                    case "Move":
                        var destination = userActionMarkers[i].gameObject.transform.position;
                        points[i + 1] = new Vector3(destination.x, 1, destination.z);
                        break;
                    case "GoToBall":
                        destination = ball.position;
                        points[i + 1] = new Vector3(destination.x, 1, destination.z);
                        break;
                    case "Kick":
                        var target = userActionMarkers[i].gameObject.transform.position;
                        points[i + 1] = new Vector3(target.x, 1, target.z);
                        break;
                }
            }

            userActionPath.SetPositions(points);
        }

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
