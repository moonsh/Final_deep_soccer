// Anthony Tiongson (ast119)

using System;
using System.Collections.Generic;
using UnityEngine;

public class CoachController : MonoBehaviour
{
    public static bool coachMode;
    public static HashSet<AIComponent> agentsWithUserActions = new HashSet<AIComponent>();
    public static HashSet<AIComponent> agentsUsingPastScenario = new HashSet<AIComponent>();
    public static Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();
    public static float countTime;
    public enum coachCommands
    {
        NONE,
        MOVE,
        KICK,
    }
    public GameObject BlackBoard;
    public GameObject BlackBoard2;
    [SerializeField] private string agentTag = "purpleAgent";
    [SerializeField] private string fieldTag = "field";
    [SerializeField] private string ballTag = "ball";
    [SerializeField] private string goalTag = "purpleGoal";
    [SerializeField] private GameObject purpleTeam;
    [SerializeField] private GameObject blueTeam;
    [SerializeField] private GameObject userActionsGUI;
    [SerializeField] private GameObject cancelUserActionsGUI;
    [SerializeField] private GameObject moveWaypointMarker;
    [SerializeField] private GameObject goToBallMarker;
    [SerializeField] private GameObject kickMarker;
    [SerializeField] private GameObject agentScenarioIndicator;
    [SerializeField] private Material defaultPurpleMaterial;
    [SerializeField] private Material defaultBlueMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material highlightMaterial;

    private Transform selectedPlayer;
    private Transform _selection;
    private coachCommands currentCommand;
    private Material defaultMaterial;
    private int count;

    public void ToggleCoachMode()
    {
        coachMode = !coachMode;
    }

    public void SetCommandModeMove()
    {
        currentCommand = coachCommands.MOVE;
    }

    public void SetCommandModeKick()
    {
        currentCommand = coachCommands.KICK;
    }

    public void ClearAllUserActions()
    {
        AIComponent[] currentAgentsWithUserActions = new AIComponent[agentsWithUserActions.Count];
        agentsWithUserActions.CopyTo(currentAgentsWithUserActions);

        foreach (var agent in currentAgentsWithUserActions)
        {
            agent.RemoveAllActions();
        }

        currentCommand = coachCommands.NONE;
    }

