using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ClearPersistentData : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        File.Delete(Application.dataPath + "/saveFile.json");
        foreach(var entry in CoachController.scenarios)
        {
            if(entry.Key[0]=='P')
            {
                CoachController.scenarios.Remove(entry.Key);
            }
        }
    }
}
