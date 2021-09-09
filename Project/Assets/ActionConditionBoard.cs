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
    public Text text1;
    public Text text2;
    
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

    public void DrawCircle(Vector3 pos, LineRenderer circle)
    {
        int points = 100;
        int R = 5;                                           
        float angle = 360f / (points - 20);                
        Quaternion q = new Quaternion();
        circle.startColor = Color.red;
        circle.endColor = Color.red;
        circle.startWidth = 0.3f;
        circle.endWidth = 0.3f;
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
    public void ResetText(Vector3 pos1, Vector3 pos2, string name)
    {
       
        DrawCircle(pos1, circle1);
        DrawCircle(pos2, circle2);

        if (name == "P1")
        {
            var temp1 = p2.position;
            var temp2 = p3.position;
            var distance1 = Mathf.Sqrt((temp1.x - pos1.x) * (temp1.x - pos1.x) + (temp1.z - pos1.z) * (temp1.z - pos1.z));
            var distance2 = Mathf.Sqrt((temp2.x - pos1.x) * (temp2.x - pos1.x) + (temp2.z - pos1.z) * (temp2.z - pos1.z));
            if(distance1 > distance2)
            {
                text1.text = "R2";
                text2.text = "R3";
            }
            else
            {
                text1.text = "R3";
                text2.text = "R2";
            }
        }
        if(name == "P2")
        {
            var temp1 = p1.position;
            var temp2 = p3.position;
            var distance1 = Mathf.Sqrt((temp1.x - pos1.x) * (temp1.x - pos1.x) + (temp1.z - pos1.z) * (temp1.z - pos1.z));
            var distance2 = Mathf.Sqrt((temp2.x - pos1.x) * (temp2.x - pos1.x) + (temp2.z - pos1.z) * (temp2.z - pos1.z));
            if (distance1 > distance2)
            {
                text1.text = "R1";
                text2.text = "R3";
            }
            else
            {
                text1.text = "R3";
                text2.text = "R1";
            }
        }
        if(name == "P3")
        {
            var temp1 = p1.position;
            var temp2 = p2.position;
            var distance1 = Mathf.Sqrt((temp1.x - pos1.x) * (temp1.x - pos1.x) + (temp1.z - pos1.z) * (temp1.z - pos1.z));
            var distance2 = Mathf.Sqrt((temp2.x - pos1.x) * (temp2.x - pos1.x) + (temp2.z - pos1.z) * (temp2.z - pos1.z));
            if (distance1 > distance2)
            {
                text1.text = "R1";
                text2.text = "R2";
            }
            else
            {
                text1.text = "R2";
                text2.text = "R1";
            }
            Debug.Log(text1.text);
        }
        if(name == "B1")
        {
            var temp1 = b2.position;
            var temp2 = b3.position;
            var distance1 = Mathf.Sqrt((temp1.x - pos1.x) * (temp1.x - pos1.x) + (temp1.z - pos1.z) * (temp1.z - pos1.z));
            var distance2 = Mathf.Sqrt((temp2.x - pos1.x) * (temp2.x - pos1.x) + (temp2.z - pos1.z) * (temp2.z - pos1.z));
            if (distance1 > distance2)
            {
                text1.text = "B2";
                text2.text = "B3";
            }
            else
            {
                text1.text = "B3";
                text2.text = "B2";
            }
        }

        if (name == "B2")
        {
            var temp1 = b1.position;
            var temp2 = b3.position;
            var distance1 = Mathf.Sqrt((temp1.x - pos1.x) * (temp1.x - pos1.x) + (temp1.z - pos1.z) * (temp1.z - pos1.z));
            var distance2 = Mathf.Sqrt((temp2.x - pos1.x) * (temp2.x - pos1.x) + (temp2.z - pos1.z) * (temp2.z - pos1.z));
            if (distance1 > distance2)
            {
                text1.text = "B1";
                text2.text = "B3";
            }
            else
            {
                text1.text = "B3";
                text2.text = "B1";
            }
        }
        if (name == "B3")
        {
            var temp1 = b1.position;
            var temp2 = b2.position;
            var distance1 = Mathf.Sqrt((temp1.x - pos1.x) * (temp1.x - pos1.x) + (temp1.z - pos1.z) * (temp1.z - pos1.z));
            var distance2 = Mathf.Sqrt((temp2.x - pos1.x) * (temp2.x - pos1.x) + (temp2.z - pos1.z) * (temp2.z - pos1.z));
            if (distance1 > distance2)
            {
                text1.text = "B1";
                text2.text = "B2";
            }
            else
            {
                text1.text = "B2";
                text2.text = "B1";
            }
        }

        text1.transform.position = new Vector3(text1.transform.position.x + pos1.z * 5, text1.transform.position.y - pos1.x * 5, text1.transform.position.z);
        text2.transform.position = new Vector3(text2.transform.position.x + pos2.z * 5, text2.transform.position.y - pos2.x * 5, text2.transform.position.z);


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
                currentScene = scenario.Value;
                agentPos.text = scenario.Value.agentPosition.ToString();
                ballPos.text = scenario.Value.ballPosition.ToString();
                selectedPlayer = CoachController.selectedPlayer;
                diff = selectedPlayer.position - currentScene.agentPosition;
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
                ResetText(pos1 + diff, pos2 + diff,selectedPlayer.name.Substring(selectedPlayer.name.Length - 2));
                count = 0;
                foreach (Vector3 pos in scenario.Value.opponentPositions)
                {
                    if (count == 0)
                    {
                        opponentPos1.text = pos.ToString();
                    }
                    else if (count == 1)
                    {
                        opponentPos2.text = pos.ToString();
                    }
                    else if (count == 2)
                    {
                        opponentPos3.text = pos.ToString();
                    }
                    count++;
                }
                Visualization(currentScene);
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
        Filter(sc, selectedPlayer, team);
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
