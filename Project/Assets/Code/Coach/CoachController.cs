// Anthony Tiongson (ast119)

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoachController : MonoBehaviour
{
    public static bool coachMode;
    public static bool sceneReset;
    public static HashSet<AIComponent> agentsWithUserActions = new HashSet<AIComponent>();
    public static HashSet<AIComponent> agentsUsingPastScenario = new HashSet<AIComponent>();
    public static Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();
    public static List<List<string>> actionSequences = new List<List<string>>();
    public static List<string> actionSequence = new List<string>();
    public static float countTime;
    public enum coachCommands
    {
        NONE,
        MOVE,
        KICK,
    }
    public GameObject BlackBoard;
    public GameObject BlackBoard2;
    public GameObject ActionSequenceBoard;
    public GameObject ActionConditionBoard;
    public static GameObject asb;
    public static GameObject acb;
    [SerializeField] private string agentTag = "purpleAgent";
    [SerializeField] private string fieldTag = "field";
    [SerializeField] private string ballTag = "ball";
    [SerializeField] private string goalTag = "purpleGoal";
    [SerializeField] private GameObject purpleTeam;
    [SerializeField] private GameObject blueTeam;
    [SerializeField] private GameObject scenariosGUI;
    [SerializeField] private GameObject saveScenariosButton;
    [SerializeField] private GameObject clearScenariosButton;
    [SerializeField] private GameObject userActionsGUI;
    [SerializeField] private GameObject cancelUserActionsButton;
    [SerializeField] private GameObject cancelAllUsersActionsButton;
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

   
    public static void toActionCondition()
    {
        asb.SetActive(false);
        acb.SetActive(true);
        
    }
    public void CancelAllUsersActions()
    {
        AIComponent[] currentAgentsWithUserActions = new AIComponent[agentsWithUserActions.Count];
        agentsWithUserActions.CopyTo(currentAgentsWithUserActions);

        foreach (var agent in currentAgentsWithUserActions)
        {
            agent.RemoveAllActions();
        }

        currentCommand = coachCommands.NONE;
    }

    public void CancelUserActions()
    {
        selectedPlayer.GetComponent<AIComponent>().RemoveAllActions();
        currentCommand = coachCommands.NONE;
    }

    public void ClearScenarios()
    {
        scenarios.Clear();
        actionSequence.Clear();
        actionSequences.Clear();
        CancelAllUsersActions();
    }

    public void LoadScenarios()
    {
        string filename = "Saved_scenarios.csv";
        string filePath = GetPath(filename);
        var fileData = File.ReadAllText(filePath);
        var lines = fileData.Split("\n"[0]);
        //Debug.Log("CoachController: lines.Length - " + lines.Length.ToString());

        for (int i = 1; i < (lines.Length - 2); i++)
        {
            var teammatePositions = new HashSet<Vector3>();
            var opponentPositions = new HashSet<Vector3>();
            var lineData = (lines[i].Trim()).Split(","[0]);
            //Debug.Log("CoachController: lineData.Length - " + lineData.Length);
            var label = lineData[0];
            //Debug.Log("CoachController: label - " + label);
            var action = lineData[1];
            var actionParameter = new Vector3(float.Parse(lineData[2]), float.Parse(lineData[3]), float.Parse(lineData[4]));
            var agentPosition = new Vector3(float.Parse(lineData[5]), float.Parse(lineData[6]), float.Parse(lineData[7]));
            var ballPosition = new Vector3(float.Parse(lineData[8]), float.Parse(lineData[9]), float.Parse(lineData[10]));
            teammatePositions.Add(new Vector3(float.Parse(lineData[11]), float.Parse(lineData[12]), float.Parse(lineData[13])));
            teammatePositions.Add(new Vector3(float.Parse(lineData[14]), float.Parse(lineData[15]), float.Parse(lineData[16])));
            opponentPositions.Add(new Vector3(float.Parse(lineData[17]), float.Parse(lineData[18]), float.Parse(lineData[19])));
            opponentPositions.Add(new Vector3(float.Parse(lineData[20]), float.Parse(lineData[21]), float.Parse(lineData[22])));
            opponentPositions.Add(new Vector3(float.Parse(lineData[23]), float.Parse(lineData[24]), float.Parse(lineData[25])));
            var ballPossessed = bool.Parse(lineData[26]);
            var teamWithBall = lineData[27];
            var reward = double.Parse(lineData[28]);
            CreateAndLogScenario(label, action, actionParameter, agentPosition, ballPosition, teammatePositions, opponentPositions,
                ballPossessed, teamWithBall, reward);
        }
    }

    public void SaveScenarios()
    {
        string filename = "Saved_scenarios.csv";
        List<string[]> rowData = new List<string[]>();
        string filePath = GetPath(filename);
        string[] rowDataTemp = new string[29];
        rowDataTemp[0] = "Label";
        rowDataTemp[1] = "Action";
        rowDataTemp[2] = "Action Paramater X";
        rowDataTemp[3] = "Action Paramater Y";
        rowDataTemp[4] = "Action Paramater Z";
        rowDataTemp[5] = "Agent Position X";
        rowDataTemp[6] = "Agent Position Y";
        rowDataTemp[7] = "Agent Position Z";
        rowDataTemp[8] = "Ball Position X";
        rowDataTemp[9] = "Ball Position Y";
        rowDataTemp[10] = "Ball Position Z";
        rowDataTemp[11] = "Teammate1 Position X";
        rowDataTemp[12] = "Teammate1 Position Y";
        rowDataTemp[13] = "Teammate1 Position Z";
        rowDataTemp[14] = "Teammat2 Position X";
        rowDataTemp[15] = "Teammat2 Position Y";
        rowDataTemp[16] = "Teammat2 Position Z";
        rowDataTemp[17] = "Opponent1 Position X";
        rowDataTemp[18] = "Opponent1 Position Y";
        rowDataTemp[19] = "Opponent1 Position Z";
        rowDataTemp[20] = "Opponent2 Position X";
        rowDataTemp[21] = "Opponent2 Position Y";
        rowDataTemp[22] = "Opponent2 Position Z";
        rowDataTemp[23] = "Opponent3 Position X";
        rowDataTemp[24] = "Opponent3 Position Y";
        rowDataTemp[25] = "Opponent3 Position Z";
        rowDataTemp[26] = "Ball Possessed";
        rowDataTemp[27] = "Team With Ball";
        rowDataTemp[28] = "Reward";
        rowData.Add(rowDataTemp);

        foreach (var entry in scenarios)
        {
            rowDataTemp = new string[29];
            var label = entry.Key;
            var scenario = entry.Value;
            List<Vector3> teammatePositions = scenario.teammatePositions.ToList();
            List<Vector3> opponentPositions = scenario.opponentPositions.ToList();
            rowDataTemp[0] = label;
            rowDataTemp[1] = scenario.action;
            rowDataTemp[2] = scenario.actionParameter.x.ToString();
            rowDataTemp[3] = scenario.actionParameter.y.ToString();
            rowDataTemp[4] = scenario.actionParameter.z.ToString();
            rowDataTemp[5] = scenario.agentPosition.x.ToString();
            rowDataTemp[6] = scenario.agentPosition.y.ToString();
            rowDataTemp[7] = scenario.agentPosition.z.ToString();
            rowDataTemp[8] = scenario.ballPosition.x.ToString();
            rowDataTemp[9] = scenario.ballPosition.y.ToString();
            rowDataTemp[10] = scenario.ballPosition.z.ToString();
            rowDataTemp[11] = teammatePositions[0].x.ToString();
            rowDataTemp[12] = teammatePositions[0].y.ToString();
            rowDataTemp[13] = teammatePositions[0].z.ToString();
            rowDataTemp[14] = teammatePositions[1].x.ToString();
            rowDataTemp[15] = teammatePositions[1].y.ToString();
            rowDataTemp[16] = teammatePositions[1].z.ToString();
            rowDataTemp[17] = opponentPositions[0].x.ToString();
            rowDataTemp[18] = opponentPositions[0].y.ToString();
            rowDataTemp[19] = opponentPositions[0].z.ToString();
            rowDataTemp[20] = opponentPositions[1].x.ToString();
            rowDataTemp[21] = opponentPositions[1].y.ToString();
            rowDataTemp[22] = opponentPositions[1].z.ToString();
            rowDataTemp[23] = opponentPositions[2].x.ToString();
            rowDataTemp[24] = opponentPositions[2].y.ToString();
            rowDataTemp[25] = opponentPositions[2].z.ToString();
            rowDataTemp[26] = scenario.ballPossessed.ToString();
            rowDataTemp[27] = scenario.teamWithBall;
            rowDataTemp[28] = scenario.reward.ToString();
            rowData.Add(rowDataTemp);
        }

        WriteToCSV(filePath, rowData);

        filename = "Saved_actionSequences.csv";
        rowData = new List<string[]>();
        filePath = GetPath(filename);
        //Debug.Log("CoachController: actionSequences.Count = " + actionSequences.Count.ToString());

        for (var i = 0; i < actionSequences.Count; i++)
        {
            rowDataTemp = new string[actionSequences[i].Count];
            //Debug.Log("CoachController: actionSequences[" + i.ToString() + "].Count = " + actionSequences[i].Count.ToString());

            for (var j = 0; j < actionSequences[i].Count; j++)
            {
                rowDataTemp[j] = actionSequences[i][j];
            }

            rowData.Add(rowDataTemp);
        }

        WriteToCSV(filePath, rowData);
    }

    public void SetCommandModeKick()
    {
        currentCommand = coachCommands.KICK;
    }

    public void SetCommandModeMove()
    {
        currentCommand = coachCommands.MOVE;
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

    private void CoachModeDisabledReset()
    {
        scenariosGUI.SetActive(false);
        ResetMaterials();
        currentCommand = coachCommands.NONE;
    }

    private void CreateAndLogScenario(string label, string action, Vector3 actionParameter, Vector3 agentPosition,
        Vector3 ballPosition, HashSet<Vector3> teammatePositions, HashSet<Vector3> opponentPositions, bool ballPossessed,
        string teamWithBall, double reward, Vector3? actionParameterSecondary = null)
    {
        var newLabel = label;
        var labelSuffix = 2;

        while (CoachController.scenarios.ContainsKey(newLabel))
        {
            newLabel = label + labelSuffix.ToString();
            labelSuffix++;
        }

        Scenario scenario = new Scenario(action, actionParameter, agentPosition, ballPosition,
            teammatePositions, opponentPositions, ballPossessed, teamWithBall, 0d);
        scenarios.Add(newLabel, scenario);
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
        ballPosition, teammatePositions, opponentPositions, ballPossessed, teamWithBall, 0d)));
    }

    // Following method is used to retrieve the relative path in regards to device platform
    private string GetPath(string filename)
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + filename;
#elif UNITY_ANDROID
        return Application.persistentDataPath+filename;
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+filename;
#else
        return Application.dataPath +"/"+filename;