    private void CoachMoveMode(Ray ray, RaycastHit hit, AIComponent selectedAgent)
    {
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;

            if (selection.CompareTag(fieldTag) || selection.CompareTag(ballTag))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    //Debug.Log("Attempting to assign a new destination for the chosen agent.");

                    if (selection.CompareTag(ballTag))
                    {
                        var action = "GoToBall";
                        var pendingLabel = action; // Default label
                        Vector3 actionParameter = hit.point; // destination in this scenario
                        selectedAgent.AddAction(pendingLabel, action, goToBallMarker, actionParameter);

                        if (selectedAgent.pendingScenarios.Count == 0)
                        {
                            CreatePendingScenario(selectedAgent, pendingLabel, action, actionParameter);
                        }
                    }
                    else
                    {
                        var action = "Move"; // Movement scenario
                        var pendingLabel = action; // Default label
                        Vector3 actionParameter = hit.point; // destination in this scenario
                        selectedAgent.AddAction(pendingLabel, action, moveWaypointMarker, actionParameter);

                        if (selectedAgent.pendingScenarios.Count == 0)
                        {
                            CreatePendingScenario(selectedAgent, pendingLabel, action, actionParameter);
                        }

                        /*/ Testing code
                        Debug.Log("selectedAgent.pendingScenario: " + selectedAgent.pendingScenario.ToString());
                        Debug.Log("selectedAgent.pendingScenario.action: " + selectedAgent.pendingScenario.action);
                        Debug.Log("selectedAgent.pendingScenario.actionParameter: " + selectedAgent.pendingScenario.actionParameter.ToString());
                        Debug.Log("teammatePositions: ");

                        foreach (var teammatePosition in selectedAgent.pendingScenario.teammatePositions)
                        {
                            Debug.Log(teammatePosition.ToString());
                        }

                        Debug.Log("opponentPositions: ");

                        foreach (var opponentPosition in selectedAgent.pendingScenario.opponentPositions)
                        {
                            Debug.Log(opponentPosition.ToString());
                        }
                        */// End testing code
                    }

                    agentsWithUserActions.Add(selectedAgent);
                    userActionsGUI.SetActive(true);
                    BlackBoard.SetActive(true);
                    currentCommand = coachCommands.NONE;
                }
            }
        }
    }

    private void CoachKickMode(Ray ray, RaycastHit hit, AIComponent selectedAgent)
    {
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;

            if (selection.CompareTag(fieldTag) || selection.CompareTag(goalTag))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    string action = "Kick"; // Kick scenario
                    var label = action; // Default label
                    var labelSuffix = 2;

                    while (CoachController.scenarios.ContainsKey(label))
                    {
                        label = label + labelSuffix.ToString();
                        labelSuffix++;
                    }

                    Vector3 actionParameter = hit.point; // Target direction in this scenario
                    selectedAgent.AddAction(label, action, kickMarker, actionParameter);
                    agentsWithUserActions.Add(selectedAgent);
                    userActionsGUI.SetActive(true);
                    BlackBoard.SetActive(true);
                    currentCommand = coachCommands.NONE;
                }
            }
        }
    }


    private void CoachModeDisabledReset()
    {
        ResetMaterials();
        currentCommand = coachCommands.NONE;
    }

    private void CreatePendingScenario(AIComponent selectedAgent, string label, string action, Vector3 actionParameter)
    {
        Vector3 agentPosition = selectedAgent.GetComponent<Transform>().position;
        Vector3 ballPosition = selectedAgent.ball.position;
        GameObject[] teammates = selectedAgent.teammates;
        GameObject[] opponents = selectedAgent.opponents;
        HashSet<Vector3> teammatePositions = new HashSet<Vector3>();
        HashSet<Vector3> opponentPositions = new HashSet<Vector3>();
        bool ballPossessed = selectedAgent.ball.GetComponent<SoccerBallController>().owner;
        string teamWithBall;

        if (ballPossessed)
        {
            teamWithBall = selectedAgent.ball.GetComponent<SoccerBallController>().owner.tag;
        }
        else
        {
            teamWithBall = "None";
        }

        foreach (var teammate in teammates)
        {
            teammatePositions.Add(teammate.transform.position);
        }

        foreach (var opponent in opponents)
        {
            opponentPositions.Add(opponent.transform.position);
        }

        selectedAgent.pendingScenarios.Add((label, new Scenario(action, actionParameter, agentPosition,
        ballPosition, teammatePositions, opponentPositions, ballPossessed, teamWithBall)));
    }

    public void ToggleTeamSelection()
    {
        ResetMaterials();

        if (agentTag.Equals("purpleAgent"))
        {
            agentTag = "blueAgent";
            goalTag = "blueGoal";
            defaultMaterial = defaultBlueMaterial;
        }
        else
        {
            agentTag = "purpleAgent";
            goalTag = "purpleGoal";
            defaultMaterial = defaultPurpleMaterial;
        }
    }

    public void ResetMaterials()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
        }

        _selection = null;

        if (selectedPlayer != null)
        {
            var selectionRenderer = selectedPlayer.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            selectedPlayer = null;
            userActionsGUI.SetActive(false);
            BlackBoard.SetActive(false);
            BlackBoard2.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        userActionsGUI.SetActive(false);
        cancelUserActionsGUI.SetActive(false);
        coachMode = false;
        currentCommand = coachCommands.NONE;
        defaultMaterial = defaultPurpleMaterial;
        BlackBoard.SetActive(false);

        foreach (var agent in purpleTeam.GetComponentsInChildren<AIComponent>())
        {
            agent.AttachAgentScenarioIndicatorObject(agentScenarioIndicator);
        }

        foreach (var agent in blueTeam.GetComponentsInChildren<AIComponent>())
        {
            agent.AttachAgentScenarioIndicatorObject(agentScenarioIndicator);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 1)
        {
            BlackBoard2.SetActive(false);
        }

        count++;

        /*if (scenarios.Count > 0) // Debugging code.
        {
            Debug.Log("Saved scenarios detected.");
            Debug.Log("Number of scenarios: " + scenarios.Count);
            int count = 1;

            foreach (var scenario in scenarios)
            {
                Debug.Log("scenario" + count + ": " + scenario.ToString());
                Debug.Log("action: " + scenario.action);
                Debug.Log("actionParamater: " + scenario.actionParameter.ToString());
                Debug.Log("agentPosition: " + scenario.agentPosition.ToString());
                Debug.Log("ballPosition: " + scenario.ballPosition.ToString());

                Debug.Log("teammatePositions: ");

                foreach (var teammatePosition in scenario.teammatePositions)
                {
                    Debug.Log(teammatePosition.ToString());
                }

                Debug.Log("opponentPositions: ");

                foreach (var opponentPosition in scenario.opponentPositions)
                {
                    Debug.Log(opponentPosition.ToString());
                }

                count++;
            }
        }*/

        if (agentsWithUserActions.Count == 0)
        {
            cancelUserActionsGUI.SetActive(false);
        }
        else
        {
            cancelUserActionsGUI.SetActive(true);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            ToggleTeamSelection();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            switch (currentCommand)
            {
                case coachCommands.NONE:
                    if (selectedPlayer != null)
                    {
                        _selection = selectedPlayer;
                        selectedPlayer = null;
                        userActionsGUI.SetActive(false);
                        BlackBoard.SetActive(false);
                        BlackBoard2.SetActive(false);
                    }
                    else
                    {
                        ToggleCoachMode();
                    }
                    break;
                default:
                    userActionsGUI.SetActive(true);
                    BlackBoard.SetActive(true);
                    currentCommand = coachCommands.NONE;
                    break;
            }
        }

        if (coachMode) // coachMode enabled: player selection.
        {
            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();

                if (selectedPlayer != _selection)
                {
                    selectionRenderer.material = defaultMaterial;
                }
                else
                {
                    selectionRenderer.material = selectedMaterial;
                }
                
                _selection = null;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (selectedPlayer == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;

                    if (selection.CompareTag(agentTag))
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();

                        if (selectionRenderer != null)
                        {
                            selectionRenderer.material = highlightMaterial;

                            if (Input.GetButtonDown("Fire1"))
                            {
                                if (selectedPlayer == null)
                                {
                                    selectedPlayer = selection;
                                    var selection_x = selection.transform.position.x;
                                    var selection_z = selection.transform.position.z;
                                    if (selection_z <= 0)
                                    {
                                        userActionsGUI.transform.position = new Vector3(selection.transform.position.x, 75, selection.transform.position.z + 10);
                                    }
                                    else
                                    {
                                        userActionsGUI.transform.position = new Vector3(selection.transform.position.x, 75, selection.transform.position.z - 10);
                                    }
                                    userActionsGUI.SetActive(true);
                                    BlackBoard.SetActive(true);
                                    BlackBoard.GetComponent<BlackBoard>().SeletectedAgent = selectedPlayer;
                                }
                            }
                        }

                        _selection = selection;
                    }
                }
            }
            else // if there is a selected player
            {
                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;

                    if (selectedPlayer == selection)
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();

                        if (Input.GetButtonDown("Fire1"))
                        {
                            selectedPlayer = null;
                            userActionsGUI.SetActive(false);
                            BlackBoard.SetActive(false);
                            BlackBoard2.SetActive(false);
                        }

                        _selection = selection;
                    }
                }

                if (selectedPlayer != null)
                {
                    switch (currentCommand)
                    {
                        case coachCommands.NONE:
                            break;
                        case coachCommands.MOVE:
                            CoachMoveMode(ray, hit, selectedPlayer.GetComponent<AIComponent>());
                            break;
                        case coachCommands.KICK:
                            CoachKickMode(ray, hit, selectedPlayer.GetComponent<AIComponent>());
                            break;
                    }
                }
            }
        }
        else // coachMode disabled, reset all agent material that has been changed
        {
            CoachModeDisabledReset();
        }

        countTime += Time.deltaTime;
    }
}
