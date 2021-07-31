using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteStrategy : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        if (ActionSequenceList.key != "strategy1")
        {
            CoachController.strategySequence.Remove(ActionSequenceList.key);
            if (ActionSequenceList.key == CoachController.currentStrategy)
                CoachController.currentStrategy = "strategy1";
        }      
    }
}
