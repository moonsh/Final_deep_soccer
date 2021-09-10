using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionConditionBoard : MonoBehaviour
{
    public Text title;
    public GameObject acb;
    public Button deleteButton;
    public Button saveButton;
    public InputField agentPos;
    public InputField ballPos;
    public InputField teammatePos1;
    public InputField teammatePos2;
    public InputField opponentPos1;
    public InputField opponentPos2;
    public InputField opponentPos3;
    private Scenario currentScene;
    private Vector3 apos;
    private Vector3 bpos;
    private HashSet<Vector3> tpos = new HashSet<Vector3>();
    private HashSet<Vector3> opos = new HashSet<Vector3>();
    public static string key;
    public Dictionary<string, Scenario> newScenarios = new Dictionary<string, Scenario>();
    private string tKey;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform b1;
    public Transform b2;
    public Transform b3;
    public Transform extra;
    [SerializeField] private Material defaultPurpleMaterial;
    [SerializeField] private Material defaultBlueMaterial;
    [SerializeField] private Material highlightMaterial;
    public Transform blueGoal;
    public Transform purpleGoal;
    private Vector3 diff;
    private Vector3 expectedBallPos;
    private HashSet<Transform> expectedTeamPositions;
    private HashSet<Transform> expectedOppoPositions;
    private Vector3 target;
    private Transform selectedPlayer;
    public Transform targetMark;
    public LineRenderer lineRenderer;
    public LineRenderer circle1;
    public LineRenderer circle2;
    public LineRenderer circle3;
    public LineRenderer circle4;
    public LineRenderer circle5;


    void Start()
    {
        saveButton.onClick.AddListener(TaskOnClick);
        deleteButton.onClick.AddListener(deleteElement);
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 VecToStr(string s)
    {
        s = s.Substring(1, s.Length - 2);
        string[] sArray = s.Split(',');

        Vector3 result = new Vector3(
             float.Parse(sArray[0]),
             float.Parse(sArray[1]),
             float.Parse(sArray[2]));
        return result;
    }

    public void DrawCircle(Vector3 pos, LineRenderer circle, int R, bool team)
    {
        int points = 100;                                          
        float angle = 360f / (points - 20);                
        Quaternion q = new Quaternion();
        if(team)
        {
            circle.startColor = Color.red;
            circle.endColor = Color.red;
            circle.material.SetColor("_Color", Color.red);
        }
        else
        {
            circle.startColor = Color.blue;
            circle.endColor = Color.blue;
            circle.material.SetColor("_Color", Color.blue);

        }
        circle.startWidth = 0.5f;
        circle.endWidth = 0.5f;
        circle.positionCount = points;
        for (int i = 0; i < points; i++)
        {
            if (i != 0)
            {
                q = Quaternion.Euler(q.eulerAngles.x + angle, q.eulerAngles.y, q.eulerAngles.z + angle);
            }
            Vector3 forwardPosition = pos + q * Vector3.down * R;
            circle.SetPosition(i, forwardPosition);
        }
    }
    


    public void resetBoard()
    {
        
        title.text = key+" Conditions";
        foreach (var scenario in CoachController.scenarios)
        {
            if (scenario.Key == key)
            {
                Vector3 pos1 = Vector3.zero;
                Vector3 pos2 = Vector3.zero;
                Vector3 pos3 = Vector3.zero;
                Vector3 pos4 = Vector3.zero;
                Vector3 pos5 = Vector3.zero;
                currentScene = scenario.Value;
                agentPos.text = scenario.Value.agentPosition.ToString();
                ballPos.text = scenario.Value.ballPosition.ToString();
                selectedPlayer = CoachController.selectedPlayer;
                diff = selectedPlayer.position - currentScene.agentPosition;
//                diff = new Vector3(0, 0, 0);
                int count = 0;
                foreach (Vector3 pos in scenario.Value.teammatePositions)
                {
                    if (count == 0)
                    {
                        teammatePos1.text = pos.ToString();
                        pos1 = pos;
                    }
                    else if (count == 1)
                    {
                        teammatePos2.text = pos.ToString();
                        pos2 = pos;
                    }
                    count++;
                }
                

                count = 0;
                foreach (Vector3 pos in scenario.Value.opponentPositions)
                {
                    if (count == 0)
                    {
                        opponentPos1.text = pos.ToString();
                        pos3 = pos;
                    }
                    else if (count == 1)
                    {
                        opponentPos2.text = pos.ToString();
                        pos4 = pos;
                    }
                    else if (count == 2)
                    {
                        opponentPos3.text = pos.ToString();
                        pos5 = pos;
                    }
                    count++;
                }
                Visualization(currentScene);
                DrawCircle(pos1 + diff, circle1, CoachController.TeamR*2, true);
                DrawCircle(pos2 + diff, circle2, CoachController.TeamR*2, true);
                DrawCircle(pos3 + diff, circle3, CoachController.OppoR*2, false);
                DrawCircle(pos4 + diff, circle4, CoachController.OppoR*2, false);
                DrawCircle(pos5 + diff, circle5, CoachController.OppoR*2, false);
            }
            else
            {
                if(!newScenarios.ContainsKey(scenario.Key))
                {
                    newScenarios.Add(scenario.Key, scenario.Value);
                }

            }
            
        }

    }


    private void Visualization(Scenario sc)
    {
        
                
        expectedBallPos = sc.ballPosition - diff;
        //the expected position of teammates based on diff
        expectedTeamPositions = new HashSet<Transform>();
        //the expected position of opponents based on diff
        expectedOppoPositions = new HashSet<Transform>();
        target = sc.actionParameter + diff;
        string team = FindSelectedAgent(selectedPlayer);
        //Filter(sc, selectedPlayer, team);
        Visualize();
    }

    private string FindSelectedAgent(Transform selectedPlayer)
    {
        if (selectedPlayer == b1)
        {
            expectedTeamPositions.Add(b2);
            expectedTeamPositions.Add(b3);
            expectedOppoPositions.Add(p1);
            expectedOppoPositions.Add(p2);
            expectedOppoPositions.Add(p3);
            return "blue";
        }
        else if (selectedPlayer == b2)
        {
            expectedTeamPositions.Add(b1);
            expectedTeamPositions.Add(b3);
            expectedOppoPositions.Add(p1);
            expectedOppoPositions.Add(p2);
            expectedOppoPositions.Add(p3);
            return "blue";
        }
        else if (selectedPlayer == b3)
        {
            expectedTeamPositions.Add(b1);
            expectedTeamPositions.Add(b2);
            expectedOppoPositions.Add(p1);
            expectedOppoPositions.Add(p2);
            expectedOppoPositions.Add(p3);
            return "blue";
        }
        else if (selectedPlayer == p1)
        {
            expectedTeamPositions.Add(p2);
            expectedTeamPositions.Add(p3);
            expectedOppoPositions.Add(b1);
            expectedOppoPositions.Add(b2);
            expectedOppoPositions.Add(b3);
            return "purple";
        }
        else if (selectedPlayer == p2)
        {
            expectedTeamPositions.Add(p1);
            expectedTeamPositions.Add(p3);
            expectedOppoPositions.Add(b1);
            expectedOppoPositions.Add(b2);
            expectedOppoPositions.Add(b3);
            return "purple";
        }
        else if (selectedPlayer == p3)
        {
            expectedTeamPositions.Add(p1);
            expectedTeamPositions.Add(p2);
            expectedOppoPositions.Add(b1);
            expectedOppoPositions.Add(b2);
            expectedOppoPositions.Add(b3);
            return "purple";
        }
        return "purple";

    }

    private void Filter(Scenario scenario, Transform selectedPlayer, string teamHasBall)
    {
        Vector3 target = scenario.actionParameter;
        if (scenario.action == "Kick")
        {
            Vector3 changedTarget = scenario.actionParameter + diff;
            float targetDistanceToGoal;
            float newTargetDistanceToGoal;
            if (teamHasBall == "blue")
            {
                targetDistanceToGoal = Mathf.Sqrt((target.x - blueGoal.position.x) * (target.x - blueGoal.position.x) + (target.z - blueGoal.position.z) * (target.z - blueGoal.position.z));
                newTargetDistanceToGoal = Mathf.Sqrt((changedTarget.x - blueGoal.position.x) * (changedTarget.x - blueGoal.position.x) + (changedTarget.z - blueGoal.position.z) * (changedTarget.z - blueGoal.position.z));
            }
            else
            {
                targetDistanceToGoal = Mathf.Sqrt((target.x - purpleGoal.position.x) * (target.x - purpleGoal.position.x) + (target.z - purpleGoal.position.z) * (target.z - purpleGoal.position.z));
                newTargetDistanceToGoal = Mathf.Sqrt((changedTarget.x - purpleGoal.position.x) * (changedTarget.x - purpleGoal.position.x) + (changedTarget.z - purpleGoal.position.z) * (changedTarget.z - purpleGoal.position.z));
            }
            if (targetDistanceToGoal < 3) //shooting:Ignore teammates and opponents not near the player
            {
                HashSet<Transform> tempSet = new HashSet<Transform>();
                foreach (Transform team in expectedTeamPositions)
                {
                    var distanceToTeam = ((selectedPlayer.position.x - team.position.x) * (selectedPlayer.position.x - team.position.x) + (selectedPlayer.position.z - team.position.z) * (selectedPlayer.position.z - team.position.z));
                    if (distanceToTeam > BlackBoard2.teamR*2)
                    {
                        tempSet.Add(team);
                    }
                }
                foreach (var temp in tempSet)
                {
                    expectedOppoPositions.Remove(temp);
                }
                HashSet<Transform> tempSet2 = new HashSet<Transform>();
                foreach (Transform oppo in expectedOppoPositions)
                {
                    var distanceToOppo = ((selectedPlayer.position.x - oppo.position.x) * (selectedPlayer.position.x - oppo.position.x) + (selectedPlayer.position.z - oppo.position.z) * (selectedPlayer.position.z - oppo.position.z));
                    if (distanceToOppo > BlackBoard2.oppoR*2)
                    {
                        tempSet2.Add(oppo);
                    }
                }
                foreach (var temp in tempSet2)
                {
                    expectedOppoPositions.Remove(temp);
                }

                if (newTargetDistanceToGoal > 3)
                {


                    expectedOppoPositions.Add(extra);
                }

            }
            else //Passing:If opponent team players not near, ignore these players positions
            {
                HashSet<Transform> tempSet = new HashSet<Transform>();
                foreach (Transform oppo in expectedOppoPositions)
                {
                    var distanceToOppo = ((selectedPlayer.position.x - oppo.position.x) * (selectedPlayer.position.x - oppo.position.x) + (selectedPlayer.position.z - oppo.position.z) * (selectedPlayer.position.z - oppo.position.z));
                    if (distanceToOppo > BlackBoard2.oppoR*2)
                    {
                        
                        tempSet.Add(oppo);
                    }
                }
                foreach (var temp in tempSet)
                {
                    expectedOppoPositions.Remove(temp);
                }
                if (newTargetDistanceToGoal < 3)
                {
                    expectedOppoPositions.Add(extra);
                }
            }
        }
        else if (scenario.action == "Move")
        {
            if (scenario.ballPossessed) //move with a ball: Teammates position ^and  near opponents
            {
                HashSet<Transform> tempSet = new HashSet<Transform>();
                foreach (Transform oppo in expectedOppoPositions)
                {
                    var distanceToOppo = ((selectedPlayer.position.x - oppo.position.x) * (selectedPlayer.position.x - oppo.position.x) + (selectedPlayer.position.z - oppo.position.z) * (selectedPlayer.position.z - oppo.position.z));
                    if (distanceToOppo > BlackBoard2.oppoR*2)
                    {
                        tempSet.Add(oppo);
                    }
                }
                foreach (var temp in tempSet)
                {
                    expectedOppoPositions.Remove(temp);
                }
            }
            else //Attack move without ball: When user add a move action to opponent area, don’t consider opponents positions
            {
                if ((teamHasBall == "purple" && target.z > 0)
                    || (teamHasBall == "blue" && target.z < 0))
                {
                    expectedOppoPositions = new HashSet<Transform>();
                }
            }
        }
    }

    private void Visualize()
    {
        
        lineRenderer.SetPosition(0, selectedPlayer.position);
        lineRenderer.SetPosition(1, target);
        
        
        foreach (Transform t in expectedTeamPositions)
        {
            t.GetComponent<Renderer>().material = highlightMaterial;
        }
        foreach (Transform t in expectedOppoPositions)
        {
            t.GetComponent<Renderer>().material = highlightMaterial;
        }
        
        targetMark.position = new Vector3(target.x, 1, target.z);
    }

    public void ResetVisualization()
    {
        p1.GetComponent<Renderer>().material = defaultPurpleMaterial;
        p2.GetComponent<Renderer>().material = defaultPurpleMaterial;
        p3.GetComponent<Renderer>().material = defaultPurpleMaterial;
        b1.GetComponent<Renderer>().material = defaultBlueMaterial;
        b2.GetComponent<Renderer>().material = defaultBlueMaterial;
        b3.GetComponent<Renderer>().material = defaultBlueMaterial;
   //     Destroy(lineRenderer);
        targetMark.position = new Vector3(100, 100, 100);
    }

    void TaskOnClick()
    {
        ResetVisualization();
        apos = VecToStr(agentPos.text);
        bpos = VecToStr(ballPos.text);
        tpos = new HashSet<Vector3>();
        opos = new HashSet<Vector3>();
        tpos.Add(VecToStr(teammatePos1.text));
        tpos.Add(VecToStr(teammatePos2.text));
        opos.Add(VecToStr(opponentPos1.text));
        opos.Add(VecToStr(opponentPos2.text));
        opos.Add(VecToStr(opponentPos3.text));
        
        currentScene = new Scenario(currentScene.action, currentScene.actionParameter, apos, bpos, tpos, opos, currentScene.ballPossessed, currentScene.teamWithBall, currentScene.reward,currentScene.strategy);
        CoachController.scenarios[key] = currentScene;
        newScenarios = new Dictionary<string, Scenario>();
        acb.SetActive(false);
    }

    void deleteElement()
    {
        CoachController.scenarios = newScenarios;
        newScenarios = new Dictionary<string, Scenario>();
        acb.SetActive(false);
    }

}
