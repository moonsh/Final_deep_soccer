using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddStrategy : MonoBehaviour
{
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        CoachController.strategyCount++;
        CoachController.strategySequence.Add("strategy" + CoachController.strategyCount);
        GameObject.Find("StrategySequenceList").GetComponent<StrategySequenceList>().updateBoard();
    }
}
