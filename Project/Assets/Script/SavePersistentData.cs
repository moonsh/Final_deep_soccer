using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class SavePersistentData : MonoBehaviour
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
        HashSet<string> saveListData = new HashSet<string>();
        if (File.Exists(Application.dataPath + "/saveFile.json"))
        {
            string jsonString = File.ReadAllText(Application.dataPath + "/saveFile.json");
            string[] jsonContents = jsonString.Split(new[] { SAVE_SEPARATOR }, System.StringSplitOptions.None);
            for (int i = 0; i < jsonContents.Length; i++)
            {
                saveListData.Add(jsonContents[i]);
            }
        }
        foreach (var entry in CoachController.scenarios)
        {
            Scenario sc = entry.Value;
            PlayerData playerData = new PlayerData();
            playerData.actionName = "Past " + sc.action;
            playerData.action = sc.action;
            playerData.actionParameter = sc.actionParameter;
            playerData.agentPosition = sc.agentPosition;
            playerData.ballPosition = sc.ballPosition;
            int count = 0;
            foreach(var pos in sc.teammatePositions)
            {
                if(count < 1)
                {
                    playerData.teampos1 = pos;
                }
                else
                {
                    playerData.teampos2 = pos;
                }
                count++;
            }
            count = 0;
            foreach (var pos in sc.opponentPositions)
            {
                if (count < 1)
                {
                    playerData.oppopos1 = pos;
                }
                else if(count < 2)
                {
                    playerData.oppopos2 = pos;
                }
                else
                {
                    playerData.oppopos3 = pos;
                }
                count++;
            }
            playerData.ballPossessed = sc.ballPossessed;
            playerData.teamWithBall = sc.teamWithBall;
            playerData.strategy = sc.strategy;
            playerData.reward = sc.reward;
            
            saveListData.Add(JsonUtility.ToJson(playerData));
            
            
        }
        string[] contents = new string[saveListData.Count];
        saveListData.CopyTo(contents);
        string saveString = string.Join(SAVE_SEPARATOR, contents);
        File.WriteAllText(Application.dataPath + "/saveFile.json", saveString);
    }

   

   

    
}
            /*string json = JsonUtility.ToJson(playerData);
            Debug.Log(json);
            string path = Application.dataPath + "/saveFile.json";
            FileStream fileStream = new FileStream(path, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }

            json = File.ReadAllText(Application.dataPath + "/saveFile.json");
            PlayerData loadPlayerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log(loadPlayerData.teampos1);*/
