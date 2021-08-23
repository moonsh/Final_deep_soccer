using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadPersistentData : MonoBehaviour
{
    private string SAVE_SEPARATOR = "#SAVE-VALUE#";
    // Start is called before the first frame update
    public Button button;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (File.Exists(Application.dataPath + "/saveFile.json"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/saveFile.json");
            string[] contents = saveString.Split(new[] { SAVE_SEPARATOR }, System.StringSplitOptions.None);
            for(int i = 0; i < contents.Length; i++)
            {
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(contents[i]);
                HashSet<Vector3> teammatePositions = new HashSet<Vector3>();
                HashSet<Vector3> opponentPositions = new HashSet<Vector3>();
                teammatePositions.Add(playerData.teampos1);
                teammatePositions.Add(playerData.teampos2);
                opponentPositions.Add(playerData.oppopos1);
                opponentPositions.Add(playerData.oppopos2);
                opponentPositions.Add(playerData.oppopos3);
                Scenario sc = new Scenario(playerData.action,playerData.actionParameter, playerData.agentPosition, playerData.ballPosition,teammatePositions,opponentPositions,playerData.ballPossessed,playerData.teamWithBall,playerData.reward,playerData.strategy);
                bool exist = false;
                foreach (var entry in CoachController.scenarios)
                {
                    if(sc.actionParameter == entry.Value.actionParameter)
                    {
                        if(sc.agentPosition == entry.Value.agentPosition)
                        {
                            exist = true;
                        }
                    }
                }
                if(!exist)
                {
                    CoachController.scenarios.Add(playerData.actionName + CoachController.pastScenarioCount, sc);
                    CoachController.pastScenarioCount++;
                }
                
            }
        }
    }
}
