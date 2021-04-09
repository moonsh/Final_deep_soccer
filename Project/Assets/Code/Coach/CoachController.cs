// Anthony Tiongson (ast119)

using System;
using System.Collections.Generic;
using UnityEngine;

public class CoachController : MonoBehaviour
{
    public static bool coachMode;
    public static HashSet<AIComponent> agentsWithUserActions = new HashSet<AIComponent>();
    public static List<Scenario> scenarios = new List<Scenario>();
    public List<GameObject> purpleAgents = new List<GameObject>();
    public List<GameObject> blueAgents = new List<GameObject>();
    public Transform ball;
    [SerializeField] private string agentTag = "purpleAgent";
    [SerializeField] private string fieldTag = "field";
    [SerializeField] private string ballTag = "ball";
    [SerializeField] private GameObject userActionsGUI;
    [SerializeField] private GameObject cancelUserActionsGUI;
    [SerializeField] private GameObject waypoint;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    private Transform[] selectedPlayer = new Transform[1];
    private Transform _selection;
    private bool coachMoveMode;

    public bool GetCoachMode()
    {
        return coachMode;
    }

    public void ToggleCoachMode()
    {
        coachMode = !coachMode;
    }

    public void ToggleCoachModeMove()
    {
        coachMoveMode = !coachMoveMode;
    }

    public void ClearAllUSerActions()
    {
        foreach (var agent in agentsWithUserActions)
        {
            agent.GetComponent<AIComponent>().ClearAllActions();
        }

        agentsWithUserActions.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        userActionsGUI.SetActive(false);
        cancelUserActionsGUI.SetActive(false);
        coachMode = false;
        coachMoveMode = false;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetButtonDown("Cancel"))
        {
            ToggleCoachMode();
        }

