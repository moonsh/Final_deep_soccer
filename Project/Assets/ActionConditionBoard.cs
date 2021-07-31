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
    void Start()
    {
        saveButton.onClick.AddListener(TaskOnClick);
        deleteButton.onClick.AddListener(deleteElement);
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

    public void resetBoard()
    {
        
        title.text = key+" Conditions";
        foreach (var scenario in CoachController.scenarios)
        {
            if (scenario.Key == key)
            {
                currentScene = scenario.Value;
                agentPos.text = scenario.Value.agentPosition.ToString();
                ballPos.text = scenario.Value.ballPosition.ToString();
                int count = 0;
                foreach (Vector3 pos in scenario.Value.teammatePositions)
                {
                    if (count == 0)
                    {
                        teammatePos1.text = pos.ToString();
                    }
                    else if (count == 1)
                    {
                        teammatePos2.text = pos.ToString();
                    }
                    count++;
                }
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
            }
            else
            {
                newScenarios.Add(scenario.Key, scenario.Value);
            }

        }

    }

    void TaskOnClick()
    {
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
