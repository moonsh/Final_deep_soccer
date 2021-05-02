// Anthony Tiongson (ast119)
// Resources: MARL soccer

using System;
using System.Collections.Generic;
using TMPro;
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
    public List<(string, string, GameObject)> userActions = new List<(string, string, GameObject)>();
    public List<(string, Scenario)> pendingScenarios = new List<(string, Scenario)>();
    public Tuple<string, Scenario> pastScenario;
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
    GameObject agentScenarioIndicator;
    GameObject agentScenarioIndicatorObject;
    Transform targetTeammate;
    bool ballMarkerVisible;
    bool passMarkerVisible;
    bool agentScenarioIndicatorVisible;
    //BTContext aiCoachContext;
    //BTContext aiRefereeContext;

    public void AddAction(string label, string userAction, GameObject marker,
        Vector3? actionParameter = null)
    {
        GameObject newMarker;

        switch (userAction)
        {
            case "Move":
                var destination = (Vector3)actionParameter;
                var markerOffset = new Vector3(destination.x, 1, destination.z);
                var newWaypoint = Instantiate(marker, markerOffset, Quaternion.identity);
                newWaypoint.layer = LayerMask.NameToLayer("Ignore Raycast");
                newMarker = newWaypoint;
                break;
            case "GoToBall":
                ballMarkerVisible = true;
                markerOffset = new Vector3(ball.position.x, 1, ball.position.y);
                var markerRotation = Quaternion.identity;
                markerRotation.eulerAngles = new Vector3(90, 45, 0);
                ballMarker = Instantiate(marker, markerOffset, markerRotation);
                ballMarker.layer = LayerMask.NameToLayer("Ignore Raycast");
                newMarker = ballMarker;
                break;
            case "Kick":
                var point = (Vector3)actionParameter;
                markerOffset = new Vector3(point.x, 1, point.z);
                markerRotation = Quaternion.identity;
                markerRotation.eulerAngles = new Vector3(90, 0, 0);
                var kickDirection = Instantiate(marker, markerOffset, markerRotation);
                kickDirection.layer = LayerMask.NameToLayer("Ignore Raycast"); ;
                newMarker = kickDirection;
                break;
            case "Pass":
                point = (Vector3)actionParameter;
                markerOffset = new Vector3(point.x, 1, point.z);
                markerRotation = Quaternion.identity;
                markerRotation.eulerAngles = new Vector3(90, 0, 0);
                kickDirection = Instantiate(marker, markerOffset, markerRotation);
                kickDirection.layer = LayerMask.NameToLayer("Ignore Raycast"); ;
                newMarker = kickDirection;
                break;
            default:
                newMarker = null;
                break;
        }

        if (newMarker != null)
        {
            userActions.Add((label, userAction, newMarker));
        }
        else
        {
            Debug.Log("AIComponent error with AddAction: null newMarker.");
        }
    }

    public void AttachAgentScenarioIndicatorObject(GameObject _agentScenarioindicatorObject)
    {
        agentScenarioIndicatorObject = _agentScenarioindicatorObject;
    }

    /*public void AddActionMove(Vector3 destination, GameObject waypoint)
    {
        Vector3 markerOffset = new Vector3(destination.x, 1, destination.z);
        userActionMoves.Add(destination);
        var newWaypoint = Instantiate(waypoint, markerOffset, Quaternion.identity);
        userActionMarkers.Add(newWaypoint);
    }*/

    public void CreateAgentScenarioIndicator(string label)
    {
        var textRotation = Quaternion.identity;
        textRotation.eulerAngles = new Vector3(90, 0, 90);
        var location = transform.position;
        var textOffset = new Vector3(location.x, 70, location.z);
        agentScenarioIndicator = Instantiate(agentScenarioIndicatorObject, textOffset, textRotation);
        agentScenarioIndicator.GetComponentInChildren<TextMeshProUGUI>().text = label;
        agentScenarioIndicatorVisible = true;
    }

    public string GetAgentScenarioIndicatorValue()
    {
        return agentScenarioIndicator.GetComponentInChildren<TextMeshProUGUI>().text;
    }

    public bool IsAgentScenarioIndicatorVisible()
    {
        return agentScenarioIndicatorVisible;
    }

    public void RemoveAction()
    {
        Destroy(userActions[0].Item3);

        // Special case checks.
        // Check to see if the marker action type creates a pending scenario, then remove it.
        if (userActions[0].Item2.Equals("Move") || userActions[0].Item2.Equals("GoToBall"))
        {
            if (pendingScenarios.Count > 0)
            {
                pendingScenarios.RemoveAt(0);
            }
        }
        // Check to see if the marker action type is of "GoToBall" to disable ballMarker.
        if (userActions[0].Item2.Equals("GoToBall"))
        {
            ballMarkerVisible = false;
        }

        userActions.RemoveAt(0);

        if (userActions.Count == 0)
        {
            userActionPath.positionCount = 0;
            CoachController.agentsWithUserActions.Remove(this);
        }
    }

    public void RemoveAgentScenarioIndicator()
    {
        if (agentScenarioIndicatorVisible)
        {
            Destroy(agentScenarioIndicator);
            CoachController.agentsUsingPastScenario.Remove(this);
            agentScenarioIndicatorVisible = false;
        }
    }

    public void RemoveAllActions()
    {
        var count = userActions.Count;

        for (var i = 0; i < count; i++)
        {
            RemoveAction();
        }
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aiContext = new BTContext(this, goal, ball, animatorController, navAgent,
            rb, opponents, teammates, userActions, pendingScenarios, pastScenario);

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
        passMarkerVisible = false;
        agentScenarioIndicatorVisible = false;
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
        var currentBallPosition = ball.position;

        if (agentScenarioIndicatorVisible)
        {
            var location = transform.position;
            var textOffset = new Vector3(location.x, 70, location.z);
            agentScenarioIndicator.transform.position = textOffset;
        }

        if (ballMarkerVisible)
        {
            var markerOffset = new Vector3(currentBallPosition.x, 1, currentBallPosition.z);
            ballMarker.transform.position = markerOffset;
        }

        if (userActions.Count > 0 && userActions[0].Item3 != null)
        {
            int lengthOfLineRenderer = userActions.Count + 1;
            LineRenderer userActionPath = GetComponent<LineRenderer>();
            userActionPath.startWidth = 0.1f;
            userActionPath.endWidth = 0.1f;
            userActionPath.positionCount = lengthOfLineRenderer;
            userActionPath.useWorldSpace = true;
            var points = new Vector3[lengthOfLineRenderer];
            points[0] = this.transform.position;

            for (int i = 0; i < lengthOfLineRenderer - 1; i++)
            {
                string action = userActions[i].Item2;

                switch (action)
                {
                    case "Move":
                        var destination = userActions[i].Item3.transform.position;
                        points[i + 1] = new Vector3(destination.x, 1, destination.z);
                        break;
                    case "GoToBall":
                        points[i + 1] = new Vector3(currentBallPosition.x, 1, currentBallPosition.z);
                        break;
                    case "Kick":
                        var target = userActions[i].Item3.transform.position;
                        points[i + 1] = new Vector3(target.x, 1, target.z);
                        break;
                    case "Pass":
                        target = targetTeammate.position;
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