#endif
    }

    private void ResetMaterials()
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
            ActionSequenceBoard.SetActive(false);
            ActionConditionBoard.SetActive(false);
        }
    }

    private void ToggleCoachMode()
    {
        coachMode = !coachMode;

        if (coachMode)
        {
            scenariosGUI.SetActive(true);

            if (scenarios.Count > 0)
            {
                saveScenariosButton.SetActive(true);
                clearScenariosButton.SetActive(true);
            }
            else
            {
                saveScenariosButton.SetActive(false);
                clearScenariosButton.SetActive(false);
            }
        }
    }

    private void ToggleTeamSelection()
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

    private void WriteToCSV(string filePath, List<string[]> rowData)
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        scenariosGUI.SetActive(false);
        userActionsGUI.SetActive(false);
        cancelAllUsersActionsButton.SetActive(false);
        coachMode = false;
        sceneReset = false;
        currentCommand = coachCommands.NONE;
        defaultMaterial = defaultPurpleMaterial;
        BlackBoard.SetActive(false);
        ActionSequenceBoard.SetActive(false);
        ActionConditionBoard.SetActive(false);
        BlackBoard2.SetActive(true);
        foreach (var agent in purpleTeam.GetComponentsInChildren<AIComponent>())
        {
            agent.AttachAgentScenarioIndicatorObject(agentScenarioIndicator);
        }

        foreach (var agent in blueTeam.GetComponentsInChildren<AIComponent>())
        {
            agent.AttachAgentScenarioIndicatorObject(agentScenarioIndicator);
        }
        asb = ActionSequenceBoard;
        acb = ActionConditionBoard;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneReset)
        {
            CancelAllUsersActions();
            sceneReset = !sceneReset;
        }

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
            cancelAllUsersActionsButton.SetActive(false);
        }
        else
        {
            cancelAllUsersActionsButton.SetActive(true);
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
                        ActionConditionBoard.SetActive(false);
                        ActionSequenceBoard.SetActive(false);
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
                            ActionConditionBoard.SetActive(false);
                            ActionSequenceBoard.SetActive(false);
                            BlackBoard2.SetActive(false);
                        }

                        _selection = selection;
                    }
                }

                if (selectedPlayer != null)
                {
                    if (agentsWithUserActions.Contains(selectedPlayer.GetComponent<AIComponent>()))
                    {
                        cancelUserActionsButton.SetActive(true);
                    }
                    else
                    {
                        cancelUserActionsButton.SetActive(false);
                    }

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