        if (coachMode) // coachMode enabled.
        {
            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();

                if (Array.IndexOf(selectedPlayer, _selection) != 0)
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

            if (selectedPlayer[0] == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;

                    if (selection.CompareTag(agentTag))
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();

                        if (selectionRenderer != null)
                        {
                            if (Array.IndexOf(selectedPlayer, selection) != 0)
                            {
                                selectionRenderer.material = highlightMaterial;
                            }

                            if (Input.GetButtonDown("Fire1"))
                            {
                                if (selectedPlayer[0] == null)
                                {
                                    selectedPlayer[0] = selection;
                                    userActionsGUI.SetActive(true);
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

                    if (selectedPlayer[0] == selection)
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();

                        if (Input.GetButtonDown("Fire1"))
                        {
                            Array.Clear(selectedPlayer, 0, 1);
                            userActionsGUI.SetActive(false);
                        }

                        _selection = selection;
                    }
                }

                if (coachMoveMode)
                {
                    var selectedAgent = selectedPlayer[0].GetComponent<AIComponent>();

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
                                    selectedAgent.AddAction("GoToBall");
                                }
                                else
                                {
                                    string action = "Move"; // Movement scenario
                                    Vector3 actionParameter = hit.point; // destination in this scenario
                                    Vector3 agentPosition = selectedAgent.GetComponent<Transform>().position;
                                    Vector3 ballPosition = selectedAgent.ball.position;
                                    GameObject[] teammates = selectedAgent.teammates;
                                    GameObject[] opponents = selectedAgent.opponents;
                                    LinkedList<Vector3> teammatePositions = new LinkedList<Vector3>();
                                    LinkedList<Vector3> opponentPositions = new LinkedList<Vector3>();

                                    foreach (var teammate in teammates)
                                    {
                                        teammatePositions.AddLast(teammate.GetComponent<Transform>().position);
                                    }

                                    foreach (var opponent in opponents)
                                    {
                                        opponentPositions.AddLast(opponent.GetComponent<Transform>().position);
                                    }

                                    selectedAgent.AddAction(action);
                                    selectedAgent.AddActionMove(actionParameter, waypoint);
                                    selectedAgent.pendingScenario = new Scenario(action, actionParameter, selectedAgent.tag, agentPosition, ballPosition, teammatePositions, opponentPositions);
                                    scenarios.Add(new Scenario(action, actionParameter, selectedAgent.tag, agentPosition, ballPosition, teammatePositions, opponentPositions));
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

                                agentsWithUserActions.Add(selectedAgent.GetComponent<AIComponent>());
                                userActionsGUI.SetActive(true);
                                coachMoveMode = !coachMoveMode;
                            }
                        }
                    }
                }
            }
        }
        else // coachMode disabled, reset all agent material that has been changed
        {

            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();
                selectionRenderer.material = defaultMaterial;
            }

            _selection = null;

            if (selectedPlayer[0] != null)
            {
                var selectionRenderer = selectedPlayer[0].GetComponent<Renderer>();
                selectionRenderer.material = defaultMaterial;
                Array.Clear(selectedPlayer, 0, 1);
                userActionsGUI.SetActive(false);
            }
        }
        ReadScenarios();
    }


    void ReadScenarios()
    {
        foreach (Scenario scenario in scenarios)
        {  
            if (scenario.teamTag == "purpleAgent")
            {
                //check if agentPosition matches
                foreach (GameObject agent in purpleAgents)
                {

                    if (Mathf.Abs(agent.transform.position.x - scenario.agentPosition.x) < 5 && Mathf.Abs(agent.transform.position.z - scenario.agentPosition.z) < 5)
                    {
                        //check if all teammate positions matches
                        foreach (GameObject teammate in purpleAgents)
                        {
                            bool teammateMatch = false;
                            foreach (Vector3 teammatePosition in scenario.teammatePositions)
                            {
                                if (teammate.transform.position == agent.transform.position)
                                {
                                    teammateMatch = true;
                                }
                                if (Mathf.Abs(teammate.transform.position.x - teammatePosition.x) < 5 && Mathf.Abs(teammate.transform.position.z - teammatePosition.z) < 5)
                                {
                                    teammateMatch = true;
                                }
                            }
                            if (teammateMatch == false)
                            {
                                break;
                            }
                        }
                        //check if all opponent position matches
                        foreach (GameObject opponent in blueAgents)
                        {
                            bool opponentMatch = false;
                            foreach (Vector3 opponentPosition in scenario.opponentPositions)
                            {
                                if (Mathf.Abs(opponent.transform.position.x - opponentPosition.x) < 5 && Mathf.Abs(opponent.transform.position.z - opponentPosition.z) < 5)
                                {
                                    opponentMatch = true;
                                }
                            }
                            if (opponentMatch == false)
                            {
                                break;
                            }
                        }
                        //check if ball's position matches
                        if(Mathf.Abs(ball.position.x - scenario.ballPosition.x)<5 && Mathf.Abs(ball.position.z - scenario.ballPosition.z) < 5)
                        {
                            agent.GetComponent<AIComponent>().userActions.Add(scenario.action);
                            agent.GetComponent<AIComponent>().AddActionMove(scenario.actionParameter, waypoint);
                            Debug.Log("found a same scenario(purple)");
                        }
                    }
                }
            }
            if (scenario.teamTag == "blueAgent")
            {
                //check if agentPosition matches
                foreach (GameObject agent in blueAgents)
                {
                    if (Mathf.Abs(agent.transform.position.x - scenario.agentPosition.x) < 5 && Mathf.Abs(agent.transform.position.z - scenario.agentPosition.z) < 5)
                    {
                        //check if all teammate positions matches
                        foreach (GameObject teammate in blueAgents)
                        {
                            bool teammateMatch = false;
                            foreach (Vector3 teammatePosition in scenario.teammatePositions)
                            {
                                if (teammate.transform.position == agent.transform.position)
                                {
                                    teammateMatch = true;
                                }
                                if (Mathf.Abs(teammate.transform.position.x - teammatePosition.x) < 5 && Mathf.Abs(teammate.transform.position.z - teammatePosition.z) < 5)
                                {
                                    teammateMatch = true;
                                }
                            }
                            if (teammateMatch == false)
                            {
                                break;
                            }
                        }
                        //check if all opponent position matches
                        foreach (GameObject opponent in purpleAgents)
                        {
                            bool opponentMatch = false;
                            foreach (Vector3 opponentPosition in scenario.opponentPositions)
                            {
                                if (Mathf.Abs(opponent.transform.position.x - opponentPosition.x) < 5 && Mathf.Abs(opponent.transform.position.z - opponentPosition.z) < 5)
                                {
                                    opponentMatch = true;
                                }
                            }
                            if (opponentMatch == false)
                            {
                                break;
                            }
                        }
                        //check if ball's position matches
                        if (Mathf.Abs(ball.position.x - scenario.ballPosition.x) < 5 && Mathf.Abs(ball.position.z - scenario.ballPosition.z) < 5)
                        {
                            agent.GetComponent<AIComponent>().userActions.Add(scenario.action);
                            agent.GetComponent<AIComponent>().AddActionMove(scenario.actionParameter, waypoint);
                            Debug.Log("found a same scenario(blue)");
                        }
                    }
                }
            }
        }
        
    }
}
